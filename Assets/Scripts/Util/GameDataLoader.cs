using LitJson;
using System.Collections.Generic;
using UnityEngine;

//JSON 파일로 미리 저장된 게임 기초 데이터 로딩하는 함수
public static class GameDataLoader
{
    // 리스트에 JSON 데이터를 부르는 버전 함수
    public static void LoadData<T>(string filePath, out List<T> list)
    {
        Debug.Log(filePath + " 로드");
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

    // 딕셔너리에 부르는 버전 오버라이드
    public static void LoadData<T>(string filePath, out Dictionary<int, T> dictionary)
    {
        Debug.Log(filePath + " 로드");
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

    //리스트를 갖는 딕셔너리에 부르는 버전 오버라이드
    public static void LoadData<T>(string filePath, out Dictionary<int, List<T>> dictionary) where T : new()
    {
        Debug.Log(filePath + " 로드");
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

                if (!dictionary.TryGetValue(index, out items)) // index가 같으면 한 대사 묶음으로 판단하고 index를 키로 갖는 리스트에 추가.
                {
                    items = new List<T>();
                    dictionary.Add(index, items);
                }

                T item = new T(); // 딕셔너리의 리스트에 저장할 한 항목.
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
