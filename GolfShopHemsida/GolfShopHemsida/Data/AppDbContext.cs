using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GolfShopHemsida.Models;
using System;

public class AppDbContext : IdentityDbContext<GolfShopUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<FollowUser> FollowUsers { get; set; } = null!;
    public DbSet<UserActivities> UserActivities { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Post-User relationship (User has many Posts)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.GolfShopUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Comment-User relationship (User has many Comments)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.GolfShopUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure FollowUser relationships
        modelBuilder.Entity<FollowUser>()
            .HasOne(f => f.Follower)
            .WithMany()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.ClientCascade); // Changed from Restrict

        modelBuilder.Entity<FollowUser>()
            .HasOne(f => f.Followed)
            .WithMany()
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Post>()
            .Property(p => p.PostId)
            .HasMaxLength(450);

        modelBuilder.Entity<FollowUser>()
            .HasKey(f => new { f.FollowerId, f.FollowedId });
    }
}
