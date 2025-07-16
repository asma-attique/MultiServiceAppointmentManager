using Microsoft.EntityFrameworkCore;
using MultiServiceAppointmentManager.Models;

namespace MultiServiceAppointmentManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Provider> Providers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Provider ⇄ Service (Many-to-Many)
            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Services)
                .WithMany(s => s.Providers);

            // ✅ Appointment ⇄ Service (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany() // Optional: .WithMany(s => s.Appointments) if you add navigation
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // or Cascade if you prefer

            // ✅ Appointment ⇄ Provider (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany() // Optional: .WithMany(p => p.Appointments) if you add navigation
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Example seed data
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Haircut", Charges = 1000 },
                new Service { Id = 2, Name = "Facial", Charges = 2000 }
            );
        }
    }
}
