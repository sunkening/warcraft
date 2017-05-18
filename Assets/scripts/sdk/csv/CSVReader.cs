using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using skn.utils;
using UnityEngine;


public class CSVReader
{

    private Dictionary<string, string> line = new Dictionary<string, string>();
    private List<string> header = new List<string>();
    private string[] linearr;
    private StreamReader sr = null;
    private FileStream fs = null;
    private int columnCount;
    private int columnIndex;
    public bool readHeader()
    {
        if (!readRecord()) return false;
        columnIndex = 0;
        columnCount = linearr.Length;
        foreach (string key in linearr)
        {
            header.Add(key);
        }
        return true;
    }
    public bool readRecord()
    {
        string strLine = null;
        line.Clear();
        linearr = null;
        if ((strLine = sr.ReadLine()) == null)
        {
            return false;
        }
        List<string> splitedList=split(strLine, Encoding.UTF8);
        linearr = splitedList.ToArray();
        columnCount = linearr.Length;
        columnIndex = 0;
        int i = 0;
        if (header.Count > 0)
        {
            foreach (string vavue in linearr)
            {
                line[header[i]] = vavue;
                i++;
            }
        }

        return true;
    }
    static List<string> split(string line, Encoding encoding)
    {
        byte[] b = encoding.GetBytes(line);
        List<string> bls = new List<string>();
        int end = b.Length - 1;

        List<byte> bl = new List<byte>();
        bool inQuote = false;
        for (int i = 0; i < b.Length; i++)
        {
            switch ((char)b[i])
            {
                case ',':
                    if (inQuote)
                        bl.Add(b[i]);
                    else
                    {
                        bls.Add(makefield(ref bl, encoding));  // 将语句中的引号去掉
                        //bls.Add(encoding.GetString(bl.ToArray()));
                        bl.Clear();
                    }
                    break;
                case '"':
                    inQuote = !inQuote;
                    bl.Add((byte)'"');
                    break;
                case '\\':
                    if (i < end)
                    {
                        switch ((char)b[i + 1])
                        {
                            case 'n':
                                bl.Add((byte)'\n');
                                i++;
                                break;
                            case 't':
                                bl.Add((byte)'\t');
                                i++;
                                break;
                            case 'r':
                                i++;
                                break;
                            default:
                                bl.Add((byte)'\\');
                                break;
                        }
                    }
                    else
                        bl.Add((byte)'\\');
                    break;
                default:
                    bl.Add(b[i]);
                    break;
            }
        }
        bls.Add(makefield(ref bl, encoding));  // 将语句中的引号去掉
       // bls.Add(encoding.GetString(bl.ToArray()));
        bl.Clear();
        return bls;
    }
    static string makefield(ref List<byte> bl, Encoding encoding)
    {
        if (bl.Count > 1 && bl[0] == '"' && bl[bl.Count - 1] == '"')
        {
            bl.RemoveAt(0);
            bl.RemoveAt(bl.Count - 1);
        }
        int n = 0;
        while (true)
        {
            if (n >= bl.Count)
                break;
            if (bl[n] == '"')
            {
                if (n < bl.Count - 1 && bl[n + 1] == '"')
                {
                    bl.RemoveAt(n + 1);
                    n++;
                }
                else
                    bl.RemoveAt(n);
            }
            else
                n++;
        }

        return encoding.GetString(bl.ToArray());
    }
    public void load(string filePath)
    {
        fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        sr = new StreamReader(fs, Encoding.UTF8);
    }
    public void load(byte[] bytes)
    {
        MemoryStream memoryStream = new MemoryStream(bytes);
        sr = new StreamReader(memoryStream, Encoding.UTF8);
    }
    public void close()
    {
        if (sr != null)
        {
            sr.Close();
        }
        if (fs != null)
        {
            fs.Close();
        }
        line.Clear();
        header.Clear();
        linearr = null;
    }

    public string get(int index)
    {
        return linearr[index];
    }
    public string get(string key)
    {
        return line[key];
    }
    public int getInt(int index)
    {
        return int.Parse(get(index));
    }
    public float getFloat(int index)
    {
        return float.Parse(get(index));
    }
    public bool getBool(int index)
    {
        return bool.Parse(get(index));
    }
    public int getInt()
    {
        return int.Parse(get(columnIndex++));
    }
    public float getFloat()
    {
        return float.Parse(get(columnIndex++));
    }
    public bool getBool()
    {
        return bool.Parse(get(columnIndex++));
    }
    public string getString()
    {
        return get(columnIndex++);
    }
    public int getColumnCount()
    {
        return columnCount;
    }
}
