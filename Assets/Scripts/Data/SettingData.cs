using DataStructs;
using LitJson;
using System.IO;
using UnityEngine;

public class SettingData : Singleton<SettingData>
{
    public SettingStruct SettingStruct { get; set; } = new SettingStruct();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadSettingData();
    }

    private void OnApplicationQuit()
    {
        SaveSettingData(); //�� �� ������ ����
    }

    private void LoadSettingData() //����� ���� �����͵� ��������
    {
        Debug.Log("Setting Data Load");
        string filePath = "Assets/Resources/Data/Setting.json";
        if (File.Exists(filePath)) //�ι�° ���� ���ĺ��ʹ� ����� ���� �����͸� �����´�.
        {
            string jsonData = File.ReadAllText(filePath);
            SettingStruct = JsonMapper.ToObject<SettingStruct>(jsonData);
        }
        else //ó�� ������ ������ ���� �⺻ ��������.
        {
            SettingStruct.tutorial = true;
            SettingStruct.bgm = 50;
            SettingStruct.effect = 50;  
        }
    }

    private void SaveSettingData() //���� ���� ��, ���� ������ ����.
    {
        string filePath = "Assets/Resources/Data/Setting.json";
        string jsonData = JsonMapper.ToJson(SettingStruct);
        File.WriteAllText(filePath, jsonData);
    }
}
