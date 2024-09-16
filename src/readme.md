### AuctionService

To install anything you will need to install a tool - dotnet ef
To check if this tool is installed or not, we can use the following command:
```bash
dotnet tool list -g
```

If it is installed, you will get the Package Id and Version. If not, you will get an empty list.

To install the tool, we can use the following command:
```bash
dotnet tool install dotnet-ef -g
```

If you want to upgrade the tool, we can use the following command:
```bash
dotnet tool update dotnet-ef -g
```

To add the migrations we will use the command(inside AuctionService folder):
```bash
dotnet ef migrations add "MigrationName" -o folderpath
```
-o means it will output the migration data to the folder.

**Go to Aucar folder**:
And to start docker run
```bash
docker compose up -d
```

If you are starting it for the first time or if you want to restart it, you can use the above comand

Go to the root **sln folder of AuctionService project**

To create a database we can use the following command
```bash
dotnet ef database update
```
Our table gets created inside postgresdb server
You should get a done message confirmation. 
 


To drop a database we can use the following command
```bash
dotnet ef database drop
```

To populate the with the initial provided values, we need to run this command
```bash
dotnet watch
```

All these activites must be done inside the src/AuctionService folder when working from terminal. 


--------------------
To add a new service, within the aucar car folder type this command
 dotnet new webapi -o src/SearchService -controllers

After this the search service will be added to the src folder. 
Later write this command

```bash
dotnet sln add src/SearchService
```

This will add the search service to the sln file.

After everything, just build using
```bash
dotnet build
```

and run using
```bash
dotnet watch
```

Sometimes while doing running the docker file 
```bash
 docker compose up -d
``` 

just check if you are logged in to docker or not

```bash
docker login
```

To use MongoDb for VSCode 
open MongoDB
Advance Connecttion
Open From 
put your credentials from docker compose file

Create the class library using the following command
```bash
dotnet new classlib -o src/Contracts
```

To add the sln to aucar, we will use 
```bash
dotnet sln add src/Contracts
```


