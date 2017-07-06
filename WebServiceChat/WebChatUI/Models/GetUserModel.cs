using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebChatUI.Models
{
    public class GetUserModel
    {
        public IList<WebChatService.User> getUser { get; set; }
        public Guid? RoomID { get; set; }
    }
}