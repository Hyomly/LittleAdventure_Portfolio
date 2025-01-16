using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TableLoader : Singleton<TableLoader>
{
    List<Dictionary<string,string>> m_table = new List<Dictionary<string,string>>();
    public int Count { get { return m_table.Count; } }
    public void Clear()
    {
        m_table.Clear();
    }

    public string GetString(string key, int index)
    {
        return m_table[index][key];
    }
    public int GetInteger(string key, int index)
    {
        return int.Parse(GetString(key, index));
    }
    public float GetFloat (string key, int index)
    {
        return float.Parse(GetString(key, index));
    }
    public byte GetByte(string key, int index)
    {
        return byte.Parse(GetString(key, index));
    }
    public bool GetBoolen(string key, int index)
    {
        return bool.Parse(GetString(key, index));
    }

    public T GetEnum<T>(string key, int index)
    {
        return (T) Enum.Parse(typeof(T),GetString( key, index));
    }
    public void LoadTable(string tableName)
    {
        var data = Resources.Load<TextAsset>("ExcelDatas/" + tableName);
        MemoryStream ms = new MemoryStream(data.bytes);
        StreamReader sr = new StreamReader(ms);

        int rowCount = int.Parse(sr.ReadLine());
        int columnCount = int.Parse(sr.ReadLine());
        string stringData = sr.ReadToEnd();

        string[] tableData = stringData.Split('\t') ;
        List<string> keyList = new List<string>();

        int offset = 1;
        for (int i = 0; i < rowCount; i++)
        {
            
            if (i == 0)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (j == columnCount - 1)
                    {
                        keyList.Add(tableData[offset].Replace("\n", ""));
                    }
                    else
                    {
                        keyList.Add(tableData[offset]);
                    }
                    offset++;
                }
            }
            else
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();
                for(int j = 0; j < columnCount; j++)
                {
                    if (j == columnCount - 1)
                    {
                        rowData.Add(keyList[j], tableData[offset].Replace("\n", ""));
                    }
                    else
                    {
                        rowData.Add(keyList[j], tableData[offset]);
                    }
                    offset++;
                }
                m_table.Add(rowData);
            }
        }
    }
}
