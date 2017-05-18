using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class PrefabTypeCfg
    {
    	public int id;
    	public string name;
    	public string remark;
        public static List<PrefabTypeCfg> dataList=new List<PrefabTypeCfg>(); 
        public static Dictionary<int , PrefabTypeCfg> dataMap=new Dictionary<int, PrefabTypeCfg>();
        public static PrefabTypeCfg get(int id)
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
                PrefabTypeCfg item = new PrefabTypeCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (PrefabTypeCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
