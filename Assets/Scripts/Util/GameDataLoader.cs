using LitJson;
using System.Collections.Generic;
using UnityEngine;

//JSON ���Ϸ� �̸� ����� ���� ���� ������ �ε��ϴ� �Լ�
public static class GameDataLoader
{
    // ����Ʈ�� JSON �����͸� �θ��� ���� �Լ�
    public static void LoadData<T>(string filePath, out List<T> list)
    {
        Debug.Log(filePath + " �ε�");
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null)
        {
            list = JsonMapper.ToObject<List<T>>(jsonData.text);
        }
        else
        {
            list = null;
        }
    }

    // ��ųʸ��� �θ��� ���� �������̵�
    public static void LoadData<T>(string filePath, out Dictionary<int, T> dictionary)
    {
        Debug.Log(filePath + " �ε�");
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null)
        {
            string jsonString = jsonData.text;
            JsonData data = JsonMapper.ToObject(jsonString);

            dictionary = new Dictionary<int, T>();

            for (int i = 0; i < data.Count; i++)
            {
                int index = (int)data[i]["index"];
                T item = JsonMapper.ToObject<T>(data[i].ToJson());

                dictionary.Add(index, item);
            }
        }
        else
        {
            dictionary = null;
        }
    }

    //����Ʈ�� ���� ��ųʸ��� �θ��� ���� �������̵�
    public static void LoadData<T>(string filePath, out Dictionary<int, List<T>> dictionary) where T : new()
    {
        Debug.Log(filePath + " �ε�");
        TextAsset jsonData = AssetLoader.Instance.Load<TextAsset>(filePath);
        if (jsonData != null)
        {
            string jsonString = jsonData.text;
            JsonData dialogData = JsonMapper.ToObject(jsonString);

            dictionary = new Dictionary<int, List<T>>();

            for (int i = 0; i < dialogData.Count; i++)
            {
                int index = (int)dialogData[i]["index"];

                List<T> items;

                if (!dictionary.TryGetValue(index, out items)) // index�� ������ �� ��� �������� �Ǵ��ϰ� index�� Ű�� ���� ����Ʈ�� �߰�.
                {
                    items = new List<T>();
                    dictionary.Add(index, items);
                }

                T item = new T(); // ��ųʸ��� ����Ʈ�� ������ �� �׸�.
                item = JsonMapper.ToObject<T>(dialogData[i].ToJson());

                items.Add(item);
            }
        }
        else
        {
            dictionary = null;
        }
    }
}
