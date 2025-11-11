-- Script để kiểm tra các bảng Hangfire trong database

-- 1. Kiểm tra xem schema hangfire có tồn tại không
SELECT 
    CASE 
        WHEN EXISTS (SELECT 1 FROM information_schema.schemata WHERE schema_name = 'hangfire')
        THEN 'Schema hangfire TỒN TẠI'
        ELSE 'Schema hangfire KHÔNG TỒN TẠI (App chưa chạy hoặc Hangfire chưa khởi tạo)'
    END as status;

-- 2. Xem tất cả các bảng trong schema hangfire (nếu có)
SELECT 
    table_schema,
    table_name,
    'hangfire' as schema_location
FROM information_schema.tables 
WHERE table_schema = 'hangfire'
ORDER BY table_name;

-- 3. So sánh: Xem các bảng trong schema public (bảng của app)
SELECT 
    table_schema,
    table_name,
    'public' as schema_location
FROM information_schema.tables 
WHERE table_schema = 'public'
    AND table_type = 'BASE TABLE'
ORDER BY table_name
LIMIT 10;

-- 4. Xem tất cả các schema trong database
SELECT 
    schema_name,
    CASE 
        WHEN schema_name = 'public' THEN 'Bảng của ứng dụng'
        WHEN schema_name = 'hangfire' THEN 'Bảng của Hangfire (background jobs)'
        WHEN schema_name IN ('pg_catalog', 'information_schema') THEN 'System schema'
        ELSE 'Schema khác'
    END as description
FROM information_schema.schemata 
WHERE schema_name NOT IN ('pg_toast', 'pg_temp_1', 'pg_toast_temp_1')
ORDER BY 
    CASE 
        WHEN schema_name = 'public' THEN 1
        WHEN schema_name = 'hangfire' THEN 2
        ELSE 3
    END,
    schema_name;

