
using System.Data;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// entity framework
builder.Services.AddDbContext<PlanetDb>(opt => opt.UseInMemoryDatabase("PlanetList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRouting();
var app = builder.Build();
// Adding try and catch will make the code robust
//Get a list of planets from the swap api
// attempt to implement some if statement
app.MapGet("api/planets", async context =>
{   
    
    try{
    // Make an HTTP GET request to the swapi
    var httpClient= new HttpClient();
    var result = await httpClient.GetStringAsync("https://swapi.dev/api/planets/");
//  we return the response in json form
    context.Response.Headers.Add("Content-Type", "application/json");
    await context.Response.WriteAsync(result);
    }
    catch (Exception ex) {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
);
// we need to get a list of the favourite planets
app.MapGet("api/planets-favourite", async context =>
{   
    try
    {
    // GetRequiredService<T> () throws an InvalidOperationException if it can't find it.
    var dbContext = context.RequestServices.GetRequiredService<PlanetDb>();
    var favouritePlanets = await dbContext.FavouritePlanets.ToListAsync();
    // we write the favourite planets as json form
    await context.Response.WriteAsJsonAsync(favouritePlanets);
    }
    catch(Exception ex){
         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
}
);

// we want to post a planet from the favouritePlanetsList 
// we use post to add data to the in-memory database
app.MapPost("/api/planets-favourite", async context =>
{   
    try{

    var dbContext = context.RequestServices.GetRequiredService<PlanetDb>();
    // read the json form of the data in the dataset FavouritePlanet
    var favouritePlanet = await context.Request.ReadFromJsonAsync<FavouritePlanet>();
    // Check if the planet is already favourited.
    // we dereference the null by using an if statement
    if(favouritePlanet!=null){
        var currentFavorite = await dbContext.FavouritePlanets.FirstOrDefaultAsync(p => p.Name == favouritePlanet.Name);

        if (currentFavorite != null)
        {
       // We return as the planet has already been favourited.
            return;
         }

     // If the planet is not already favorited, add it to the database.
    dbContext.FavouritePlanets.Add(favouritePlanet);
    await dbContext.SaveChangesAsync();
    }
    }
    catch(Exception ex){
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

});

app.MapDelete("/api/planets-favourite/{id}", async context =>
{
    try
    {
    // retrieve the id value from the route values
    int? id = context.GetRouteValue("id") as int?;

    var dbContext = context.RequestServices.GetRequiredService<PlanetDb>();

    var favoritePlanet = await dbContext.FavouritePlanets.FindAsync(id);

    if (favoritePlanet == null)
    {
        return;
    }

    dbContext.FavouritePlanets.Remove(favoritePlanet);
    await dbContext.SaveChangesAsync();

    }
    catch (Exception e){
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
    });

app.MapGet("api/planets-random", async context =>
    {
        
        var dbContext = context.RequestServices.GetRequiredService< PlanetDb>();
        var httpClient = new HttpClient();
        // From the FavouritePlanets database set we select the planet Names and assign to a list
        var favouritePlanets = await dbContext.FavouritePlanets.Select(planet => planet.Name).ToListAsync(); 
        // we assign random planet to be null
        string randomPlanet=null!;

        while(randomPlanet== null){
            // we get the randomPlanet using the swap api link 
            var result = await httpClient.GetFromJsonAsync<Planet>("https://swapi.dev/api/planets/");
            
            if(result!=null){
            
                 var planetName = result.Name;
                
                 if(!favouritePlanets.Contains(planetName)){
                    randomPlanet= planetName;
                }

            var finalResult= new {Name= randomPlanet};
   
             await context.Response.WriteAsJsonAsync(finalResult);
            }
        }
    }
);


app.Run();
