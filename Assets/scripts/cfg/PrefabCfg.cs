using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class PrefabCfg
    {
    	public int id;
    	public int type;
    	public string resourcePath;
    	public string resourceName;
    	public string name;
    	public string remark;
        public static List<PrefabCfg> dataList=new List<PrefabCfg>(); 
        public static Dictionary<int , PrefabCfg> dataMap=new Dictionary<int, PrefabCfg>();
        public static PrefabCfg get(int id)
        {
            if (!dataMap.ContainsKey(id))
            {
                return null;
            }
            return dataMap[id];
        }
        public static void load(byte[] bytes)
        {
			dataList.Clear();
			dataMap.Clear();
            CSVReader r = new CSVReader( );
            r.load(bytes);
            List<string> attrList = new  List<string>();
            List<string> typeList = new List<string>();
            // 读取注释
			r.readRecord();
            r.readHeader();
            int count = r.getColumnCount();
            for (int i = 0; i < count; i++)
            {
                attrList.Add(r.get(i));
            }
            r.readRecord();
            for (int i = 0; i < count; i++)
            {
                typeList.Add(r.get(i));
            }
            while (r.readRecord())
            {
                PrefabCfg item = new PrefabCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (PrefabCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
