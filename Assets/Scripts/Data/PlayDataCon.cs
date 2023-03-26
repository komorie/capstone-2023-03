using LitJson;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

//�÷��̾� ����
[System.Serializable]
public class PlayerStats
{
    public string name;
    public int level;
    public int positionX;
    public int positionY;
}

//�÷��̾� ��
[System.Serializable]
public class PlayerCard
{
    public int cardIndex;
    public int count;
}

//�� ��ġ ������
public class RoomInfo
{
    public bool isCleared;
    public Define.EventType type;
    public int positionX;
    public int positionY;
}

//�÷��̾ �����ϰ� �ε��ؾ� �ϴ� �����͵�. ����, ��, ��ġ, �� ��
[System.Serializable]
public class PlayData
{
    public PlayerStats playerStats;
    public List<PlayerCard> deck;
}

//�����͸� �ҷ��ͼ� playData�� �����ص�. Ȥ�� ������ ������ �� playData�� json���Ϸ� �����ϴ� ����.
//�ٸ� Ŭ�����鿡���� playData�� �����ؼ� ����� �����͸� ����ϰų�, ������ ��ȭ ��(Ȥ�� ���̺� �õ� ��) playData�� �ǽð����� ������.
public class PlayDataCon : Singleton<PlayDataCon>
{
    public PlayData PlayData { get; set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        LoadPlayData();
        SavePlayData();
    }

    public void SavePlayData()
    {
        //�ѱ��� �������̰�, ���ٷ� ��� �Ǵ� ������... ����� ��������
        string jsonData = JsonMapper.ToJson(PlayData);
        File.WriteAllText($"Assets/Resources/Data/PlayData.json", jsonData);
    }

    public void LoadPlayData()
    {
        string filePath = $"Assets/Resources/Data/PlayData.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            PlayData = JsonMapper.ToObject<PlayData>(jsonData);
        }
        else
        {
            Debug.Log("����� ���̺� ������ ����!");
        }
    }
}
