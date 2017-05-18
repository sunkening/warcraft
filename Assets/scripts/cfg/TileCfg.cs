using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class TileCfg
    {
    	public int id;
    	public string resourcePath;
    	public string resourceName;
    	public int type;
    	public float height;
    	public bool landAllowed;
    	public string remark;
        public static List<TileCfg> dataList=new List<TileCfg>(); 
        public static Dictionary<int , TileCfg> dataMap=new Dictionary<int, TileCfg>();
        public static TileCfg get(int id)
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
                TileCfg item = new TileCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (TileCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
