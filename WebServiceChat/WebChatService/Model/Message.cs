using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebChatService.Model
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public User From { get; set; }

        [DataMember]
        public User To { get; set; }

        [DataMember]
        public DateTime SendDate { get; set; }
    }
}