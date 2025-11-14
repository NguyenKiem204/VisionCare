# VisionCare – Clean Architecture & Design Patterns

## Mục tiêu thuyết trình
- Giải thích vì sao dự án áp dụng Clean Architecture và lợi ích đối với VisionCare.
- Minh hoạ luồng nghiệp vụ chính (đặt lịch & thanh toán VNPay) bằng sơ đồ Mermaid.
- Làm rõ các design pattern đang dùng và tác động của chúng.
- Chuẩn bị script thuyết trình giúp khán giả dễ theo dõi.

## Cấu trúc 4 tầng Clean Architecture

```mermaid
flowchart TB
    %% ================================
    %%  LAYERS
    %% ================================
    subgraph WebAPI["🔴 WebAPI Layer"]
        BookingController["BookingController\n- Nhận HTTP request\n- Ánh xạ DTO\n- Gọi Application Service"]
        PaymentController["PaymentController\n- Xử lý VNPay callback\n- Trả response cho FE"]
        SignalRHub["BookingHub (SignalR)\n- Push realtime cho client"]
    end

    subgraph Application["🟡 Application Layer"]
        BookingDto["CreateBookingRequestDto\n- DTO đầu vào"]
        BookingServiceNode["BookingService\n- Thực thi use case đặt lịch\n- Gọi repository/interface"]
        ScheduleJob["ScheduleGenerationJob\n- Hangfire job gọi ScheduleGenerationService"]
        IAppointmentRepo["IAppointmentRepository\n- Hợp đồng truy xuất lịch khám"]
        IVNPaySvc["IVNPayService\n- Interface thanh toán"]
    end

    subgraph Domain["🟢 Domain Layer"]
        AppointmentEntity["Appointment Entity\n- Trạng thái & luật nghiệp vụ"]
        DoctorEntity["Doctor Entity"]
        CodeGenerator["AppointmentCodeGenerator\n- Domain Service tạo mã VC-YYYYMMDD-XXXXXX"]
    end

    subgraph Infrastructure["🔵 Infrastructure Layer"]
        DbContext["VisionCareDbContext (EF Core)"]
        AppointmentRepoImpl["AppointmentRepository\n- Triển khai IAppointmentRepository\n- Lưu/đọc DB"]
        VNPaySvcImpl["VNPayService\n- Triển khai IVNPayService\n- Ký & verify VNPay"]
        CheckoutRepo["CheckoutRepository\n- Cập nhật trạng thái thanh toán"]
        ExternalSystems["PostgreSQL / VNPay / Redis / Hangfire Storage"]
    end

    %% ================================
    %%  RELATIONSHIPS
    %% ================================
    BookingController --> BookingDto
    BookingDto --> BookingServiceNode
    BookingServiceNode --> AppointmentEntity
    BookingServiceNode --> CodeGenerator
    BookingServiceNode --> IAppointmentRepo
    BookingServiceNode --> IVNPaySvc
    SignalRHub --> BookingServiceNode
    PaymentController --> IVNPaySvc
    ScheduleJob --> BookingServiceNode

    IAppointmentRepo -.-> AppointmentRepoImpl
    AppointmentRepoImpl --> DbContext
    AppointmentRepoImpl --> ExternalSystems

    IVNPaySvc -.-> VNPaySvcImpl
    VNPaySvcImpl --> ExternalSystems
    CheckoutRepo --> ExternalSystems
    BookingServiceNode --> CheckoutRepo

    AppointmentEntity --> DoctorEntity

    %% ================================
    %%  STYLING
    %% ================================
    classDef domain fill:#a7f3d0,stroke:#16a34a,color:#064e3b
    classDef app fill:#fef08a,stroke:#ca8a04,color:#78350f
    classDef infra fill:#bfdbfe,stroke:#1d4ed8,color:#1e3a8a
    classDef api fill:#fecaca,stroke:#b91c1c,color:#7f1d1d

    class WebAPI,BookingController,PaymentController,SignalRHub api
    class Application,BookingServiceNode,IAppointmentRepo,IVNPaySvc,BookingDto,ScheduleJob app
    class Domain,AppointmentEntity,DoctorEntity,CodeGenerator domain
    class Infrastructure,AppointmentRepoImpl,VNPaySvcImpl,DbContext,ExternalSystems,CheckoutRepo infra
```

- **Domain Layer (`be/src/Domain`)**  
  Chứa entity thuần C#, value object, domain service (`AppointmentCodeGenerator`). Không phụ thuộc framework hay tầng ngoài.
- **Application Layer (`be/src/Application`)**  
  Định nghĩa use case qua service (`BookingService`, `ScheduleGenerationService`), DTO, validator và **các interface dùng chung** (ví dụ `IAppointmentRepository`, `IVNPayService`) để tầng Infrastructure implement thông qua DI.
