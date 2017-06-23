package cfg;

import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import skn.util.BeanUtils;
import com.code.generater.Util;
import com.csvreader.CsvReader;

public class TerrainBrickCfg
{
	public Integer id;
	public String path;
	public String name;
	public List<TerrainBrick> dataList = new ArrayList<TerrainBrick>();
 
 	public void load(String path) throws Exception {
		CsvReader r = new CsvReader(path + "terrainBrick.csv", ',',
				Charset.forName("UTF-8"));
		List<String> attrList = new ArrayList<String>();
		List<String> typeList = new ArrayList<String>();
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
			TerrainBrick item = new TerrainBrick();
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






