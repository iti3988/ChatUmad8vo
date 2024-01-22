using Newtonsoft.Json;
using System;

namespace estadaresFinal.Common.Models
{
    public class Chat
    {
        public int IdChat { get; set; }
        public int IdChatType { get; set; }
        public DateTime DateTime { get; set; }
        public string ChatTypeName { get; set; }
        public string Email { get; set; }
        public string MessageDescription { get; set; }
    }
}
