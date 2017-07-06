using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatUI.Models
{
    public class ModelList
    {
        public GetRoomListModel GetRoomListModel { get; set; }
        public GetUserModel GetUserModel { get; set; }
        public LoginUserModel LoginUserModel { get; set; }
        public MessageModel MessageModel { get; set; }
        public MessageModel<WebChatService.Message> GetMessageModel { get; set; }
        public RoomResultModel RoomResultModel { get; set; }
    }
}