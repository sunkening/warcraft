using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
namespace skn.utils
{
    class BeanUtil
    {
        static public void setPublicProperty(object bean, string name, object value)
        {
            Type type = bean.GetType();
            FieldInfo fInfo = type.GetField(name);
            fInfo.SetValue(bean, value);
        }
    }
}
