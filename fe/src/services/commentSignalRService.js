import * as signalR from "@microsoft/signalr";

/**
 * SignalR Service for Comment Hub
 * Handles real-time updates for blog comments
 */
class CommentSignalRService {
  constructor() {
    this.connection = null;
    this.listeners = new Map(); // event -> Set of callbacks
    this.isConnected = false;
  }

  /**
   * Get or create SignalR connection
   * @returns {signalR.HubConnection}
   */
  getConnection() {
    if (this.connection && this.isConnected) {
      return this.connection;
    }

    const baseUrl = import.meta.env.VITE_API_BASE_URL || "http://localhost:5000";
    const hubUrl = `${baseUrl}/hubs/comment`;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
          return localStorage.getItem("accessToken") || "";
        },
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    // Set up connection event handlers
    this.connection.onclose(() => {
      console.log("[CommentSignalR] Connection closed");
      this.isConnected = false;
    });

    this.connection.onreconnecting(() => {
      console.log("[CommentSignalR] Reconnecting...");
    });

    this.connection.onreconnected(() => {
      console.log("[CommentSignalR] Reconnected");
      this.isConnected = true;
    });

    return this.connection;
  }

  /**
   * Start SignalR connection
   */
  async start() {
    try {
      const conn = this.getConnection();
      if (conn.state === signalR.HubConnectionState.Disconnected) {
        await conn.start();
        this.isConnected = true;
        console.log("[CommentSignalR] Connected to Comment Hub");
      }
    } catch (error) {
      console.error("[CommentSignalR] Connection error:", error);
      throw error;
    }
  }

  /**
   * Stop SignalR connection
   */
  async stop() {
    if (this.connection && this.isConnected) {
      await this.connection.stop();
      this.isConnected = false;
      console.log("[CommentSignalR] Disconnected from Comment Hub");
    }
  }

  /**
   * Join blog group to receive real-time comment updates
   * @param {number} blogId - Blog ID
   */
  async joinBlogGroup(blogId) {
    try {
      const conn = this.getConnection();
      if (conn.state !== signalR.HubConnectionState.Connected) {
        await this.start();
      }

      await conn.invoke("JoinBlogGroup", blogId);
      console.log(`[CommentSignalR] Joined blog group: blogId=${blogId}`);
    } catch (error) {
      console.error("[CommentSignalR] Error joining blog group:", error);
      throw error;
    }
  }

  /**
   * Leave blog group
   * @param {number} blogId - Blog ID
   */
  async leaveBlogGroup(blogId) {
    try {
      const conn = this.getConnection();
      if (conn.state === signalR.HubConnectionState.Connected) {
        await conn.invoke("LeaveBlogGroup", blogId);
        console.log(`[CommentSignalR] Left blog group: blogId=${blogId}`);
      }
    } catch (error) {
      console.error("[CommentSignalR] Error leaving blog group:", error);
    }
  }

  /**
   * Subscribe to a SignalR event
   * @param {string} eventName - Event name (e.g., "NewComment")
   * @param {Function} callback - Callback function
   * @returns {Function} Unsubscribe function
   */
  on(eventName, callback) {
    if (!this.listeners.has(eventName)) {
      this.listeners.set(eventName, new Set());

      // Set up SignalR handler for this event
      const conn = this.getConnection();
      conn.on(eventName, (data) => {
        const callbacks = this.listeners.get(eventName);
        if (callbacks) {
          callbacks.forEach((cb) => {
            try {
              cb(data);
            } catch (error) {
              console.error(`[CommentSignalR] Error in callback for ${eventName}:`, error);
            }
          });
        }
      });
    }

    this.listeners.get(eventName).add(callback);

    // Return unsubscribe function
    return () => {
      const callbacks = this.listeners.get(eventName);
      if (callbacks) {
        callbacks.delete(callback);
      }
    };
  }

  /**
   * Unsubscribe from a SignalR event
   * @param {string} eventName - Event name
   * @param {Function} callback - Callback function to remove
   */
  off(eventName, callback) {
    const callbacks = this.listeners.get(eventName);
    if (callbacks) {
      callbacks.delete(callback);
    }
  }

  /**
   * Remove all listeners for an event
   * @param {string} eventName - Event name
   */
  removeAllListeners(eventName) {
    if (eventName) {
      this.listeners.delete(eventName);
      const conn = this.getConnection();
      conn.off(eventName);
    } else {
      // Remove all listeners
      this.listeners.forEach((_, event) => {
        const conn = this.getConnection();
        conn.off(event);
      });
      this.listeners.clear();
    }
  }
}

// Export singleton instance
export const commentSignalRService = new CommentSignalRService();

// Export class for testing
export default CommentSignalRService;

