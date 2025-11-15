using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Data;

public partial class VisionCareDbContext : DbContext
{
    public VisionCareDbContext() { }

    public VisionCareDbContext(DbContextOptions<VisionCareDbContext> options)
        : base(options) { }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Aggregatedcounter> Aggregatedcounters { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Certificatedoctor> Certificatedoctors { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<Commentblog> Commentblogs { get; set; }

    public virtual DbSet<Contentstory> Contentstories { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerrank> Customerranks { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<Degreedoctor> Degreedoctors { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Doctorabsence> Doctorabsences { get; set; }

    public virtual DbSet<Doctorschedule> Doctorschedules { get; set; }

    public virtual DbSet<Encounter> Encounters { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Feedbackdoctor> Feedbackdoctors { get; set; }

    public virtual DbSet<Feedbackservice> Feedbackservices { get; set; }

    public virtual DbSet<Followup> Followups { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Imagesservice> Imagesservices { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Jobparameter> Jobparameters { get; set; }

    public virtual DbSet<Jobqueue> Jobqueues { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<VisionCare.Infrastructure.Models.Lock> Locks { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<Medicalhistory> Medicalhistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Otpservice> Otpservices { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Permissionrole> Permissionroles { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Prescriptionline> Prescriptionlines { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Sectioncontent> Sectioncontents { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicesdetail> Servicesdetails { get; set; }

    public virtual DbSet<Servicestype> Servicestypes { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Weeklyschedule> Weeklyschedules { get; set; }

    public virtual DbSet<Workshift> Workshifts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Name=DefaultConnection");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Email, "accounts_email_key").IsUnique();

            entity.HasIndex(e => e.FacebookId, "accounts_facebook_id_key").IsUnique();

            entity.HasIndex(e => e.GoogleId, "accounts_google_id_key").IsUnique();

            entity.HasIndex(e => e.Username, "accounts_username_key").IsUnique();

            entity.HasIndex(e => e.Email, "idx_accounts_email");

            entity
                .HasIndex(e => e.Email, "idx_accounts_email_lookup")
                .HasFilter("((status)::text = 'Active'::text)");

            entity
                .HasIndex(e => e.FacebookId, "idx_accounts_facebook_id")
                .HasFilter("(facebook_id IS NOT NULL)");

            entity
                .HasIndex(e => e.GoogleId, "idx_accounts_google_id")
                .HasFilter("(google_id IS NOT NULL)");

            entity.HasIndex(e => e.RoleId, "idx_accounts_role");

            entity
                .HasIndex(e => e.RoleId, "idx_accounts_role_lookup")
                .HasFilter("((status)::text = 'Active'::text)");

            entity.HasIndex(e => e.Status, "idx_accounts_status");

            entity.HasIndex(e => e.Username, "idx_accounts_username");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity
                .Property(e => e.AccessFailedCount)
                .HasDefaultValue((short)0)
                .HasColumnName("access_failed_count");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
            entity
                .Property(e => e.EmailConfirmationToken)
                .HasMaxLength(255)
                .HasColumnName("email_confirmation_token");
            entity
                .Property(e => e.EmailConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("email_confirmed");
            entity.Property(e => e.FacebookId).HasMaxLength(255).HasColumnName("facebook_id");
            entity.Property(e => e.GoogleId).HasMaxLength(255).HasColumnName("google_id");
            entity
                .Property(e => e.LastLogin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login");
            entity
                .Property(e => e.LastPasswordChange)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_password_change");
            entity
                .Property(e => e.LockoutEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lockout_end");
            entity.Property(e => e.PasswordHash).HasMaxLength(255).HasColumnName("password_hash");
            entity
                .Property(e => e.PasswordResetExpires)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("password_reset_expires");
            entity
                .Property(e => e.PasswordResetToken)
                .HasMaxLength(255)
                .HasColumnName("password_reset_token");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username).HasMaxLength(100).HasColumnName("username");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accounts_role_id_fkey");
        });

        modelBuilder.Entity<Aggregatedcounter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aggregatedcounter_pkey");

            entity.ToTable("aggregatedcounter", "hangfire");

            entity.HasIndex(e => e.Key, "aggregatedcounter_key_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.AppointmentCode, "appointment_appointment_code_key").IsUnique();

            entity.HasIndex(e => e.AppointmentCode, "idx_appointment_code");

            entity.HasIndex(e => e.AppointmentDatetime, "idx_appointments_datetime");

            entity.HasIndex(e => e.DoctorId, "idx_appointments_doctor");

            entity.HasIndex(e => e.PatientId, "idx_appointments_patient");

            entity.HasIndex(e => e.Status, "idx_appointments_status");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ActualCost).HasPrecision(10, 2).HasColumnName("actual_cost");
            entity
                .Property(e => e.AppointmentCode)
                .HasMaxLength(20)
                .HasColumnName("appointment_code");
            entity
                .Property(e => e.AppointmentDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("appointment_datetime");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity
                .Property(e => e.PaymentStatus)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Unpaid'::character varying")
                .HasColumnName("payment_status");
            entity.Property(e => e.ServiceDetailId).HasColumnName("service_detail_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Scheduled'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("appointment_created_by_fkey");

            entity
                .HasOne(d => d.Discount)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("appointment_discount_id_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_doctor_id_fkey");

            entity
                .HasOne(d => d.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_patient_id_fkey");

            entity
                .HasOne(d => d.ServiceDetail)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_service_detail_id_fkey");
        });

        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("auditlogs_pkey");

            entity.ToTable("auditlogs");

            entity.HasIndex(e => e.AccountId, "idx_audit_account");

            entity.HasIndex(e => new { e.Resource, e.ResourceId }, "idx_audit_resource");

            entity.HasIndex(e => e.Timestamp, "idx_audit_timestamp");

            entity.Property(e => e.AuditId).HasColumnName("audit_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Action).HasMaxLength(100).HasColumnName("action");
            entity.Property(e => e.Details).HasColumnType("jsonb").HasColumnName("details");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            entity.Property(e => e.Resource).HasMaxLength(50).HasColumnName("resource");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.Success).HasColumnName("success");
            entity
                .Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("auditlogs_account_id_fkey");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("banner_pkey");

            entity.ToTable("banner");

            entity
                .HasIndex(e => new { e.DisplayOrder, e.Status }, "banner_display_order_status_key")
                .IsUnique();

            entity.Property(e => e.BannerId).HasColumnName("banner_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0).HasColumnName("display_order");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ImageUrl).HasMaxLength(255).HasColumnName("image_url");
            entity.Property(e => e.LinkUrl).HasMaxLength(255).HasColumnName("link_url");
            entity
                .Property(e => e.StartDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("start_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Title).HasMaxLength(255).HasColumnName("title");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("blog_pkey");

            entity.ToTable("blog");

            entity.HasIndex(e => e.Slug, "blog_slug_key").IsUnique();

            entity.HasIndex(e => e.AuthorId, "idx_blog_author");

            entity.HasIndex(e => e.Slug, "idx_blog_slug").IsUnique();

            entity.HasIndex(e => new { e.Status, e.PublishedAt }, "idx_blog_status_published");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Excerpt).HasColumnName("excerpt");
            entity.Property(e => e.FeaturedImage).HasMaxLength(255).HasColumnName("featured_image");
            entity
                .Property(e => e.PublishedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("published_at");
            entity.Property(e => e.Slug).HasMaxLength(500).HasColumnName("slug");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Draft'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Title).HasMaxLength(500).HasColumnName("title");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.ViewCount).HasDefaultValue(0).HasColumnName("view_count");

            entity
                .HasOne(d => d.Author)
                .WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("blog_author_id_fkey");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("certificate_pkey");

            entity.ToTable("certificate");

            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Certificatedoctor>(entity =>
        {
            entity
                .HasKey(e => new { e.DoctorId, e.CertificateId })
                .HasName("certificatedoctor_pkey");

            entity.ToTable("certificatedoctor");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity
                .Property(e => e.CertificateImage)
                .HasMaxLength(255)
                .HasColumnName("certificate_image");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.IssuedBy).HasMaxLength(255).HasColumnName("issued_by");
            entity.Property(e => e.IssuedDate).HasColumnName("issued_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Certificate)
                .WithMany(p => p.Certificatedoctors)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certificatedoctor_certificate_id_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Certificatedoctors)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("certificatedoctor_doctor_id_fkey");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.CheckoutId).HasName("checkout_pkey");

            entity.ToTable("checkout");

            entity.HasIndex(e => e.AppointmentId, "idx_checkout_appointment");

            entity.HasIndex(e => e.GatewayTransactionId, "idx_checkout_gateway_txn");

            entity.HasIndex(e => e.TransactionStatus, "idx_checkout_status");

            entity.Property(e => e.CheckoutId).HasColumnName("checkout_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.GatewayResponse)
                .HasColumnType("jsonb")
                .HasColumnName("gateway_response");
            entity
                .Property(e => e.GatewayTransactionId)
                .HasMaxLength(255)
                .HasColumnName("gateway_transaction_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity
                .Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("payment_date");
            entity
                .Property(e => e.PaymentGateway)
                .HasMaxLength(50)
                .HasColumnName("payment_gateway");
            entity.Property(e => e.RefundAmount).HasPrecision(10, 2).HasColumnName("refund_amount");
            entity
                .Property(e => e.RefundCompletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refund_completed_at");
            entity.Property(e => e.RefundReason).HasColumnName("refund_reason");
            entity
                .Property(e => e.RefundRequestedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refund_requested_at");
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2).HasColumnName("total_amount");
            entity
                .Property(e => e.TransactionCode)
                .HasMaxLength(100)
                .HasColumnName("transaction_code");
            entity
                .Property(e => e.TransactionStatus)
                .HasMaxLength(30)
                .HasColumnName("transaction_status");
            entity
                .Property(e => e.TransactionType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'VNPay'::character varying")
                .HasColumnName("transaction_type");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("checkout_appointment_id_fkey");
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("claims_pkey");

            entity.ToTable("claims");

            entity
                .HasIndex(
                    e => new
                    {
                        e.AccountId,
                        e.ClaimType,
                        e.ClaimValue,
                    },
                    "claims_account_id_claim_type_claim_value_key"
                )
                .IsUnique();

            entity.HasIndex(e => new { e.AccountId, e.ClaimType }, "idx_claims_account_type");

            entity.Property(e => e.ClaimId).HasColumnName("claim_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ClaimType).HasMaxLength(50).HasColumnName("claim_type");
            entity.Property(e => e.ClaimValue).HasMaxLength(255).HasColumnName("claim_value");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity
                .Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.Claims)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("claims_account_id_fkey");
        });

        modelBuilder.Entity<Commentblog>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("commentblog_pkey");

            entity.ToTable("commentblog");

            entity.HasIndex(e => e.BlogId, "idx_comment_blog");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Author)
                .WithMany(p => p.Commentblogs)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("commentblog_author_id_fkey");

            entity
                .HasOne(d => d.Blog)
                .WithMany(p => p.Commentblogs)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("commentblog_blog_id_fkey");

            entity
                .HasOne(d => d.ParentComment)
                .WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("commentblog_parent_comment_id_fkey");
        });

        modelBuilder.Entity<Contentstory>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("contentstories_pkey");

            entity.ToTable("contentstories");

            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0).HasColumnName("display_order");
            entity.Property(e => e.PatientImage).HasMaxLength(255).HasColumnName("patient_image");
            entity.Property(e => e.PatientName).HasMaxLength(255).HasColumnName("patient_name");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.StoryContent).HasColumnName("story_content");
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("counter_pkey");

            entity.ToTable("counter", "hangfire");

            entity.HasIndex(e => e.Expireat, "ix_hangfire_counter_expireat");

            entity.HasIndex(e => e.Key, "ix_hangfire_counter_key");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.AccountId).ValueGeneratedNever().HasColumnName("account_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Avatar).HasMaxLength(255).HasColumnName("avatar");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.FullName).HasMaxLength(255).HasColumnName("full_name");
            entity.Property(e => e.Gender).HasMaxLength(10).HasColumnName("gender");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
            entity.Property(e => e.RankId).HasColumnName("rank_id");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .HasConstraintName("customers_account_id_fkey");

            entity
                .HasOne(d => d.Rank)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("customers_rank_id_fkey");
        });

        modelBuilder.Entity<Customerrank>(entity =>
        {
            entity.HasKey(e => e.RankId).HasName("customerrank_pkey");

            entity.ToTable("customerrank");

            entity.Property(e => e.RankId).HasColumnName("rank_id");
            entity.Property(e => e.MinAmount).HasPrecision(12, 2).HasColumnName("min_amount");
            entity.Property(e => e.RankName).HasMaxLength(50).HasColumnName("rank_name");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.DegreeId).HasName("degree_pkey");

            entity.ToTable("degree");

            entity.Property(e => e.DegreeId).HasColumnName("degree_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<Degreedoctor>(entity =>
        {
            entity.HasKey(e => new { e.DoctorId, e.DegreeId }).HasName("degreedoctor_pkey");

            entity.ToTable("degreedoctor");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.DegreeId).HasColumnName("degree_id");
            entity
                .Property(e => e.CertificateImage)
                .HasMaxLength(255)
                .HasColumnName("certificate_image");
            entity.Property(e => e.IssuedBy).HasMaxLength(255).HasColumnName("issued_by");
            entity.Property(e => e.IssuedDate).HasColumnName("issued_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Degree)
                .WithMany(p => p.Degreedoctors)
                .HasForeignKey(d => d.DegreeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("degreedoctor_degree_id_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Degreedoctors)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("degreedoctor_doctor_id_fkey");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("discount_pkey");

            entity.ToTable("discount");

            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.DiscountName).HasMaxLength(100).HasColumnName("discount_name");
            entity
                .Property(e => e.DiscountPercent)
                .HasPrecision(5, 2)
                .HasColumnName("discount_percent");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.RankId).HasColumnName("rank_id");
            entity
                .Property(e => e.StartDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("start_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Rank)
                .WithMany(p => p.Discounts)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("discount_rank_id_fkey");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("doctors_pkey");

            entity.ToTable("doctors");

            entity.HasIndex(e => e.SpecializationId, "idx_doctors_specialization");

            entity.Property(e => e.AccountId).ValueGeneratedNever().HasColumnName("account_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Avatar).HasMaxLength(255).HasColumnName("avatar");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.FullName).HasMaxLength(255).HasColumnName("full_name");
            entity.Property(e => e.Gender).HasMaxLength(10).HasColumnName("gender");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
            entity
                .Property(e => e.Rating)
                .HasPrecision(2, 1)
                .HasDefaultValueSql("0.0")
                .HasColumnName("rating");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .HasConstraintName("doctors_account_id_fkey");

            entity
                .HasOne(d => d.Specialization)
                .WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctors_specialization_id_fkey");
        });

        modelBuilder.Entity<Doctorabsence>(entity =>
        {
            entity.HasKey(e => e.AbsenceId).HasName("doctorabsence_pkey");

            entity.ToTable("doctorabsence");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "idx_doctor_absence_dates");

            entity.HasIndex(e => e.DoctorId, "idx_doctor_absence_doctor");

            entity.HasIndex(e => e.Status, "idx_doctor_absence_status");

            entity.Property(e => e.AbsenceId).HasColumnName("absence_id");
            entity
                .Property(e => e.AbsenceType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Leave'::character varying")
                .HasColumnName("absence_type");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsResolved).HasDefaultValue(false).HasColumnName("is_resolved");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Doctorabsences)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("doctorabsence_doctor_id_fkey");
        });

        modelBuilder.Entity<Doctorschedule>(entity =>
        {
            entity.HasKey(e => e.DoctorScheduleId).HasName("doctorschedule_pkey");

            entity.ToTable("doctorschedule");

            entity
                .HasIndex(e => e.IsActive, "idx_doctor_schedules_active")
                .HasFilter("(is_active = true)");

            entity.HasIndex(e => e.DoctorId, "idx_doctor_schedules_doctor");

            entity.HasIndex(e => e.ShiftId, "idx_doctor_schedules_shift");

            entity.Property(e => e.DoctorScheduleId).HasColumnName("doctor_schedule_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");
            entity
                .Property(e => e.RecurrenceRule)
                .HasMaxLength(50)
                .HasDefaultValueSql("'WEEKLY'::character varying")
                .HasColumnName("recurrence_rule");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Doctorschedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("doctorschedule_doctor_id_fkey");

            entity
                .HasOne(d => d.Equipment)
                .WithMany(p => p.Doctorschedules)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("doctorschedule_equipment_id_fkey");

            entity
                .HasOne(d => d.Room)
                .WithMany(p => p.Doctorschedules)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("doctorschedule_room_id_fkey");

            entity
                .HasOne(d => d.Shift)
                .WithMany(p => p.Doctorschedules)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("doctorschedule_shift_id_fkey");
        });

        modelBuilder.Entity<Encounter>(entity =>
        {
            entity.HasKey(e => e.EncounterId).HasName("encounters_pkey");

            entity.ToTable("encounters");

            entity.HasIndex(e => e.AppointmentId, "idx_encounters_appointment");

            entity.HasIndex(e => e.DoctorId, "idx_encounters_doctor");

            entity.Property(e => e.EncounterId).HasColumnName("encounter_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Assessment).HasColumnName("assessment");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Objective).HasColumnName("objective");
            entity.Property(e => e.Plan).HasColumnName("plan");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Draft'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Subjective).HasColumnName("subjective");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Encounters)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("fk_encounter_appointment");

            entity
                .HasOne(d => d.Customer)
                .WithMany(p => p.Encounters)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_encounter_customer");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Encounters)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_encounter_doctor");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("equipment_pkey");

            entity.ToTable("equipment");

            entity.HasIndex(e => e.Location, "idx_equipment_location");

            entity.HasIndex(e => e.Manufacturer, "idx_equipment_manufacturer");

            entity.HasIndex(e => e.Name, "idx_equipment_name");

            entity.HasIndex(e => e.Status, "idx_equipment_status");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LastMaintenanceDate).HasColumnName("last_maintenance_date");
            entity.Property(e => e.Location).HasMaxLength(255).HasColumnName("location");
            entity.Property(e => e.Manufacturer).HasMaxLength(255).HasColumnName("manufacturer");
            entity.Property(e => e.Model).HasMaxLength(255).HasColumnName("model");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");
            entity.Property(e => e.SerialNumber).HasMaxLength(255).HasColumnName("serial_number");
            entity
                .Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Feedbackdoctor>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("feedbackdoctor_pkey");

            entity.ToTable("feedbackdoctor");

            entity.HasIndex(e => e.AppointmentId, "idx_feedback_doctor_appointment");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.FeedbackDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("feedback_date");
            entity.Property(e => e.FeedbackText).HasColumnName("feedback_text");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RespondedBy).HasColumnName("responded_by");
            entity
                .Property(e => e.ResponseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("response_date");
            entity.Property(e => e.ResponseText).HasColumnName("response_text");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Feedbackdoctors)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("feedbackdoctor_appointment_id_fkey");

            entity
                .HasOne(d => d.RespondedByNavigation)
                .WithMany(p => p.Feedbackdoctors)
                .HasForeignKey(d => d.RespondedBy)
                .HasConstraintName("feedbackdoctor_responded_by_fkey");
        });

        modelBuilder.Entity<Feedbackservice>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("feedbackservice_pkey");

            entity.ToTable("feedbackservice");

            entity.HasIndex(e => e.AppointmentId, "idx_feedback_service_appointment");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.FeedbackDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("feedback_date");
            entity.Property(e => e.FeedbackText).HasColumnName("feedback_text");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RespondedBy).HasColumnName("responded_by");
            entity
                .Property(e => e.ResponseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("response_date");
            entity.Property(e => e.ResponseText).HasColumnName("response_text");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Feedbackservices)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("feedbackservice_appointment_id_fkey");

            entity
                .HasOne(d => d.RespondedByNavigation)
                .WithMany(p => p.Feedbackservices)
                .HasForeignKey(d => d.RespondedBy)
                .HasConstraintName("feedbackservice_responded_by_fkey");
        });

        modelBuilder.Entity<Followup>(entity =>
        {
            entity.HasKey(e => e.FollowUpId).HasName("followup_pkey");

            entity.ToTable("followup");

            entity.Property(e => e.FollowUpId).HasColumnName("follow_up_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.NextAppointmentDate).HasColumnName("next_appointment_date");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Followups)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("followup_appointment_id_fkey");
        });

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hash_pkey");

            entity.ToTable("hash", "hangfire");

            entity.HasIndex(e => new { e.Key, e.Field }, "hash_key_field_key").IsUnique();

            entity.HasIndex(e => e.Expireat, "ix_hangfire_hash_expireat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity.Property(e => e.Field).HasColumnName("field");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Imagesservice>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("imagesservice_pkey");

            entity.ToTable("imagesservice");

            entity.Property(e => e.ServiceId).ValueGeneratedNever().HasColumnName("service_id");
            entity.Property(e => e.ImageAfter).HasMaxLength(255).HasColumnName("image_after");
            entity.Property(e => e.ImageBefore).HasMaxLength(255).HasColumnName("image_before");
            entity.Property(e => e.ImageMain).HasMaxLength(255).HasColumnName("image_main");

            entity
                .HasOne(d => d.Service)
                .WithOne(p => p.Imagesservice)
                .HasForeignKey<Imagesservice>(d => d.ServiceId)
                .HasConstraintName("imagesservice_service_id_fkey");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_pkey");

            entity.ToTable("job", "hangfire");

            entity.HasIndex(e => e.Expireat, "ix_hangfire_job_expireat");

            entity.HasIndex(e => e.Statename, "ix_hangfire_job_statename");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Arguments).HasColumnType("jsonb").HasColumnName("arguments");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity
                .Property(e => e.Invocationdata)
                .HasColumnType("jsonb")
                .HasColumnName("invocationdata");
            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Statename).HasColumnName("statename");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
        });

        modelBuilder.Entity<Jobparameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobparameter_pkey");

            entity.ToTable("jobparameter", "hangfire");

            entity.HasIndex(e => new { e.Jobid, e.Name }, "ix_hangfire_jobparameter_jobidandname");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Jobid).HasColumnName("jobid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
            entity.Property(e => e.Value).HasColumnName("value");

            entity
                .HasOne(d => d.Job)
                .WithMany(p => p.Jobparameters)
                .HasForeignKey(d => d.Jobid)
                .HasConstraintName("jobparameter_jobid_fkey");
        });

        modelBuilder.Entity<Jobqueue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jobqueue_pkey");

            entity.ToTable("jobqueue", "hangfire");

            entity
                .HasIndex(
                    e => new
                    {
                        e.Fetchedat,
                        e.Queue,
                        e.Jobid,
                    },
                    "ix_hangfire_jobqueue_fetchedat_queue_jobid"
                )
                .HasNullSortOrder(
                    new[]
                    {
                        NullSortOrder.NullsFirst,
                        NullSortOrder.NullsLast,
                        NullSortOrder.NullsLast,
                    }
                );

            entity.HasIndex(e => new { e.Jobid, e.Queue }, "ix_hangfire_jobqueue_jobidandqueue");

            entity.HasIndex(
                e => new { e.Queue, e.Fetchedat },
                "ix_hangfire_jobqueue_queueandfetchedat"
            );

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fetchedat).HasColumnName("fetchedat");
            entity.Property(e => e.Jobid).HasColumnName("jobid");
            entity.Property(e => e.Queue).HasColumnName("queue");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_pkey");

            entity.ToTable("list", "hangfire");

            entity.HasIndex(e => e.Expireat, "ix_hangfire_list_expireat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<VisionCare.Infrastructure.Models.Lock>(entity =>
        {
            entity.HasNoKey().ToTable("lock", "hangfire");

            entity.HasIndex(e => e.Resource, "lock_resource_key").IsUnique();

            entity.Property(e => e.Acquired).HasColumnName("acquired");
            entity.Property(e => e.Resource).HasColumnName("resource");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_pkey");

            entity.ToTable("machine");

            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasMaxLength(255).HasColumnName("image_url");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity.Property(e => e.Specifications).HasColumnName("specifications");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Medicalhistory>(entity =>
        {
            entity.HasKey(e => e.MedicalHistoryId).HasName("medicalhistory_pkey");

            entity.ToTable("medicalhistory");

            entity.HasIndex(e => e.AppointmentId, "idx_medical_history_appointment");

            entity.Property(e => e.MedicalHistoryId).HasColumnName("medical_history_id");
            entity.Property(e => e.AdditionalTests).HasColumnName("additional_tests");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Prescription).HasColumnName("prescription");
            entity.Property(e => e.Symptoms).HasColumnName("symptoms");
            entity.Property(e => e.Treatment).HasColumnName("treatment");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.VisionLeft).HasPrecision(3, 2).HasColumnName("vision_left");
            entity.Property(e => e.VisionRight).HasPrecision(3, 2).HasColumnName("vision_right");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Medicalhistories)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("medicalhistory_appointment_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => e.EncounterId, "idx_orders_encounter");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EncounterId).HasColumnName("encounter_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.OrderType).HasMaxLength(50).HasColumnName("order_type");
            entity.Property(e => e.ResultUrl).HasColumnName("result_url");
            entity
                .Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Requested'::character varying")
                .HasColumnName("status");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Encounter)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.EncounterId)
                .HasConstraintName("fk_order_encounter");
        });

        modelBuilder.Entity<Otpservice>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("otpservices_pkey");

            entity.ToTable("otpservices");

            entity
                .HasIndex(e => new { e.AccountId, e.OtpType }, "idx_otp_account_type")
                .HasFilter("(used_at IS NULL)");

            entity.Property(e => e.OtpId).HasColumnName("otp_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Attempts).HasDefaultValue((short)0).HasColumnName("attempts");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity
                .Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");
            entity.Property(e => e.OtpHash).HasMaxLength(255).HasColumnName("otp_hash");
            entity.Property(e => e.OtpType).HasMaxLength(30).HasColumnName("otp_type");
            entity
                .Property(e => e.UsedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("used_at");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.Otpservices)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("otpservices_account_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.HasIndex(e => e.PermissionName, "permission_permission_name_key").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Action).HasMaxLength(20).HasColumnName("action");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity
                .Property(e => e.PermissionName)
                .HasMaxLength(100)
                .HasColumnName("permission_name");
            entity.Property(e => e.Resource).HasMaxLength(50).HasColumnName("resource");
        });

        modelBuilder.Entity<Permissionrole>(entity =>
        {
            entity.HasKey(e => new { e.PermissionId, e.RoleId }).HasName("permissionrole_pkey");

            entity.ToTable("permissionrole");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity
                .Property(e => e.GrantedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("granted_at");

            entity
                .HasOne(d => d.Permission)
                .WithMany(p => p.Permissionroles)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("permissionrole_permission_id_fkey");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.Permissionroles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("permissionrole_role_id_fkey");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("prescriptions_pkey");

            entity.ToTable("prescriptions");

            entity.HasIndex(e => e.EncounterId, "idx_prescriptions_encounter");

            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EncounterId).HasColumnName("encounter_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Encounter)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.EncounterId)
                .HasConstraintName("fk_prescription_encounter");
        });

        modelBuilder.Entity<Prescriptionline>(entity =>
        {
            entity.HasKey(e => e.LineId).HasName("prescriptionlines_pkey");

            entity.ToTable("prescriptionlines");

            entity.HasIndex(e => e.PrescriptionId, "idx_lines_prescription");

            entity.Property(e => e.LineId).HasColumnName("line_id");
            entity.Property(e => e.Dosage).HasMaxLength(100).HasColumnName("dosage");
            entity.Property(e => e.DrugCode).HasMaxLength(100).HasColumnName("drug_code");
            entity.Property(e => e.DrugName).HasMaxLength(255).HasColumnName("drug_name");
            entity.Property(e => e.Duration).HasMaxLength(100).HasColumnName("duration");
            entity.Property(e => e.Frequency).HasMaxLength(100).HasColumnName("frequency");
            entity.Property(e => e.Instructions).HasColumnName("instructions");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");

            entity
                .HasOne(d => d.Prescription)
                .WithMany(p => p.Prescriptionlines)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("fk_line_prescription");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("refreshtokens_pkey");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.AccountId, "idx_refresh_tokens_account");

            entity
                .HasIndex(e => e.ExpiresAt, "idx_refresh_tokens_expires")
                .HasFilter("(revoked_at IS NULL)");

            entity
                .HasIndex(e => e.AccountId, "idx_refreshtokens_account_lookup")
                .HasFilter("(revoked_at IS NULL)");

            entity
                .HasIndex(e => e.ExpiresAt, "idx_refreshtokens_cleanup")
                .HasFilter("(revoked_at IS NULL)");

            entity
                .HasIndex(e => e.TokenHash, "idx_refreshtokens_hash")
                .HasFilter("(revoked_at IS NULL)");

            entity
                .HasIndex(e => new { e.RevokedAt, e.ExpiresAt }, "idx_refreshtokens_valid")
                .HasFilter("(revoked_at IS NULL)");

            entity.HasIndex(e => e.TokenHash, "refreshtokens_token_hash_key").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip");
            entity
                .Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");
            entity
                .Property(e => e.RevokedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("revoked_at");
            entity.Property(e => e.TokenHash).HasMaxLength(255).HasColumnName("token_hash");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("refreshtokens_account_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleName, "role_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleDescription).HasColumnName("role_description");
            entity.Property(e => e.RoleName).HasMaxLength(50).HasColumnName("role_name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.HasIndex(e => e.Status, "idx_rooms_status");

            entity.HasIndex(e => e.RoomCode, "rooms_room_code_key").IsUnique();

            entity.HasIndex(e => e.RoomName, "rooms_room_name_key").IsUnique();

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Capacity).HasDefaultValue(1).HasColumnName("capacity");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Location).HasMaxLength(255).HasColumnName("location");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RoomCode).HasMaxLength(20).HasColumnName("room_code");
            entity.Property(e => e.RoomName).HasMaxLength(100).HasColumnName("room_name");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("schedules_pkey");

            entity.ToTable("schedules");

            entity.HasIndex(e => e.ScheduleDate, "idx_schedules_date");

            entity.HasIndex(e => e.DoctorId, "idx_schedules_doctor");

            entity.HasIndex(e => new { e.DoctorId, e.ScheduleDate }, "idx_schedules_doctor_date");

            entity
                .HasIndex(e => e.EquipmentId, "idx_schedules_equipment")
                .HasFilter("(equipment_id IS NOT NULL)");

            entity.HasIndex(e => e.RoomId, "idx_schedules_room").HasFilter("(room_id IS NOT NULL)");

            entity.HasIndex(e => e.Status, "idx_schedules_status");

            entity
                .HasIndex(
                    e => new
                    {
                        e.DoctorId,
                        e.SlotId,
                        e.ScheduleDate,
                    },
                    "schedules_doctor_id_slot_id_schedule_date_key"
                )
                .IsUnique();

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.ScheduleDate).HasColumnName("schedule_date");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Available'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("schedules_doctor_id_fkey");

            entity
                .HasOne(d => d.Equipment)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.EquipmentId)
                .HasConstraintName("schedules_equipment_id_fkey");

            entity
                .HasOne(d => d.Room)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("schedules_room_id_fkey");

            entity
                .HasOne(d => d.Slot)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedules_slot_id_fkey");
        });

