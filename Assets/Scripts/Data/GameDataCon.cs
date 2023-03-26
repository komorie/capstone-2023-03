using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int index;
    public Sprite portrait;
    public string name;
    public string description;
    public int cost;
    public int damage;
    public int heal;
    public int attackCount;

    public string attackRange;
    public string attribute; //�Ӽ�
    public string damageType;
    public string rarity; //���
    public string passive; //Ư�� ȿ��?
}

public class GameDataCon : Singleton<GameDataCon>
{

    //���� ��ο� �´� ��������Ʈ ���� ��ü�� ����� ��ųʸ�
    public Dictionary<string, Sprite> SpriteDic { get; set; } = new Dictionary<string, Sprite>();

    //ī�� ��ü�� ����� ����Ʈ
    private List<Card> CardList { get; set; } = new List<Card>();

    //��ȭ �α� ��ü�� ����� ����Ʈ
    private List<Dialog> DialogList { get; set; } = new List<Dialog>();
}
