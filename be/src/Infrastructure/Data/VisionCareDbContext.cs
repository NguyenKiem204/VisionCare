using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Data;

public partial class VisionCareDbContext : DbContext
{
    public VisionCareDbContext() { }

    public VisionCareDbContext(DbContextOptions<VisionCareDbContext> options)
        : base(options) { }

    public virtual DbSet<Account> Accounts { get; set; }

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

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerrank> Customerranks { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<Degreedoctor> Degreedoctors { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Feedbackdoctor> Feedbackdoctors { get; set; }

    public virtual DbSet<Feedbackservice> Feedbackservices { get; set; }

    public virtual DbSet<Followup> Followups { get; set; }

    public virtual DbSet<Imagesservice> Imagesservices { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<Medicalhistory> Medicalhistories { get; set; }

    public virtual DbSet<Otpservice> Otpservices { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Permissionrole> Permissionroles { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicesdetail> Servicesdetails { get; set; }

    public virtual DbSet<Servicestype> Servicestypes { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

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

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.AppointmentDatetime, "idx_appointments_datetime");

            entity.HasIndex(e => e.DoctorId, "idx_appointments_doctor");

            entity.HasIndex(e => e.PatientId, "idx_appointments_patient");

            entity.HasIndex(e => e.Status, "idx_appointments_status");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ActualCost).HasPrecision(10, 2).HasColumnName("actual_cost");
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

            entity.HasIndex(e => e.AuthorId, "idx_blog_author");

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

            entity.HasIndex(e => e.TransactionStatus, "idx_checkout_status");

            entity.Property(e => e.CheckoutId).HasColumnName("checkout_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity
                .Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("payment_date");
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

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("schedules_pkey");

            entity.ToTable("schedules");

            entity.HasIndex(e => new { e.DoctorId, e.ScheduleDate }, "idx_schedules_doctor_date");

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
                .HasOne(d => d.Slot)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedules_slot_id_fkey");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
