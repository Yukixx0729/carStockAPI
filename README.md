# carStockAPI （backend only）

### **Technologies used**

---

- Back End: C#, ASP .net core, Entityframework core, Sql server
- Test: xUnit, in memory database

### **API Description**

---

- `/register` sign up new dealer
- `/login` login a dealer
- `/logout` logout the current dealer

- `/api/Cars(post)` create a new car
- `/api/Cars/id/{id}(get)` display the car information by id
- `/api/Cars/id/{id}(put)` update the car by id
- `/api/Cars/dealer` list all the cars under the login dealer
- `/api/Cars/filter` list all the cars under the login dealer with certain filters

### **Steps for local environment**

- After cloning the repo to local environment, `cd CarServer`
- Run `dotnet restore` to download and install all necessary NuGet packages
- As the api is running using sql server, run `touch appsettings.json` (macOS/Linux) , run `New-Item appsettings.json -Type File` (window)
- Copy the information and change your database connection string with your own information
- ```
  {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.AspNetCore.Authentication": "Information"
      }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
      "Default": "Server=localhost,1433;Database={databasename};User Id={your id};Password={your password};TrustServerCertificate =True"
    }
  }
  ```

- Run `dotnet ef database update` to apply migration
- Run `dotnet build` to ensure everything is configured correctly
- Run `dotnet watch run` to test the API

- Register dealer `/register`
- eg:
  `{
  "email": "tiger@qq.com",
  "password": "ADpass123."
}`
- Login dealer `/login` , pick either `useCookies` or `useSessionCookies` be true.
- eg:
  `{
  "email": "tiger@qq.com",
  "password": "ADpass123.",
  "twoFactorCode": "",
  "twoFactorRecoveryCode": ""
}`

- Now you can test the cars api

- To run the unit tests, cd to the test file, and run `dotnet test`
