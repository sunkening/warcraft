using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class AudioCfg
    {
    	public int id;
    	public string resourcePath;
    	public string resourceName;
    	public string remark;
        public static List<AudioCfg> dataList=new List<AudioCfg>(); 
        public static Dictionary<int , AudioCfg> dataMap=new Dictionary<int, AudioCfg>();
        public static AudioCfg get(int id)
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
                AudioCfg item = new AudioCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (AudioCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
