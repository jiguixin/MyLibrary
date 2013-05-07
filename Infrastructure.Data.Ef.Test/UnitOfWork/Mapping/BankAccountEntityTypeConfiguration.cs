

using System.Data.Entity.ModelConfiguration;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;

namespace Infrastructure.Data.Ef.Test.UnitOfWork.Mapping
{
    /// <summary>
    /// The entity type configuration
    /// </summary>
    class BankAccountEntityTypeConfiguration
        :EntityTypeConfiguration<BankAccount>
    {
        public BankAccountEntityTypeConfiguration()
        {
            //key and properties

            this.HasKey(ba => ba.Id);

            this.Property(ba => ba.Balance)
                .HasPrecision(14, 2);
            this.Property(ba => ba.CreateTime).HasColumnType("datetime2"); 

            //associations))))
            this.HasRequired(ba => ba.Customer)
                .WithMany()
                .HasForeignKey(ba => ba.CustomerId)
                .WillCascadeOnDelete(false);

            this.HasMany(ba => ba.BankAccountActivity)
                .WithRequired()
                .HasForeignKey(ba => ba.BankAccountId)
                .WillCascadeOnDelete(true);

        }
    }
}
