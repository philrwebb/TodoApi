using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;

        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
            {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City") { Id = 1, Description = "The one with that big park." },
                new City("Antwerp") { Id = 2, Description = "The one with the cathedral that was never really finished." },
                new City("Paris") { Id = 3, Description = "The one with that big tower." }
            );
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park", "The most visited urban park in the United States.") { Id = 1, CityId = 1 },
                new PointOfInterest("Empire State Building", "A 102 skyscraper located in Midtown Manhattan.") { Id = 2, CityId = 1 },
                new PointOfInterest("Cathedral of Our Lady", "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.") {Id = 3, CityId = 2},
                new PointOfInterest("Antwerp Central Station", "The the finest example of railway architecture in Belgium.") {Id = 4, CityId = 2},
                new PointOfInterest("Eiffel Tower", "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.") { Id = 5, CityId = 3},
                new PointOfInterest("The Louvre", "The world's largest museum.") { Id = 6, CityId = 3}
            );


            base.OnModelCreating(modelBuilder);
        }

    }
}