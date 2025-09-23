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

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<CertificateDoctor> CertificateDoctors { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Commentblog> Commentblogs { get; set; }

    public virtual DbSet<ContentStory> ContentStories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customerrank> Customerranks { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<DegreeDoctor> DegreeDoctors { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<FeedbackDoctor> FeedbackDoctors { get; set; }

    public virtual DbSet<FeedbackService> FeedbackServices { get; set; }

    public virtual DbSet<FollowUp> FollowUps { get; set; }

    public virtual DbSet<ImagesService> ImagesServices { get; set; }

    public virtual DbSet<ImagesType> ImagesTypes { get; set; }

    public virtual DbSet<ImagesVideo> ImagesVideos { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<Medicalhistory> Medicalhistories { get; set; }

    public virtual DbSet<OtpService> OtpServices { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicesDetail> ServicesDetails { get; set; }

    public virtual DbSet<ServicesType> ServicesTypes { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<TokenGoogle> TokenGoogles { get; set; }

    public virtual DbSet<TokenUser> TokenUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Connection string is configured in Program.cs via dependency injection
        // No need to configure here when using DI
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Username, "accounts_username_key").IsUnique();

            entity.HasIndex(e => e.Email, "idx_accounts_email");

            entity.HasIndex(e => e.Username, "idx_accounts_username");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
            entity.Property(e => e.FacebookId).HasMaxLength(255).HasColumnName("facebook_id");
            entity.Property(e => e.FirstConfirm).HasMaxLength(30).HasColumnName("first_confirm");
            entity.Property(e => e.GoogleId).HasMaxLength(255).HasColumnName("google_id");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50).HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusAccount).HasMaxLength(10).HasColumnName("status_account");
            entity.Property(e => e.Username).HasMaxLength(255).HasColumnName("username");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("accounts_role_id_fkey");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity
                .HasIndex(
                    e => new
                    {
                        e.DoctorId,
                        e.SlotId,
                        e.AppointmentDate,
                    },
                    "appointment_doctor_id_slot_id_appointment_date_key"
                )
                .IsUnique();

            entity.HasIndex(e => e.AppointmentDate, "idx_appointments_date");

            entity.HasIndex(e => e.DoctorId, "idx_appointments_doctor");

            entity.HasIndex(e => e.PatientId, "idx_appointments_patient");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Actualcost).HasPrecision(18, 2).HasColumnName("actualcost");
            entity
                .Property(e => e.AppointmentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("appointment_date");
            entity
                .Property(e => e.AppointmentStatus)
                .HasMaxLength(255)
                .HasColumnName("appointment_status");
            entity.Property(e => e.Discountid).HasColumnName("discountid");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.ServiceDetailId).HasColumnName("service_detail_id");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity
                .HasOne(d => d.Discount)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Discountid)
                .HasConstraintName("appointment_discountid_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("appointment_doctor_id_fkey");

            entity
                .HasOne(d => d.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("appointment_patient_id_fkey");

            entity
                .HasOne(d => d.ServiceDetail)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceDetailId)
                .HasConstraintName("appointment_service_detail_id_fkey");

            entity
                .HasOne(d => d.Slot)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("appointment_slot_id_fkey");

            entity
                .HasOne(d => d.Staff)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("appointment_staff_id_fkey");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("banner_pkey");

            entity.ToTable("banner");

            entity.Property(e => e.BannerId).HasColumnName("banner_id");
            entity
                .Property(e => e.BannerDescription)
                .HasMaxLength(255)
                .HasColumnName("banner_description");
            entity.Property(e => e.BannerName).HasMaxLength(255).HasColumnName("banner_name");
            entity.Property(e => e.BannerStatus).HasMaxLength(10).HasColumnName("banner_status");
            entity.Property(e => e.BannerTitle).HasMaxLength(255).HasColumnName("banner_title");
            entity.Property(e => e.HrefBanner).HasMaxLength(255).HasColumnName("href_banner");
            entity.Property(e => e.LinkBanner).HasMaxLength(255).HasColumnName("link_banner");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("blog_pkey");

            entity.ToTable("blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BlogContent).HasColumnName("blog_content");
            entity
                .Property(e => e.CreatedDateBlog)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date_blog");
            entity
                .Property(e => e.TitleImageBlog)
                .HasMaxLength(255)
                .HasColumnName("title_image_blog");
            entity.Property(e => e.TitleMeta).HasColumnName("title_meta");

            entity
                .HasOne(d => d.Author)
                .WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("blog_author_id_fkey");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("certificate_pkey");

            entity.ToTable("certificate");

            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity
                .Property(e => e.CertificateName)
                .HasMaxLength(255)
                .HasColumnName("certificate_name");
        });

        modelBuilder.Entity<CertificateDoctor>(entity =>
        {
            entity
                .HasKey(e => new { e.DoctorId, e.CertificateId })
                .HasName("certificate_doctor_pkey");

            entity.ToTable("certificate_doctor");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.CertificateId).HasColumnName("certificate_id");
            entity
                .Property(e => e.CertificateImage)
                .HasMaxLength(255)
                .HasColumnName("certificate_image");
            entity
                .Property(e => e.DateCertificate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_certificate");
            entity
                .Property(e => e.DateChange)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_change");
            entity.Property(e => e.IssuedBy).HasMaxLength(255).HasColumnName("issued_by");
            entity.Property(e => e.Status).HasMaxLength(255).HasColumnName("status");

            entity
                .HasOne(d => d.Certificate)
                .WithMany(p => p.CertificateDoctors)
                .HasForeignKey(d => d.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certificate_doctor_certificate_id_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.CertificateDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certificate_doctor_doctor_id_fkey");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.CheckoutId).HasName("checkout_pkey");

            entity.ToTable("checkout");

            entity.Property(e => e.CheckoutId).HasColumnName("checkout_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.CheckoutCode).HasMaxLength(255).HasColumnName("checkout_code");
            entity
                .Property(e => e.CheckoutTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("checkout_time");
            entity.Property(e => e.TotalBill).HasPrecision(18, 2).HasColumnName("total_bill");
            entity
                .Property(e => e.TransactionStatus)
                .HasMaxLength(255)
                .HasColumnName("transaction_status");
            entity
                .Property(e => e.TransactionType)
                .HasMaxLength(255)
                .HasColumnName("transaction_type");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("checkout_appointment_id_fkey");
        });

        modelBuilder.Entity<Commentblog>(entity =>
        {
            entity.HasKey(e => e.CommentBlogId).HasName("commentblog_pkey");

            entity.ToTable("commentblog");

            entity.Property(e => e.CommentBlogId).HasColumnName("comment_blog_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.Comment).HasMaxLength(255).HasColumnName("comment");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(e => e.Tuongtac).HasColumnName("tuongtac");

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
        });

        modelBuilder.Entity<ContentStory>(entity =>
        {
            entity.HasKey(e => e.PatientName).HasName("content_stories_pkey");

            entity.ToTable("content_stories");

            entity.Property(e => e.PatientName).HasMaxLength(255).HasColumnName("patient_name");
            entity.Property(e => e.ContentStories).HasColumnName("content_stories");
            entity.Property(e => e.ImagePatient).HasMaxLength(255).HasColumnName("image_patient");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.AccountId).ValueGeneratedNever().HasColumnName("account_id");
            entity.Property(e => e.Address).HasMaxLength(500).HasColumnName("address");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.FullName).HasMaxLength(255).HasColumnName("full_name");
            entity.Property(e => e.Gender).HasMaxLength(50).HasColumnName("gender");
            entity
                .Property(e => e.ImageProfileUser)
                .HasMaxLength(255)
                .HasColumnName("image_profile_user");
            entity.Property(e => e.Rankid).HasColumnName("rankid");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customers_account_id_fkey");

            entity
                .HasOne(d => d.Rank)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.Rankid)
                .HasConstraintName("customers_rankid_fkey");
        });

        modelBuilder.Entity<Customerrank>(entity =>
        {
            entity.HasKey(e => e.Rankid).HasName("customerrank_pkey");

            entity.ToTable("customerrank");

            entity.Property(e => e.Rankid).HasColumnName("rankid");
            entity.Property(e => e.Minamount).HasColumnName("minamount");
            entity.Property(e => e.Rankname).HasMaxLength(50).HasColumnName("rankname");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.DegreeId).HasName("degree_pkey");

            entity.ToTable("degree");

            entity.Property(e => e.DegreeId).HasColumnName("degree_id");
            entity.Property(e => e.DegreeName).HasMaxLength(255).HasColumnName("degree_name");
        });

        modelBuilder.Entity<DegreeDoctor>(entity =>
        {
            entity.HasKey(e => e.DegreeDoctorId).HasName("degree_doctor_pkey");

            entity.ToTable("degree_doctor");

            entity.Property(e => e.DegreeDoctorId).HasColumnName("degree_doctor_id");
            entity
                .Property(e => e.DateChange)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_change");
            entity
                .Property(e => e.DateDegree)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_degree");
            entity.Property(e => e.DegreeId).HasColumnName("degree_id");
            entity.Property(e => e.DegreeImage).HasMaxLength(255).HasColumnName("degree_image");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.IssuedBy).HasMaxLength(255).HasColumnName("issued_by");
            entity.Property(e => e.Status).HasMaxLength(255).HasColumnName("status");
            entity.Property(e => e.Version).HasDefaultValue(1).HasColumnName("version");

            entity
                .HasOne(d => d.Degree)
                .WithMany(p => p.DegreeDoctors)
                .HasForeignKey(d => d.DegreeId)
                .HasConstraintName("degree_doctor_degree_id_fkey");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.DegreeDoctors)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("degree_doctor_doctor_id_fkey");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Discountid).HasName("discount_pkey");

            entity.ToTable("discount");

            entity.Property(e => e.Discountid).HasColumnName("discountid");
            entity.Property(e => e.Discountname).HasMaxLength(50).HasColumnName("discountname");
            entity.Property(e => e.Enddate).HasColumnName("enddate");
            entity.Property(e => e.Percent).HasColumnName("percent");
            entity.Property(e => e.Rankid).HasColumnName("rankid");
            entity.Property(e => e.Status).HasDefaultValue(true).HasColumnName("status");

            entity
                .HasOne(d => d.Rank)
                .WithMany(p => p.Discounts)
                .HasForeignKey(d => d.Rankid)
                .HasConstraintName("discount_rankid_fkey");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("doctors_pkey");

            entity.ToTable("doctors");

            entity.HasIndex(e => e.AccountId, "doctors_account_id_key").IsUnique();

            entity.HasIndex(e => e.SpecializationId, "idx_doctors_specialization");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address).HasMaxLength(500).HasColumnName("address");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.DoctorName).HasMaxLength(255).HasColumnName("doctor_name");
            entity.Property(e => e.DoctorStatus).HasMaxLength(255).HasColumnName("doctor_status");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.Gender).HasMaxLength(50).HasColumnName("gender");
            entity.Property(e => e.ProfileImage).HasMaxLength(255).HasColumnName("profile_image");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .HasConstraintName("doctors_account_id_fkey");

            entity
                .HasOne(d => d.Specialization)
                .WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("doctors_specialization_id_fkey");
        });

        modelBuilder.Entity<FeedbackDoctor>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("feedback_doctor_pkey");

            entity.ToTable("feedback_doctor");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.FeedbackDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("feedback_date");
            entity.Property(e => e.FeedbackRating).HasColumnName("feedback_rating");
            entity.Property(e => e.FeedbackText).HasMaxLength(255).HasColumnName("feedback_text");
            entity
                .Property(e => e.ResponseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("response_date");
            entity.Property(e => e.ResponseText).HasMaxLength(255).HasColumnName("response_text");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.FeedbackDoctors)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("feedback_doctor_appointment_id_fkey");

            entity
                .HasOne(d => d.Staff)
                .WithMany(p => p.FeedbackDoctors)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("feedback_doctor_staff_id_fkey");
        });

        modelBuilder.Entity<FeedbackService>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("feedback_service_pkey");

            entity.ToTable("feedback_service");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity
                .Property(e => e.FeedbackDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("feedback_date");
            entity.Property(e => e.FeedbackRating).HasColumnName("feedback_rating");
            entity.Property(e => e.FeedbackText).HasMaxLength(255).HasColumnName("feedback_text");
            entity
                .Property(e => e.ResponseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("response_date");
            entity.Property(e => e.ResponseText).HasMaxLength(255).HasColumnName("response_text");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity
                .HasOne(d => d.Appointment)
                .WithMany(p => p.FeedbackServices)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("feedback_service_appointment_id_fkey");

            entity
                .HasOne(d => d.Staff)
                .WithMany(p => p.FeedbackServices)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("feedback_service_staff_id_fkey");
        });

        modelBuilder.Entity<FollowUp>(entity =>
        {
            entity.HasNoKey().ToTable("follow_up");

            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
            entity
                .Property(e => e.FollowUpDescription)
                .HasMaxLength(255)
                .HasColumnName("follow_up_description");
            entity.Property(e => e.NextFollowUpDate).HasColumnName("next_follow_up_date");
            entity.Property(e => e.Patientname).HasMaxLength(255).HasColumnName("patientname");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
        });

        modelBuilder.Entity<ImagesService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("images_service_pkey");

            entity.ToTable("images_service");

            entity.Property(e => e.ServiceId).ValueGeneratedNever().HasColumnName("service_id");
            entity.Property(e => e.ImageAfter).HasMaxLength(255).HasColumnName("image_after");
            entity.Property(e => e.ImageBefore).HasMaxLength(255).HasColumnName("image_before");
            entity.Property(e => e.ImageMain).HasMaxLength(255).HasColumnName("image_main");

            entity
                .HasOne(d => d.Service)
                .WithOne(p => p.ImagesService)
                .HasForeignKey<ImagesService>(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("images_service_service_id_fkey");
        });

        modelBuilder.Entity<ImagesType>(entity =>
        {
            entity.HasKey(e => e.ImageTypeId).HasName("images_type_pkey");

            entity.ToTable("images_type");

            entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");
            entity.Property(e => e.ImageType).HasMaxLength(255).HasColumnName("image_type");
        });

        modelBuilder.Entity<ImagesVideo>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("images_video_pkey");

            entity.ToTable("images_video");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity
                .Property(e => e.ImageDescription)
                .HasMaxLength(255)
                .HasColumnName("image_description");
            entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");
            entity.Property(e => e.ImageUrl).HasMaxLength(255).HasColumnName("image_url");

            entity
                .HasOne(d => d.ImageType)
                .WithMany(p => p.ImagesVideos)
                .HasForeignKey(d => d.ImageTypeId)
                .HasConstraintName("images_video_image_type_id_fkey");
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.MachineId).HasName("machine_pkey");

            entity.ToTable("machine");

            entity.Property(e => e.MachineId).HasColumnName("machine_id");
            entity.Property(e => e.MachineDescription).HasColumnName("machine_description");
            entity.Property(e => e.MachineImg).HasMaxLength(255).HasColumnName("machine_img");
            entity.Property(e => e.MachineName).HasMaxLength(255).HasColumnName("machine_name");
        });

        modelBuilder.Entity<Medicalhistory>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("medicalhistory_pkey");

            entity.ToTable("medicalhistory");

            entity
                .Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("appointment_id");
            entity
                .Property(e => e.AdditionalTests)
                .HasMaxLength(255)
                .HasColumnName("additional_tests");
            entity
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Diagnosis).HasMaxLength(255).HasColumnName("diagnosis");
            entity.Property(e => e.Note).HasMaxLength(255).HasColumnName("note");
            entity.Property(e => e.Prescription).HasMaxLength(255).HasColumnName("prescription");
            entity.Property(e => e.Symptoms).HasMaxLength(255).HasColumnName("symptoms");
            entity.Property(e => e.Treatment).HasMaxLength(255).HasColumnName("treatment");
            entity.Property(e => e.VisionLeft).HasPrecision(3, 2).HasColumnName("vision_left");
            entity.Property(e => e.VisionRight).HasPrecision(3, 2).HasColumnName("vision_right");

            entity
                .HasOne(d => d.Appointment)
                .WithOne(p => p.Medicalhistory)
                .HasForeignKey<Medicalhistory>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medicalhistory_appointment_id_fkey");
        });

        modelBuilder.Entity<OtpService>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("otp_services_pkey");

            entity.ToTable("otp_services");

            entity.Property(e => e.OtpId).HasColumnName("otp_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity
                .Property(e => e.CreatedOtpTime)
                .HasMaxLength(255)
                .HasColumnName("created_otp_time");
            entity.Property(e => e.Otp).HasMaxLength(20).HasColumnName("otp");
            entity
                .Property(e => e.OtpExpiryDate)
                .HasMaxLength(255)
                .HasColumnName("otp_expiry_date");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.OtpServices)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("otp_services_account_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity
                .Property(e => e.PermissionName)
                .HasMaxLength(255)
                .HasColumnName("permission_name");

            entity
                .HasMany(d => d.Roles)
                .WithMany(p => p.Permissions)
                .UsingEntity<Dictionary<string, object>>(
                    "PermissionRole",
                    r =>
                        r.HasOne<Role>()
                            .WithMany()
                            .HasForeignKey("RoleId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("permission_role_role_id_fkey"),
                    l =>
                        l.HasOne<Permission>()
                            .WithMany()
                            .HasForeignKey("PermissionId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("permission_role_permission_id_fkey"),
                    j =>
                    {
                        j.HasKey("PermissionId", "RoleId").HasName("permission_role_pkey");
                        j.ToTable("permission_role");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("permission_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    }
                );
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasMaxLength(255).HasColumnName("role_name");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity
                .HasKey(e => new
                {
                    e.DoctorId,
                    e.SlotId,
                    e.ScheduleDate,
                })
                .HasName("schedules_pkey");

            entity.ToTable("schedules");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.ScheduleDate).HasColumnName("schedule_date");
            entity
                .Property(e => e.ScheduleStatus)
                .HasMaxLength(255)
                .HasColumnName("schedule_status");

            entity
                .HasOne(d => d.Doctor)
                .WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
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
            entity
                .Property(e => e.ServiceBenefit)
                .HasMaxLength(1000)
                .HasColumnName("service_benefit");
            entity
                .Property(e => e.ServiceDescription)
                .HasMaxLength(255)
                .HasColumnName("service_description");
            entity
                .Property(e => e.ServiceIntroduce)
                .HasMaxLength(1000)
                .HasColumnName("service_introduce");
            entity.Property(e => e.ServiceName).HasMaxLength(255).HasColumnName("service_name");
            entity.Property(e => e.ServiceStatus).HasMaxLength(20).HasColumnName("service_status");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

            entity
                .HasOne(d => d.Specialization)
                .WithMany(p => p.Services)
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("services_specialization_id_fkey");
        });

        modelBuilder.Entity<ServicesDetail>(entity =>
        {
            entity.HasKey(e => e.ServiceDetailId).HasName("services_detail_pkey");

            entity.ToTable("services_detail");

            entity.Property(e => e.ServiceDetailId).HasColumnName("service_detail_id");
            entity.Property(e => e.Cost).HasPrecision(18, 2).HasColumnName("cost");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");

            entity
                .HasOne(d => d.Service)
                .WithMany(p => p.ServicesDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("services_detail_service_id_fkey");

            entity
                .HasOne(d => d.ServiceType)
                .WithMany(p => p.ServicesDetails)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("services_detail_service_type_id_fkey");
        });

        modelBuilder.Entity<ServicesType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("services_type_pkey");

            entity.ToTable("services_type");

            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity
                .Property(e => e.DurationService)
                .HasMaxLength(50)
                .HasColumnName("duration_service");
            entity
                .Property(e => e.ServiceTypeName)
                .HasMaxLength(255)
                .HasColumnName("service_type_name");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("slots_pkey");

            entity.ToTable("slots");

            entity
                .HasIndex(
                    e => new
                    {
                        e.StartTime,
                        e.EndTime,
                        e.ServiceTypeId,
                    },
                    "slots_start_time_end_time_service_type_id_key"
                )
                .IsUnique();

            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.EndTime).HasMaxLength(255).HasColumnName("end_time");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity.Property(e => e.StartTime).HasMaxLength(20).HasColumnName("start_time");

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
            entity
                .Property(e => e.SpecializationName)
                .HasMaxLength(255)
                .HasColumnName("specialization_name");
            entity
                .Property(e => e.SpecializationStatus)
                .HasMaxLength(255)
                .HasColumnName("specialization_status");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("staff_pkey");

            entity.ToTable("staff");

            entity.HasIndex(e => e.AccountId, "staff_account_id_key").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AdminAddress).HasMaxLength(255).HasColumnName("admin_address");
            entity.Property(e => e.AdminDob).HasColumnName("admin_dob");
            entity.Property(e => e.AdminFullname).HasMaxLength(255).HasColumnName("admin_fullname");
            entity.Property(e => e.AdminGender).HasMaxLength(50).HasColumnName("admin_gender");
            entity
                .Property(e => e.AdminHiredDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("admin_hired_date");
            entity.Property(e => e.AdminSalary).HasPrecision(18, 2).HasColumnName("admin_salary");
            entity
                .Property(e => e.ImageProfileAdmin)
                .HasMaxLength(255)
                .HasColumnName("image_profile_admin");

            entity
                .HasOne(d => d.Account)
                .WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.AccountId)
                .HasConstraintName("staff_account_id_fkey");
        });

        modelBuilder.Entity<TokenGoogle>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("token_google_pkey");

            entity.ToTable("token_google");

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity
                .Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Token).HasMaxLength(255).HasColumnName("token");
        });

        modelBuilder.Entity<TokenUser>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("token_user_pkey");

            entity.ToTable("token_user");

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity
                .Property(e => e.CreatedDateToken)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date_token");
            entity.Property(e => e.TokenUser1).HasMaxLength(255).HasColumnName("token_user");

            entity
                .HasOne(d => d.Account)
                .WithMany(p => p.TokenUsers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("token_user_account_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
