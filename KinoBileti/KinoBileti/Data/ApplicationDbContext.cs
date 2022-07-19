using KinoBileti.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinoBileti.Data
{
    public class ApplicationDbContext : IdentityDbContext<KinoBiletUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bilet> Bilets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCards { get; set; }
        public virtual DbSet<BiletInShoppingCart> BiletInShoppings { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<BiletInOrder> BiletInOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bilet>()
              .Property(z => z.Id)
              .ValueGeneratedOnAdd();
            builder.Entity<ShoppingCart>()
              .Property(z => z.Id)
              .ValueGeneratedOnAdd();
            builder.Entity<BiletInShoppingCart>()
             .HasKey(z => new { z.ShoppingCartId, z.BiletId });

            builder.Entity<BiletInShoppingCart>()
                .HasOne(z => z.bilet)
                .WithMany(z => z.biletInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<BiletInShoppingCart>()
               .HasOne(z => z.shoppingCart)
               .WithMany(z => z.biletInShoppingCarts)
               .HasForeignKey(z => z.BiletId);

            builder.Entity<ShoppingCart>()
                .HasOne<KinoBiletUser>(z => z.owner)
                .WithOne(z => z.userCart)
                .HasForeignKey<ShoppingCart>(z => z.ownerId);
            builder.Entity<BiletInOrder>()
                .HasKey(z => new { z.BiletId, z.OrderId });
            builder.Entity<BiletInOrder>()
                .HasOne(z => z.BIlet)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.BiletId);
            builder.Entity<BiletInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.Bilets)
                .HasForeignKey(z => z.OrderId);
            builder.Entity<Order>()
                .HasOne<KinoBiletUser>(z => z.user)
                .WithMany(z => z.Orders)
               ;
        }
    }
}