        modelBuilder.Entity<Schema>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("schema_pkey");

            entity.ToTable("schema", "hangfire");

            entity.Property(e => e.Version).ValueGeneratedNever().HasColumnName("version");
        });

        modelBuilder.Entity<Sectioncontent>(entity =>
        {
            entity.HasKey(e => e.SectionKey).HasName("sectioncontent_pkey");

            entity.ToTable("sectioncontent");

            entity.Property(e => e.SectionKey).HasMaxLength(100).HasColumnName("section_key");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ImageUrl).HasMaxLength(255).HasColumnName("image_url");
            entity.Property(e => e.MoreData).HasColumnType("jsonb").HasColumnName("more_data");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_pkey");

            entity.ToTable("server", "hangfire");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data).HasColumnType("jsonb").HasColumnName("data");
            entity.Property(e => e.Lastheartbeat).HasColumnName("lastheartbeat");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Benefits).HasColumnName("benefits");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");

            entity
                .HasOne(d => d.Specialization)
                .WithMany(p => p.Services)
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("services_specialization_id_fkey");
        });

        modelBuilder.Entity<Servicesdetail>(entity =>
        {
            entity.HasKey(e => e.ServiceDetailId).HasName("servicesdetail_pkey");

            entity.ToTable("servicesdetail");

            entity.Property(e => e.ServiceDetailId).HasColumnName("service_detail_id");
            entity.Property(e => e.Cost).HasPrecision(10, 2).HasColumnName("cost");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");

            entity
                .HasOne(d => d.Service)
                .WithMany(p => p.Servicesdetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("servicesdetail_service_id_fkey");

            entity
                .HasOne(d => d.ServiceType)
                .WithMany(p => p.Servicesdetails)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicesdetail_service_type_id_fkey");
        });

        modelBuilder.Entity<Servicestype>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("servicestype_pkey");

            entity.ToTable("servicestype");

            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("set_pkey");

            entity.ToTable("set", "hangfire");

            entity.HasIndex(e => e.Expireat, "ix_hangfire_set_expireat");

            entity.HasIndex(e => new { e.Key, e.Score }, "ix_hangfire_set_key_score");

            entity.HasIndex(e => new { e.Key, e.Value }, "set_key_value_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expireat).HasColumnName("expireat");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("slots_pkey");

            entity.ToTable("slots");

            entity
                .HasIndex(
                    e => new { e.StartTime, e.ServiceTypeId },
                    "slots_start_time_service_type_id_key"
                )
                .IsUnique();

            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity
                .HasOne(d => d.ServiceType)
                .WithMany(p => p.Slots)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("slots_service_type_id_fkey");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("specialization_pkey");

            entity.ToTable("specialization");

            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            entity
                .Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Active'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("staff_pkey");

            entity.ToTable("staff");

            entity.Property(e => e.AccountId).ValueGeneratedNever().HasColumnName("account_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Avatar).HasMaxLength(255).HasColumnName("avatar");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.FullName).HasMaxLength(255).HasColumnName("full_name");
            entity.Property(e => e.Gender).HasMaxLength(10).HasColumnName("gender");
            entity
                .Property(e => e.HiredDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("hired_date");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
            entity.Property(e => e.Salary).HasPrecision(12, 2).HasColumnName("salary");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.AccountId)
                .HasConstraintName("staff_account_id_fkey");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("state_pkey");

            entity.ToTable("state", "hangfire");

            entity.HasIndex(e => e.Jobid, "ix_hangfire_state_jobid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Data).HasColumnType("jsonb").HasColumnName("data");
            entity.Property(e => e.Jobid).HasColumnName("jobid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Updatecount).HasDefaultValue(0).HasColumnName("updatecount");

            entity
                .HasOne(d => d.Job)
                .WithMany(p => p.States)
                .HasForeignKey(d => d.Jobid)
                .HasConstraintName("state_jobid_fkey");
        });

        modelBuilder.Entity<Weeklyschedule>(entity =>
        {
            entity.HasKey(e => e.WeeklyScheduleId).HasName("weeklyschedule_pkey");

            entity.ToTable("weeklyschedule");

            entity.HasIndex(e => e.DoctorId, "idx_weekly_schedules_doctor");

            entity
                .HasIndex(
                    e => new
                    {
                        e.DoctorId,
                        e.DayOfWeek,
                        e.SlotId,
                    },
                    "weeklyschedule_doctor_id_day_of_week_slot_id_key"
                )
                .IsUnique();

            entity.Property(e => e.WeeklyScheduleId).HasColumnName("weekly_schedule_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity
                .Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Weeklyschedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("weeklyschedule_doctor_id_fkey");

            entity
                .HasOne(d => d.Slot)
                .WithMany(p => p.Weeklyschedules)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("weeklyschedule_slot_id_fkey");
        });

        modelBuilder.Entity<Workshift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("workshift_pkey");

            entity.ToTable("workshift");

            entity
                .HasIndex(e => e.IsActive, "idx_work_shift_active")
                .HasFilter("(is_active = true)");

            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("is_active");
            entity.Property(e => e.ShiftName).HasMaxLength(100).HasColumnName("shift_name");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity
                .Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
