using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TCM.Models
{
    public class TCMContext : IdentityDbContext<TCMUser>
    {
        private IConfigurationRoot _config;

        //we are using this constructor injection, so that we don't have to know fully how to construct this object but dependency injection layer will do it for us
        //we also have to provide DbContextOptions (as below), in other case optionsBuilder will not work correctly
        public TCMContext(IConfigurationRoot Configuration, DbContextOptions options) : base(options)
        {
            _config = Configuration;
        }

        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<Step> Steps { get; set; }

        //code for configuring and plugging-in the database
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:TCMContextConnection"]);
        }

    }
}
