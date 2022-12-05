using Microsoft.EntityFrameworkCore;
using NotificationsApi.Database.Configurations;
using NotificationsApi.Database.Tables;

namespace NotificationsApi.Database
{
    public partial class AspenContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        public DbSet<Subscriber> Subscribers { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        public AspenContext()
        {
        }

        public AspenContext(DbContextOptions<AspenContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Aspen;Trusted_Connection=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new SubscriptionConfiguration().Configure(modelBuilder.Entity<Subscription>());
            new SubscriberConfiguration().Configure(modelBuilder.Entity<Subscriber>());
            new NotificationConfiguration().Configure(modelBuilder.Entity<Notification>());
        }
    }

}