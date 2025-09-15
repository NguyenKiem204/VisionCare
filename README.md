\# 🏥 Vision Care Project

Dự án quản lý phòng khám mắt, bao gồm \*\*Frontend (React + Vite + Tailwind)\*\* và \*\*Backend (ASP.NET Core Web API)\*\*.  

Mục tiêu: xây dựng hệ thống đặt lịch, quản lý dịch vụ, và hỗ trợ bệnh nhân dễ dàng tiếp cận dịch vụ y tế.



---



\## 📂 Cấu trúc thư mục



```



my-clinic-project/

├── frontend/         # React Vite + Tailwind

├── backend/          # ASP.NET Core Web API

├── docs/             # Tài liệu dự án (API docs, ERD, thiết kế)

├── docker/           # File docker-compose, Dockerfile

├── .gitignore

└── README.md



````



---



\## 🚀 Công nghệ sử dụng



\### Frontend

\- ⚛️ React 19 + Vite

\- 🎨 TailwindCSS

\- 🔄 Axios (kết nối API)

\- 🛠️ React Router DOM



\### Backend

\- 🌐 ASP.NET Core 8 Web API

\- 🗄️ Entity Framework Core

\- ✅ FluentValidation

\- 📦 AutoMapper

\- 🔒 JWT Authentication



\### Khác

\- 🐳 Docker / Docker Compose

\- 📄 Swagger (API Docs)



---



\## ⚙️ Hướng dẫn cài đặt



\### 1. Clone repo

```bash

git clone https://github.com/NguyenKiem204/VisionCare.git

cd VisionCare

````



\### 2. Backend (ASP.NET Core API)



```bash

cd backend

dotnet restore

dotnet run

```



API mặc định chạy tại: `http://localhost:5000`



\### 3. Frontend (React Vite)



```bash

cd frontend

npm install

npm run dev

```



Frontend chạy tại: `http://localhost:5173`



---



\## 📖 API Docs



Khi chạy backend, có thể truy cập Swagger tại:

👉 `http://localhost:8080/swagger`



---



\## 🐳 Chạy bằng Docker (tuỳ chọn)



```bash

docker-compose up --build

```



---



\## 👨‍💻 Thành viên phát triển



\* \*\*Nguyễn Kiểm\*\* – Fullstack Developer



---



\## 📌 Ghi chú



\* File `.env` chứa các biến môi trường (DB connection, JWT secret) không được commit.

\* Sử dụng `.env.example` để tham khảo cấu hình.



---



\## 🗓️ Roadmap (dự kiến)



\* \[x] Khởi tạo monorepo (FE + BE)

\* \[x] Thiết kế DB + API cơ bản

\* \[ ] Đặt lịch hẹn online

\* \[ ] Tích hợp AI hỗ trợ tư vấn (chatbot, gợi ý dịch vụ)

\* \[ ] Triển khai CI/CD lên Azure / AWS



---



```



---



👉 File này vừa đủ \*\*technical\*\* (hướng dẫn setup), vừa có \*\*giới thiệu\*\* (nhìn repo chuyên nghiệp hơn).  



Bạn có muốn mình viết thêm luôn file \*\*`.env.example` cho cả FE và BE\*\* để đưa vào repo cho gọn không?

```



