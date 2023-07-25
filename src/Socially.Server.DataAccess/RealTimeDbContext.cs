using Microsoft.EntityFrameworkCore;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public class RealTimeDbContext : DbContext
    {

        public DbSet<PostRealTime> Posts { get; set; }


    }
}
