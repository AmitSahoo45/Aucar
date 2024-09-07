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


