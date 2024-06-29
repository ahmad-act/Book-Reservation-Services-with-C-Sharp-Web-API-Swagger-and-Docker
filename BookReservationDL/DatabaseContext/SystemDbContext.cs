using BookReservationModel.DisplayModel;
using BookReservationModel.Model;
using Microsoft.EntityFrameworkCore;

namespace BookReservationDL.DatabaseContext
{
    public class SystemDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ReservedBook>();
            modelBuilder.Ignore<ReservationHistory>();
        }
    }
}
