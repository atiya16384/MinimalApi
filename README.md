# Star Wars Planets API

This project is a minimal API developed in C# that interacts with the Star Wars API (SWAPI) and allows users to manage a list of their favourite Star Wars planets.

## Features

1. **GET a list of Planets from the SWAPI API**: The API fetches and displays a list of all Star Wars planets available in the SWAPI.

2. **GET a list of favourite Planets**: Users can view a list of their favourite Star Wars planets.

3. **POST a favourite Planet**: Users can add a planet to their list of favourites. Each planet can only be favourited once.

4. **DELETE a favourite planet**: Users can remove a planet from their list of favourites.

## Development

This API is developed using C# and ASP.NET Core. The data is stored in an in-memory Entity Framework database.

## Tutorial

For a detailed guide on how to develop a minimal API using ASP.NET Core, refer to this [tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio).

## Usage

To use the API, send HTTP requests to the appropriate endpoints with the required parameters. The API will respond with the requested data, or a status message indicating the success or failure of the operation.

## License

This project is open source and available under the [MIT License](LICENSE).