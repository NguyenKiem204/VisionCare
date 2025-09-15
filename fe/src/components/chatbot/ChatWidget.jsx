import React from "react";
import { MessageCircle, X } from "lucide-react";
import useChat from "../../hooks/useChat";
import ChatWindow from "./ChatWindow";

const ChatWidget = () => {
  const chat = useChat();

  return (
    <>
      <ChatWindow chat={chat} />
      <button
        onClick={() => (chat.isOpen ? chat.close() : chat.open())}
        className="fixed bottom-5 right-5 z-40 w-14 h-14 rounded-full bg-gradient-to-br from-blue-500 to-emerald-500 text-white shadow-xl flex items-center justify-center hover:scale-105 transition-transform"
        aria-label="Chatbot"
      >
        {chat.isOpen ? (
          <X className="w-6 h-6" />
        ) : (
          <MessageCircle className="w-7 h-7" />
        )}
      </button>
    </>
  );
};

export default ChatWidget;
