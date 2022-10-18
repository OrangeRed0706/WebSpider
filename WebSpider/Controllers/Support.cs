using Microsoft.AspNetCore.Mvc;
using WebSpider.Context.Entities;
using WebSpider.Services.Interface;


namespace WebSpider.Controllers;
[ApiController]
[Route("[controller]")]
public class Support : ControllerBase
{
    private readonly IBahamutSpiderServices _bahamutSpider;

    public Support(IBahamutSpiderServices bahamutSpidertext)
    {
        _bahamutSpider = bahamutSpidertext;
    }

    [HttpGet]
    [Route("GetRecentPosts")]
    public async Task<IEnumerable<Post>> GetRecentPosts(int count)
    {
        return await _bahamutSpider.GetRecentPosts(count);
    }

    [HttpGet]
    [Route("ClearOldPost")]
    public IActionResult ClearOldPost()
    {
        _bahamutSpider.RegularHouseKeeping();

        return Ok("Old Post Clear");
    }

    [HttpGet]
    [Route("RemoveAllPost")]
    public IActionResult RemoveAllPost()
    {
        _bahamutSpider.RemoveAllPost();

        return Ok("All Post Clear");
    }

    [HttpGet]
    [Route("GetAllPost")]
    public async Task<IEnumerable<Post>> GetAllPost()
    {
        return await _bahamutSpider.GetAllPost();
    }

    [HttpGet]
    [Route("GetNewPost")]
    public async Task<IEnumerable<Post>> GetNewsPost()
    {
        var newPosts = await _bahamutSpider.GetNewsPost();
        return newPosts.ToList();
    }
}