- **Infrastructure Layer (`be/src/Infrastructure`)**  
  Thực thi interface từ Application: repository EF Core (`AppointmentRepository`), mapper (`AppointmentMapper`), adapter thanh toán (`VNPayService`), background integration (Hangfire, Redis).
- **WebAPI Layer (`be/src/WebAPI`)**  
  Expose REST & SignalR: controller (`BookingController`), middleware auth, cấu hình DI (`Program.cs`) kết nối bên ngoài. Đây là gateway giữa client và hệ thống.

> React FE vẫn tồn tại nhưng nằm ngoài sơ đồ Clean Architecture backend; FE tương tác với WebAPI qua HTTP/SignalR.

## Nguyên lý trọng tâm
- **Dependency Rule**: `WebAPI` biết `Application`, nhưng `Application` chỉ thấy `Domain` & interface; `Infrastructure` implement interface từ `Application`.
- **Use Case First**: Business flow được gom trong service lớp Application (`BookingService`, `ScheduleGenerationService`).
- **Testability**: Các repository & service được mock thông qua interface => dễ unit test (thư mục `tests/`).
- **Replaceability**: VNPay hoặc storage có thể thay thế bằng dịch vụ khác chỉ cần implement cùng interface.

## Ví dụ feature: Đặt lịch khám với VNPay

### 1. Dòng chảy qua 4 tầng

```mermaid
sequenceDiagram
    participant FE as Frontend
    participant API as WebAPI Layer\nBookingController
    participant APP as Application Layer\nBookingService
    participant INF as Infrastructure Layer\nAppointmentRepository & VNPayService
    participant DOM as Domain Layer\nAppointment Entity

    FE->>API: POST /api/booking
    API->>APP: HandleBookingAsync(dto)
    APP->>DOM: Tạo đối tượng Appointment
    APP->>INF: IAppointmentRepository.CheckConflict()
    INF->>DOM: Trả về lịch Domain
    APP->>INF: IAppointmentRepository.AddAsync()
    INF->>DOM: Lưu xuống DB (EF Model)
    APP->>INF: IVNPayService.CreatePaymentUrlAsync()
    INF-->>APP: URL thanh toán VNPay
    APP-->>API: BookingResultDto (AppointmentCode, PaymentUrl)
    API-->>FE: HTTP 200 response
```

### 2. Callback thanh toán (WebAPI ↔ Application ↔ Infrastructure)

```mermaid
sequenceDiagram
    participant VNPay as VNPay Gateway
    participant API as WebAPI Layer\nPaymentController
    participant APP as Application Layer\nPayment Orchestrator
    participant INF as Infrastructure Layer\nVNPayService & CheckoutRepository

    VNPay-->>API: GET /api/payment/vnpay/callback?vnp_...
    API->>APP: ProcessVNPayCallback(query)
    APP->>INF: IVNPayService.VerifyCallbackAsync()
    INF-->>APP: Kết quả (IsSuccess, OrderId, Amount)
    APP->>INF: ICheckoutRepository.UpdateStatus()
    API-->>VNPay: 200 OK
```

```mermaid
sequenceDiagram
    participant User as Khách hàng (FE)
    participant FE as React Booking Page
    participant API as BookingController
    participant AppSvc as BookingService (Application)
    participant Repo as Repositories (Infrastructure)
    participant Dom as Domain Entities

    User->>FE: Chọn bác sĩ, dịch vụ, thời gian
    FE->>API: POST /api/booking (payload)
    API->>AppSvc: HandleBookingAsync(requestDto)
    AppSvc->>Repo: CheckAvailability(doctorId, datetime)
    Repo->>Dom: Trả về Schedule/Appointment Domain Model
    AppSvc->>Repo: CreateAppointment(...)
    Repo->>Dom: Lưu Appointment Domain -> EF Model -> DB
    AppSvc-->>API: BookingResultDto
    API-->>FE: 200 OK (appointment code, payment URL)
    FE-->>User: Hiển thị xác nhận & nút thanh toán
```

### 2. Thanh toán VNPay callback

```mermaid
sequenceDiagram
    participant VNPay as VNPay Gateway
    participant FE as BookingPaymentCallback.jsx
    participant API as PaymentController
    participant PaySvc as VNPayService
    participant Repo as CheckoutRepository

    VNPay-->>FE: Redirect with query params
    FE->>API: GET /api/payment/vnpay/callback?vnp_...
    API->>PaySvc: VerifyCallbackAsync(params)
    PaySvc-->>API: Kết quả (IsSuccess, OrderId, Amount)
    API->>Repo: UpdateCheckoutStatus(OrderId, status)
    API-->>FE: RedirectInstruction/JSON
    FE-->>User: Thông báo thanh toán thành công/thất bại
```

## Design Pattern Quan Trọng Nhất: Repository Pattern

