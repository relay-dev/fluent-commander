﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentCommander.EntityFramework
{
    public static class DbContextExtensions
    {
        internal static IServiceProvider ServiceProvider { get; set; }

        public static DbContextCommandBuilder BuildCommand(this DbContext dbContext)
        {
            var builder = (DbContextCommandBuilder)ServiceProvider.GetRequiredService(typeof(DbContextCommandBuilder));

            return builder.SetContext(dbContext);
        }
    }
}
