using DevTreks.Models;
//using System.Data.Entity.ModelConfiguration;

namespace DevTreks.Data.Mappings
{
    //public class AccountToServiceMap : EntityTypeConfiguration<AccountToService>
    //{
    //    public AccountToServiceMap()
    //    {
    //        HasRequired(ats => ats.Service)
    //        .WithMany(s => s.AccountToService)
    //        .HasForeignKey(ats => ats.ServiceId);

    //        HasRequired(ats => ats.Account)
    //        .WithMany(a => a.AccountToService)
    //        .HasForeignKey(ats => ats.AccountId);

    //        //or put this in OnModelCreating
    //        //one to many relationship and the foreign key name is not BlogId in model class or Blog_Id in db:
    //        //modelBuilder.Entity<AccountToService>()
    //        //    .HasRequired(ats => ats.Service)
    //        //    .WithMany(s => s.AccountToService)
    //        //    .HasForeignKey(ats => ats.ServiceId);
    //        //modelBuilder.Entity<AccountToService>()
    //        //   .HasRequired(ats => ats.Account)
    //        //   .WithMany(a => a.AccountToService)
    //        //   .HasForeignKey(ats => ats.AccountId);
    //    }
    //}
}