### 🏆 Tại sao Repository Pattern là quan trọng nhất?

**Repository Pattern** là nền tảng của Clean Architecture trong VisionCare vì:

1. **Tách biệt hoàn toàn Business Logic và Data Access**
   - Application layer chỉ biết interface (`IAppointmentRepository`, `IDoctorRepository`...)
   - Không phụ thuộc vào EF Core, SQL, hay bất kỳ ORM nào
   - Business logic thuần túy, dễ đọc và bảo trì

2. **Tuân thủ Dependency Inversion Principle (DIP)**
   - Application định nghĩa contract (interface)
   - Infrastructure implement contract
   - Dependency flow: Application → Interface ← Infrastructure

3. **Testability cực cao**
   - Dễ dàng mock repository trong unit test
   - Test business logic mà không cần database thật
   - Tăng tốc độ test và độ tin cậy

4. **Linh hoạt thay đổi persistence layer**
   - Có thể đổi từ PostgreSQL sang MongoDB, Redis, hay API external
   - Chỉ cần implement lại interface, không sửa Application code

5. **Quy mô lớn trong dự án**
   - **36+ repository interfaces** trong Application layer
   - Mọi service đều phụ thuộc vào repository
   - Pattern được áp dụng nhất quán toàn hệ thống

### 📊 Sơ đồ Repository Pattern trong VisionCare

#### 1. Kiến trúc tổng quan Repository Pattern

```mermaid
flowchart TB
    subgraph Application["🟡 Application Layer"]
        BookingService["BookingService\n- Business Logic\n- Use Cases"]
        IAppointmentRepo["IAppointmentRepository\n(Interface Contract)"]
        IScheduleRepo["IScheduleRepository\n(Interface Contract)"]
        IDoctorRepo["IDoctorRepository\n(Interface Contract)"]
    end

    subgraph Domain["🟢 Domain Layer"]
        Appointment["Appointment Entity\n- Domain Rules\n- Business Logic"]
        Schedule["Schedule Entity"]
        Doctor["Doctor Entity"]
    end

    subgraph Infrastructure["🔵 Infrastructure Layer"]
        AppointmentRepo["AppointmentRepository\n- Implements IAppointmentRepository\n- Uses EF Core"]
        ScheduleRepo["ScheduleRepository\n- Implements IScheduleRepository"]
        DoctorRepo["DoctorRepository\n- Implements IDoctorRepository"]
        DbContext["VisionCareDbContext\n(EF Core)"]
        Mapper["AppointmentMapper\n- Domain ↔ EF Model"]
    end

    subgraph Database["💾 PostgreSQL Database"]
        DB[(Tables: appointments,<br/>schedules, doctors...)]
    end

    %% Application uses interfaces
    BookingService --> IAppointmentRepo
    BookingService --> IScheduleRepo
    BookingService --> IDoctorRepo
    BookingService --> Appointment
    BookingService --> Schedule
    BookingService --> Doctor

    %% Infrastructure implements interfaces
    IAppointmentRepo -.->|"implements"| AppointmentRepo
    IScheduleRepo -.->|"implements"| ScheduleRepo
    IDoctorRepo -.->|"implements"| DoctorRepo

    %% Infrastructure uses Domain
    AppointmentRepo --> Appointment
    ScheduleRepo --> Schedule
    DoctorRepo --> Doctor

    %% Infrastructure uses EF Core
    AppointmentRepo --> DbContext
    ScheduleRepo --> DbContext
    DoctorRepo --> DbContext
    AppointmentRepo --> Mapper

    %% EF Core connects to database
    DbContext --> DB

    %% Styling
    classDef app fill:#fef08a,stroke:#ca8a04,color:#78350f
    classDef domain fill:#a7f3d0,stroke:#16a34a,color:#064e3b
    classDef infra fill:#bfdbfe,stroke:#1d4ed8,color:#1e3a8a
    classDef db fill:#e0e7ff,stroke:#6366f1,color:#312e81

    class Application,BookingService,IAppointmentRepo,IScheduleRepo,IDoctorRepo app
    class Domain,Appointment,Schedule,Doctor domain
    class Infrastructure,AppointmentRepo,ScheduleRepo,DoctorRepo,DbContext,Mapper infra
    class Database,DB db
```

### 💡 Ví dụ cụ thể: BookingService sử dụng Repository

```csharp
// Application/Services/Booking/BookingService.cs
public class BookingService : IBookingService
{
    // Inject repository qua INTERFACE, không phải implementation
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IDoctorRepository _doctorRepository;

    public BookingService(
        IAppointmentRepository appointmentRepository,
        IScheduleRepository scheduleRepository,
        IDoctorRepository doctorRepository
    )
    {
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
    {
        // Business logic: Kiểm tra availability
        var isAvailable = await _scheduleRepository
            .IsScheduleAvailableAsync(request.DoctorId, request.SlotId, request.Date);
        
        if (!isAvailable)
            throw new ValidationException("Slot không còn trống");

        // Business logic: Tạo appointment
        var appointment = new Appointment { /* ... */ };
        await _appointmentRepository.AddAsync(appointment);

        return new BookingResult { /* ... */ };
    }
}
```

