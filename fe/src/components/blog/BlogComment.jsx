import { useState } from "react";
import { createComment } from "../../services/commentService";
import { formatDistanceToNow } from "date-fns";
import { vi } from "date-fns/locale";

const BlogComment = ({ comment, blogId, onReply, onDelete, currentUserId, currentUserRole }) => {
  const [showReplyForm, setShowReplyForm] = useState(false);
  const [replyText, setReplyText] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleReply = async (e) => {
    e.preventDefault();
    if (!replyText.trim()) return;

    setIsSubmitting(true);
    try {
      await createComment(blogId, {
        parentCommentId: comment.commentId,
        commentText: replyText,
      });
      setReplyText("");
      setShowReplyForm(false);
      if (onReply) onReply();
    } catch (error) {
      console.error("Error replying to comment:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const canDelete = currentUserRole === "Admin" || comment.authorId === currentUserId;

  // Get avatar URL or use default
  const getAvatarUrl = () => {
    if (comment.authorAvatar) return comment.authorAvatar;
    // Default avatar based on first letter
    const firstLetter = (comment.authorName || "A")[0].toUpperCase();
    return `https://ui-avatars.com/api/?name=${encodeURIComponent(firstLetter)}&background=3b82f6&color=fff&size=128`;
  };

  // Format time ago
  const getTimeAgo = () => {
    if (!comment.createdAt) return "";
    try {
      const date = new Date(comment.createdAt);
      if (isNaN(date.getTime())) return "";
      
      const timeAgo = formatDistanceToNow(date, {
        addSuffix: true,
        locale: vi,
      });
      
      // If less than 1 minute, show "vừa xong"
      if (timeAgo.includes("vài giây") || timeAgo.includes("giây") || (timeAgo.includes("phút") && timeAgo.includes("1"))) {
        return "vừa xong";
      }
      
      return timeAgo;
    } catch {
      return "";
    }
  };

  return (
    <div className={`flex gap-3 mb-4 ${comment.parentCommentId ? "ml-12" : ""}`}>
      {/* Avatar */}
      <div className="flex-shrink-0">
        <img
          src={getAvatarUrl()}
          alt={comment.authorName || "User"}
          className="w-10 h-10 rounded-full object-cover ring-2 ring-gray-200 dark:ring-gray-700"
        />
      </div>

      {/* Comment Content */}
      <div className="flex-1 min-w-0">
        <div className="bg-gray-100 dark:bg-gray-800 rounded-2xl px-4 py-2 mb-2">
          <div className="flex items-center gap-2 mb-1">
            <span className="font-semibold text-gray-900 dark:text-white text-sm">
              {comment.authorName || "Anonymous"}
            </span>
            <span className="text-xs text-gray-500 dark:text-gray-400">
              {getTimeAgo()}
            </span>
          </div>
          <p className="text-gray-800 dark:text-gray-200 text-sm whitespace-pre-wrap break-words">
            {comment.commentText}
          </p>
        </div>

        {/* Actions */}
        <div className="flex items-center gap-4 ml-2 mb-2">
          <button
            onClick={() => setShowReplyForm(!showReplyForm)}
            className="text-xs font-semibold text-gray-600 hover:text-blue-600 dark:text-gray-400 dark:hover:text-blue-400 transition-colors"
          >
            {showReplyForm ? "Hủy" : "Trả lời"}
          </button>
          {canDelete && onDelete && (
            <button
              onClick={() => onDelete(comment.commentId)}
              className="text-xs font-semibold text-red-600 hover:text-red-800 dark:text-red-400 dark:hover:text-red-300 transition-colors"
            >
              Xóa
            </button>
          )}
        </div>

        {/* Reply Form */}
        {showReplyForm && (
          <form onSubmit={handleReply} className="mb-4">
            <div className="flex gap-3">
              <div className="flex-shrink-0">
                <img
                  src={getAvatarUrl()}
                  alt="You"
                  className="w-8 h-8 rounded-full object-cover ring-2 ring-gray-200 dark:ring-gray-700"
                />
              </div>
              <div className="flex-1">
                <textarea
                  value={replyText}
                  onChange={(e) => setReplyText(e.target.value)}
                  placeholder="Viết phản hồi..."
                  className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white text-sm resize-none"
                  rows="2"
                  autoFocus
                />
                <div className="flex gap-2 mt-2">
                  <button
                    type="submit"
                    disabled={isSubmitting || !replyText.trim()}
                    className="px-4 py-1.5 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                  >
                    {isSubmitting ? "Đang gửi..." : "Gửi"}
                  </button>
                  <button
                    type="button"
                    onClick={() => {
                      setShowReplyForm(false);
                      setReplyText("");
                    }}
                    className="px-4 py-1.5 bg-gray-200 dark:bg-gray-700 text-gray-700 dark:text-gray-300 text-sm rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors"
                  >
                    Hủy
                  </button>
                </div>
              </div>
            </div>
          </form>
        )}

        {/* Replies */}
        {comment.replies && comment.replies.length > 0 && (
          <div className="mt-2">
            {comment.replies.map((reply) => (
              <BlogComment
                key={reply.commentId}
                comment={reply}
                blogId={blogId}
                onReply={onReply}
                onDelete={onDelete}
                currentUserId={currentUserId}
                currentUserRole={currentUserRole}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default BlogComment;

