using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactMgmtApp.Api
{
    public class ApiResponseBase<T>
    {
        public bool Success { get; set; }
        public string StatusMessage { get; set; }
        public T PayLoad { get; set; }
        public string AddnlInfo { get; set; }
    }
}