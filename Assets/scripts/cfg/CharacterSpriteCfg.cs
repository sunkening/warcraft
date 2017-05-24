using System.Collections.Generic;
using skn.utils;
namespace  cfg
{

 public class CharacterSpriteCfg
    {
    	public int id;
    	public string name;
    	public int frameRate;
    	public List<int> runAnim;
    	public List<int> idleAnim1;
    	public List<int> idleAnim2;
    	public List<int> attackAnim;
    	public int dieAnim;
        public static List<CharacterSpriteCfg> dataList=new List<CharacterSpriteCfg>(); 
        public static Dictionary<int , CharacterSpriteCfg> dataMap=new Dictionary<int, CharacterSpriteCfg>();
        public static CharacterSpriteCfg get(int id)
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
                CharacterSpriteCfg item = new CharacterSpriteCfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (CharacterSpriteCfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
