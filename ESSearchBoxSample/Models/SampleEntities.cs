using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace ESSearchBoxSample.Models
{
    public class SampleEntities : DbContext
    {
        public DbSet<DocumentModel> DocumentModels { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<SampleEntities, Configuration>());
        //}
    }
}