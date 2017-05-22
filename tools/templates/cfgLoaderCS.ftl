using System.Collections.Generic;
using skn.utils;
namespace  ${entity.namespace}
{
 <#-- freemaker 的注释-->

 public class ${entity.csvName?cap_first}Cfg
    {
   	 	<#list entity.properties as property>
    	public ${property.javaType} ${property.propertyName};
		</#list>
        public static List<${entity.csvName?cap_first}Cfg> dataList=new List<${entity.csvName?cap_first}Cfg>(); 
        public static Dictionary<int , ${entity.csvName?cap_first}Cfg> dataMap=new Dictionary<int, ${entity.csvName?cap_first}Cfg>();
        public static ${entity.csvName?cap_first}Cfg get(int id)
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
                ${entity.csvName?cap_first}Cfg item = new ${entity.csvName?cap_first}Cfg();
                for (int i = 0; i < count; i++)
                {
                    object value = StringUtil.convertValue(r.get(i), typeList[i]);
                    BeanUtil.setPublicProperty(item, attrList[i], value);
                }
                dataList.Add(item);
            }
            r.close();
            foreach (${entity.csvName?cap_first}Cfg t in dataList)
            {
                dataMap[t.id] = t;
            }
        }
    }
}
    