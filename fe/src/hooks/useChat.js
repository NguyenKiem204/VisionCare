import { useRef, useState } from "react";
import { mockApi } from "../utils/api";

const systemGreeting = {
  role: "assistant",
  text: "Xin chào! Tôi là VisionBot 👋\nTôi có thể giúp bạn đặt lịch, tư vấn dịch vụ, giờ làm việc, địa chỉ...",
  ts: Date.now(),
};

export const useChat = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [isTyping, setIsTyping] = useState(false);
  const [messages, setMessages] = useState([systemGreeting]);
  const inputRef = useRef(null);

  const open = () => setIsOpen(true);
  const close = () => setIsOpen(false);

  const pushAssistant = (payload) => {
    const msg =
      typeof payload === "string"
        ? { role: "assistant", text: payload, ts: Date.now() }
        : { role: "assistant", ts: Date.now(), ...payload };
    setMessages((m) => [...m, msg]);
  };

  const send = async (payload) => {
    // payload can be string or action object
    if (typeof payload === "object" && payload?.type === "book_prompt") {
      pushAssistant({
        type: "cta",
        text: "Bạn muốn đặt lịch ngay bây giờ chứ?",
        cta: { label: "Đặt lịch ngay", href: "/booking" },
      });
      return;
    }

    const text = typeof payload === "string" ? payload : payload?.text;
    if (!text?.trim()) return;
    setMessages((m) => [
      ...m,
      { role: "user", text: text.trim(), ts: Date.now() },
    ]);
    setIsTyping(true);

    try {
      // Call real API
      const response = await mockApi.sendChatMessage(text);

      if (response.success) {
        setMessages((m) => [...m, { ...response.data, ts: Date.now() }]);
      } else {
        setMessages((m) => [
          ...m,
          {
            role: "assistant",
            text: "Xin lỗi, tôi gặp sự cố kỹ thuật. Vui lòng thử lại sau hoặc liên hệ hotline 028 1234 5678.",
            ts: Date.now(),
          },
        ]);
      }
    } catch (error) {
      console.error("Chat API error:", error);
      setMessages((m) => [
        ...m,
        {
          role: "assistant",
          text: "Xin lỗi, tôi gặp sự cố kỹ thuật. Vui lòng thử lại sau hoặc liên hệ hotline 028 1234 5678.",
          ts: Date.now(),
        },
      ]);
    } finally {
      setIsTyping(false);
    }
  };

  const generateReply = (text) => {
    const lower = text.toLowerCase();
    if (/(đặt lịch|booking|lịch)/.test(lower)) {
      return {
        role: "assistant",
        type: "cta",
        text: "Bạn có thể bấm nút bên dưới để mở trang đặt lịch.",
        cta: { label: "Đặt lịch ngay", href: "/booking" },
        ts: Date.now(),
      };
    }
    if (/(giờ mở|giờ làm|mấy giờ)/.test(lower)) {
      return {
        role: "assistant",
        text: "Giờ làm việc: T2–T6 8:00–17:30, T7 8:00–12:00, CN nghỉ (trừ cấp cứu).",
        ts: Date.now(),
      };
    }
    if (/(địa chỉ|ở đâu|map)/.test(lower)) {
      return {
        role: "assistant",
        text: "Địa chỉ: 123 Nguyễn Huệ, Q1, TP.HCM. Tôi có thể gửi bạn chỉ đường Google Maps.",
        ts: Date.now(),
      };
    }
    if (/(giá|bảng giá|lasik)/.test(lower)) {
      return {
        role: "assistant",
        text: "Tham khảo giá: Khám tổng quát 200–300k, Chụp OCT 400k, Phẫu thuật Lasik 25–45 triệu/2 mắt (tùy chỉ định).",
        ts: Date.now(),
      };
    }
    return {
      role: "assistant",
      text: "Mình đã ghi nhận câu hỏi. Bạn có thể mô tả chi tiết hơn để mình hỗ trợ chính xác ạ?",
      ts: Date.now(),
    };
  };

  return { isOpen, open, close, isTyping, messages, send, inputRef };
};

export default useChat;
