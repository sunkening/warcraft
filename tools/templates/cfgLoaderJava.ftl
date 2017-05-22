package ${entity.javaPackage};

import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import skn.util.BeanUtils;
import com.code.generater.Util;
import com.csvreader.CsvReader;

 <#-- freemaker 的注释-->
public class ${entity.csvName?cap_first}Cfg
{
		<#list entity.properties as property>
	public ${property.javaType} ${property.propertyName};
		</#list>
	public List<${entity.csvName?cap_first}> dataList = new ArrayList<${entity.csvName?cap_first}>();
 
 	public void load(String path) throws Exception {
		CsvReader r = new CsvReader(path + "${entity.csvName}.csv", ',',
				Charset.forName("UTF-8"));
		List<String> attrList = new ArrayList<String>();
		List<String> typeList = new ArrayList<String>();
		// 读取注释
		r.readRecord();
		// 读取表头
		r.readRecord();
		int count = r.getColumnCount();
		System.out.println(count);
		for (int i = 0; i < count; i++) {
			attrList.add(r.get(i));
		}
		// 读取类型
		r.readRecord();
		for (int i = 0; i < count; i++) {
			typeList.add(r.get(i));
		}
		while (r.readRecord()) {
			${entity.csvName?cap_first} item = new ${entity.csvName?cap_first}();
			for (int i = 0; i < count; i++) {
				String attributeName = attrList.get(i);
				Object value = Util.convertValue(r.get(i), typeList.get(i));
				BeanUtils.setProperty(item, attributeName, value);
			}
			dataList.add(item);
		}
		r.close();
	}
    
}






 