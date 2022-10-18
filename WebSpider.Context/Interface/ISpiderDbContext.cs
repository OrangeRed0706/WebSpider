using Microsoft.EntityFrameworkCore;
using WebSpider.Context.Entities;

namespace WebSpiderDbContext.Interface;

public interface ISpiderDbContext
{
    DbSet<Post> Posts { get; set; }
}