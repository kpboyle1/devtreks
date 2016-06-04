using DevTreks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
//2.0.0 temp fix
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DevTreks.Data.DataAccess
{
    public partial class DevTreksContext : DbContext
    {
        //2.0.0 All of the LinkedViewTo models need the xmldoc property
        //just use a string -xml recognizes strings
        public DevTreksContext(
            DbContextOptions options) : base(options)
        {
        }
        //binary max length
        public int BinaryMaxLength { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //2.0.0 temp fix: https://docs.efproject.net/en/latest/miscellaneous/rc1-rc2-upgrade.html
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //keep rc1 names and find out more about this
                entity.Relational().TableName = entity.DisplayName();
            }

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountClassId).HasName("ixAccountClassId");

                entity.HasIndex(e => e.GeoRegionId).HasName("ixAccountGeoRegionId");

                entity.Property(e => e.AccountClassId).HasDefaultValue(1);

                entity.Property(e => e.AccountDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs summary description");

                entity.Property(e => e.AccountEmail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("some.one@some.where");

                entity.Property(e => e.AccountLongDesc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.AccountURI)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue("needs uri");

                entity.Property(e => e.GeoRegionId).HasDefaultValue(1);

                entity.Ignore(e => e.ClubDocFullPath);
                entity.Ignore(e => e.PrivateAuthorizationLevel);
                entity.Ignore(e => e.URIFull);
                entity.Ignore(e => e.AccountToPayment);
                entity.Ignore(e => e.TotalCost);
                entity.Ignore(e => e.NetCost);

                entity.HasOne(d => d.AccountClass).WithMany(p => p.Account).HasForeignKey(d => d.AccountClassId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.GeoRegion).WithMany(p => p.Account).HasForeignKey(d => d.GeoRegionId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AccountClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.AccountClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.AccountClassName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.AccountClassNum)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");
            });

            modelBuilder.Entity<AccountToAddIn>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToAccountLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToAccountAccountId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.HasOne(d => d.LinkedView).WithMany(p => p.AccountToAddIn).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToAddIn).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<AccountToAudit>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountId).HasName("ixAccountToAuditAccountId");

                entity.Property(e => e.ClubInUseAuthorizationLevel)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.EditDate).HasColumnType("smalldatetime");

                entity.Property(e => e.EditedDocFullPath)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.EditedDocURI)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.MemberRole)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.ServerSubAction)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToAudit).HasForeignKey(d => d.AccountId);
            });

            modelBuilder.Entity<AccountToCredit>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.CardEndMonth)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardEndYear)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardFullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardFullNumber)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.CardNumberSalt).HasMaxLength(128);

                entity.Property(e => e.CardShortNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardState)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CardType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToCredit).HasForeignKey(d => d.AccountId);
            });

            modelBuilder.Entity<AccountToIncentive>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasOne(d => d.AccountToService).WithMany(p => p.AccountToIncentive).HasForeignKey(d => d.AccountToServiceId);

                entity.HasOne(d => d.Incentive).WithMany(p => p.AccountToIncentive).HasForeignKey(d => d.IncentiveId);
                
                entity.Ignore(e => e.TotalCost);
                entity.Ignore(e => e.NetCost);
            });
            //2.0.0 changed model to allow direct data entry w/o use of calculator
            modelBuilder.Entity<AccountToLocal>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToLocalLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToLocalAccountId");

                entity.Property(e => e.CurrencyGroupId).HasDefaultValue(1);

                entity.Property(e => e.DataSourcePriceId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceTechId).HasDefaultValue(1);

                entity.Property(e => e.GeoCodePriceId).HasDefaultValue(1);

                entity.Property(e => e.GeoCodeTechId).HasDefaultValue(1);

                entity.Property(e => e.IsDefaultLinkedView).HasDefaultValue(false);

                entity.Property(e => e.LinkedViewId).HasDefaultValue(1);

                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.LinkingNodeId).HasDefaultValue(1);

                entity.Property(e => e.LocalDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.RatingGroupId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.UnitGroupId).HasDefaultValue(1);

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToLocal).HasForeignKey(d => d.LinkingNodeId);

                //2.0.0 changes

                //entity.HasOne(d => d.LinkedView).WithMany(p => p.AccountToLocal).HasForeignKey(d => d.LinkedViewId);

                entity.Property(e => e.NominalRate)
                    .HasColumnType("real")
                    .HasDefaultValue(3.000);

                entity.Property(e => e.RealRate)
                    .HasColumnType("real")
                    .HasDefaultValue(1.500);

                entity.Property(e => e.UnitGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.CurrencyGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.DataSourceTech)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.GeoCodeTech)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.DataSourcePrice)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.GeoCodePrice)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
                entity.Property(e => e.RatingGroup)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");
            });

            modelBuilder.Entity<AccountToMember>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountId).HasName("ixAccountToMemberAccountId");

                entity.HasIndex(e => e.MemberId).HasName("ixAccountToMemberMemberId");

                entity.Property(e => e.IsDefaultClub).HasDefaultValue(false);

                entity.Property(e => e.MemberRole)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("contributor");

                entity.Ignore(e => e.ClubInUse);
                entity.Ignore(e => e.AuthorizationLevel);
                entity.Ignore(e => e.URIFull);
                entity.Ignore(e => e.MemberDocFullPath);

                entity.HasOne(d => d.ClubDefault).WithMany(p => p.AccountToMember).HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Member).WithMany(p => p.AccountToMember).HasForeignKey(d => d.MemberId);
            });

            modelBuilder.Entity<AccountToNetwork>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountId).HasName("ixAccountToNetworkAccountId");

                entity.HasIndex(e => e.NetworkId).HasName("ixAccountToNetworkToNetworkId");

                entity.Property(e => e.DefaultGetDataFromType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("web");

                entity.Property(e => e.DefaultStoreDataAtType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("web");

                entity.Property(e => e.IsDefaultNetwork).HasDefaultValue(false);

                entity.Property(e => e.NetworkId).HasDefaultValue(1);

                entity.Property(e => e.NetworkRole)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("contributor");

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToNetwork).HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Network).WithMany(p => p.AccountToNetwork).HasForeignKey(d => d.NetworkId);
            });

            modelBuilder.Entity<AccountToPayment>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountToServiceId).HasName("ixAccountToPaymentServiceId");

                entity.Property(e => e.CreditDue).HasColumnType("money");

                entity.Property(e => e.CreditDueDate).HasColumnType("datetime");

                entity.Property(e => e.CreditPaid).HasColumnType("money");

                entity.Property(e => e.CreditPaidDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDue).HasColumnType("money");

                entity.Property(e => e.PaymentDueDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentPaid).HasColumnType("money");

                entity.Property(e => e.PaymentPaidDate).HasColumnType("datetime");

                entity.HasOne(d => d.AccountToService).WithMany(p => p.AccountToPayment).HasForeignKey(d => d.AccountToServiceId);
            });

            modelBuilder.Entity<AccountToService>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.AccountId).HasName("ixAccountToServiceAccountId");

                entity.HasIndex(e => e.ServiceId).HasName("ixAccountToServiceServiceId");

                entity.Property(e => e.AccountId).HasDefaultValue(1);

                entity.Property(e => e.Amount1).HasDefaultValue(1);

                entity.Property(e => e.AuthorizationLevel).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(1));

                entity.Ignore(e => e.IsSelected);
                entity.Ignore(e => e.OwningClubId);
                entity.Ignore(e => e.HostServiceFee);
                entity.Ignore(e => e.HostServiceRate);
                entity.Ignore(e => e.TotalCost);
                entity.Ignore(e => e.NetCost);

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.IsOwner).HasDefaultValue(false);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("none");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("received");

                entity.Property(e => e.StatusDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.HasOne(d => d.Account).WithMany(p => p.AccountToService).HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Service).WithMany(p => p.AccountToService).HasForeignKey(d => d.ServiceId);
            });

            

            modelBuilder.Entity<BudgetSystem>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixBudgetSystemToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixBudgetSystemToBudgetSystemTypeId");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(1)/(1))/(2009");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.ServiceId).HasDefaultValue(200);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.BudgetSystem).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.BudgetSystemType).WithMany(p => p.BudgetSystem).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BudgetSystemToEnterprise>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemId).HasName("ixBudgetSystemToEnterprise_BudgetSystemId");

                entity.Property(e => e.BudgetSystemId).HasDefaultValue(1);

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(3);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.InitialValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.Num2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("n/a");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.BudgetSystem).WithMany(p => p.BudgetSystemToEnterprise).HasForeignKey(d => d.BudgetSystemId);
            });

            modelBuilder.Entity<BudgetSystemToInput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemToOperationId).HasName("ixBudgetSystemToInput_BudgetSystemToOperationId");

                entity.HasIndex(e => e.InputId).HasName("ixBudgetSystemToInput_InputId");

                entity.Property(e => e.BudgetSystemToOperationId).HasDefaultValue(0);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputId).HasDefaultValue(0);

                entity.Property(e => e.InputPrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice3Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputTimes).HasDefaultValue(0f);

                entity.Property(e => e.InputUseAOHOnly).HasDefaultValue(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.BudgetSystemToOperation).WithMany(p => p.BudgetSystemToInput).HasForeignKey(d => d.BudgetSystemToOperationId);

                entity.HasOne(d => d.InputSeries).WithMany(p => p.BudgetSystemToInput).HasForeignKey(d => d.InputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BudgetSystemToOperation>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemToTimeId).HasName("ixBudSysToOperationToTimeId");

                entity.HasIndex(e => e.OperationId).HasName("ixBudSysToOperationOperationId");

                entity.Property(e => e.Amount).HasDefaultValue(0f);

                entity.Property(e => e.BudgetSystemToTimeId).HasDefaultValue(0);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10101");

                entity.Property(e => e.OperationId).HasDefaultValue(0);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(0f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("ac");

                entity.HasOne(d => d.BudgetSystemToTime).WithMany(p => p.BudgetSystemToOperation).HasForeignKey(d => d.BudgetSystemToTimeId);

                entity.HasOne(d => d.Operation).WithMany(p => p.BudgetSystemToOperation).HasForeignKey(d => d.OperationId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BudgetSystemToOutcome>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemToTimeId).HasName("ixBudgetSystemToOutcomeToTimeId");

                entity.HasIndex(e => e.OutcomeId).HasName("ixBudgetSystemToOutcomeToOutcomeId");

                entity.Property(e => e.Amount).HasDefaultValue(0f);

                entity.Property(e => e.BudgetSystemToTimeId).HasDefaultValue(0);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.OutcomeId).HasDefaultValue(0);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(0f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("acre");

                entity.HasOne(d => d.BudgetSystemToTime).WithMany(p => p.BudgetSystemToOutcome).HasForeignKey(d => d.BudgetSystemToTimeId);

                entity.HasOne(d => d.Outcome).WithMany(p => p.BudgetSystemToOutcome).HasForeignKey(d => d.OutcomeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BudgetSystemToOutput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemToOutcomeId).HasName("ixBudgetSystemToOuput_BudgetSystemToOutcomeId");

                entity.HasIndex(e => e.OutputId).HasName("ixBudgetSystemToOutput_OutputId");

                entity.Property(e => e.BudgetSystemToOutcomeId).HasDefaultValue(0);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.Property(e => e.OutputAmount1).HasDefaultValue(0f);

                entity.Property(e => e.OutputCompositionAmount).HasDefaultValue(1f);

                entity.Property(e => e.OutputCompositionUnit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("ac");

                entity.Property(e => e.OutputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputId).HasDefaultValue(0);

                entity.Property(e => e.OutputTimes).HasDefaultValue(1f);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.BudgetSystemToOutcome).WithMany(p => p.BudgetSystemToOutput).HasForeignKey(d => d.BudgetSystemToOutcomeId);

                entity.HasOne(d => d.OutputSeries).WithMany(p => p.BudgetSystemToOutput).HasForeignKey(d => d.OutputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BudgetSystemToTime>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.BudgetSystemToEnterpriseId).HasName("ixBudgetSystemToTime_BudgetSystemoEnterpriseId");

                entity.HasIndex(e => e.Date).HasName("ixBudgetSystemToTimeDate");

                entity.Property(e => e.AOHFactor).HasDefaultValue(1f);

                entity.Property(e => e.BudgetSystemToEnterpriseId).HasDefaultValue(1);

                entity.Property(e => e.CommonRefYorN).HasDefaultValue(true);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DiscountYorN).HasDefaultValue(true);

                entity.Property(e => e.EnterpriseAmount).HasDefaultValue(1f);

                entity.Property(e => e.EnterpriseName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("Enterprise 1");

                entity.Property(e => e.EnterpriseUnit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.GrowthPeriods).HasDefaultValue(1);

                entity.Property(e => e.GrowthTypeId).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(1));

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("Year 1");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.HasOne(d => d.BudgetSystemToEnterprise).WithMany(p => p.BudgetSystemToTime).HasForeignKey(d => d.BudgetSystemToEnterpriseId);
            });

            modelBuilder.Entity<BudgetSystemType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ComponentClassId).HasName("ixComponentComponentClassId");

                entity.Property(e => e.Amount).HasDefaultValue(1f);

                entity.Property(e => e.ComponentClassId).HasDefaultValue(1);

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.Num2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(1f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("each");

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.ComponentClass).WithMany(p => p.Component).HasForeignKey(d => d.ComponentClassId);
            });

            modelBuilder.Entity<ComponentClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixComponentClassToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixComponentClassToComponentTypeId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10");

                entity.Property(e => e.PriceListYorN).HasDefaultValue(false);

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.ComponentClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ComponentType).WithMany(p => p.ComponentClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ComponentToInput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ComponentId).HasName("ixComponentToInputComponentId");

                entity.HasIndex(e => e.InputId).HasName("ixComponentToInputInputId");

                entity.Property(e => e.ComponentId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputId).HasDefaultValue(1);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice3Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputTimes).HasDefaultValue(1f);

                entity.Property(e => e.InputUseAOHOnly).HasDefaultValue(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("default");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.Component).WithMany(p => p.ComponentToInput).HasForeignKey(d => d.ComponentId);

                entity.HasOne(d => d.InputSeries).WithMany(p => p.ComponentToInput).HasForeignKey(d => d.InputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ComponentType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<CostSystem>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixCostSystemToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixCostSystemCostSystemTypeId");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(1)/(1))/(2003");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.ServiceId).HasDefaultValue(300);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.CostSystem).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CostSystemType).WithMany(p => p.CostSystem).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CostSystemToComponent>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ComponentId).HasName("ixCostSystemToComponent_ComponentId");

                entity.HasIndex(e => e.CostSystemToTimeId).HasName("ixCostSystemToComponent_CostSystemToTimeId");

                entity.Property(e => e.Amount).HasDefaultValue(0f);

                entity.Property(e => e.ComponentId).HasDefaultValue(0);

                entity.Property(e => e.CostSystemToTimeId).HasDefaultValue(0);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10101");

                entity.Property(e => e.ResourceWeight).HasDefaultValue(0f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("ac");

                entity.HasOne(d => d.Component).WithMany(p => p.CostSystemToComponent).HasForeignKey(d => d.ComponentId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CostSystemToTime).WithMany(p => p.CostSystemToComponent).HasForeignKey(d => d.CostSystemToTimeId);
            });

            modelBuilder.Entity<CostSystemToInput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CostSystemToComponentId).HasName("ixCostSystemToInput_CostSystemToComponentId");

                entity.HasIndex(e => e.InputId).HasName("ixCostSystemToInput_InputId");

                entity.Property(e => e.CostSystemToComponentId).HasDefaultValue(0);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputId).HasDefaultValue(0);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice3Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputTimes).HasDefaultValue(1f);

                entity.Property(e => e.InputUseAOHOnly).HasDefaultValue(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.CostSystemToComponent).WithMany(p => p.CostSystemToInput).HasForeignKey(d => d.CostSystemToComponentId);

                entity.HasOne(d => d.InputSeries).WithMany(p => p.CostSystemToInput).HasForeignKey(d => d.InputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CostSystemToOutcome>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CostSystemToTimeId).HasName("ixCostSystemToOutcomeToTimeId");

                entity.HasIndex(e => e.OutcomeId).HasName("ixCostSystemToOutcomeToOutcomeId");

                entity.Property(e => e.Amount).HasDefaultValue(1f);

                entity.Property(e => e.CostSystemToTimeId).HasDefaultValue(0);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.OutcomeId).HasDefaultValue(0);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(0f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("acre");

                entity.HasOne(d => d.CostSystemToTime).WithMany(p => p.CostSystemToOutcome).HasForeignKey(d => d.CostSystemToTimeId);

                entity.HasOne(d => d.Outcome).WithMany(p => p.CostSystemToOutcome).HasForeignKey(d => d.OutcomeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CostSystemToOutput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CostSystemToOutcomeId).HasName("ixCostSystemToOuput_CostSystemToOutcomeId");

                entity.HasIndex(e => e.OutputId).HasName("ixCostSystemToOutput_OutputId");

                entity.Property(e => e.CostSystemToOutcomeId).HasDefaultValue(0);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.Property(e => e.OutputAmount1).HasDefaultValue(0f);

                entity.Property(e => e.OutputCompositionAmount).HasDefaultValue(1f);

                entity.Property(e => e.OutputCompositionUnit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("ac");

                entity.Property(e => e.OutputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputId).HasDefaultValue(0);

                entity.Property(e => e.OutputTimes).HasDefaultValue(1f);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.CostSystemToOutcome).WithMany(p => p.CostSystemToOutput).HasForeignKey(d => d.CostSystemToOutcomeId);

                entity.HasOne(d => d.OutputSeries).WithMany(p => p.CostSystemToOutput).HasForeignKey(d => d.OutputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CostSystemToPractice>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CostSystemId).HasName("ixCostSystemToPracticeToCostSystemId");

                entity.Property(e => e.CostSystemId).HasDefaultValue(1);

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(3);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.InitialValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.Num2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("n/a");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.CostSystem).WithMany(p => p.CostSystemToPractice).HasForeignKey(d => d.CostSystemId);
            });

            modelBuilder.Entity<CostSystemToTime>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CostSystemToPracticeId).HasName("ixCostSystemToTimeToCostSystemToPracticeId");

                entity.HasIndex(e => e.Date).HasName("ixCostSystemToTimeDate");

                entity.Property(e => e.AOHFactor).HasDefaultValue(1f);

                entity.Property(e => e.CommonRefYorN).HasDefaultValue(true);

                entity.Property(e => e.CostSystemToPracticeId).HasDefaultValue(1);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DiscountYorN).HasDefaultValue(true);

                entity.Property(e => e.GrowthPeriods).HasDefaultValue(1);

                entity.Property(e => e.GrowthTypeId).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(1));

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("Year 1");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.Property(e => e.PracticeAmount).HasDefaultValue(1f);

                entity.Property(e => e.PracticeName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("Enterprise1");

                entity.Property(e => e.PracticeUnit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.HasOne(d => d.CostSystemToPractice).WithMany(p => p.CostSystemToTime).HasForeignKey(d => d.CostSystemToPracticeId);
            });

            modelBuilder.Entity<CostSystemType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.CurrencyClassId).HasName("IX_CurrencyClassId");

                entity.Property(e => e.CurrencyDate).HasColumnType("datetime");

                entity.HasOne(d => d.CurrencyClass).WithMany(p => p.Currency).HasForeignKey(d => d.CurrencyClassId);
            });

            modelBuilder.Entity<CurrencyClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.CurrencyClassAbbrev)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.CurrencyClassDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CurrencyClassName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<CurrencyConversion>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.Currency1Id).HasName("IX_Currency1Id");

                entity.Property(e => e.CurrencyFromName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.CurrencyToName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Currency1).WithMany(p => p.CurrencyConversion).HasForeignKey(d => d.Currency1Id);
            });

            modelBuilder.Entity<DataSourcePrice>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.GeoCodeId).HasName("ixDataSourcePriceGeoCodeId");

                entity.Property(e => e.AccountId).HasDefaultValue(1);

                entity.Property(e => e.DSPriceDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no description");

                entity.Property(e => e.DSPriceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.Property(e => e.DSPriceURL)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no url");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);
            });

            modelBuilder.Entity<DataSourceTech>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.GeoCodeId).HasName("ixDataSourceTechGeoCodeId");

                entity.Property(e => e.AccountId).HasDefaultValue(1);

                entity.Property(e => e.DSTechDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no description");

                entity.Property(e => e.DSTechName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.Property(e => e.DSTechURL)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no url");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);
            });

            modelBuilder.Entity<DevPack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.DevPackDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DevPackDocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.DevPackKeywords)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs keywords");

                entity.Property(e => e.DevPackLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.DevPackName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.DevPackNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");
                entity.Property(e => e.DevPackMetaDataXml).IsRequired(false);
            });

            modelBuilder.Entity<DevPackClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixServiceToDevPackClass");

                entity.HasIndex(e => e.TypeId).HasName("ixDevPackTypeToDevPackClass");

                entity.Property(e => e.DevPackClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DevPackClassName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.DevPackClassNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.DevPackClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.DevPackType).WithMany(p => p.DevPackClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DevPackClassToDevPack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.DevPackClassId).HasName("ixDevPackClassId");

                entity.HasIndex(e => e.DevPackId).HasName("ixDevPackId");

                entity.HasIndex(e => e.ParentId).HasName("ixDevPackClassToDevPackParentId");

                entity.Property(e => e.DevPackClassAndPackDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("description");

                entity.Property(e => e.DevPackClassAndPackFileExtensionType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");

                entity.Property(e => e.DevPackClassAndPackName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("name");

                entity.Property(e => e.DevPackClassAndPackSortLabel)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("label");

                entity.Property(e => e.DevPackClassId).HasDefaultValue(1);

                entity.Property(e => e.DevPackId).HasDefaultValue(1);

                entity.HasOne(d => d.DevPackClass).WithMany(p => p.DevPackClassToDevPack).HasForeignKey(d => d.DevPackClassId);

                entity.HasOne(d => d.DevPack).WithMany(p => p.DevPackClassToDevPack).HasForeignKey(d => d.DevPackId);

                entity.HasOne(d => d.DevPackClassToDevPack2).WithMany(p => p.DevPackClassToDevPack1).HasForeignKey(d => d.ParentId);
            });

            modelBuilder.Entity<DevPackPart>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.DevPackPartDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DevPackPartFileName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no file on hand");

                entity.Property(e => e.DevPackPartKeywords)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs keywords");

                entity.Property(e => e.DevPackPartLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.DevPackPartName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.DevPackPartNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("a");

                entity.Property(e => e.DevPackPartVirtualURIPattern)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("none");

                entity.Property(e => e.DevPackPartXmlDoc).IsRequired(false);
            });

            modelBuilder.Entity<DevPackPartToResourcePack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.DevPackToDevPackPartId).HasName("ixDevPackToDevPackPartId");

                entity.HasIndex(e => e.ResourcePackId).HasName("ixResourcePackId");

                entity.Property(e => e.SortLabel)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");

                entity.HasOne(d => d.DevPackToDevPackPart).WithMany(p => p.DevPackPartToResourcePack).HasForeignKey(d => d.DevPackToDevPackPartId);

                entity.HasOne(d => d.ResourcePack).WithMany(p => p.DevPackPartToResourcePack).HasForeignKey(d => d.ResourcePackId);
            });

            modelBuilder.Entity<DevPackToDevPackPart>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.DevPackClassToDevPackId).HasName("ixDevPackClassToDevPackId");

                entity.HasIndex(e => e.DevPackPartId).HasName("ixDevPackPartId");

                entity.Property(e => e.DevPackToDevPackPartDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DevPackToDevPackPartFileExtensionType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");

                entity.Property(e => e.DevPackToDevPackPartName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.DevPackToDevPackPartSortLabel)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("sort label");

                entity.HasOne(d => d.DevPackClassToDevPack).WithMany(p => p.DevPackToDevPackPart).HasForeignKey(d => d.DevPackClassToDevPackId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.DevPackPart).WithMany(p => p.DevPackToDevPackPart).HasForeignKey(d => d.DevPackPartId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DevPackType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<GeoCodes>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ParentId).HasName("IX_GeoCodes_ParentId");

                entity.Property(e => e.PKId).ValueGeneratedNever();

                entity.Property(e => e.GeoCodeNameId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("A");

                entity.Property(e => e.GeoCodeParentNameId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("A");

                entity.Property(e => e.GeoDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("Default");

                entity.Property(e => e.GeoName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("Default");

                entity.Property(e => e.NodeType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("node");

                entity.Property(e => e.ParentId).HasDefaultValue(1);

                entity.Property(e => e.TocParentPath)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("tocPath-0");

                entity.Property(e => e.TocPath)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("tocPath-0");

                entity.Property(e => e.URI)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValue("http://default");

                entity.HasOne(d => d.GeoCode1).WithMany(p => p.GeoCodes1).HasForeignKey(d => d.ParentId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<GeoRegion>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.GeoRegionDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GeoRegionName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.GeoRegionNum)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Incentive>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.IncentiveAmount1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveClassId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("description");

                entity.Property(e => e.IncentiveName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("name");

                entity.Property(e => e.IncentiveNum)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("label");

                entity.Property(e => e.IncentiveRate1).HasDefaultValue(1f);

                entity.HasOne(d => d.IncentiveClass).WithMany(p => p.Incentive).HasForeignKey(d => d.IncentiveClassId);
            });

            modelBuilder.Entity<IncentiveClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.IncentiveClassDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IncentiveClassName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.IncentiveClassNum)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Input>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.InputClassId).HasName("ixInputInputClassId");

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(3);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.InputClassId).HasDefaultValue(1);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputPrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputPrice3)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputUnit1)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no unit");

                entity.Property(e => e.InputUnit2)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no unit");

                entity.Property(e => e.InputUnit3)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("each");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.InputClass).WithMany(p => p.Input).HasForeignKey(d => d.InputClassId);
            });

            modelBuilder.Entity<InputClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixInputClassToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixInputClassInputClassTypeId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));
                //entity.Property(e => e.DocStatus).HasDefaultValue(4);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10");

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.InputClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.InputType).WithMany(p => p.InputClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<InputSeries>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.InputId).HasName("ixInputTimeSeriesInputId");

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(12)-(31))-(2002");

                entity.Property(e => e.InputId).HasDefaultValue(1);

                entity.Property(e => e.InputLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputPrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputPrice3)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.InputUnit1)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.InputUnit2)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.InputUnit3)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.Input).WithMany(p => p.InputSeries).HasForeignKey(d => d.InputId);
            });

            modelBuilder.Entity<InputType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<LinkedView>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewPackId).HasName("ixLinkedViewLinkedViewPackId");

                entity.Property(e => e.LinkedViewAddInHostName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("none");

                entity.Property(e => e.LinkedViewAddInName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("none");

                entity.Property(e => e.LinkedViewDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.LinkedViewFileExtensionType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("none");

                entity.Property(e => e.LinkedViewFileName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.LinkedViewLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.LinkedViewNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");
                entity.Property(e => e.LinkedViewXml).IsRequired(false);

                entity.HasOne(d => d.LinkedViewPack).WithMany(p => p.LinkedView).HasForeignKey(d => d.LinkedViewPackId);
            });

            modelBuilder.Entity<LinkedViewClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixLinkedViewClassServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixLinkedViewClassLinkedViewTypeId");

                entity.Property(e => e.LinkedViewClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.LinkedViewClassName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.LinkedViewClassNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");

                entity.HasOne(d => d.Service).WithMany(p => p.LinkedViewClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.LinkedViewType).WithMany(p => p.LinkedViewClass).HasForeignKey(d => d.TypeId);
            });

            modelBuilder.Entity<LinkedViewPack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewClassId).HasName("ixLinkedViewPackLinkedViewClassId");

                entity.Property(e => e.LinkedViewPackDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.LinkedViewPackDocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.LinkedViewPackKeywords)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs keywords");

                entity.Property(e => e.LinkedViewPackLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.LinkedViewPackName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.LinkedViewPackNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");
                entity.Property(e => e.LinkedViewPackMetaDataXml).IsRequired(false);

                entity.HasOne(d => d.LinkedViewClass).WithMany(p => p.LinkedViewPack).HasForeignKey(d => d.LinkedViewClassId);
            });

            modelBuilder.Entity<LinkedViewToBudgetSystem>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToBudgetSystemLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToBudgetSystemBudgetSystemId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToBudgetSystem).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.BudgetSystem).WithMany(p => p.LinkedViewToBudgetSystem).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToBudgetSystemToEnterprise>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToBudgetSystemToEnterpriseLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToBudgetSystemToEnterpriseBudgetSystemToEnterpriseId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToBudgetSystemToEnterprise).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.BudgetSystemToEnterprise).WithMany(p => p.LinkedViewToBudgetSystemToEnterprise).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToBudgetSystemToTime>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToBudgetSystemToTimeLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToBudgetSystemToTimeBudgetSystemToTimeId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToBudgetSystemToTime).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.BudgetSystemToTime).WithMany(p => p.LinkedViewToBudgetSystemToTime).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToComponent>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToComponentLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToComponentComponentId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToComponent).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Component).WithMany(p => p.LinkedViewToComponent).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToComponentClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToComponentClassLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToComponentClassComponentClassId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToComponentClass).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.ComponentClass).WithMany(p => p.LinkedViewToComponentClass).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToCostSystem>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToCostSystemLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToCostSystemCostSystemId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToCostSystem).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.CostSystem).WithMany(p => p.LinkedViewToCostSystem).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToCostSystemToPractice>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToCostSystemToPracticeLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToCostSystemToPracticeCostSystemToPracticeId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToCostSystemToPractice).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.CostSystemToPractice).WithMany(p => p.LinkedViewToCostSystemToPractice).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToCostSystemToTime>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToCostSystemToTimeLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToCostSystemToTimeCostSystemToTimeId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToCostSystemToTime).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.CostSystemToTime).WithMany(p => p.LinkedViewToCostSystemToTime).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToDevPackJoin>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToDevPackJoinLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToDevPackJoinDevPackJoinId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToDevPackJoin).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.DevPackClassToDevPack).WithMany(p => p.LinkedViewToDevPackJoin).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToDevPackPartJoin>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToDevPackPartJoinLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToDevPackPartJoinDevPackPartJoinId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToDevPackPartJoin).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.DevPackToDevPackPart).WithMany(p => p.LinkedViewToDevPackPartJoin).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToInput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToInputLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToInputInputId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToInput).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Input).WithMany(p => p.LinkedViewToInput).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToInputClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToInputClassLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToInputClassInputClassId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToInputClass).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.InputClass).WithMany(p => p.LinkedViewToInputClass).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToInputSeries>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToInputSeriesLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToInputSeriesInputSeriesId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToInputSeries).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.InputSeries).WithMany(p => p.LinkedViewToInputSeries).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOperation>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOperationLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOperationOperationId");

                entity.Property(e => e.IsDefaultLinkedView).HasDefaultValue(false);

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOperation).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Operation).WithMany(p => p.LinkedViewToOperation).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOperationClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOperationClassLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOperationClassOperationClassId");

                entity.Property(e => e.IsDefaultLinkedView).HasDefaultValue(false);

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOperationClass).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.OperationClass).WithMany(p => p.LinkedViewToOperationClass).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOutcome>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOutcomeToLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOutcomeToLinkingNodeId");

                entity.Property(e => e.IsDefaultLinkedView).HasDefaultValue(false);

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOutcome).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Outcome).WithMany(p => p.LinkedViewToOutcome).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOutcomeClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOutcomeClassToLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOutcomeClassToLinkingNodeId");

                entity.Property(e => e.IsDefaultLinkedView).HasDefaultValue(false);

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOutcomeClass).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.OutcomeClass).WithMany(p => p.LinkedViewToOutcomeClass).HasForeignKey(d => d.LinkingNodeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LinkedViewToOutput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOutputLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOutputOutputId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOutput).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Output).WithMany(p => p.LinkedViewToOutput).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOutputClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOutputClassLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOutputClassOutputClassId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOutputClass).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.OutputClass).WithMany(p => p.LinkedViewToOutputClass).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToOutputSeries>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToOutputSeriesLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToOutputSeriesOutputSeriesId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToOutputSeries).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.OutputSeries).WithMany(p => p.LinkedViewToOutputSeries).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToResource>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToResourceLinkedViewId");

                entity.HasIndex(e => e.LinkingNodeId).HasName("ixLinkedViewToResourceLinkingNodeId");

                entity.Property(e => e.LinkedViewName)
                    .IsRequired()
                    .HasMaxLength(75);
                entity.Property(e => e.LinkingXmlDoc).IsRequired(false);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToResource).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.Resource).WithMany(p => p.LinkedViewToResource).HasForeignKey(d => d.LinkingNodeId);
            });

            modelBuilder.Entity<LinkedViewToResourcePack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.LinkedViewId).HasName("ixLinkedViewToResourcePackLinkedViewId");

                entity.HasIndex(e => e.ResourcePackId).HasName("ixLinkedViewToResourcePackResourcePackId");

                entity.Property(e => e.SortLabel)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.HasOne(d => d.LinkedView).WithMany(p => p.LinkedViewToResourcePack).HasForeignKey(d => d.LinkedViewId);

                entity.HasOne(d => d.ResourcePack).WithMany(p => p.LinkedViewToResourcePack).HasForeignKey(d => d.ResourcePackId);
            });

            modelBuilder.Entity<LinkedViewType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.GeoRegionId).HasName("ixMemberGeoRegionId");

                entity.HasIndex(e => e.MemberClassId).HasName("ixMemberMemberClassId");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValue("none");

                entity.Property(e => e.GeoRegionId).HasDefaultValue(1);

                entity.Property(e => e.MemberAddress1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("needs address");

                entity.Property(e => e.MemberAddress2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("none");

                entity.Property(e => e.MemberCity)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("needs city");

                entity.Property(e => e.MemberClassId).HasDefaultValue(1);

                entity.Property(e => e.MemberCountry)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("needs country");

                entity.Property(e => e.MemberDesc)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.MemberEmail)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValue("needs email");

                entity.Property(e => e.MemberFax)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");

                entity.Property(e => e.MemberFirstName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.MemberJoinedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.MemberLastChangedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.MemberLastName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs last name");

                entity.Property(e => e.MemberOrganization)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("none");

                entity.Property(e => e.MemberPhone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs phone");

                entity.Property(e => e.MemberPhone2)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasDefaultValue("needs mobile");

                entity.Property(e => e.MemberState)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("needs state");

                entity.Property(e => e.MemberUrl)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue("none");

                entity.Property(e => e.MemberZip)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValue("needs zip");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDefaultValue("needs user name");

                entity.HasOne(d => d.GeoRegion).WithMany(p => p.Member).HasForeignKey(d => d.GeoRegionId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.MemberClass).WithMany(p => p.Member).HasForeignKey(d => d.MemberClassId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MemberClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.MemberClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.MemberClassName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.MemberClassNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");
            });

            modelBuilder.Entity<Network>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.GeoRegionId).HasName("ixNetworkGeoRegionId");

                entity.HasIndex(e => e.NetworkClassId).HasName("ixNetworkClassNetworkId");

                entity.Property(e => e.AdminConnection)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GeoRegionId).HasDefaultValue(1);

                entity.Property(e => e.NetworkDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.NetworkName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.NetworkURIPartName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("needsuripartname");

                entity.Property(e => e.WebConnection)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.WebDbPath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.WebFileSystemPath)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Ignore(e => e.IsDefault);
                entity.Ignore(e => e.URIFull);
                entity.Ignore(e => e.XmlConnection);

                entity.HasOne(d => d.GeoRegion).WithMany(p => p.Network).HasForeignKey(d => d.GeoRegionId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.NetworkClass).WithMany(p => p.Network).HasForeignKey(d => d.NetworkClassId);
            });

            modelBuilder.Entity<NetworkClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.NetworkClassControllerName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs controller name");

                entity.Property(e => e.NetworkClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.NetworkClassLabel)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.NetworkClassName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.NetworkClassUserLanguage)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDefaultValue("en-us");
                entity.Ignore(e => e.IsSelected);
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.OperationClassId).HasName("ixOperationClassIdToOperation");

                entity.Property(e => e.Amount).HasDefaultValue(1f);

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.Num2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.OperationClassId).HasDefaultValue(0);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(1f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("acre");

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.OperationClass).WithMany(p => p.Operation).HasForeignKey(d => d.OperationClassId);
            });

            modelBuilder.Entity<OperationClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixOperationClassToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixOperationClassToOperationTypeId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10");

                entity.Property(e => e.PriceListYorN).HasDefaultValue(false);

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.OperationClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.OperationType).WithMany(p => p.OperationClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OperationToInput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.InputId).HasName("ixOperationToInputToInputId");

                entity.HasIndex(e => e.OperationId).HasName("ixOperationToInputToOperationId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.InputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.InputId).HasDefaultValue(1);

                entity.Property(e => e.InputPrice1Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice2Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputPrice3Amount).HasDefaultValue(0f);

                entity.Property(e => e.InputTimes).HasDefaultValue(1f);

                entity.Property(e => e.InputUseAOHOnly).HasDefaultValue(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("default");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.OperationId).HasDefaultValue(1);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.InputSeries).WithMany(p => p.OperationToInput).HasForeignKey(d => d.InputId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Operation).WithMany(p => p.OperationToInput).HasForeignKey(d => d.OperationId);
            });

            modelBuilder.Entity<OperationType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Outcome>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.OutcomeClassId).HasName("icOutComeToOutcomeClass");

                entity.Property(e => e.Amount).HasDefaultValue(1f);

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.EffectiveLife).HasDefaultValue(1f);

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.LastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.Num2)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label2");

                entity.Property(e => e.OutcomeClassId).HasDefaultValue(1);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.ResourceWeight).HasDefaultValue(1f);

                entity.Property(e => e.SalvageValue)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("acre");

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.OutcomeClass).WithMany(p => p.Outcome).HasForeignKey(d => d.OutcomeClassId);
            });

            modelBuilder.Entity<OutcomeClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixOutcomeClassToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixOutcomeClassToType");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.OutcomeClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.OutcomeType).WithMany(p => p.OutcomeClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OutcomeToOutput>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.OutcomeId).HasName("ixOutcomeToOutputToOutcome");

                entity.HasIndex(e => e.OutputId).HasName("ixOutcomeToOutputToOutputSeries");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.IncentiveAmount)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.IncentiveRate).HasDefaultValue(0f);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("default");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.OutcomeId).HasDefaultValue(1);

                entity.Property(e => e.OutputAmount1).HasDefaultValue(0f);

                entity.Property(e => e.OutputCompositionAmount).HasDefaultValue(0f);

                entity.Property(e => e.OutputCompositionUnit)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("each");

                entity.Property(e => e.OutputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputId).HasDefaultValue(1);

                entity.Property(e => e.OutputTimes).HasDefaultValue(1f);

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.HasOne(d => d.Outcome).WithMany(p => p.OutcomeToOutput).HasForeignKey(d => d.OutcomeId);

                entity.HasOne(d => d.OutputSeries).WithMany(p => p.OutcomeToOutput).HasForeignKey(d => d.OutputId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OutcomeType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Output>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.OutputClassId).HasName("ixOutputToOutputClassId");

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(3);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.OutputAmount1).HasDefaultValue(0f);

                entity.Property(e => e.OutputClassId).HasDefaultValue(1);

                entity.Property(e => e.OutputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputPrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.OutputUnit1)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no unit");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.OutputClass).WithMany(p => p.Output).HasForeignKey(d => d.OutputClassId);
            });

            modelBuilder.Entity<OutputClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixOutputClassToServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixOutputClassOutputClassTypeId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.DocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A10");

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.OutputClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.OutputType).WithMany(p => p.OutputClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OutputSeries>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.OutputId).HasName("ixOutputTimeSeriesOutputId");

                entity.Property(e => e.CurrencyClassId).HasDefaultValue(1);

                entity.Property(e => e.DataSourceId).HasDefaultValue(1);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.GeoCodeId).HasDefaultValue(1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NominalRateId).HasDefaultValue(1);

                entity.Property(e => e.Num)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("no label");

                entity.Property(e => e.OutputAmount1).HasDefaultValue(0f);

                entity.Property(e => e.OutputDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(12)/(31))/(2002");

                entity.Property(e => e.OutputId).HasDefaultValue(1);

                entity.Property(e => e.OutputLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.OutputPrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.OutputUnit1)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("none");

                entity.Property(e => e.RatingClassId).HasDefaultValue(1);

                entity.Property(e => e.RealRateId).HasDefaultValue(1);

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.HasOne(d => d.Output).WithMany(p => p.OutputSeries).HasForeignKey(d => d.OutputId);
            });

            modelBuilder.Entity<OutputType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("A");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no name");

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.RateClassId).HasName("IX_RateClassId");

                entity.Property(e => e.RateDate).HasColumnType("datetime");

                entity.Property(e => e.RateEnum)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("nominal");

                entity.Property(e => e.RateName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.RateClass).WithMany(p => p.Rate).HasForeignKey(d => d.RateClassId);
            });

            modelBuilder.Entity<RateClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.RateClassName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RateClassYear).HasColumnType("datetime");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.RatingClassId).HasName("ixRatingRatingClassId");

                entity.Property(e => e.RatingClassId).HasDefaultValue(0);

                entity.Property(e => e.RatingName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.Property(e => e.RatingValue).HasDefaultValue(0f);
            });

            modelBuilder.Entity<RatingClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.RatingClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no description");

                entity.Property(e => e.RatingClassName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");
            });

            modelBuilder.Entity<DevTreks.Models.Resource>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ResourcePackId).HasName("ixResourceResourceClassId");

                //2.0.0: EF error msg: Data type 'varbinary' is not supported in this form.
                //Either specify the length explicitly in the type name, for example as 'varbinary(16)', or remove the data type and use APIs such as HasMaxLength to allow EF choose the data type
                string varbinary = string.Concat("varbinary(", this.BinaryMaxLength.ToString(), ")");
                entity.Property(e => e.ResourceBinary).HasColumnType(varbinary);
                //entity.Property(e => e.ResourceBinary).HasColumnType("varbinary");

                entity.Property(e => e.ResourceFileName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("no file on hand");

                entity.Property(e => e.ResourceGeneralType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("image");

                entity.Property(e => e.ResourceLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.ResourceLongDesc)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValue("needs long description");

                entity.Property(e => e.ResourceMimeType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("application/xhtml+xml");

                entity.Property(e => e.ResourceName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.ResourceNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("txt");

                entity.Property(e => e.ResourcePackId).HasDefaultValue(2);

                entity.Property(e => e.ResourceShortDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.ResourceTagNameForApps)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("none");

                entity.HasOne(d => d.ResourcePack).WithMany(p => p.Resource).HasForeignKey(d => d.ResourcePackId);
            });

            modelBuilder.Entity<ResourceClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ServiceId).HasName("ixResourceClassServiceId");

                entity.HasIndex(e => e.TypeId).HasName("ixResourceClassResourceTypeId");

                entity.Property(e => e.ResourceClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.ResourceClassName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.ResourceClassNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");

                entity.Property(e => e.ServiceId).HasDefaultValue(1);

                entity.Property(e => e.TypeId).HasDefaultValue(1);

                entity.HasOne(d => d.Service).WithMany(p => p.ResourceClass).HasForeignKey(d => d.ServiceId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ResourceType).WithMany(p => p.ResourceClass).HasForeignKey(d => d.TypeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ResourcePack>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.ResourceClassId).HasName("ixResourcePackResourceClassId");

                entity.Property(e => e.ResourceClassId).HasDefaultValue(1);

                entity.Property(e => e.ResourcePackDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs description");

                entity.Property(e => e.ResourcePackDocStatus).HasDefaultValue(Helpers.GeneralHelpers.ConvertInt32Int16(4));

                entity.Property(e => e.ResourcePackKeywords)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("needs keywords");

                entity.Property(e => e.ResourcePackLastChangedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.ResourcePackName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .HasDefaultValue("needs name");

                entity.Property(e => e.ResourcePackNum)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDefaultValue("none");
                entity.Property(e => e.ResourcePackMetaDataXml).IsRequired(false);

                entity.HasOne(d => d.ResourceClass).WithMany(p => p.ResourcePack).HasForeignKey(d => d.ResourceClassId);
            });

            modelBuilder.Entity<ResourceType>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("needs label");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.NetworkId).HasDefaultValue(0);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(0);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.NetworkId).HasName("ixServiceNetworkId");

                entity.Property(e => e.NetworkId).HasDefaultValue(1);

                entity.Property(e => e.ServiceClassId).HasDefaultValue(1);

                entity.Property(e => e.ServiceCurrency1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("USA Dollar");

                entity.Property(e => e.ServiceDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("description");

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("name");

                entity.Property(e => e.ServiceNum)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("label");

                entity.Property(e => e.ServicePrice1)
                    .HasColumnType("money")
                    .HasDefaultValue(0m);

                entity.Property(e => e.ServiceUnit1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("each");

                entity.Ignore(e => e.IsSelected);
                entity.Ignore(e => e.SubscribedClubs);

                entity.HasOne(d => d.Network).WithMany(p => p.Service).HasForeignKey(d => d.NetworkId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ServiceClass).WithMany(p => p.Service).HasForeignKey(d => d.ServiceClassId);
            });

            modelBuilder.Entity<ServiceClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.ServiceClassDesc)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ServiceClassName)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.ServiceClassNum)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Ignore(e => e.IsSelected);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.HasIndex(e => e.UnitClassId).HasName("ixUnitClassId");

                entity.Property(e => e.UnitClassId).HasDefaultValue(1);

                entity.Property(e => e.UnitName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.Property(e => e.UnitNameAbbrev)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");
            });

            modelBuilder.Entity<UnitClass>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.UnitClassDesc)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValue("no description");

                entity.Property(e => e.UnitClassName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.Property(e => e.UnitClassType)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDefaultValue("metric");
            });

            modelBuilder.Entity<UnitConversion>(entity =>
            {
                entity.HasKey(e => e.PKId);

                entity.Property(e => e.IsBestConversion).HasDefaultValue(false);

                entity.Property(e => e.Unit1Id).HasDefaultValue(0);

                entity.Property(e => e.Unit2Id).HasDefaultValue(9);

                entity.Property(e => e.UnitConversionFactor).HasDefaultValue(1f);

                entity.Property(e => e.UnitToClassId).HasDefaultValue(1);

                entity.Property(e => e.UnitToName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasDefaultValue("no name");

                entity.HasOne(d => d.Unit1).WithMany(p => p.UnitConversion).HasForeignKey(d => d.Unit1Id);
            });

            modelBuilder.Entity<sysdiagrams>(entity =>
            {
            entity.HasKey(e => e.diagram_id);
            //2.0.0
            string varbinary = string.Concat("varbinary(", this.BinaryMaxLength.ToString(), ")");
            entity.Property(e => e.definition).HasColumnType(varbinary);
            });
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountClass> AccountClass { get; set; }
        public virtual DbSet<AccountToAddIn> AccountToAddIn { get; set; }
        public virtual DbSet<AccountToAudit> AccountToAudit { get; set; }
        public virtual DbSet<AccountToCredit> AccountToCredit { get; set; }
        public virtual DbSet<AccountToIncentive> AccountToIncentive { get; set; }
        public virtual DbSet<AccountToLocal> AccountToLocal { get; set; }
        public virtual DbSet<AccountToMember> AccountToMember { get; set; }
        public virtual DbSet<AccountToNetwork> AccountToNetwork { get; set; }
        public virtual DbSet<AccountToPayment> AccountToPayment { get; set; }
        public virtual DbSet<AccountToService> AccountToService { get; set; }
        
        public virtual DbSet<BudgetSystem> BudgetSystem { get; set; }
        public virtual DbSet<BudgetSystemToEnterprise> BudgetSystemToEnterprise { get; set; }
        public virtual DbSet<BudgetSystemToInput> BudgetSystemToInput { get; set; }
        public virtual DbSet<BudgetSystemToOperation> BudgetSystemToOperation { get; set; }
        public virtual DbSet<BudgetSystemToOutcome> BudgetSystemToOutcome { get; set; }
        public virtual DbSet<BudgetSystemToOutput> BudgetSystemToOutput { get; set; }
        public virtual DbSet<BudgetSystemToTime> BudgetSystemToTime { get; set; }
        public virtual DbSet<BudgetSystemType> BudgetSystemType { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<ComponentClass> ComponentClass { get; set; }
        public virtual DbSet<ComponentToInput> ComponentToInput { get; set; }
        public virtual DbSet<ComponentType> ComponentType { get; set; }
        public virtual DbSet<CostSystem> CostSystem { get; set; }
        public virtual DbSet<CostSystemToComponent> CostSystemToComponent { get; set; }
        public virtual DbSet<CostSystemToInput> CostSystemToInput { get; set; }
        public virtual DbSet<CostSystemToOutcome> CostSystemToOutcome { get; set; }
        public virtual DbSet<CostSystemToOutput> CostSystemToOutput { get; set; }
        public virtual DbSet<CostSystemToPractice> CostSystemToPractice { get; set; }
        public virtual DbSet<CostSystemToTime> CostSystemToTime { get; set; }
        public virtual DbSet<CostSystemType> CostSystemType { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyClass> CurrencyClass { get; set; }
        public virtual DbSet<CurrencyConversion> CurrencyConversion { get; set; }
        public virtual DbSet<DataSourcePrice> DataSourcePrice { get; set; }
        public virtual DbSet<DataSourceTech> DataSourceTech { get; set; }
        public virtual DbSet<DevPack> DevPack { get; set; }
        public virtual DbSet<DevPackClass> DevPackClass { get; set; }
        public virtual DbSet<DevPackClassToDevPack> DevPackClassToDevPack { get; set; }
        public virtual DbSet<DevPackPart> DevPackPart { get; set; }
        public virtual DbSet<DevPackPartToResourcePack> DevPackPartToResourcePack { get; set; }
        public virtual DbSet<DevPackToDevPackPart> DevPackToDevPackPart { get; set; }
        public virtual DbSet<DevPackType> DevPackType { get; set; }
        public virtual DbSet<GeoCodes> GeoCodes { get; set; }
        public virtual DbSet<GeoRegion> GeoRegion { get; set; }
        public virtual DbSet<Incentive> Incentive { get; set; }
        public virtual DbSet<IncentiveClass> IncentiveClass { get; set; }
        public virtual DbSet<Input> Input { get; set; }
        public virtual DbSet<InputClass> InputClass { get; set; }
        public virtual DbSet<InputSeries> InputSeries { get; set; }
        public virtual DbSet<InputType> InputType { get; set; }
        public virtual DbSet<LinkedView> LinkedView { get; set; }
        public virtual DbSet<LinkedViewClass> LinkedViewClass { get; set; }
        public virtual DbSet<LinkedViewPack> LinkedViewPack { get; set; }
        public virtual DbSet<LinkedViewToBudgetSystem> LinkedViewToBudgetSystem { get; set; }
        public virtual DbSet<LinkedViewToBudgetSystemToEnterprise> LinkedViewToBudgetSystemToEnterprise { get; set; }
        public virtual DbSet<LinkedViewToBudgetSystemToTime> LinkedViewToBudgetSystemToTime { get; set; }
        public virtual DbSet<LinkedViewToComponent> LinkedViewToComponent { get; set; }
        public virtual DbSet<LinkedViewToComponentClass> LinkedViewToComponentClass { get; set; }
        public virtual DbSet<LinkedViewToCostSystem> LinkedViewToCostSystem { get; set; }
        public virtual DbSet<LinkedViewToCostSystemToPractice> LinkedViewToCostSystemToPractice { get; set; }
        public virtual DbSet<LinkedViewToCostSystemToTime> LinkedViewToCostSystemToTime { get; set; }
        public virtual DbSet<LinkedViewToDevPackJoin> LinkedViewToDevPackJoin { get; set; }
        public virtual DbSet<LinkedViewToDevPackPartJoin> LinkedViewToDevPackPartJoin { get; set; }
        public virtual DbSet<LinkedViewToInput> LinkedViewToInput { get; set; }
        public virtual DbSet<LinkedViewToInputClass> LinkedViewToInputClass { get; set; }
        public virtual DbSet<LinkedViewToInputSeries> LinkedViewToInputSeries { get; set; }
        public virtual DbSet<LinkedViewToOperation> LinkedViewToOperation { get; set; }
        public virtual DbSet<LinkedViewToOperationClass> LinkedViewToOperationClass { get; set; }
        public virtual DbSet<LinkedViewToOutcome> LinkedViewToOutcome { get; set; }
        public virtual DbSet<LinkedViewToOutcomeClass> LinkedViewToOutcomeClass { get; set; }
        public virtual DbSet<LinkedViewToOutput> LinkedViewToOutput { get; set; }
        public virtual DbSet<LinkedViewToOutputClass> LinkedViewToOutputClass { get; set; }
        public virtual DbSet<LinkedViewToOutputSeries> LinkedViewToOutputSeries { get; set; }
        public virtual DbSet<LinkedViewToResource> LinkedViewToResource { get; set; }
        public virtual DbSet<LinkedViewToResourcePack> LinkedViewToResourcePack { get; set; }
        public virtual DbSet<LinkedViewType> LinkedViewType { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberClass> MemberClass { get; set; }
        public virtual DbSet<Network> Network { get; set; }
        public virtual DbSet<NetworkClass> NetworkClass { get; set; }
        public virtual DbSet<Operation> Operation { get; set; }
        public virtual DbSet<OperationClass> OperationClass { get; set; }
        public virtual DbSet<OperationToInput> OperationToInput { get; set; }
        public virtual DbSet<OperationType> OperationType { get; set; }
        public virtual DbSet<Outcome> Outcome { get; set; }
        public virtual DbSet<OutcomeClass> OutcomeClass { get; set; }
        public virtual DbSet<OutcomeToOutput> OutcomeToOutput { get; set; }
        public virtual DbSet<OutcomeType> OutcomeType { get; set; }
        public virtual DbSet<Output> Output { get; set; }
        public virtual DbSet<OutputClass> OutputClass { get; set; }
        public virtual DbSet<OutputSeries> OutputSeries { get; set; }
        public virtual DbSet<OutputType> OutputType { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<RateClass> RateClass { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<RatingClass> RatingClass { get; set; }
        public virtual DbSet<DevTreks.Models.Resource> Resource { get; set; }
        public virtual DbSet<ResourceClass> ResourceClass { get; set; }
        public virtual DbSet<ResourcePack> ResourcePack { get; set; }
        public virtual DbSet<ResourceType> ResourceType { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceClass> ServiceClass { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitClass> UnitClass { get; set; }
        public virtual DbSet<UnitConversion> UnitConversion { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        protected void Dispose(bool disposing)
        {
            //close the connection to the database
            base.Database.CloseConnection();
            base.Dispose();
        }
    }
}