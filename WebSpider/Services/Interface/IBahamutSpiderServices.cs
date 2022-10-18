using WebSpider.Context.Entities;
namespace WebSpider.Services.Interface;

public interface IBahamutSpiderServices
{
    Task RegularHouseKeeping();
    Task<IEnumerable<Post>> GetAllPost();
    void RemoveAllPost();

    Task<IEnumerable<Post>> GetNewsPost();
    Task<IEnumerable<Post>> GetRecentPosts(int count);
}