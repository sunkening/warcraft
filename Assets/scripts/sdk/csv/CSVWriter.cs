using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
public class CSVWriter
{
    private StreamWriter sw;
    private FileStream fs;
    private char splitChar = ',';
    public CSVWriter(string filename)
    {
        fs = new FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        sw = new StreamWriter(fs, Encoding.UTF8);
    }
    public CSVWriter(FileStream fs)
    {
        this.fs = fs;
        sw = new StreamWriter(fs, Encoding.UTF8);
    }
    public void writeNext(string[] strs)
    {
        string line = "";
        foreach (string str in strs)
        {
            if (str==null)
            {
                line += str + splitChar;
                continue;
            }
            string s = str;
            if (s.Contains("\""))
            {
                s=s.Replace("\"", "\"\"");
            }
            if (s.Contains(","))
            {
                line += "\"" + s + "\"" + splitChar;
            }
            else
            {
                line += s + splitChar;
            }
        }
        sw.WriteLine(line.Remove(line.Length - 1));
    }
    
    public void writeNext(List<string> strs)
    {
        writeNext(strs.ToArray());
    }
    public void close()
    {
        sw.Close();
        // fs.Close();
    }
}

