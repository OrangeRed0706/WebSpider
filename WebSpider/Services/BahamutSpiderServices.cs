using AngleSharp;
using AngleSharp.Dom;
using WebSpider.Accessor.Interface;
using WebSpider.Context.Entities;
using WebSpider.Extensions;
using WebSpider.Services.Interface;

namespace WebSpider.Services;

public class BahamutSpiderServices : IBahamutSpiderServices
{
    private readonly ISqlLiteAccessor _sqlLiteAccessor;

    public BahamutSpiderServices(ISqlLiteAccessor sqlLiteAccessor)
    {
        _sqlLiteAccessor = sqlLiteAccessor;
    }

    async Task IBahamutSpiderServices.RegularHouseKeeping()
    {
        var oldPosts = await _sqlLiteAccessor.GetOldPosts(DayOfWeek.Saturday);
        await _sqlLiteAccessor.RemovePosts(oldPosts);
    }

    async Task<IEnumerable<Post>> IBahamutSpiderServices.GetAllPost()
    {
        return await _sqlLiteAccessor.GetAllPosts();
    }

    void IBahamutSpiderServices.RemoveAllPost()
    {
        _sqlLiteAccessor.RemoveAllPosts();
    }

    async Task<IEnumerable<Post>> IBahamutSpiderServices.GetNewsPost()
    {
        var config = Configuration.Default
            .WithDefaultLoader()
            .WithDefaultCookies();
        var browser = BrowsingContext.New(config);
        var baseUrl = "https://gnn.gamer.com.tw/";
        var url = new Url(baseUrl);
        var document = await browser.OpenAsync(url);
        var postList = new List<Post>();
        var oldPosts = await _sqlLiteAccessor.GetRecentPosts(30);
        for (var i = 0; i < 12; i++)
        {
            var postSource = document.QuerySelectorAll($"#BH-master > div.BH-lbox.GN-lbox2 > div:nth-child({i})");
            var post = postSource.Select(post =>
            {
                var titleElement = post.QuerySelector(".GN-lbox2D");
                var title = titleElement?.QuerySelector("a")?.InnerHtml;
                string?[]? describe = post.QuerySelector(".GN-lbox2C")?.InnerHtml.Split(@"<a href=""");
                var image = post.QuerySelector(".GN-lbox2E > a > img")?.GetAttribute("src");
                var link = titleElement?.QuerySelector("a")?.GetAttribute("href");
                return new Post
                {
                    Title = title,
                    Describe = describe?[0],
                    Link = $"https:{link}",
                    ImageUri = image,
                    Time = DateTime.Now
                };
            }).FirstOrDefault();
            if (post == null) continue;
            var isNotExistPost = oldPosts.None(x => x.Title == post.Title);
            if (isNotExistPost)
            {
                postList.Add(post);
            }
        }
        if (postList.Any())
        {
            await _sqlLiteAccessor.AddPosts(postList);
        }

        return postList;
    }

    async Task<IEnumerable<Post>> IBahamutSpiderServices.GetRecentPosts(int count)
    {
        return await _sqlLiteAccessor.GetRecentPosts(count);
    }
}