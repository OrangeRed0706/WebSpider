using Discord;
using WebSpider.Context.Entities;

namespace WebSpider.Utility.Interface
{
    public interface ISendMessage
    {
        Task SendDiscordWebHookMessages(Embed embed);
        Task SendBahamutDiscordWebHookMessages(Post post, bool favorite = false);
    }
}
