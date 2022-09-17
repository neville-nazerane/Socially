﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Socially.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.Server.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        public DbSet<ProfileImage> ProfileImages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                   .Property(u => u.CreatedOn)
                   .HasDefaultValueSql("GETUTCDATE()");

            base.OnModelCreating(builder);
        }
    }
}
