using Microsoft.EntityFrameworkCore;
using WebSpider.Context.Entities;
using WebSpiderDbContext.Interface;

namespace WebSpider.Context;

public sealed class SpiderDbContext : DbContext, ISpiderDbContext
{
    public SpiderDbContext(DbContextOptions<SpiderDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; } = null!;
}

