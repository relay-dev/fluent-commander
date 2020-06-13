# How To

[Creating and Updating the entity model](https://www.learnentityframeworkcore.com/walkthroughs/existing-database)

### .NET Core CLI:

Install:
	Microsoft.EntityFrameworkCore
	Microsoft.EntityFrameworkCore.Design
	Microsoft.EntityFrameworkCore.SqlServer
	Microsoft.EntityFrameworkCore.Tools

Tools > Command Line > Developer Command Prompt

cd tests\IntegrationTests.EntityFramework

dotnet tool install --global dotnet-ef --version 3.1.4

dotnet ef dbcontext scaffold "Data Source=guroo-trieve-dev.database.windows.net;Database=DatabaseCommander;User ID=SystemUser;Password=d{{DatabasePassword}};Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -f
//dotnet ef dbcontext scaffold "Data Source=localhost\\SQLEXPRESS;Database=DatabaseCommander;Integrated Security=SSPI;" Microsoft.EntityFrameworkCore.SqlServer -o Entities -f

Be sure to remove the connection string from the generated DbContext method OnConfiguring(). It contains the password in plain text

### Test method conventions:

public void MethodName_ExpectedBehavior_StateUnderTest()
{
	// Arrange

	//Act

	//Assert
}