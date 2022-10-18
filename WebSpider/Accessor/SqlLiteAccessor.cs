using Microsoft.EntityFrameworkCore;
using WebSpider.Accessor.Interface;
using WebSpider.Context;
using WebSpider.Context.Entities;

namespace WebSpider.Accessor;

public class SqlLiteAccessor : ISqlLiteAccessor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlLiteAccessor(IServiceScopeFactory serviceScopeFactory)
    {
        _scopeFactory = serviceScopeFactory;
    }

    async Task<IEnumerable<Post>> ISqlLiteAccessor.GetRecentPosts(int count)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        return await db.Posts.OrderByDescending(x => x.Time).Take(count).ToListAsync();
    }

    async Task<IEnumerable<Post>> ISqlLiteAccessor.GetOldPosts(DayOfWeek dayOfWeek)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        return await db.Posts.Where(post => DateTime.Now.AddDays((int)dayOfWeek) < post.Time).ToListAsync();
    }

    async Task ISqlLiteAccessor.AddPost(Post post)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        db.Posts.Add(post);
        await db.SaveChangesAsync();
    }

    async Task ISqlLiteAccessor.AddPosts(IEnumerable<Post> posts)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        db.Posts.AddRange(posts);
        await db.SaveChangesAsync();
    }

    async Task ISqlLiteAccessor.RemovePost(Post post)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        db.Posts.Remove(post);
        await db.SaveChangesAsync();
    }

    async Task ISqlLiteAccessor.RemovePosts(IEnumerable<Post> post)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        db.Posts.RemoveRange(post);
        await db.SaveChangesAsync();
    }

    async Task<IEnumerable<Post>> ISqlLiteAccessor.GetAllPosts()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        return await db.Posts.ToListAsync();

    }

    async Task ISqlLiteAccessor.RemoveAllPosts()
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<SpiderDbContext>();
        var posts = await db.Posts.ToListAsync();
        db.Posts.RemoveRange(posts);
        await db.SaveChangesAsync();
    }
}