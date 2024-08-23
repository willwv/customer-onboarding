using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Databases.SqlServer.Configurations
{
    public class AddressessConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(address => address.Id);
            builder.HasIndex(address => address.Id)
                .IsUnique();


            builder.HasOne(address => address.User)
                .WithMany(user => user.Addresses)
                .HasForeignKey(address => address.UserId);

            builder.HasIndex(address => address.UserId);
        }
    }
}
