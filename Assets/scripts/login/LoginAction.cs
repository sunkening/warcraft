using msg.login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace skntcp
{
    class LoginAction : BaseAction<SAccount>
    {
        override public void doAction(System.Object message)
        {
            SAccount account = (SAccount)message;
            Debug.Log(account.info);
        }
 
    }
}
