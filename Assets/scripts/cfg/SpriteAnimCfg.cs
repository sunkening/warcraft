using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class SpriteAnimCfg
    {
    	public int id;
    	public string name;
    	public bool isMultiple;
    	public int fixelsPerUnit;
    	public string resourcePath;
    	public string resourceName;
    	public int nBegin;
    	public int nEnd;
    	public int frameWidth;
    	public int frameHeight;
    	public int row;
        public static List<SpriteAnimCfg> dataList=new List<SpriteAnimCfg>(); 
        public static Dictionary<int , SpriteAnimCfg> dataMap=new Dictionary<int, SpriteAnimCfg>();
        public static SpriteAnimCfg get(int id)
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
                SpriteAnimCfg item = new SpriteAnimCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (SpriteAnimCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
