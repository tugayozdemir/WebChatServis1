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
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        User Login(User user);

        [OperationContract]
        Room CreateRoom(Room room);

        [OperationContract]
        IList<Room> GetRoomList();

        [OperationContract]
        IList<User> GetUserList(Room room);

        [OperationContract]
        IList<User> GetUser();

        [OperationContract]
        IList<User> JoinRoom(Room room, User user);

        [OperationContract]
        bool SendMessage(Message message);

        [OperationContract]
        IList<Message> GetMessages(User user);
    }
}
