using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebChatUI.Models
{
    public class GetRoomListModel
    {
       
        public IList<WebChatService.Room> roomList { get; set; }
    }
}