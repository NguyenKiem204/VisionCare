import * as signalR from "@microsoft/signalr";

/**
 * SignalR Service for Booking Hub
 * Handles real-time updates for booking slots and appointments
 */
class SignalRService {
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
    const hubUrl = `${baseUrl}/hubs/booking`;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
          return localStorage.getItem("accessToken") || "";
        },
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection.onclose(() => {
      console.log("[SignalR] Connection closed");
      this.isConnected = false;
    });

    this.connection.onreconnecting(() => {
      console.log("[SignalR] Reconnecting...");
    });

    this.connection.onreconnected(() => {
      console.log("[SignalR] Reconnected");
      this.isConnected = true;
    });

    return this.connection;
  }

  
  async start() {
    try {
      const conn = this.getConnection();
      if (conn.state === signalR.HubConnectionState.Disconnected) {
        await conn.start();
        this.isConnected = true;
        console.log("[SignalR] Connected to Booking Hub");
      }
    } catch (error) {
      console.error("[SignalR] Connection error:", error);
      throw error;
    }
  }


  async stop() {
    if (this.connection && this.isConnected) {
      await this.connection.stop();
      this.isConnected = false;
      console.log("[SignalR] Disconnected from Booking Hub");
    }
  }


  async joinSlotsGroup(doctorId, date) {
    try {
      const conn = this.getConnection();
      if (conn.state !== signalR.HubConnectionState.Connected) {
        await this.start();
      }

      // Normalize date format to yyyyMMdd
      const normalizedDate = date.includes("-")
        ? date.replace(/-/g, "")
        : date;

      await conn.invoke("JoinSlotsGroup", doctorId, normalizedDate);
      console.log(`[SignalR] Joined slots group: doctorId=${doctorId}, date=${normalizedDate}`);
    } catch (error) {
      console.error("[SignalR] Error joining slots group:", error);
      throw error;
    }
  }

  async leaveSlotsGroup(doctorId, date) {
    try {
      const conn = this.getConnection();
      if (conn.state === signalR.HubConnectionState.Connected) {
        const normalizedDate = date.includes("-") ? date.replace(/-/g, "") : date;
        await conn.invoke("LeaveSlotsGroup", doctorId, normalizedDate);
        console.log(`[SignalR] Left slots group: doctorId=${doctorId}, date=${normalizedDate}`);
      }
    } catch (error) {
      console.error("[SignalR] Error leaving slots group:", error);
    }
  }

  async joinAdminGroup() {
    try {
      const conn = this.getConnection();
      if (conn.state !== signalR.HubConnectionState.Connected) {
        await this.start();
      }

      await conn.invoke("JoinAdminGroup");
      console.log("[SignalR] Joined admin group");
    } catch (error) {
      console.error("[SignalR] Error joining admin group:", error);
      throw error;
    }
  }

  async leaveAdminGroup() {
    try {
      const conn = this.getConnection();
      if (conn.state === signalR.HubConnectionState.Connected) {
        await conn.invoke("LeaveAdminGroup");
        console.log("[SignalR] Left admin group");
      }
    } catch (error) {
      console.error("[SignalR] Error leaving admin group:", error);
    }
  }

  on(eventName, callback) {
    if (!this.listeners.has(eventName)) {
      this.listeners.set(eventName, new Set());

      const conn = this.getConnection();
      conn.on(eventName, (data) => {
        const callbacks = this.listeners.get(eventName);
        if (callbacks) {
          callbacks.forEach((cb) => {
            try {
              cb(data);
            } catch (error) {
              console.error(`[SignalR] Error in callback for ${eventName}:`, error);
            }
          });
        }
      });
    }

    this.listeners.get(eventName).add(callback);

    return () => {
      const callbacks = this.listeners.get(eventName);
      if (callbacks) {
        callbacks.delete(callback);
      }
    };
  }

  off(eventName, callback) {
    const callbacks = this.listeners.get(eventName);
    if (callbacks) {
      callbacks.delete(callback);
    }
  }

  removeAllListeners(eventName) {
    if (eventName) {
      this.listeners.delete(eventName);
      const conn = this.getConnection();
      conn.off(eventName);
    } else {
      this.listeners.forEach((_, event) => {
        const conn = this.getConnection();
        conn.off(event);
      });
      this.listeners.clear();
    }
  }
}

export const signalRService = new SignalRService();

export default SignalRService;
