using WebSpider.Context.Entities;

namespace WebSpider.Accessor.Interface;

public interface ISqlLiteAccessor
{
    Task<IEnumerable<Post>> GetOldPosts(DayOfWeek dayOfWeek);
    Task AddPost(Post post);
    Task AddPosts(IEnumerable<Post> posts);
    Task RemovePost(Post post);
    Task RemovePosts(IEnumerable<Post> post);
    Task<IEnumerable<Post>> GetAllPosts();
    Task RemoveAllPosts();
    Task<IEnumerable<Post>> GetRecentPosts(int count);
}