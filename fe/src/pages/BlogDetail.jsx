import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getBlogBySlug, incrementViewCount } from "../services/blogService";
import { getComments, createComment, deleteComment } from "../services/commentService";
import { commentSignalRService } from "../services/commentSignalRService";
import { useAuth } from "../contexts/AuthContext";
import BlogComment from "../components/blog/BlogComment";  
import { format } from "date-fns";
import toast from "react-hot-toast";

export default function BlogDetail() {
  const { slug } = useParams();
  const { user, isAuthenticated } = useAuth();
  const [blog, setBlog] = useState(null);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [commentText, setCommentText] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const currentUserId = user?.accountId || user?.id || null;
  const currentUserRole = user?.roleName || user?.role || null;

  useEffect(() => {
    if (slug) {
      loadBlog();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [slug]);

  useEffect(() => {
    if (blog?.blogId) {
      loadComments();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [blog?.blogId]);

  // Setup SignalR for real-time comments - join hub immediately when blog is loaded
  useEffect(() => {
    if (!blog?.blogId) return;

    let unsubscribe = null;
    let isMounted = true;

    const setupSignalR = async () => {
      try {
        // Start connection if not already connected
        await commentSignalRService.start();
        
        // Join blog group to receive real-time updates
        await commentSignalRService.joinBlogGroup(blog.blogId);
        
        // Listen for new comments
        unsubscribe = commentSignalRService.on("NewComment", (newComment) => {
          if (!isMounted) return;
          
          // Helper function to check if comment exists in tree
          const commentExists = (comments, commentId) => {
            for (const comment of comments) {
              if (comment.commentId === commentId) return true;
              if (comment.replies && comment.replies.length > 0) {
                if (commentExists(comment.replies, commentId)) return true;
              }
            }
            return false;
          };
          
          // Helper function to add comment to tree
          const addCommentToTree = (comments, newComment) => {
            // If it's a reply, find parent and add to replies
            if (newComment.parentCommentId) {
              return comments.map(comment => {
                // Check if this is the parent
                if (comment.commentId === newComment.parentCommentId) {
                  // Check if reply already exists
                  const replyExists = comment.replies?.some(r => r.commentId === newComment.commentId);
                  if (replyExists) return comment;
                  
                  return {
                    ...comment,
                    replies: [...(comment.replies || []), newComment]
                  };
                }
                // Check nested replies
                if (comment.replies && comment.replies.length > 0) {
                  const updatedReplies = addCommentToTree(comment.replies, newComment);
                  if (updatedReplies !== comment.replies) {
                    return { ...comment, replies: updatedReplies };
                  }
                }
                return comment;
              });
            }
            
            // Top-level comment - check if exists
            const exists = comments.some(c => c.commentId === newComment.commentId);
            if (exists) return comments;
            
            return [...comments, newComment];
          };
          
          // Add new comment to the list
          setComments((prev) => {
            // Check if comment already exists in entire tree
            if (commentExists(prev, newComment.commentId)) {
              return prev;
            }
            
            return addCommentToTree(prev, newComment);
          });
          
          // Update comment count (only if not already counted)
          setBlog((prev) => {
            const currentCount = prev?.commentCount || 0;
            // Only increment if this is a new comment (not a duplicate)
            return {
              ...prev,
              commentCount: currentCount + 1
            };
          });
        });
      } catch {
        // SignalR setup failed, comments will still work via polling
      }
    };

    setupSignalR();

    // Cleanup: leave group and unsubscribe when component unmounts or blog changes
    return () => {
      isMounted = false;
      if (unsubscribe) {
        unsubscribe();
      }
      if (blog?.blogId) {
        commentSignalRService.leaveBlogGroup(blog.blogId).catch(() => {});
      }
    };
  }, [blog?.blogId]);

  const loadBlog = async () => {
    setLoading(true);
    try {
      const response = await getBlogBySlug(slug);
      if (response.success && response.data) {
        setBlog(response.data);
        // Increment view count
        try {
          await incrementViewCount(response.data.blogId);
        } catch (error) {
          console.error("Error incrementing view count:", error);
        }
      }
    } catch (error) {
      console.error("Error loading blog:", error);
      toast.error("Không thể tải blog");
    } finally {
      setLoading(false);
    }
  };

  const loadComments = async () => {
    if (!blog?.blogId) return;
    try {
      const response = await getComments(blog.blogId);
      if (response.success && response.data) {
        setComments(response.data || []);
      }
    } catch (error) {
      console.error("Error loading comments:", error);
    }
  };

  const handleSubmitComment = async (e) => {
    e.preventDefault();
    if (!commentText.trim()) return;

    setIsSubmitting(true);
    try {
      await createComment(blog.blogId, {
        commentText: commentText.trim(),
      });
      
      // Comment will be added via SignalR automatically
      // Don't add to state here to avoid duplicates
      
      setCommentText("");
      toast.success("Bình luận đã được gửi");
    } catch (error) {
      console.error("Error submitting comment:", error);
      toast.error("Không thể gửi bình luận");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDeleteComment = async (commentId) => {
    if (!window.confirm("Bạn có chắc chắn muốn xóa bình luận này?")) return;

    try {
      await deleteComment(blog.blogId, commentId);
      toast.success("Xóa bình luận thành công");
      loadComments();
    } catch (error) {
      console.error("Error deleting comment:", error);
      toast.error("Không thể xóa bình luận");
    }
  };

  const formatDate = (date) => {
    if (!date) return "";
    try {
      return format(new Date(date), "dd/MM/yyyy HH:mm");
    } catch {
      return "";
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!blog) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-600 dark:text-gray-400">Không tìm thấy blog</p>
        <Link to="/blogs" className="text-blue-600 hover:text-blue-800 dark:text-blue-400 mt-4 inline-block">
          Quay lại danh sách blog
        </Link>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-4xl mx-auto px-4 py-8">
        {/* Back Button */}
        <Link
          to="/blogs"
          className="inline-flex items-center text-blue-600 hover:text-blue-800 dark:text-blue-400 mb-6"
        >
          ← Quay lại danh sách blog
        </Link>

        {/* Blog Content */}
        <article className="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden mb-8">
          {blog.featuredImage && (
            <div className="h-64 md:h-96 overflow-hidden">
              <img
                src={blog.featuredImage}
                alt={blog.title}
                className="w-full h-full object-cover"
              />
            </div>
          )}

          <div className="p-6 md:p-8">
            <h1 className="text-3xl md:text-4xl font-bold text-gray-900 dark:text-white mb-4">
              {blog.title}
            </h1>

            <div className="flex items-center gap-4 text-sm text-gray-600 dark:text-gray-400 mb-6">
              {blog.authorName && (
                <span>Tác giả: {blog.authorName}</span>
              )}
              {blog.publishedAt && (
                <span>• {formatDate(blog.publishedAt)}</span>
              )}
              <span>• 👁️ {blog.viewCount || 0} lượt xem</span>
              <span>• 💬 {blog.commentCount || 0} bình luận</span>
            </div>

            {blog.excerpt && (
              <div className="text-lg text-gray-700 dark:text-gray-300 mb-6 italic border-l-4 border-blue-500 pl-4">
                {blog.excerpt}
              </div>
            )}

            <div
              className="prose prose-lg max-w-none dark:prose-invert"
              dangerouslySetInnerHTML={{ __html: blog.content }}
            />
          </div>
        </article>

        {/* Comments Section */}
        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 md:p-8">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-6">
            Bình luận ({blog.commentCount || 0})
          </h2>

          {/* Comment Form */}
          {isAuthenticated && currentUserId ? (
            <form onSubmit={handleSubmitComment} className="mb-8">
              <div className="flex gap-3">
                <div className="flex-shrink-0">
                  <img
                    src={(() => {
                      if (user?.avatar) return user.avatar;
                      const firstLetter = (user?.fullName || user?.username || "U")[0].toUpperCase();
                      return `https://ui-avatars.com/api/?name=${encodeURIComponent(firstLetter)}&background=3b82f6&color=fff&size=128`;
                    })()}
                    alt="You"
                    className="w-10 h-10 rounded-full object-cover ring-2 ring-gray-200 dark:ring-gray-700"
                  />
                </div>
                <div className="flex-1">
                  <textarea
                    value={commentText}
                    onChange={(e) => setCommentText(e.target.value)}
                    placeholder="Viết bình luận của bạn..."
                    className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white mb-3 resize-none"
                    rows="3"
                  />
                  <button
                    type="submit"
                    disabled={isSubmitting || !commentText.trim()}
                    className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors text-sm font-semibold"
                  >
                    {isSubmitting ? "Đang gửi..." : "Gửi bình luận"}
                  </button>
                </div>
              </div>
            </form>
          ) : (
            <div className="mb-8 p-4 bg-gray-100 dark:bg-gray-700 rounded-lg">
              <p className="text-gray-700 dark:text-gray-300">
                Vui lòng <Link to="/login" className="text-blue-600 hover:text-blue-800 dark:text-blue-400">đăng nhập</Link> để bình luận
              </p>
            </div>
          )}

          {/* Comments List */}
          <div className="space-y-4">
            {comments.length === 0 ? (
              <p className="text-gray-600 dark:text-gray-400 text-center py-8">
                Chưa có bình luận nào. Hãy là người đầu tiên bình luận!
              </p>
            ) : (
              comments.map((comment) => (
                <BlogComment
                  key={comment.commentId}
                  comment={comment}
                  blogId={blog.blogId}
                  onReply={loadComments}
                  onDelete={handleDeleteComment}
                  currentUserId={currentUserId}
                  currentUserRole={currentUserRole}
                />
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

