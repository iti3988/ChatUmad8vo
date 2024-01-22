using estandaresFinal.Data;
using estandaresFinal.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace estandaresFinal.Controllers.API
{
    [Route("api/[Controller]")]
    public class ChatsController : Controller
    {
        private readonly DataContext dataContext;

        public ChatsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        private async Task<Chat> AddChat(string chatTypeName, string email, string messageDescription, int idChat)
        {
            var chat = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == idChat);

            if (chat == null)
            {
                var chatType = await dataContext.ChatTypes.FirstOrDefaultAsync(c => c.Name == chatTypeName);
                dataContext.Chats.Add(new Chat { Date = DateTime.Now, ChatType = chatType });

                var result = await dataContext.SaveChangesAsync();

                int lastChatId = dataContext.Chats.Max(c => c.Id);
                var chatNew = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == lastChatId);
                var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                dataContext.ChatMembers.Add(new ChatMember { Chat = chatNew, User = user });
                dataContext.Messages.Add(new Message { Chat = chatNew, User = user, Date = DateTime.Now, MessageDescription = messageDescription });
                await dataContext.SaveChangesAsync();
            }

            if (chat != null)
            {
                var chatMod = await dataContext.Chats.FirstOrDefaultAsync(c => c.Id == idChat);
                var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                dataContext.ChatMembers.Add(new ChatMember { Chat = chatMod, User = user });
                dataContext.Messages.Add(new Message { Chat = chatMod, User = user, Date = DateTime.Now, MessageDescription = messageDescription });
                await dataContext.SaveChangesAsync();
            }

            return chat;
        }

        // Listo (Quiza)
        public IActionResult GetChatsAsync()
        {
            //var chat = this.dataContext.ChatMembers
            //    .Include(cm => cm.Chat)
            //        .ThenInclude(ch => ch.ChatType)
            //    .Include(cm => cm.User)
            //        .Where(u => u.Messages);

            var chat = this.dataContext.Messages
            .Include(m => m.Chat)
                .ThenInclude(ch => ch.ChatType)
            .Include(m => m.User);

            return Ok(chat.ToList());
        }

        // Listo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chat = this.dataContext.Chats.Find(id);

            if (chat == null)
            {
                return BadRequest(ModelState);
            }

            dataContext.Chats.Remove(chat);
            await dataContext.SaveChangesAsync();
            return Ok(chat);
        }

        // Listo
        [HttpPost]
        public async Task<IActionResult> PostChat([FromBody] estadaresFinal.Common.Models.Chat chatCommon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chatDetails = await AddChat
            (
                chatCommon.ChatTypeName,
                chatCommon.Email,
                chatCommon.MessageDescription,
                chatCommon.IdChat
            );

            return Ok(chatDetails);
        }
    }
}