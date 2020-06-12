# FluentCommander.EntityFramework.SqlServer

This package will use Entity Frameworks DbContext's Database to execute commands when it can. When it cannot, it will fall back on the behavior of DatabaseCommandBuilder.

From a client perspective, this package simply provides an extension method on DbContext to access a DatabaseCommandBuilder instance.