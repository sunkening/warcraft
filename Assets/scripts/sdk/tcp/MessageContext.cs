using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace skntcp
{
   public class MessageContext
    {
        public int msgtype;
        public Object message;
        public IAction action;
    }
}
