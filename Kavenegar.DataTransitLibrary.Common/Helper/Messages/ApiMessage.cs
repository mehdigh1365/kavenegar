using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Helper.Messages
{
    public class ApiMessage
    {
        public ApiMessage()
        {

        }

        public ApiMessage(string message)
        {
            Message = message;
        }
        public string Message { get; set; }

        public string Detail { get; set; }

    }
}
