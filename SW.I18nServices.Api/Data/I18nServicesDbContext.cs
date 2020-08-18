using Microsoft.EntityFrameworkCore;
using SW.I18nServices.Api.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nServices.Api
{
    public class I18nServicesDbContext : DbContext
    {

        public const string ConnectionString = "I18nServicesDb";

        public I18nServicesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
