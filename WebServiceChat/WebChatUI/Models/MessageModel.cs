using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatUI.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }


        public string Text { get; set; }


        public WebChatService.User From { get; set; }


        public WebChatService.User To { get; set; }


        public DateTime SendDate { get; set; }
    }
    public class MessageModel<T> : MessageModel
    {
        public IList<T> getMessage;
    }

}