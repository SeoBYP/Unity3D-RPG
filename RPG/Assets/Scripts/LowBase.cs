using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;


public class LowBase : MonoBehaviour
{
    public Dictionary<int, Dictionary<string, string>> InfoDic = new Dictionary<int, Dictionary<string, string>>();


    public void LoadData(string path)
    {
        TextAsset asset = Resources.Load<TextAsset>(path);
        string[] rows = asset.text.Split('\n');
        rows[0] = rows[0].Replace("\r", "");
        string[] subjects = rows[0].Split(',');

        for(int i = 1; i < rows.Length; i++)
        {
            rows[i] = rows[i].Replace("\r", "");
            string[] cols = rows[i].Split(',');

            int tableindex = 0;
            int.TryParse(cols[0], out tableindex);

            if (!InfoDic.ContainsKey(tableindex))
            {
                InfoDic.Add(tableindex, new Dictionary<string, string>());
            }

            for(int j = 1; j < cols.Length; j++)
            {
                if(InfoDic[tableindex].ContainsKey(subjects[j]) == false)
                {
                    InfoDic[tableindex].Add(subjects[j], cols[j]);
                }
            }
        }

    }

    public void LoadSaveData(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);

        byte[] byteArr = GetByteArr(fs);

        string text = System.Text.Encoding.UTF8.GetString(byteArr);

        string[] rows = text.Split('\n');
        rows[0] = rows[0].Replace("\r", "");
        string[] subjects = rows[0].Split(',');



        for (int i = 1; i < rows.Length; i++)
        {
            if (rows[i] == "")
                continue;
            rows[i] = rows[i].Replace("\r", "");
            string[] cols = rows[i].Split(',');

            int tableindex = 0;
            int.TryParse(cols[0], out tableindex);
            if (!InfoDic.ContainsKey(tableindex))
            {
                InfoDic.Add(tableindex, new Dictionary<string, string>());
            }
            for (int j = 1; j < cols.Length; j++)
            {
                if (InfoDic[tableindex].ContainsKey(subjects[j]) == false)
                {
                    InfoDic[tableindex].Add(subjects[j], cols[j]);
                }
            }
        }
    }

    public byte[] GetByteArr(FileStream fp)
    {
        byte[] bytes = null;

        try
        {
            bytes = new byte[fp.Length];
            fp.Read(bytes, 0, (int)fp.Length);
        }
        catch(IOException exception)
        {
            print(exception);
        }
        return bytes;
    }

    public int ToInter(int tableIndex, string subject)
    {
        int data = 0;
        if (InfoDic.ContainsKey(tableIndex))
        {
            if (InfoDic[tableIndex].ContainsKey(subject))
            {
                int.TryParse(InfoDic[tableIndex][subject], out data);
            }
        }

        return data;

    }
    public float Tofloat(int tableIndex, string subject)
    {
        float data = 0;
        if (InfoDic.ContainsKey(tableIndex))
        {
            if (InfoDic[tableIndex].ContainsKey(subject))
            {
                float.TryParse(InfoDic[tableIndex][subject], out data);
            }
        }

        return data;

    }
    public string Tostring(int tableIndex, string subject)
    {
        string data = null;
        if (InfoDic.ContainsKey(tableIndex))
        {
            if (InfoDic[tableIndex].ContainsKey(subject))
            {
                data = InfoDic[tableIndex][subject];
            }
        }

        return data;

    }
    //public bool LoadFileInfo(string path)
    //{
    //    bool isExits = File.Exists(path);
    //    if (!isExits)
    //        return false;
    //    FileStream fp = new FileStream(path, FileMode.Open);
    //    //byte[] byteArr = GetByteArr(fp);
    //    string text = System.Text.Encoding.UTF8.GetString(byteArr);
    //    string[] rows = text.Split('\n');
    //    //string[] cols = text.Split(',');

    //    for(int i = 1; i < rows.Length; i++)
    //    {
    //        string currRow = rows[i].Replace("\r", "");
    //        string[] cols = currRow.Split(',');
            
    //    }
    //    return true;
    //}

    public static FileStream Open(string path, string filename, FileMode fileMode)
    {
        //디렉토리가 없다면 생성할 수 있도록 합니다.
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = path + "/" + filename;
        if (fileMode == FileMode.CreateNew || fileMode == FileMode.Create)
        {
            //filemode가 새로운 파일을 만들경우, 해당 파일이 있다면 삭제하고 새로 생성할 수 있도록 합니다.
            if (File.Exists(path))
                File.Delete(path);
        }
        FileStream fs = new FileStream(path, fileMode);
        return fs;
    }

    public static void Close(FileStream fs)
    {
        if (fs != null)
            fs.Close();
    }
}
