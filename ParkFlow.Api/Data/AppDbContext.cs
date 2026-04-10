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
		public DbSet<PriceConfigModel> PriceConfigs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<VehicleModel>(entity => {
				entity.HasIndex(v => v.LicensePlate).IsUnique();
				entity.Property(v => v.LicensePlate).HasMaxLength(7).IsRequired();
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

			modelBuilder.Entity<PriceConfigModel>(entity => {
				entity.HasKey(p => p.Id);
				entity.Property(p => p.FirstHourValue).HasPrecision(18, 2).IsRequired();
				entity.Property(p => p.AdditionalHourValue).HasPrecision(18, 2).IsRequired();
				entity.Property(p => p.DailyValue).HasPrecision(18, 2).IsRequired();
				entity.Property(p => p.IsActive).IsRequired();
				entity.Property(p => p.ToleranceMinutes).IsRequired();
			});

			modelBuilder.Entity<PriceConfigModel>().HasData(new PriceConfigModel
			{
				Id = 1,
				IsActive = false,
				ToleranceMinutes = 15,
				FirstHourValue = 10.00m,
				AdditionalHourValue = 5.00m,
				DailyValue = 50.00m,
				LastUpdatedAt = new DateTime(2026, 1, 1)
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