**Điểm quan trọng:**
- `BookingService` không biết gì về EF Core, SQL, hay PostgreSQL
- Chỉ cần biết interface `IAppointmentRepository` có method `AddAsync()`
- Infrastructure layer tự lo việc map Domain → EF Model → Database

#### 2. Sequence Diagram: Luồng tạo Booking với Repository Pattern

```mermaid
sequenceDiagram
    participant Controller as BookingController<br/>(WebAPI)
    participant Service as BookingService<br/>(Application)
    participant IRepo as IAppointmentRepository<br/>(Interface)
    participant Repo as AppointmentRepository<br/>(Infrastructure)
    participant Mapper as AppointmentMapper
    participant DbContext as VisionCareDbContext<br/>(EF Core)
    participant DB as PostgreSQL<br/>Database
    participant Domain as Appointment Entity<br/>(Domain)

    Controller->>Service: CreateBookingAsync(request)
    
    Note over Service: Business Logic: Validate request
    
    Service->>IRepo: CheckAvailability(doctorId, slotId, date)
    IRepo->>Repo: CheckAvailability(...)
    Repo->>DbContext: Query schedules
    DbContext->>DB: SELECT * FROM schedules...
    DB-->>DbContext: Schedule data
    DbContext-->>Repo: Schedule entities
    Repo-->>IRepo: bool isAvailable
    IRepo-->>Service: isAvailable = true
    
    Note over Service: Business Logic: Create Appointment
    
    Service->>Domain: new Appointment { ... }
    Domain-->>Service: appointment (Domain Entity)
    
    Service->>IRepo: AddAsync(appointment)
    IRepo->>Repo: AddAsync(appointment)
    
    Note over Repo,Mapper: Convert Domain → EF Model
    
    Repo->>Mapper: ToInfrastructure(appointment)
    Mapper-->>Repo: AppointmentModel (EF)
    Repo->>DbContext: Appointments.Add(model)
    DbContext->>DB: INSERT INTO appointments...
    DB-->>DbContext: Success
    DbContext-->>Repo: Saved entity
    Repo->>Mapper: ToDomain(savedEntity)
    Mapper-->>Repo: Appointment (Domain)
    Repo-->>IRepo: Appointment (Domain)
    IRepo-->>Service: Appointment (Domain)
    
    Service-->>Controller: BookingResultDto
    Controller-->>Controller: 200 OK
```

**Giải thích luồng:**
1. **Controller** nhận request từ client
2. **Service** (Application layer) xử lý business logic, chỉ gọi qua **Interface**
3. **Repository** (Infrastructure) implement interface, sử dụng **Mapper** để convert
4. **DbContext** (EF Core) tương tác với **Database**
5. Dữ liệu quay lại qua Domain Entity, không phải EF Model

#### 3. Class Diagram: Cấu trúc Repository Pattern

```mermaid
classDiagram
    class IAppointmentRepository {
        <<interface>>
        +GetAllAsync() IEnumerable~Appointment~
        +GetByIdAsync(int id) Appointment?
        +AddAsync(Appointment) Task~Appointment~
        +UpdateAsync(Appointment) Task
        +DeleteAsync(int id) Task
        +GetByDoctorAsync(int, DateTime?) IEnumerable~Appointment~
    }
    
    class AppointmentRepository {
        -_context: VisionCareDbContext
        +GetAllAsync() IEnumerable~Appointment~
        +GetByIdAsync(int id) Appointment?
        +AddAsync(Appointment) Task~Appointment~
        +UpdateAsync(Appointment) Task
        +DeleteAsync(int id) Task
        +GetByDoctorAsync(int, DateTime?) IEnumerable~Appointment~
    }
    
    class BookingService {
        -_appointmentRepo: IAppointmentRepository
        -_scheduleRepo: IScheduleRepository
        +CreateBookingAsync(request) Task~BookingResult~
        +GetAvailableSlotsAsync(...) Task~IEnumerable~
    }
    
    class Appointment {
        <<Domain Entity>>
        +Id: int
        +AppointmentDate: DateTime?
        +DoctorId: int?
        +PatientId: int?
        +AddItem(item)
        +Validate()
    }
    
    class AppointmentMapper {
        <<static>>
        +ToDomain(EFModel) Appointment
        +ToInfrastructure(Domain) EFModel
    }
    
    class VisionCareDbContext {
        +Appointments: DbSet~AppointmentModel~
        +SaveChangesAsync() Task
    }
    
    IAppointmentRepository <|.. AppointmentRepository : implements
    BookingService --> IAppointmentRepository : depends on
    AppointmentRepository --> Appointment : returns
    AppointmentRepository --> AppointmentMapper : uses
    AppointmentRepository --> VisionCareDbContext : uses
    AppointmentMapper --> Appointment : converts to/from
```

