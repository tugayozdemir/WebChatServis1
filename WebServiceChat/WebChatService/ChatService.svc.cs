using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WebChatService.Model;

namespace WebChatService
{
    public class ChatService : IChatService
    {
        private static IList<Room> roomList;
        public Room CreateRoom(Room room)
        {
            room.Id = Guid.NewGuid();
            roomList = roomList ?? new List<Room>();

            roomList.Add(room);

            return room;
        }

        private static IList<Message> messageList;
        public IList<Message> GetMessages(User user)
        {
            if (messageList == null) return null;

            var messages = messageList.Where(x => x.To.Id == user.Id).ToList();
            foreach (var item in messages)
            {
                messageList.Remove(item);
            }
            return messages;
        }

        public IList<Room> GetRoomList()
        {
            return roomList;
        }

        public IList<User> JoinRoom(Room room, User user)
        {
            var selectedRoom = roomList.FirstOrDefault(x => x.Id == room.Id);
            var selectedUser = userList.FirstOrDefault(x => x.Id == user.Id);
            if (selectedRoom != null)
            {
                selectedRoom.Users = selectedRoom.Users ?? new List<User>();
                selectedRoom.Users.Add(selectedUser);

                return selectedRoom.Users;
            }
            else
            {
                return null;
            }
        }

        private static IList<User> userList;
        public User Login(User user)
        {
            user.Id = Guid.NewGuid();
            userList = userList ?? new List<User>();
            roomList = roomList ?? new List<Room>();

            userList.Add(user);

            return user;
        }
        public IList<User> GetUser()
        {
            return userList;
        }

        
        public bool SendMessage(Message message)
        {
            try
            {
                message.Id = Guid.NewGuid();
                message.SendDate = DateTime.Now;

                messageList = messageList ?? new List<Message>();
                messageList.Add(message);
                
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IList<User> GetUserList(Room room)
        {
            if (roomList == null) return null;
            return roomList.FirstOrDefault(x => x.Id == room.Id).Users;
        }
    }
}
