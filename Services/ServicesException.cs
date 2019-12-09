using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class ServicesException:Exception
    {
        public int code { set; get; }
        public ServicesException(string msg,int code):base(msg)
        {
            this.code = code;
        }
    }
}
