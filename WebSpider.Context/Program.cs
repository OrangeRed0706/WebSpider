using Microsoft.EntityFrameworkCore;
using WebSpider.Context.DbContextFactory;

var factory = new WebSpiderDbContextFactory();
using var context = factory.CreateDbContext(args);
var db = context.Database;
db.Migrate();