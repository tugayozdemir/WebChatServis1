using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChatUI.Models;

namespace WebChatUI.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        WebChatService.ChatServiceClient chatService;
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public JsonResult LoginData()
        {
            
            var userData = Session["UserData"] as LoginUserModel;
    
        string resultMessage = string.Format("<br/> {0}-<a href=\"/Home/Logout\">Çıkış</a>", userData.UserName);
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Login(LoginUserModel loginUser)
        {
            JsonResultModel jsonResult = new JsonResultModel();
            
            try
            {
                chatService = new WebChatService.ChatServiceClient();
                chatService.Login(new WebChatService.User()
                {           
                    UserName = loginUser.UserName
                });
                loginUser.Id = chatService.GetUser().FirstOrDefault(x => x.UserName == loginUser.UserName).Id;
                
                Session["UserData"] = loginUser;
                Session["UserName"] = loginUser.UserName;
                Session["UserID"] = loginUser.Id;
                jsonResult.IsSuccess = true;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jsonResult.IsSuccess = false;
                jsonResult.ExMessage = ex.Message;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
                
            }
        }

      
        public JsonResult CreateRoom(RoomResultModel roomResult)
        {
            JsonResultModel jsonResult = new JsonResultModel();
            try
            {
                chatService = new WebChatService.ChatServiceClient();
                chatService.CreateRoom(new WebChatService.Room {

                    Name = roomResult.roomName
                });
                jsonResult.IsSuccess = true;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                jsonResult.ExMessage = ex.Message;
                jsonResult.IsSuccess = false;
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
              
            }



        }

  
        public ActionResult Rooms()
        {
            chatService = new WebChatService.ChatServiceClient();
            var rooms = chatService.GetRoomList().ToList();
            if (rooms.Count() == 0) 
            {
                GetRoomListModel getRoom = new GetRoomListModel();
                getRoom = null;
                return View(getRoom);
            }
            else
            {
                return View(new GetRoomListModel()
                {
                    roomList = rooms
                });
            }
           
        }

        public ActionResult Users(Guid? Id) 
        { 
            GetUserModel getUserNull = new GetUserModel();
            getUserNull = null;
            try
            {
                chatService = new WebChatService.ChatServiceClient();
                var room = chatService.GetRoomList().FirstOrDefault(x => x.Id == Id);
                WebChatService.User logUser = new WebChatService.User();
                logUser.UserName = (Session["UserData"] as LoginUserModel).UserName;
                logUser.Id = (Session["UserData"] as LoginUserModel).Id;
                var users = chatService.JoinRoom(room, logUser);

                return View(new GetUserModel()
                {
                    getUser = users,
                    RoomID = Id
                });
            }
            catch (Exception)
            {
                return View(getUserNull);
            }
        }

        [NoCache]
        [HandleError]
        public JsonResult GetChat(Guid userTwoID)
        {
            chatService = new WebChatService.ChatServiceClient();
            var userTwo = chatService.GetUser().FirstOrDefault(x => x.Id == userTwoID);
            GetChatModel getChatUser = new GetChatModel();
            getChatUser.UserTwo = userTwo;
            return Json(getChatUser,JsonRequestBehavior.AllowGet);

        }

        
        public JsonResult SendMessage(string messages , string FromUser , string ToUser)
        {
            chatService = new WebChatService.ChatServiceClient();
            var fromUser = chatService.GetUser().FirstOrDefault(x => x.UserName == FromUser.Substring(1, FromUser.Length - 2));
            var toUser = chatService.GetUser().FirstOrDefault(x => x.UserName == ToUser);
            MessageModel message = new MessageModel();
            message.From = fromUser;
            message.To = toUser;
            message.Text = messages;
            var result = SendMessage(message);
            

            
            WebChatService.Message sendMessage = new WebChatService.Message();
            sendMessage.Text = messages;
            sendMessage.To = toUser;
            sendMessage.From = fromUser;
            sendMessage.SendDate = DateTime.Now;
            if (result)
            {

                return Json(sendMessage,JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }



        }

        bool SendMessage(MessageModel message)
        {
            
                chatService = new WebChatService.ChatServiceClient();
                WebChatService.Message wsMessage = new WebChatService.Message();
                wsMessage.Text = message.Text;
                wsMessage.From = message.From;
                wsMessage.To = message.To;
                var result = chatService.SendMessage(wsMessage);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
     

        }


        [NoCache]
        [HandleError]
        public JsonResult GetChatMessage(string fromUser, string toUser)
        {
            chatService = new WebChatService.ChatServiceClient();
            var User = chatService.GetUser().FirstOrDefault(x => x.UserName == fromUser.Substring(1, fromUser.Length-2));
            var messageList = chatService.GetMessages(User);
            if (messageList != null)
            {
                var message = messageList.FirstOrDefault(x => x.From.UserName == toUser);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
       
            else
            {
             
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            
            
           
        }
  
        
    }                                                                     
}