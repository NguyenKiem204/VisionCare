# Cấu Trúc Routes VisionCare

## 🗂️ **Route Organization**

### **1. Public Routes** (`/`)

Không cần authentication, ai cũng có thể truy cập.

```
/ → Home
/services → Services
/equipment → Equipment
/contact → Contact
```

**Layout**: `PublicLayout` (có Header, Footer, ChatWidget)

### **2. Protected Routes** (`/`)

Cần authentication, nhưng không cần role cụ thể.

```
/booking → Booking (cần đăng nhập)
/profile → Profile (cần đăng nhập)
```

**Layout**: `PublicLayout` (có Header, Footer, ChatWidget)

### **3. Admin Routes** (`/admin`)

Cần authentication + role "admin".

```
/admin/login → AdminLogin (không cần auth)
/admin → AdminDashboard
/admin/bookings → AdminBookings
/admin/patients → AdminPatients
/admin/services → AdminServices
/admin/settings → AdminSettings
```

**Layout**: `AdminLayout` (có sidebar, top bar)

### **4. Doctor Routes** (`/doctor`)

Cần authentication + role "doctor".

```
/doctor → DoctorDashboard
/doctor/patients → DoctorPatients
/doctor/schedule → DoctorSchedule
```

**Layout**: `DoctorLayout` (có sidebar, top bar)

### **5. Staff Routes** (`/staff`)

Cần authentication + role "staff".

```
/staff → StaffDashboard
/staff/bookings → StaffBookings
/staff/patients → StaffPatients
```

**Layout**: `StaffLayout` (có sidebar, top bar)

## 🎨 **Layout System**

### **PublicLayout**

- Header với navigation
- Footer
- ChatWidget
- Dành cho: Home, Services, Equipment, Contact, Booking, Profile

### **AdminLayout**

- Sidebar navigation (Admin)
- Top bar với user info
- Màu chủ đạo: Blue
- Dành cho: Tất cả admin routes

### **DoctorLayout**

- Sidebar navigation (Doctor)
- Top bar với user info
- Màu chủ đạo: Green
- Dành cho: Tất cả doctor routes

### **StaffLayout**

- Sidebar navigation (Staff)
- Top bar với user info
- Màu chủ đạo: Orange
- Dành cho: Tất cả staff routes

## 🔐 **Authentication & Authorization**

### **Public Access**

- Không cần đăng nhập
- Routes: `/`, `/services`, `/equipment`, `/contact`

### **Authenticated Access**

- Cần đăng nhập (bất kỳ role nào)
- Routes: `/booking`, `/profile`

### **Role-Based Access**

- **Admin**: `/admin/*` - Chỉ user có role "admin"
- **Doctor**: `/doctor/*` - Chỉ user có role "doctor"
- **Staff**: `/staff/*` - Chỉ user có role "staff"

## 📁 **File Structure**

```
src/
├── routes/
│   ├── PublicRoutes.jsx      # Public routes
│   ├── ProtectedRoutes.jsx   # Protected routes
│   ├── AdminRoutes.jsx       # Admin routes
│   ├── DoctorRoutes.jsx      # Doctor routes
│   └── StaffRoutes.jsx       # Staff routes
├── layouts/
│   ├── PublicLayout.jsx      # Layout cho public
│   ├── AdminLayout.jsx        # Layout cho admin
│   ├── DoctorLayout.jsx       # Layout cho doctor
│   └── StaffLayout.jsx        # Layout cho staff
├── pages/
│   ├── Home.jsx
│   ├── Services.jsx
│   ├── Equipment.jsx
│   ├── Contact.jsx
│   ├── Booking.jsx
│   ├── Profile.jsx
│   ├── admin/
│   │   ├── Login.jsx
│   │   ├── Dashboard.jsx
│   │   ├── Bookings.jsx
│   │   ├── Patients.jsx
│   │   ├── Services.jsx
│   │   └── Settings.jsx
│   ├── doctor/
│   │   ├── Dashboard.jsx
│   │   ├── Patients.jsx
│   │   └── Schedule.jsx
│   └── staff/
│       ├── Dashboard.jsx
│       ├── Bookings.jsx
│       └── Patients.jsx
└── App.jsx
```

## 🚀 **Navigation Flow**

### **User Journey**

1. **Guest User**: Truy cập public routes → Đăng nhập → Truy cập protected routes
2. **Admin User**: Đăng nhập → Thấy admin menu → Truy cập admin routes
3. **Doctor User**: Đăng nhập → Thấy doctor menu → Truy cập doctor routes
4. **Staff User**: Đăng nhập → Thấy staff menu → Truy cập staff routes

### **Menu Display Logic**

```javascript
// Header menu
{
  hasRole && hasRole("admin") && <AdminMenu />;
}
{
  hasRole && hasRole("doctor") && <DoctorMenu />;
}
{
  hasRole && hasRole("staff") && <StaffMenu />;
}
```

## 🔧 **Route Protection**

### **ProtectedRoute Component**

```javascript
<ProtectedRoute requiredRole="admin">
  <AdminComponent />
</ProtectedRoute>
```

### **Auto Redirects**

- Chưa đăng nhập → Redirect to login
- Sai role → Redirect to unauthorized
- Token expired → Auto refresh hoặc redirect to login

## 📱 **Responsive Design**

### **Mobile Support**

- Tất cả layouts đều responsive
- Hamburger menu cho mobile
- Touch-friendly navigation

### **Breakpoints**

- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

## 🎯 **Benefits**

### **1. Clear Separation**

- Public vs Protected vs Role-based routes
- Easy to understand and maintain

### **2. Scalable**

- Dễ dàng thêm routes mới
- Dễ dàng thêm roles mới

### **3. Secure**

- Role-based access control
- Protected routes với authentication

### **4. User Experience**

- Layout phù hợp với từng role
- Navigation rõ ràng và intuitive

## 🔄 **Future Enhancements**

### **Planned Features**

- Patient routes (`/patient/*`)
- Guest routes (`/guest/*`)
- API routes (`/api/*`)
- Webhook routes (`/webhook/*`)

### **Role Extensions**

- Super Admin
- Manager
- Receptionist
- Technician
