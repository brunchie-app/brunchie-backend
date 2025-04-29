using brunchie_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pomelo.EntityFrameworkCore.MySql.Query.Expressions.Internal;
using System.Reflection.Emit;




namespace brunchie_backend.DataBase
{
    public class OrderConfig : IEntityTypeConfiguration<Order>

    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.OrderId);


            builder.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(p => p.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(p => p.VendorId)
                 .OnDelete(DeleteBehavior.Cascade);



        }
    }

    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>

    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(c => c.Id);


            builder.HasOne<Order>()
                 .WithMany()
                 .HasForeignKey(p => p.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MenuItem>()
                 .WithMany()
                 .HasForeignKey(p => p.MenuItemId)
                 .OnDelete(DeleteBehavior.Cascade);



        }
    }

    public class MenuItemConfig : IEntityTypeConfiguration<MenuItem>

    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.HasKey(c => c.ItemId);


            builder.HasOne<User>()
                 .WithMany()
                 .HasForeignKey(p => p.VendorId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>

    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(c => c.FeedbackId);


            builder.HasOne<Order>()
                 .WithMany()
                 .HasForeignKey(p => p.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class CampusConfig : IEntityTypeConfiguration<Campus>

    {
        public void Configure(EntityTypeBuilder<Campus> builder)
        {
            builder.HasKey(c => c.CampusId);


        }
    }
}
