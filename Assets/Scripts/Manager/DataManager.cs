/*using LitJson;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //����Ʈ�� JSON �����ͷ� ����
    public void SaveJson<T>(T collection, string dataName)
    {
        JsonData jsonData = JsonMapper.ToJson(collection);
        File.WriteAllText($"Assets/Resources/Data/{dataName}.json", jsonData.ToString());
    }

    //JSON ������ ��������
    public JsonData LoadJson(string dataName)
    {
        string path = $"Assets/Resources/Data/{dataName}.json";
        string JsonString = File.ReadAllText(path);
        JsonData data = JsonMapper.ToObject(JsonString);
        return data;
    }

}*/
