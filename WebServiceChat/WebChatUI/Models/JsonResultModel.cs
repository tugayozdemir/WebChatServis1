using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatUI.Models
{
    public class JsonResultModel
    {
        public bool IsSuccess { get; set; }
        public string ExMessage { get; set; }
    }

    public class JsonResultModel<T> : JsonResultModel
    {
        public IList<T> DataList { get; set; }
    }

}