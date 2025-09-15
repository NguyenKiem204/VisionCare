import React from "react";
import { Send, Loader2 } from "lucide-react";
import Button from "../common/Button";

const Avatar = () => (
  <div className="w-9 h-9 rounded-full bg-white/20 border border-white/30 flex items-center justify-center mr-2">
    <span className="text-white font-semibold">VB</span>
  </div>
);

const QuickActions = ({ onAction }) => (
  <div className="flex flex-wrap gap-2 p-3 border-t bg-gray-50">
    <Button
      size="sm"
      variant="secondary"
      onClick={() => onAction({ type: "book_prompt" })}
    >
      🗓️ Đặt lịch khám
    </Button>
    <Button
      size="sm"
      variant="secondary"
      onClick={() => onAction("Bảng giá dịch vụ")}
    >
      💰 Bảng giá
    </Button>
    <Button
      size="sm"
      variant="secondary"
      onClick={() => onAction("Địa chỉ phòng khám")}
    >
      📍 Địa chỉ
    </Button>
    <Button
      size="sm"
      variant="secondary"
      onClick={() => onAction("Giờ mở cửa")}
    >
      ⏰ Giờ mở cửa
    </Button>
    <Button
      size="sm"
      variant="secondary"
      onClick={() => onAction("Tư vấn Lasik")}
    >
      🔍 Tư vấn Lasik
    </Button>
  </div>
);

const AssistantBubble = ({ message }) => {
  const isCta = message.type === "cta" && message.cta;
  return (
    <div className="bg-gray-100 text-gray-800 px-3 py-2 rounded-2xl max-w-[80%]">
      <div className="whitespace-pre-line">{message.text}</div>
      {isCta && (
        <div className="mt-2">
          <a
            href={message.cta.href}
            className="inline-flex items-center px-3 py-1.5 rounded-full bg-blue-600 text-white text-sm hover:bg-blue-700"
          >
            {message.cta.label}
          </a>
        </div>
      )}
    </div>
  );
};

const ChatWindow = ({ chat }) => {
  if (!chat.isOpen) return null;

  const listRef = React.useRef(null);
  const [showSuggestions, setShowSuggestions] = React.useState(true);

  React.useEffect(() => {
    const saved = localStorage.getItem("vc_chat_suggestions");
    if (saved === "hidden") setShowSuggestions(false);
  }, []);

  React.useEffect(() => {
    const el = listRef.current;
    if (!el) return;
    el.scrollTo({ top: el.scrollHeight, behavior: "smooth" });
  }, [chat.messages, chat.isTyping, showSuggestions]);

  const toggleSuggestions = () => {
    setShowSuggestions((v) => {
      const next = !v;
      localStorage.setItem("vc_chat_suggestions", next ? "shown" : "hidden");
      return next;
    });
  };

  return (
    <div className="fixed bottom-24 right-5 z-50 w-[360px] max-w-[90vw] bg-white rounded-2xl shadow-2xl overflow-hidden">
      {/* Header */}
      <div className="flex items-center justify-between px-4 py-3 bg-gradient-to-r from-blue-600 to-emerald-600 text-white">
        <div className="flex items-center">
          <Avatar />
          <div>
            <p className="font-semibold">VisionBot - Trợ lý ảo</p>
            <p className="text-xs text-white/80">
              Đang online • Phản hồi trong 1 phút
            </p>
          </div>
        </div>
        <button
          onClick={toggleSuggestions}
          className="text-xs px-2 py-1 rounded-full bg-white/15 hover:bg-white/25 border border-white/20"
        >
          {showSuggestions ? "Ẩn gợi ý" : "Hiện gợi ý"}
        </button>
      </div>

      {/* Messages */}
      <div ref={listRef} className="h-80 overflow-y-auto p-3 space-y-3">
        {chat.messages.map((m, idx) => (
          <div
            key={idx}
            className={`flex ${
              m.role === "assistant" ? "justify-start" : "justify-end"
            }`}
          >
            {m.role === "assistant" ? (
              <AssistantBubble message={m} />
            ) : (
              <div className="bg-blue-600 text-white px-3 py-2 rounded-2xl max-w-[80%]">
                {m.text}
              </div>
            )}
          </div>
        ))}
        {chat.isTyping && (
          <div className="flex justify-start">
            <div className="bg-gray-100 text-gray-800 px-3 py-2 rounded-2xl inline-flex items-center gap-2">
              <Loader2 className="w-4 h-4 animate-spin" />
              <span>Đang nhập...</span>
            </div>
          </div>
        )}
      </div>

      {showSuggestions && <QuickActions onAction={chat.send} />}

      {/* Input */}
      <form
        onSubmit={(e) => {
          e.preventDefault();
          chat.send(chat.inputRef.current?.value || "");
          if (chat.inputRef.current) chat.inputRef.current.value = "";
        }}
        className="flex items-center gap-2 p-3 border-t"
      >
        <input
          ref={chat.inputRef}
          className="flex-1 border rounded-full px-4 py-2"
          placeholder="Nhập câu hỏi..."
        />
        <Button
          type="submit"
          variant="primary"
          size="sm"
          className="!rounded-full"
        >
          <Send className="w-4 h-4" />
        </Button>
      </form>
    </div>
  );
};

export default ChatWindow;
