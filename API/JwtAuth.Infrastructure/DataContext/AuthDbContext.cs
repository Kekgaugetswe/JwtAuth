using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Infrastructure.DataContext;

public class AuthDbContext(DbContextOptions options) : IdentityDbContext(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "18ed58d0-8bb6-49b4-87f1-47bd8a8d8003";
        var writerRoleId = "5e6c376e-5ae5-472d-ab22-13e6b2b73c17";

        // create reader and reader and writer role
        var roles = new List<IdentityRole>{
            new IdentityRole(){
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId

            },
            new IdentityRole(){
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };
        // seed the roles

        builder.Entity<IdentityRole>().HasData(roles);


        //Create an Admin user

        var adminUserId = "cb4b3ccf-a0a4-4dbd-8a1c-6c72998883a8";

        var admin = new IdentityUser() {
            Id = adminUserId,
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            NormalizedEmail = "admin@gmail.com".ToUpper(),
            NormalizedUserName = "admin@gmail.com".ToUpper(),
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
        builder.Entity<IdentityUser>().HasData(admin);

        // Give Roles to admin

        var adminRoles =  new List<IdentityUserRole<string>>{
            new(){
                UserId = adminUserId,
                RoleId  =  readerRoleId,

            },
            new(){
                UserId = adminUserId,
                RoleId  =  writerRoleId,
            },

        };
        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}
