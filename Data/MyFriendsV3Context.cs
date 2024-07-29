using Microsoft.EntityFrameworkCore;
using MyFriendsV3.Models;

namespace MyFriendsV3.Data
{
    public class MyFriendsV3Context : DbContext
    {
        public MyFriendsV3Context (DbContextOptions<MyFriendsV3Context> options)
            : base(options)
        {
        }

        public DbSet<MyFriendsV3.Models.Picture> Picture { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between User and Picture
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserPictures)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Configure the one-to-one relationship between User and ProfilePicture
            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfilePicture)
                .WithOne()
                .HasForeignKey<User>(u => u.ProfilePictureId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
        public DbSet<MyFriendsV3.Models.User> User { get; set; } = default!;
    }
}
