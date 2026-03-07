using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Models;

namespace ParkFlow.Api.Data
{
	public class AppDbContext: DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<VehicleModel> Vehicles { get; set; }
		public DbSet<ParkingSpotModel> ParkingSpots { get; set; }
		public DbSet<TicketModel> Tickets { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleModel>(entity => {
                entity.HasIndex(v => v.LicensePlate).IsUnique();
                entity.Property(v => v.LicensePlate).HasMaxLength(8).IsRequired();
                entity.Property(v => v.Model).HasMaxLength(50).IsRequired();
                entity.Property(v => v.Color).HasMaxLength(20);
            });

            modelBuilder.Entity<ParkingSpotModel>(entity => {
                entity.HasIndex(p => p.SpotNumber).IsUnique();
                entity.Property(p => p.SpotNumber).HasMaxLength(10).IsRequired();
            });

            modelBuilder.Entity<TicketModel>(entity => {
                entity.Property(t => t.TotalAmount).HasPrecision(18, 2);

                entity.HasOne(t => t.Vehicle)
                      .WithMany()
                      .HasForeignKey(t => t.VehicleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ParkingSpot)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(t => t.ParkingSpotId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
	}
}
