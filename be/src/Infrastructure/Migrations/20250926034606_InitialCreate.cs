using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VisionCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banner",
                columns: table => new
                {
                    banner_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    link_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("banner_pkey", x => x.banner_id);
                });

            migrationBuilder.CreateTable(
                name: "certificate",
                columns: table => new
                {
                    certificate_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("certificate_pkey", x => x.certificate_id);
                });

            migrationBuilder.CreateTable(
                name: "contentstories",
                columns: table => new
                {
                    story_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    patient_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    story_content = table.Column<string>(type: "text", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("contentstories_pkey", x => x.story_id);
                });

            migrationBuilder.CreateTable(
                name: "customerrank",
                columns: table => new
                {
                    rank_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rank_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    min_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customerrank_pkey", x => x.rank_id);
                });

            migrationBuilder.CreateTable(
                name: "degree",
                columns: table => new
                {
                    degree_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("degree_pkey", x => x.degree_id);
                });

            migrationBuilder.CreateTable(
                name: "machine",
                columns: table => new
                {
                    machine_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    specifications = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("machine_pkey", x => x.machine_id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    resource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    action = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("permission_pkey", x => x.permission_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role_description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "servicestype",
                columns: table => new
                {
                    service_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    duration_minutes = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servicestype_pkey", x => x.service_type_id);
                });

            migrationBuilder.CreateTable(
                name: "specialization",
                columns: table => new
                {
                    specialization_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("specialization_pkey", x => x.specialization_id);
                });

            migrationBuilder.CreateTable(
                name: "discount",
                columns: table => new
                {
                    discount_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discount_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    discount_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    rank_id = table.Column<int>(type: "integer", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("discount_pkey", x => x.discount_id);
                    table.ForeignKey(
                        name: "discount_rank_id_fkey",
                        column: x => x.rank_id,
                        principalTable: "customerrank",
                        principalColumn: "rank_id");
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    email_confirmation_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    lockout_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    access_failed_count = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)0),
                    password_reset_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password_reset_expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_password_change = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    google_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    facebook_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_login = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accounts_pkey", x => x.account_id);
                    table.ForeignKey(
                        name: "accounts_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "permissionrole",
                columns: table => new
                {
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    granted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("permissionrole_pkey", x => new { x.permission_id, x.role_id });
                    table.ForeignKey(
                        name: "permissionrole_permission_id_fkey",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "permissionrole_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "slots",
                columns: table => new
                {
                    slot_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    service_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("slots_pkey", x => x.slot_id);
                    table.ForeignKey(
                        name: "slots_service_type_id_fkey",
                        column: x => x.service_type_id,
                        principalTable: "servicestype",
                        principalColumn: "service_type_id");
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    benefits = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    specialization_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("services_pkey", x => x.service_id);
                    table.ForeignKey(
                        name: "services_specialization_id_fkey",
                        column: x => x.specialization_id,
                        principalTable: "specialization",
                        principalColumn: "specialization_id");
                });

            migrationBuilder.CreateTable(
                name: "auditlogs",
                columns: table => new
                {
                    audit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(type: "integer", nullable: true),
                    action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    resource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    resource_id = table.Column<int>(type: "integer", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ip_address = table.Column<IPAddress>(type: "inet", nullable: true),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    details = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("auditlogs_pkey", x => x.audit_id);
                    table.ForeignKey(
                        name: "auditlogs_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "blog",
                columns: table => new
                {
                    blog_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    excerpt = table.Column<string>(type: "text", nullable: true),
                    featured_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Draft'::character varying"),
                    published_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    view_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blog_pkey", x => x.blog_id);
                    table.ForeignKey(
                        name: "blog_author_id_fkey",
                        column: x => x.author_id,
                        principalTable: "accounts",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "claims",
                columns: table => new
                {
                    claim_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    claim_value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("claims_pkey", x => x.claim_id);
                    table.ForeignKey(
                        name: "claims_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    rank_id = table.Column<int>(type: "integer", nullable: true),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_pkey", x => x.account_id);
                    table.ForeignKey(
                        name: "customers_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "customers_rank_id_fkey",
                        column: x => x.rank_id,
                        principalTable: "customerrank",
                        principalColumn: "rank_id");
                });

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    experience_years = table.Column<short>(type: "smallint", nullable: true),
                    specialization_id = table.Column<int>(type: "integer", nullable: false),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    rating = table.Column<decimal>(type: "numeric(2,1)", precision: 2, scale: 1, nullable: true, defaultValueSql: "0.0"),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("doctors_pkey", x => x.account_id);
                    table.ForeignKey(
                        name: "doctors_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "doctors_specialization_id_fkey",
                        column: x => x.specialization_id,
                        principalTable: "specialization",
                        principalColumn: "specialization_id");
                });

            migrationBuilder.CreateTable(
                name: "otpservices",
                columns: table => new
                {
                    otp_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    otp_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    otp_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    used_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    attempts = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("otpservices_pkey", x => x.otp_id);
                    table.ForeignKey(
                        name: "otpservices_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refreshtokens",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    revoked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_ip = table.Column<IPAddress>(type: "inet", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("refreshtokens_pkey", x => x.token_id);
                    table.ForeignKey(
                        name: "refreshtokens_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    account_id = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    hired_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    salary = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("staff_pkey", x => x.account_id);
                    table.ForeignKey(
                        name: "staff_account_id_fkey",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "imagesservice",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    image_main = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    image_before = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    image_after = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("imagesservice_pkey", x => x.service_id);
                    table.ForeignKey(
                        name: "imagesservice_service_id_fkey",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicesdetail",
                columns: table => new
                {
                    service_detail_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    service_type_id = table.Column<int>(type: "integer", nullable: false),
                    cost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servicesdetail_pkey", x => x.service_detail_id);
                    table.ForeignKey(
                        name: "servicesdetail_service_id_fkey",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "servicesdetail_service_type_id_fkey",
                        column: x => x.service_type_id,
                        principalTable: "servicestype",
                        principalColumn: "service_type_id");
                });

            migrationBuilder.CreateTable(
                name: "commentblog",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blog_id = table.Column<int>(type: "integer", nullable: false),
                    author_id = table.Column<int>(type: "integer", nullable: true),
                    parent_comment_id = table.Column<int>(type: "integer", nullable: true),
                    comment_text = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("commentblog_pkey", x => x.comment_id);
                    table.ForeignKey(
                        name: "commentblog_author_id_fkey",
                        column: x => x.author_id,
                        principalTable: "accounts",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "commentblog_blog_id_fkey",
                        column: x => x.blog_id,
                        principalTable: "blog",
                        principalColumn: "blog_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "commentblog_parent_comment_id_fkey",
                        column: x => x.parent_comment_id,
                        principalTable: "commentblog",
                        principalColumn: "comment_id");
                });

            migrationBuilder.CreateTable(
                name: "certificatedoctor",
                columns: table => new
                {
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    certificate_id = table.Column<int>(type: "integer", nullable: false),
                    issued_date = table.Column<DateOnly>(type: "date", nullable: true),
                    issued_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    certificate_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("certificatedoctor_pkey", x => new { x.doctor_id, x.certificate_id });
                    table.ForeignKey(
                        name: "certificatedoctor_certificate_id_fkey",
                        column: x => x.certificate_id,
                        principalTable: "certificate",
                        principalColumn: "certificate_id");
                    table.ForeignKey(
                        name: "certificatedoctor_doctor_id_fkey",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "degreedoctor",
                columns: table => new
                {
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    degree_id = table.Column<int>(type: "integer", nullable: false),
                    issued_date = table.Column<DateOnly>(type: "date", nullable: true),
                    issued_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    certificate_image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Active'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("degreedoctor_pkey", x => new { x.doctor_id, x.degree_id });
                    table.ForeignKey(
                        name: "degreedoctor_degree_id_fkey",
                        column: x => x.degree_id,
                        principalTable: "degree",
                        principalColumn: "degree_id");
                    table.ForeignKey(
                        name: "degreedoctor_doctor_id_fkey",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    slot_id = table.Column<int>(type: "integer", nullable: false),
                    schedule_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Available'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedules_pkey", x => x.schedule_id);
                    table.ForeignKey(
                        name: "schedules_doctor_id_fkey",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "schedules_slot_id_fkey",
                        column: x => x.slot_id,
                        principalTable: "slots",
                        principalColumn: "slot_id");
                });

            migrationBuilder.CreateTable(
                name: "appointment",
                columns: table => new
                {
                    appointment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_id = table.Column<int>(type: "integer", nullable: false),
                    doctor_id = table.Column<int>(type: "integer", nullable: false),
                    service_detail_id = table.Column<int>(type: "integer", nullable: false),
                    discount_id = table.Column<int>(type: "integer", nullable: true),
                    appointment_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true, defaultValueSql: "'Scheduled'::character varying"),
                    actual_cost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("appointment_pkey", x => x.appointment_id);
                    table.ForeignKey(
                        name: "appointment_created_by_fkey",
                        column: x => x.created_by,
                        principalTable: "accounts",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "appointment_discount_id_fkey",
                        column: x => x.discount_id,
                        principalTable: "discount",
                        principalColumn: "discount_id");
                    table.ForeignKey(
                        name: "appointment_doctor_id_fkey",
                        column: x => x.doctor_id,
                        principalTable: "doctors",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "appointment_patient_id_fkey",
                        column: x => x.patient_id,
                        principalTable: "customers",
                        principalColumn: "account_id");
                    table.ForeignKey(
                        name: "appointment_service_detail_id_fkey",
                        column: x => x.service_detail_id,
                        principalTable: "servicesdetail",
                        principalColumn: "service_detail_id");
                });

            migrationBuilder.CreateTable(
                name: "checkout",
                columns: table => new
                {
                    checkout_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    transaction_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    transaction_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    payment_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("checkout_pkey", x => x.checkout_id);
                    table.ForeignKey(
                        name: "checkout_appointment_id_fkey",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedbackdoctor",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<short>(type: "smallint", nullable: true),
                    feedback_text = table.Column<string>(type: "text", nullable: true),
                    feedback_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    response_text = table.Column<string>(type: "text", nullable: true),
                    response_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    responded_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feedbackdoctor_pkey", x => x.feedback_id);
                    table.ForeignKey(
                        name: "feedbackdoctor_appointment_id_fkey",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "feedbackdoctor_responded_by_fkey",
                        column: x => x.responded_by,
                        principalTable: "staff",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "feedbackservice",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    rating = table.Column<short>(type: "smallint", nullable: true),
                    feedback_text = table.Column<string>(type: "text", nullable: true),
                    feedback_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    response_text = table.Column<string>(type: "text", nullable: true),
                    response_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    responded_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feedbackservice_pkey", x => x.feedback_id);
                    table.ForeignKey(
                        name: "feedbackservice_appointment_id_fkey",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "feedbackservice_responded_by_fkey",
                        column: x => x.responded_by,
                        principalTable: "staff",
                        principalColumn: "account_id");
                });

            migrationBuilder.CreateTable(
                name: "followup",
                columns: table => new
                {
                    follow_up_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    next_appointment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'Pending'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("followup_pkey", x => x.follow_up_id);
                    table.ForeignKey(
                        name: "followup_appointment_id_fkey",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medicalhistory",
                columns: table => new
                {
                    medical_history_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    diagnosis = table.Column<string>(type: "text", nullable: true),
                    symptoms = table.Column<string>(type: "text", nullable: true),
                    treatment = table.Column<string>(type: "text", nullable: true),
                    prescription = table.Column<string>(type: "text", nullable: true),
                    vision_left = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    vision_right = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    additional_tests = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("medicalhistory_pkey", x => x.medical_history_id);
                    table.ForeignKey(
                        name: "medicalhistory_appointment_id_fkey",
                        column: x => x.appointment_id,
                        principalTable: "appointment",
                        principalColumn: "appointment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "accounts_email_key",
                table: "accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "accounts_facebook_id_key",
                table: "accounts",
                column: "facebook_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "accounts_google_id_key",
                table: "accounts",
                column: "google_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "accounts_username_key",
                table: "accounts",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_accounts_email",
                table: "accounts",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_facebook_id",
                table: "accounts",
                column: "facebook_id",
                filter: "(facebook_id IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_google_id",
                table: "accounts",
                column: "google_id",
                filter: "(google_id IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_role",
                table: "accounts",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_status",
                table: "accounts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_accounts_username",
                table: "accounts",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "idx_appointments_datetime",
                table: "appointment",
                column: "appointment_datetime");

            migrationBuilder.CreateIndex(
                name: "idx_appointments_doctor",
                table: "appointment",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "idx_appointments_patient",
                table: "appointment",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "idx_appointments_status",
                table: "appointment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_created_by",
                table: "appointment",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_discount_id",
                table: "appointment",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointment_service_detail_id",
                table: "appointment",
                column: "service_detail_id");

            migrationBuilder.CreateIndex(
                name: "idx_audit_account",
                table: "auditlogs",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "idx_audit_resource",
                table: "auditlogs",
                columns: new[] { "resource", "resource_id" });

            migrationBuilder.CreateIndex(
                name: "idx_audit_timestamp",
                table: "auditlogs",
                column: "timestamp");

            migrationBuilder.CreateIndex(
                name: "banner_display_order_status_key",
                table: "banner",
                columns: new[] { "display_order", "status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_blog_author",
                table: "blog",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_blog_status_published",
                table: "blog",
                columns: new[] { "status", "published_at" });

            migrationBuilder.CreateIndex(
                name: "IX_certificatedoctor_certificate_id",
                table: "certificatedoctor",
                column: "certificate_id");

            migrationBuilder.CreateIndex(
                name: "idx_checkout_appointment",
                table: "checkout",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "idx_checkout_status",
                table: "checkout",
                column: "transaction_status");

            migrationBuilder.CreateIndex(
                name: "claims_account_id_claim_type_claim_value_key",
                table: "claims",
                columns: new[] { "account_id", "claim_type", "claim_value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_claims_account_type",
                table: "claims",
                columns: new[] { "account_id", "claim_type" });

            migrationBuilder.CreateIndex(
                name: "idx_comment_blog",
                table: "commentblog",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_commentblog_author_id",
                table: "commentblog",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_commentblog_parent_comment_id",
                table: "commentblog",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_rank_id",
                table: "customers",
                column: "rank_id");

            migrationBuilder.CreateIndex(
                name: "IX_degreedoctor_degree_id",
                table: "degreedoctor",
                column: "degree_id");

            migrationBuilder.CreateIndex(
                name: "IX_discount_rank_id",
                table: "discount",
                column: "rank_id");

            migrationBuilder.CreateIndex(
                name: "idx_doctors_specialization",
                table: "doctors",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "idx_feedback_doctor_appointment",
                table: "feedbackdoctor",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedbackdoctor_responded_by",
                table: "feedbackdoctor",
                column: "responded_by");

            migrationBuilder.CreateIndex(
                name: "idx_feedback_service_appointment",
                table: "feedbackservice",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedbackservice_responded_by",
                table: "feedbackservice",
                column: "responded_by");

            migrationBuilder.CreateIndex(
                name: "IX_followup_appointment_id",
                table: "followup",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "idx_medical_history_appointment",
                table: "medicalhistory",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "idx_otp_account_type",
                table: "otpservices",
                columns: new[] { "account_id", "otp_type" },
                filter: "(used_at IS NULL)");

            migrationBuilder.CreateIndex(
                name: "permission_permission_name_key",
                table: "permission",
                column: "permission_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissionrole_role_id",
                table: "permissionrole",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_account",
                table: "refreshtokens",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "idx_refresh_tokens_expires",
                table: "refreshtokens",
                column: "expires_at",
                filter: "(revoked_at IS NULL)");

            migrationBuilder.CreateIndex(
                name: "refreshtokens_token_hash_key",
                table: "refreshtokens",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "role_role_name_key",
                table: "role",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_schedules_doctor_date",
                table: "schedules",
                columns: new[] { "doctor_id", "schedule_date" });

            migrationBuilder.CreateIndex(
                name: "IX_schedules_slot_id",
                table: "schedules",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "schedules_doctor_id_slot_id_schedule_date_key",
                table: "schedules",
                columns: new[] { "doctor_id", "slot_id", "schedule_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_services_specialization_id",
                table: "services",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicesdetail_service_id",
                table: "servicesdetail",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_servicesdetail_service_type_id",
                table: "servicesdetail",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_slots_service_type_id",
                table: "slots",
                column: "service_type_id");

            migrationBuilder.CreateIndex(
                name: "slots_start_time_service_type_id_key",
                table: "slots",
                columns: new[] { "start_time", "service_type_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditlogs");

            migrationBuilder.DropTable(
                name: "banner");

            migrationBuilder.DropTable(
                name: "certificatedoctor");

            migrationBuilder.DropTable(
                name: "checkout");

            migrationBuilder.DropTable(
                name: "claims");

            migrationBuilder.DropTable(
                name: "commentblog");

            migrationBuilder.DropTable(
                name: "contentstories");

            migrationBuilder.DropTable(
                name: "degreedoctor");

            migrationBuilder.DropTable(
                name: "feedbackdoctor");

            migrationBuilder.DropTable(
                name: "feedbackservice");

            migrationBuilder.DropTable(
                name: "followup");

            migrationBuilder.DropTable(
                name: "imagesservice");

            migrationBuilder.DropTable(
                name: "machine");

            migrationBuilder.DropTable(
                name: "medicalhistory");

            migrationBuilder.DropTable(
                name: "otpservices");

            migrationBuilder.DropTable(
                name: "permissionrole");

            migrationBuilder.DropTable(
                name: "refreshtokens");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "certificate");

            migrationBuilder.DropTable(
                name: "blog");

            migrationBuilder.DropTable(
                name: "degree");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "appointment");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "slots");

            migrationBuilder.DropTable(
                name: "discount");

            migrationBuilder.DropTable(
                name: "doctors");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "servicesdetail");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "customerrank");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "servicestype");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "specialization");
        }
    }
}
