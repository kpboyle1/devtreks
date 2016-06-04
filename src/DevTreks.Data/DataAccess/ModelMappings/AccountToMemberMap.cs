using DevTreks.Models;
//using Microsoft.EntityFrameworkCore.ModelConfiguration;

namespace DevTreks.Data.Mappings
{
    //public class AccountToMemberMap : EntityTypeConfiguration<AccountToMember>
    //{
    //    public AccountToMemberMap()
    //    {
    //        HasRequired(ac => ac.Member)
    //        .WithMany(m => m.AccountToMember)
    //        .HasForeignKey(ac => ac.MemberId);

    //        HasRequired(ac => ac.ClubDefault)
    //        .WithMany(a => a.AccountToMember)
    //        .HasForeignKey(ac => ac.AccountId);
    //        //or put this in OnModelCreating
    //        //one to many relationship and the foreign key name is not BlogId in model class or Blog_Id in db:
    //        //modelBuilder.Entity<AccountToMember>()
    //        //    .HasRequired(ac => ac.Member)
    //        //    .WithMany(m => m.AccountToMember)
    //        //    .HasForeignKey(ac => ac.MemberId);
    //        //modelBuilder.Entity<AccountToMember>()
    //        //   .HasRequired(ac => ac.ClubDefault)
    //        //   .WithMany(a => a.AccountToMember)
    //        //   .HasForeignKey(ac => ac.AccountId);
    //    }
    //}
}
