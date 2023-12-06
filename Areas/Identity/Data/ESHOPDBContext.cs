﻿using ESHOP.Areas.Identity.Data;
using ESHOP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ESHOP.Data;

public class ESHOPDBContext : IdentityDbContext<ESHOPUser>
{
    public ESHOPDBContext(DbContextOptions<ESHOPDBContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<ESHOP.Models.Contact> Contact { get; set; } = default!;
}