**Giải thích:**
- **Interface** (`IAppointmentRepository`) định nghĩa contract ở Application layer
- **Implementation** (`AppointmentRepository`) ở Infrastructure layer
- **Service** chỉ phụ thuộc vào interface, không biết implementation
- **Mapper** chuyển đổi giữa Domain Entity và EF Model

#### 4. So sánh: Có Repository vs Không có Repository

```mermaid
flowchart LR
    subgraph WithRepo["✅ CÓ Repository Pattern"]
        A1[BookingService] -->|depends on| I1[IAppointmentRepository<br/>Interface]
        I1 -.->|implements| R1[AppointmentRepository]
        R1 --> DB1[(Database)]
        A1 -.->|"✅ Testable<br/>✅ Flexible<br/>✅ Clean"| Benefits1
    end
    
    subgraph WithoutRepo["❌ KHÔNG có Repository"]
        A2[BookingService] -->|direct dependency| Db2[DbContext<br/>EF Core]
        Db2 --> DB2[(Database)]
        A2 -.->|"❌ Hard to test<br/>❌ Tight coupling<br/>❌ Violates DIP"| Problems2
    end
    
    style WithRepo fill:#a7f3d0,stroke:#16a34a
    style WithoutRepo fill:#fecaca,stroke:#b91c1c
```

**Lợi ích khi có Repository:**
- ✅ **Testable**: Mock interface dễ dàng
- ✅ **Flexible**: Đổi database không ảnh hưởng business logic
- ✅ **Clean**: Tuân thủ Dependency Inversion Principle
- ✅ **Maintainable**: Tách biệt rõ ràng giữa các layer

#### 5. Dependency Injection Flow với Repository

```mermaid
flowchart TD
    Start[Program.cs<br/>Startup] --> DI1[Application/DependencyInjection.cs]
    DI1 --> DI2[Infrastructure/DependencyInjection.cs]
    
    DI2 --> Reg1["services.AddScoped&lt;<br/>IAppointmentRepository,<br/>AppointmentRepository&gt;()"]
    
    Reg1 --> Container[DI Container]
    
    Container -->|injects| Controller[BookingController]
    Container -->|injects| Service[BookingService]
    
    Service -->|uses| Interface[IAppointmentRepository<br/>Interface]
    Interface -.->|resolves to| Implementation[AppointmentRepository<br/>Implementation]
    
    Implementation --> DbContext[VisionCareDbContext]
    DbContext --> DB[(PostgreSQL)]
    
    style Container fill:#fef08a,stroke:#ca8a04
    style Interface fill:#bfdbfe,stroke:#1d4ed8
    style Implementation fill:#bfdbfe,stroke:#1d4ed8
```

**Giải thích DI Flow:**
1. **Program.cs** gọi `AddApplication()` và `AddInfrastructure()`
2. **DependencyInjection.cs** đăng ký: `IAppointmentRepository` → `AppointmentRepository`
3. **DI Container** tự động inject vào constructor của `BookingService`
4. **BookingService** nhận được `AppointmentRepository` nhưng chỉ biết qua interface
5. Khi runtime, DI container resolve implementation thực tế

### 📈 Thống kê Repository Pattern trong VisionCare

- **36+ Repository Interfaces** trong `Application/Interfaces/`
- **36+ Repository Implementations** trong `Infrastructure/Repositories/`
- **100% Services** sử dụng repository pattern
- **0 direct database access** từ Application layer

### ✅ Lợi ích thực tế

1. **Onboarding dev mới**: Dễ hiểu vì pattern nhất quán
2. **Unit testing**: Mock repository trong 5 phút
3. **Thay đổi database**: Chỉ sửa Infrastructure, Application không đổi
4. **Code review**: Dễ review vì tách biệt rõ ràng
5. **Performance**: Có thể cache ở repository level mà không ảnh hưởng business logic

---

## Design Patterns trong VisionCare

