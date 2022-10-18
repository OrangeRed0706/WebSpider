using System.Security.Cryptography;
using WebSpider.Interface;
using WebSpider.Services.Interface;
using WebSpider.Utility.Interface;

namespace WebSpider.Job
{
    public class BahamutSpiderJob : IBahamutSpiderJob
    {
        private readonly IBahamutSpiderServices _bahamutSpider;
        private readonly ILogger<BahamutSpiderJob> _logger;

        private ISendMessage SendMessage { get; set; }

        public BahamutSpiderJob(ISendMessage sendMessage, IBahamutSpiderServices bahamutSpider, ILogger<BahamutSpiderJob> logger)
        {
            SendMessage = sendMessage;
            _bahamutSpider = bahamutSpider;
            _logger = logger;
        }

        async Task IBahamutSpiderJob.RunJob()
        {
            var random = RandomNumberGenerator.GetInt32(1000, 280000);
            await Task.Delay(random);
            _logger.LogInformation("Start Job");
            var posts = await _bahamutSpider.GetNewsPost();
            await Task.WhenAll(posts.Select(async post =>
            {
                if (post.Title != null && (post.Title.Contains("英雄聯盟") || post.Title.Contains("寶可夢")))
                {
                    await SendMessage.SendBahamutDiscordWebHookMessages(post, true);
                }
                else
                {
                    await SendMessage.SendBahamutDiscordWebHookMessages(post);
                }
            }));
        }
    }
}