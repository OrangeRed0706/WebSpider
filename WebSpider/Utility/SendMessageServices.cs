using System.Security.Cryptography;
using Discord;
using Discord.Webhook;
using Microsoft.Extensions.Options;
using WebSpider.Context.Entities;
using WebSpider.Options;
using WebSpider.Utility.Interface;


namespace WebSpider.Utility
{
    public class SendMessageServices : ISendMessage
    {
        public DiscordWebHookOption Options { get; }
        public SendMessageServices(IOptions<DiscordWebHookOption> options)
        {
            Options = options.Value;
        }

        async Task ISendMessage.SendBahamutDiscordWebHookMessages(Post post, bool favorite)
        {
            var embed = new EmbedBuilder
            {
                Title = post.Title,
                Description = post.Describe,
                Author = new EmbedAuthorBuilder
                {
                    Name = Options.WebHookAuthorName,
                    IconUrl = Options.WebHookAuthorIconUrl
                },
                Url = post.Link,
                Color = GetRandomColor(),
                Timestamp = DateTimeOffset.Now,
                ImageUrl = post.ImageUri,
            };
            if (favorite)
            {
                await SendDiscordWebHookFavoriteMessages(embed.Build());
            }
            
            await SendDiscordWebHookMessages(embed.Build());

            Color GetRandomColor()
            {
                return new Color(GetRandom(), GetRandom(), GetRandom());
                
                byte GetRandom()
                {
                    return (byte)RandomNumberGenerator.GetInt32(0, 255);
                }
            }
        }
        public async Task SendDiscordWebHookMessages(Embed embed)
        {
            using var client = new DiscordWebhookClient(Options.WebHookUrl);
            await client.SendMessageAsync(embeds: new[] { embed });
        }

        async Task SendDiscordWebHookFavoriteMessages(Embed embed)
        {
            using var client = new DiscordWebhookClient("https://discord.com/api/webhooks/914486320112476182/twd5jHC0vTPcjnAOGOkba_giH8NdKpTQkoH9HWPySF6R6xX2N4u56OadgfMpg_Qe-bea");
            await client.SendMessageAsync(embeds: new[] { embed });
        }
    }
}
