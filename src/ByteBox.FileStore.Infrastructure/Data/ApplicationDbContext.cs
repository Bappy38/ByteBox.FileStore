﻿using Microsoft.EntityFrameworkCore;

namespace ByteBox.FileStore.Infrastructure.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var infrastructureAssembly = typeof(ApplicationDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(infrastructureAssembly);
        base.OnModelCreating(modelBuilder);
    }
}