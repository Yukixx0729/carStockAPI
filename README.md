# carStockAPI （backend only）

### **Technologies used**

---

- Back End: C#, ASP .net core, Entityframework core, Sql server
- Test: xUnit, in memory database

### **API Description**

---

- `/register` signing up new dealer
- `/login` login for logging in a dealer
- `/logout` logout the current dealer

- `/api/Cars(post)` creating a new car
- `/api/Cars/id/{id}(get)` displaying the car by id
- `/api/Cars/id/{id}(put)` updating the car by id
- `/api/Cars/dealer` listing all the cars under the login dealer
- `/api/Cars/filter` listing all the cars under the login dealer and certain filters

### **Steps for cloning**

- After cloning the repo to local environment, `cd CarServer`
- Run `dotnet restore` to download and install all necessary NuGet packages
- As the api is running using sql server, `touch appsettings.json`
- Copy the information and change your database connection string with your own information
- ```{
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