| Pattern | Mục đích | Hiện diện trong dự án | Giải thích |
| --- | --- | --- | --- |
| **Repository** ⭐ | Tách persistence khỏi domain | `Infrastructure/Repositories/*Repository.cs` | **Pattern quan trọng nhất** - 36+ repositories, nền tảng của Clean Architecture |
| **Data Mapper** | Chuyển đổi model giữa tầng | `Infrastructure/Mappings/*Mapper.cs`, `Application/Mappings/MappingProfile.cs` | Mapper biến EF model ↔ Domain entity, giúp Domain thuần C#. |
| **Service Layer / Use Case** | Gom nghiệp vụ thành use case | `Application/Services/*Service.cs` | `BookingService`, `ScheduleGenerationService`, `AuthService` triển khai logic nghiệp vụ tập trung. |
| **Dependency Injection + Interface (Inversion of Control)** | Giảm coupling, thay thế dễ | `Application/DependencyInjection.cs`, `Infrastructure/DependencyInjection.cs`, `Program.cs` | Interface nằm ở Application, Implementation được đăng ký (Scoped/Singleton) ở Infrastructure/WebAPI. |
| **Singleton** | Một instance duy nhất dùng chung | `Infrastructure/DependencyInjection.cs` với `AddSingleton<IJwtTokenService, JwtTokenService>()` | Bảo đảm việc phát JWT thống nhất toàn hệ thống. |
| **Adapter** | Chuyển đổi interface không tương thích | `Infrastructure/Services/Payment/VNPayService.cs` (triển khai `IVNPayService`) | Bọc SDK VNPay, cung cấp API nội bộ tạo URL & verify callback. |
| **Observer (Pub/Sub)** | Phát sự kiện tới nhiều client | `WebAPI/Hubs/BookingHub.cs`, `CommentHub.cs` | SignalR giúp thông báo realtime khi lịch/ bình luận thay đổi. |
| **Strategy (Thông qua FluentValidation)** | Hoán đổi thuật toán/logic linh hoạt | `Application/Validators/*.cs` | Mỗi validator là một chiến lược kiểm tra DTO khác nhau, tiêm bằng DI. |
| **Command (Job + Background Task)** | Đóng gói hành động thành object | `Application/Services/Scheduling/ScheduleGenerationJob` + Hangfire Scheduler | Job biểu diễn “Generate schedules” và có thể được enqueue/retry như command. |

> Những pattern như Builder, Decorator, Proxy hiện chưa có implementation rõ ràng trong dự án. Nếu muốn áp dụng (ví dụ builder cho email template, decorator cho logging middleware chuyên biệt), có thể bổ sung sau.

## Observer Pattern trong VisionCare: SignalR Real-time Notifications

### 📡 Observer Pattern với SignalR

VisionCare sử dụng **Observer Pattern** thông qua SignalR để gửi thông báo real-time cho nhiều clients khi có sự kiện xảy ra (đặt lịch, bình luận blog).

### 🏗️ Kiến trúc Observer Pattern

```mermaid
flowchart TB
    subgraph Subject["📢 Subject (Observable)"]
        BookingHub["BookingHub<br/>(SignalR Hub)"]
        CommentHub["CommentHub<br/>(SignalR Hub)"]
        HubContext["IHubContext<br/>(SignalR Context)"]
    end

    subgraph Publisher["📤 Publisher (Event Source)"]
        BookingController["BookingController<br/>- HoldSlot()<br/>- CreateBooking()<br/>- CancelBooking()"]
        CommentController["CommentBlogController<br/>- CreateComment()"]
    end

    subgraph Observers["👁️ Observers (Subscribers)"]
        Client1["Frontend Client 1<br/>(User đang xem slots)"]
        Client2["Frontend Client 2<br/>(User khác xem slots)"]
        Client3["Admin Dashboard<br/>(Xem booking dashboard)"]
        Client4["Blog Viewer<br/>(Đang xem blog)"]
    end

    subgraph Groups["👥 SignalR Groups"]
        Group1["slots:doctorId:date<br/>(Users xem slots)"]
        Group2["admin:bookings<br/>(Admin dashboard)"]
        Group3["blog:blogId<br/>(Blog viewers)"]
    end

    %% Publisher notifies Subject
    BookingController -->|"SendAsync('SlotHeld', data)"| HubContext
    BookingController -->|"SendAsync('BookingCreated', data)"| HubContext
    CommentController -->|"SendAsync('NewComment', data)"| HubContext

    %% Subject manages groups
    HubContext --> BookingHub
    HubContext --> CommentHub
    BookingHub --> Group1
    BookingHub --> Group2
    CommentHub --> Group3

    %% Observers subscribe to groups
    Client1 -.->|"JoinSlotsGroup()"| Group1
    Client2 -.->|"JoinSlotsGroup()"| Group1
    Client3 -.->|"JoinAdminGroup()"| Group2
    Client4 -.->|"JoinBlogGroup()"| Group3

    %% Subject notifies all observers in group
    Group1 -.->|"Notify all"| Client1
    Group1 -.->|"Notify all"| Client2
    Group2 -.->|"Notify all"| Client3
    Group3 -.->|"Notify all"| Client4

    %% Styling
    classDef subject fill:#fef08a,stroke:#ca8a04,color:#78350f
    classDef publisher fill:#bfdbfe,stroke:#1d4ed8,color:#1e3a8a
    classDef observer fill:#a7f3d0,stroke:#16a34a,color:#064e3b
    classDef group fill:#e0e7ff,stroke:#6366f1,color:#312e8a

    class Subject,BookingHub,CommentHub,HubContext subject
    class Publisher,BookingController,CommentController publisher
    class Observers,Client1,Client2,Client3,Client4 observer
    class Groups,Group1,Group2,Group3 group
```

