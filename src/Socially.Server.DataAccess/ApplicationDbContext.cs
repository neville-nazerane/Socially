using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Socially.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.Server.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
