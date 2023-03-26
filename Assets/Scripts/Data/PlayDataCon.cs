using LitJson;
using System.Collections.Generic;
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
    public string cardIndex;
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

//�÷��̾ �����ϰ� �ε��ؾ� �ϴ� �����͵�.
[System.Serializable]
public class PlayData
{
    public PlayerStats playerStats;
    public List<PlayerCard> deck;
    public List<RoomInfo> map; //�� ����
}

//�����͸� �ҷ��ͼ� playData�� �����ص�. Ȥ�� ������ ������ �� playData�� json���Ϸ� �����ϴ� ����.
//�ٸ� Ŭ�����鿡���� playData�� �����ؼ� ����� �����͸� ����ϰų�, ������ ��ȭ ��(Ȥ�� ���̺� �õ� ��) playData�� �ǽð����� ������.
public class PlayDataCon : Singleton<PlayDataCon>
{
    public PlayData PlayData { get; set; }


    public void SavePlayData()
    {

    }

    public void LoadPlayData()
    {
        string filePath = $"Assets/Resources/Data/PlayData.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            PlayData = JsonMapper.ToObject<PlayData>(jsonData);

            Debug.Log("Player Name: " + PlayData.playerStats.name);
            Debug.Log("Player Level: " + PlayData.playerStats.level);
            Debug.Log("Player ��ġX: " + PlayData.playerStats.positionX);
            Debug.Log("Player ��ġY: " + PlayData.playerStats.positionY);


            foreach (PlayerCard card in PlayData.deck)
            {
                Debug.Log("Inventory Card: " + card.cardIndex + ", Count: " + card.count);
            }
        }
        else
        {
            Debug.Log("����� ���̺� ������ ����!");
        }
    }
}
