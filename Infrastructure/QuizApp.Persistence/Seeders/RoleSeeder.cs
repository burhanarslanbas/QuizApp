using Microsoft.AspNetCore.Identity;
using QuizApp.Domain.Constants;
using QuizApp.Domain.Entities.Identity;

namespace QuizApp.Persistence.Seeders;

public class RoleSeeder
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleSeeder(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        // Student Role
        if (!await _roleManager.RoleExistsAsync("Student"))
        {
            var studentRole = new AppRole
            {
                Name = "Student",
                Description = "Öğrenci rolü - Quiz görüntüleme ve çözme yetkileri",
                IsActive = true
            };

            await _roleManager.CreateAsync(studentRole);
            await AddClaimsToRoleAsync(studentRole, RoleClaims.StudentClaims);
        }

        // Teacher Role
        if (!await _roleManager.RoleExistsAsync("Teacher"))
        {
            var teacherRole = new AppRole
            {
                Name = "Teacher",
                Description = "Öğretmen rolü - Quiz, soru ve kategori yönetim yetkileri",
                IsActive = true
            };

            await _roleManager.CreateAsync(teacherRole);
            await AddClaimsToRoleAsync(teacherRole, RoleClaims.TeacherClaims);
        }

        // Admin Role
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            var adminRole = new AppRole
            {
                Name = "Admin",
                Description = "Yönetici rolü - Tüm yetkiler",
                IsActive = true
            };

            await _roleManager.CreateAsync(adminRole);
            await AddClaimsToRoleAsync(adminRole, RoleClaims.AdminClaims);
        }
    }

    private async Task AddClaimsToRoleAsync(AppRole role, string[] claims)
    {
        foreach (var claim in claims)
        {
            await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", claim));
        }
    }
} 