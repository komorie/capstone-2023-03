using DataStructs;
using LitJson;
using System.IO;
using UnityEngine;

public class SettingData : Singleton<SettingData>
{
    public SettingStruct Setting { get; set; } = new SettingStruct();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadSettingData();
    }

    private void OnApplicationQuit()
    {
        SaveSettingData(); //끌 때 설정값 저장
    }

    private void SaveSettingData() //게임 종료 시, 설정 데이터 저장.
    {
        string jsonData = JsonMapper.ToJson(Setting);
        string filePath = Path.Combine(Application.persistentDataPath, "Setting.json");
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadSettingData() //런타임으로 저장/불러오기를 하는 데이터이므로 GameDataLoader 사용 X
    {
        Debug.Log("Setting Data Load");
        string filePath = Path.Combine(Application.persistentDataPath, "Setting.json");

        if (File.Exists(filePath)) //두번째 실행 이후부터는 저장된 세팅 데이터를 가져온다.
        {
            string dataAsJson = File.ReadAllText(filePath);
            Setting = JsonUtility.FromJson<SettingStruct>(dataAsJson);
        }
        else //처음 게임을 실행할 때는 기본 설정으로.
        {
            Setting.tutorial = true;
            Setting.bgm = 50;
            Setting.effect = 50;
        }
    }
}
