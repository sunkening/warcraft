using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace skn.utils
{

    class StringUtil
    {
        const string CFG_TYPE_INT = "int";
        const string CFG_TYPE_STRING = "string";
        const string CFG_TYPE_FLOAT = "float";
        const string CFG_TYPE_BOOL = "bool";
        const string CFG_TYPE_LIST_BOOL = "List<bool>";
        const string CFG_TYPE_LIST_INT = "List<int>";
        const string CFG_TYPE_LIST_STRING = "List<string>";
        const string CFG_TYPE_LIST_FLOAT = "List<float>";
        static public object convertValue(string value, string type)
        {
            if (null==value||value=="")
            {
                return null;
            }
            switch (type)
            {
                case CFG_TYPE_INT:
                    return int.Parse(value);
                case CFG_TYPE_STRING:
                    return value;
                case CFG_TYPE_FLOAT:
                    return float.Parse(value);
                case CFG_TYPE_BOOL:
                    return bool.Parse(value);
                case CFG_TYPE_LIST_BOOL:
                    {
                        string[] vs=value.Split('|');
                        List<bool> l = new List<bool>();
                        foreach (string s in vs)
                        {
                            l.Add(bool.Parse(s));
                        }
                        return l;
                    }
                case CFG_TYPE_LIST_INT:
                    {
                        string[] vs = value.Split('|');
                        List<int> l = new List<int>();
                        foreach (string s in vs)
                        {
                            l.Add(int.Parse(s));
                        }
                        return l;
                    }
                case CFG_TYPE_LIST_STRING:
                    {
                        string[] vs = value.Split('|');
                        List<string> l = new List<string>();
                        foreach (string s in vs)
                        {
                            l.Add(s);
                        }
                        return l;
                    }
                case CFG_TYPE_LIST_FLOAT:
                    {
                        string[] vs = value.Split('|');
                        List<float> l = new List<float>();
                        foreach (string s in vs)
                        {
                            l.Add(float.Parse(s));
                        }
                        return l;
                    }
                default:
                    return null;
            }
        }
    }
}