### 📊 Sequence Diagram: Luồng Observer Pattern

```mermaid
sequenceDiagram
    participant Client1 as Frontend Client 1<br/>(Observer)
    participant Client2 as Frontend Client 2<br/>(Observer)
    participant Hub as BookingHub<br/>(Subject)
    participant Controller as BookingController<br/>(Publisher)
    participant Service as BookingService
    participant DB as Database

    Note over Client1,Client2: Subscribe Phase
    Client1->>Hub: JoinSlotsGroup(doctorId: 1, date: "20240115")
    Hub->>Hub: Add Client1 to group "slots:1:20240115"
    Client2->>Hub: JoinSlotsGroup(doctorId: 1, date: "20240115")
    Hub->>Hub: Add Client2 to group "slots:1:20240115"

    Note over Client1,Client2: Both clients now observing slot changes

    Note over Controller,DB: Event Occurs
    Client1->>Controller: POST /api/booking/hold-slot
    Controller->>Service: HoldSlotAsync(request)
    Service->>DB: Save hold to cache
    Service-->>Controller: HoldSlotResponse

    Note over Controller,Hub: Notify All Observers
    Controller->>Hub: _hubContext.Clients.Group("slots:1:20240115")<br/>.SendAsync("SlotHeld", data)

    Note over Hub,Client2: Broadcast to All Subscribers
    Hub->>Client1: "SlotHeld" event (WebSocket)
    Hub->>Client2: "SlotHeld" event (WebSocket)

    Note over Client1,Client2: Both clients update UI in real-time
    Client1->>Client1: Update UI: Slot marked as "Held"
    Client2->>Client2: Update UI: Slot marked as "Held"
```

### 💡 Ví dụ Code: Observer Pattern trong Action

**1. Subject (BookingHub) - Quản lý Observers:**
```csharp
// WebAPI/Hubs/BookingHub.cs
public class BookingHub : Hub
{
    // Observer subscribe vào group
    public async Task JoinSlotsGroup(int doctorId, string date)
    {
        var groupName = $"slots:{doctorId}:{date}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}
```

**2. Publisher (BookingController) - Phát sự kiện:**
```csharp
// WebAPI/Controllers/BookingController.cs
[HttpPost("hold-slot")]
public async Task<ActionResult<HoldSlotResponse>> HoldSlot([FromBody] HoldSlotRequest request)
{
    var response = await _bookingService.HoldSlotAsync(request);
    
    // Notify all observers in group
    var groupName = $"slots:{request.DoctorId}:{request.ScheduleDate:yyyyMMdd}";
    await _hubContext.Clients.Group(groupName).SendAsync("SlotHeld", new {
        doctorId = request.DoctorId,
        slotId = request.SlotId,
        date = request.ScheduleDate.ToString("yyyyMMdd"),
        holdToken = response.HoldToken
    });
    
    return Ok(response);
}
```

**3. Observer (Frontend) - Nhận thông báo:**
```javascript
// fe/src/hooks/useBooking.js
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/booking")
    .build();

// Subscribe vào group
await connection.invoke("JoinSlotsGroup", doctorId, date);

// Lắng nghe sự kiện
connection.on("SlotHeld", (data) => {
    // Update UI khi có slot bị hold
    setSlots(prevSlots => 
        prevSlots.map(slot => 
            slot.id === data.slotId 
                ? { ...slot, status: "held" }
                : slot
        )
    );
});
```

### ✅ Lợi ích Observer Pattern trong VisionCare

1. **Real-time Updates**: Users thấy thay đổi ngay lập tức, không cần refresh
2. **Decoupling**: Controller không cần biết có bao nhiêu clients đang lắng nghe
3. **Scalable**: Có thể thêm nhiều observers mà không sửa code Publisher
4. **Group-based**: Chỉ notify những clients quan tâm (theo doctorId, date, blogId)
5. **Automatic Cleanup**: SignalR tự động remove observer khi disconnect

### 🔄 So sánh: Observer Pattern vs Polling

