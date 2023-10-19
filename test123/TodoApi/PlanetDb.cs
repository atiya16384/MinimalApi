using Microsoft.EntityFrameworkCore;
// we are using the microsoftEnttityCore.DbContext class
class PlanetDb : DbContext
{
    
    public PlanetDb(DbContextOptions<PlanetDb> options)
        : base(options) { }
    
    // we create the database sets for each get point
     public DbSet<FavouritePlanet> FavouritePlanets => Set<FavouritePlanet>();
     public DbSet<Planet> Planets=> Set<Planet>();

}
