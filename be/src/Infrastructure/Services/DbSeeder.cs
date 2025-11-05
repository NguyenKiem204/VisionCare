using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Services;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VisionCareDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<object>>();

        var isEnabled = bool.Parse(config["AdminSeed:Enabled"] ?? "false");
        if (!isEnabled)
        {
            logger.LogInformation("Admin seeding is disabled in configuration.");
            return;
        }

        try
        {
            var username = config["AdminSeed:Username"] ?? "admin";
            var email = config["AdminSeed:Email"] ?? "admin@visioncare.com";
            var password = config["AdminSeed:Password"] ?? "admin123";
            var roleName = config["AdminSeed:Role"] ?? "Admin";

            var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (adminRole == null)
            {
                adminRole = new Role { RoleName = roleName, RoleDescription = "Administrator" };
                db.Roles.Add(adminRole);
                await db.SaveChangesAsync();
                logger.LogInformation("Created {RoleName} role.", roleName);
            }

            var admin = await db.Accounts.FirstOrDefaultAsync(a =>
                a.Email == email || a.Username == username
            );
            if (admin == null)
            {
                admin = new Account
                {
                    Email = email,
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    EmailConfirmed = true,
                    Status = "Active",
                    RoleId = adminRole.RoleId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                db.Accounts.Add(admin);
                await db.SaveChangesAsync();
                logger.LogWarning(
                    "Admin user created with username: {Username}, email: {Email}. Please change the default password!",
                    username,
                    email
                );
            }
            else
            {
                logger.LogInformation(
                    "Admin user already exists with email: {Email} or username: {Username}",
                    email,
                    username
                );
            }
            var roleNames = new[] { "Doctor", "Staff", "Customer" };
            foreach (var rn in roleNames)
            {
                if (!await db.Roles.AnyAsync(r => r.RoleName == rn))
                {
                    db.Roles.Add(new Role { RoleName = rn, RoleDescription = rn });
                    await db.SaveChangesAsync();
                    logger.LogInformation("Created {RoleName} role.", rn);
                }
            }

            await SeedUserIfEnabledAsync(
                db,
                logger,
                config,
                section: "DoctorSeed",
                defaultRole: "Doctor",
                defaultUsername: "doctor",
                defaultEmail: "doctor@visioncare.com",
                defaultPassword: "doctor123"
            );
            await SeedUserIfEnabledAsync(
                db,
                logger,
                config,
                section: "StaffSeed",
                defaultRole: "Staff",
                defaultUsername: "staff",
                defaultEmail: "staff@visioncare.com",
                defaultPassword: "staff123"
            );
            await SeedUserIfEnabledAsync(
                db,
                logger,
                config,
                section: "CustomerSeed",
                defaultRole: "Customer",
                defaultUsername: "customer",
                defaultEmail: "customer@visioncare.com",
                defaultPassword: "customer123"
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding admin user.");
        }
    }

    private static async Task SeedUserIfEnabledAsync(
        VisionCareDbContext db,
        ILogger<object> logger,
        IConfiguration config,
        string section,
        string defaultRole,
        string defaultUsername,
        string defaultEmail,
        string defaultPassword
    )
    {
        var enabled = bool.Parse(config[$"{section}:Enabled"] ?? "false");
        if (!enabled)
            return;

        var username = config[$"{section}:Username"] ?? defaultUsername;
        var email = config[$"{section}:Email"] ?? defaultEmail;
        var password = config[$"{section}:Password"] ?? defaultPassword;
        var roleName = config[$"{section}:Role"] ?? defaultRole;

        var role = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        if (role == null)
        {
            role = new Role { RoleName = roleName, RoleDescription = roleName };
            db.Roles.Add(role);
            await db.SaveChangesAsync();
            logger.LogInformation("Created {RoleName} role.", roleName);
        }

        var existing = await db.Accounts.FirstOrDefaultAsync(a =>
            a.Email == email || a.Username == username
        );
        if (existing != null)
        {
            logger.LogInformation(
                "{Role} user already exists: {Email}/{Username}",
                roleName,
                email,
                username
            );
            return;
        }

        var account = new Account
        {
            Email = email,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            EmailConfirmed = true,
            Status = "Active",
            RoleId = role.RoleId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        // Create corresponding profile record based on role
        if (roleName == "Doctor")
        {
            var existingDoctor = await db.Doctors.FirstOrDefaultAsync(d =>
                d.AccountId == account.AccountId
            );
            if (existingDoctor == null)
            {
                // Get first specialization (or use ID 1 if available)
                var specialization = await db.Specializations.FirstOrDefaultAsync();
                if (specialization == null)
                {
                    logger.LogWarning(
                        "No specialization found. Cannot create Doctor record for {Email}. Please seed specializations first.",
                        email
                    );
                }
                else
                {
                    var doctor = new Doctor
                    {
                        AccountId = account.AccountId,
                        FullName = config[$"{section}:FullName"] ?? username,
                        Phone = config[$"{section}:Phone"] ?? null,
                        ExperienceYears = short.Parse(config[$"{section}:ExperienceYears"] ?? "5"),
                        SpecializationId = specialization.SpecializationId,
                        Gender = config[$"{section}:Gender"] ?? "Male",
                        Status = "Active",
                    };
                    db.Doctors.Add(doctor);
                    await db.SaveChangesAsync();
                    logger.LogInformation(
                        "Created Doctor record for {Email} with specialization {SpecializationName}",
                        email,
                        specialization.Name
                    );

                    // Create sample Degree and Certificate for doctor
                    var firstDegree = await db.Degrees.FirstOrDefaultAsync();
                    if (firstDegree != null)
                    {
                        var degreeDoctor = new Degreedoctor
                        {
                            DoctorId = doctor.AccountId,
                            DegreeId = firstDegree.DegreeId,
                            IssuedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-5)),
                            IssuedBy = "Đại học Y Dược",
                            Status = "Active",
                        };
                        db.Degreedoctors.Add(degreeDoctor);
                    }

                    var firstCertificate = await db.Certificates.FirstOrDefaultAsync();
                    if (firstCertificate != null)
                    {
                        var certDoctor = new Certificatedoctor
                        {
                            DoctorId = doctor.AccountId,
                            CertificateId = firstCertificate.CertificateId,
                            IssuedDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                            IssuedBy = "Bộ Y Tế",
                            ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddYears(3)),
                            Status = "Active",
                        };
                        db.Certificatedoctors.Add(certDoctor);
                    }

                    await db.SaveChangesAsync();
                    logger.LogInformation("Created sample Degree and Certificate for Doctor {Email}", email);
                }
            }
        }
        else if (roleName == "Staff")
        {
            var existingStaff = await db.Staff.FirstOrDefaultAsync(s =>
                s.AccountId == account.AccountId
            );
            if (existingStaff == null)
            {
                var staff = new Staff
                {
                    AccountId = account.AccountId,
                    FullName = config[$"{section}:FullName"] ?? username,
                    Phone = config[$"{section}:Phone"] ?? null,
                    Gender = config[$"{section}:Gender"] ?? "Male",
                    HiredDate = DateOnly.FromDateTime(DateTime.Now),
                };
                db.Staff.Add(staff);
                await db.SaveChangesAsync();
                logger.LogInformation("Created Staff record for {Email}", email);
            }
        }
        else if (roleName == "Customer")
        {
            var existingCustomer = await db.Customers.FirstOrDefaultAsync(c =>
                c.AccountId == account.AccountId
            );
            if (existingCustomer == null)
            {
                // Get default rank (Bronze - rank_id = 1)
                var defaultRank = await db.Customerranks.FirstOrDefaultAsync(r =>
                    r.RankName == "Bronze"
                );
                var customer = new Customer
                {
                    AccountId = account.AccountId,
                    FullName = config[$"{section}:FullName"] ?? username,
                    Phone = config[$"{section}:Phone"] ?? null,
                    Gender = config[$"{section}:Gender"] ?? "Male",
                    RankId = defaultRank?.RankId,
                };
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                logger.LogInformation("Created Customer record for {Email}", email);
            }
        }

        logger.LogInformation("Seeded {Role} user: {Email}", roleName, email);
    }
}