```mermaid
flowchart LR
    subgraph Observer["✅ Observer Pattern (SignalR)"]
        C1[Client 1] -->|Subscribe| Hub[SignalR Hub]
        C2[Client 2] -->|Subscribe| Hub
        Event[Event Occurs] -->|Notify| Hub
        Hub -->|Push| C1
        Hub -->|Push| C2
        Note1["✅ Real-time<br/>✅ Efficient<br/>✅ Server push"]
    end

    subgraph Polling["❌ Polling (Traditional)"]
        C3[Client 1] -->|"GET /api/slots<br/>(every 5s)"| API[API]
        C4[Client 2] -->|"GET /api/slots<br/>(every 5s)"| API
        API -->|Response| C3
        API -->|Response| C4
        Note2["❌ Delay<br/>❌ Wasteful<br/>❌ Client pull"]
    end

    style Observer fill:#a7f3d0,stroke:#16a34a
    style Polling fill:#fecaca,stroke:#b91c1c
```

**Observer Pattern tốt hơn vì:**
- ✅ **Real-time**: Thông báo ngay khi có sự kiện
- ✅ **Efficient**: Không cần polling liên tục
- ✅ **Server push**: Server chủ động gửi, không đợi client hỏi

## Liên kết Frontend với Clean Architecture
- FE chỉ gọi endpoint qua lớp service (`fe/src/services/bookingService.js`…), không biết về DB.
- React Context quản lý trạng thái đăng nhập (`AuthContext`), hooks (`useBooking`) điều phối gọi API.
- Component `BookingPaymentCallback.jsx` xử lý redirect từ VNPay, đọc query string và hiển thị kết quả.
- Lợi ích: Dễ thay backend khác mà không ảnh hưởng cấu trúc component, chỉ cần update service layer FE.

## Script gợi ý cho thuyết trình (10-12 phút)

1. **Mở bài (1 phút)**  
   - Giới thiệu nhóm & bài toán VisionCare.  
   - Đặt mục tiêu: đảm bảo khả năng mở rộng, dễ bảo trì.

2. **Clean Architecture là gì? (2 phút)**  
   - Trình chiếu sơ đồ layer (Mermaid).  
   - Nhấn mạnh nguyên lý Dependency Rule, Use Case-centric, testability.

3. **Ánh xạ vào VisionCare (3 phút)**  
   - Đi từ Domain → Application → Infrastructure → WebAPI → React FE.  
   - Ví dụ cụ thể: `BookingService` gọi `AppointmentRepository`.  
   - Nêu lợi ích khi thay đổi DB hoặc nhà cung cấp thanh toán.

4. **Luồng nghiệp vụ trọng điểm (3 phút)**  
   - Sử dụng sequence thứ nhất giải thích quy trình đặt lịch.  
   - Sequence thanh toán VNPay cho thấy vai trò adapter.  
   - Flow Hangfire thể hiện cross-cutting concern.

5. **Design Pattern Quan Trọng Nhất: Repository Pattern (3 phút)**  
   - **Nhấn mạnh**: Repository Pattern là nền tảng của Clean Architecture
   - Trình chiếu sơ đồ Mermaid minh họa cách Repository hoạt động
   - Giải thích 5 lý do tại sao quan trọng (tách biệt, DIP, testability, linh hoạt, quy mô)
   - Ví dụ code `BookingService` sử dụng repository
   - Thống kê: 36+ repositories, 100% services sử dụng
   - Lợi ích thực tế: onboarding, testing, thay đổi DB

6. **Các Design Patterns khác (1 phút)**  
   - Trình bày nhanh bảng pattern còn lại (Adapter, Singleton, Strategy, Observer...)
   - Nhấn mạnh Repository là quan trọng nhất, các pattern khác hỗ trợ

7. **Frontend góc nhìn Clean Architecture (1 phút)**  
   - FE tôn trọng boundary, dùng service + context.  
   - Lợi ích khi backend thay đổi.

8. **Kết luận & Q&A (1 phút)**  
   - Tổng kết: Repository Pattern là nền tảng, giúp đạt 3 mục tiêu: maintainability, scalability, testability.  
   - Mời đặt câu hỏi.

## Tips khi trình bày
- **Nhấn mạnh Repository Pattern**: Đây là pattern quan trọng nhất, dành 3 phút để giải thích kỹ
- Chuẩn bị demo nhanh (ví dụ đặt một lịch trên FE) rồi quay lại slide để phân tích layer tương ứng
- Khi trình bày Repository Pattern, chỉ vào sơ đồ Mermaid và giải thích:
  - Application chỉ biết interface
  - Infrastructure implement interface
  - Dependency flow: Application → Interface ← Infrastructure
- Nhấn mạnh lợi ích thực tế: 
  - Onboarding dev mới dễ vì pattern nhất quán
  - Unit test dễ vì mock repository
  - Thay database chỉ sửa Infrastructure
- Giữ slide trực quan: chuyển giữa sơ đồ Mermaid và bảng pattern, hạn chế chữ dài
- Có thể phát handout đường link tới tài liệu `.md` này để người nghe xem lại

---

> *Tài liệu này nằm ở `docs/visioncare-clean-architecture.md`. Cập nhật thêm ví dụ code hoặc hình ảnh theo nhu cầu thuyết trình.*

