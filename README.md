# File Guardian

FileGuardian API is a .NET server application designed to store and manage information about files and their associated risk levels. It provides endpoints for creating, retrieving, and sharing files, as well as managing user groups.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)

## Launch the server
```
cd src/FileGuardian.Api
dotnet run
```
You can now open http://localhost:5113/swagger/index.html to view the Swagger UI and make requests to the server.

## How to test

### Populate with random data (optional)
Running `populate_db/populate.py` will make requests to the server to create a random set of files, users, and groups, and relationships between them.

1. Install [Python 3.x](https://www.python.org/downloads/)
2. Install [pip](https://pip.pypa.io/en/stable/installation/) `python -m ensurepip --upgrade`
4. `cd populate_db`
5. Install the project requirements: `pip install -r requirements.txt`
```
python populate.py <number_of_files> <number_of_users> <number_of_groups>
```

### User interface
Open `index.html` to interact with a server through a basic interface.

### View database
1. Download [DB Browser for SQLite](https://sqlitebrowser.org/)
2. Open the `fileGuardian.db` file from `src/FileGuardian.Api`

## Technologies

- [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
- [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/)
- [Entity Framework Extensions](https://entityframework-extensions.net/)
- [SQLite](https://www.sqlite.org/)
- [AutoMapper](https://automapper.org/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [NSubstitute](https://nsubstitute.github.io/)
- [NUnit](https://nunit.org/)
