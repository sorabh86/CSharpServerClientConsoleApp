# dotnet core with VSCode

Create new solution in current folder
```cs 
> dotnet new sln -n mysolution
```
List Added project in solution
```cs
> dotnet sln mysolution.sln list
```
<center>OR</center> 

```cs
> dotnet sln list
```

```cs
// create new console application
> dotnet new console -o myapp
// add your console application in solution
> dotnet sln mysolution.sln add myapp\myapp.csproj
// remove your application to solution
> dotnet sln mysolution.sln remove myapp\myapp.csproj

// create new class library
> dotnet new classlib -o mylib
// add your classlibrary to solution
> dotnet sln mysolution.sln add mylib\mylib.csproj --solution-folder mylibs


// run project
> dotnet run

// build release
> dotnet release --configuration Release

> dotnet build

// add reference to access class library to App project 
> dotnet add myapp/myapp.csproj reference mylib/mylib.csproj

```
