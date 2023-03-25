using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;


//LitJson �÷����� ��ƾ���
//��ȭ ������ ����� Ŭ����
//Dialog: �� ���� ��ȭ ������ ����� Ŭ����
//DialogData: JSON���� ������, ��ü ��ȭ ������ �����. UI ��� ������ �� �ְ� ��.

public class Dialog
{
    public Sprite portrait;
    public string name;
    public string line;

    public Dialog(Sprite portrait, string name, string line)
    {
        this.portrait = portrait;
        this.name = name;
        this.line = line;
    }
}

public class DialogData : Singleton<DialogData>
{
    //�̸� JSON ���Ͽ��� ��縦 �����ͼ� ��ųʸ��� ����.
    //I/O�� �׶��׋� �ϴ� �� �ð� �Ҹ� ũ�Ƿ�, �޸𸮿� ���� �÷����� ���
    private Dictionary<int, List<Dialog>> DialogDic { get; set; } = new Dictionary<int, List<Dialog>>(); 

    //���� ������ ��� �����־�� ��.
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        //JSON �����͸� �����ͼ�, ��ųʸ��� ����.
        //Index���� �����Ͽ� �����ؼ� ���߿� Index�� ��ȭ ������ �������� ��.
        JsonData dialogData = DataManager.Instance.LoadJson("Dialog");

        for (int i = 0; i < dialogData.Count; i++)
        {
            List<Dialog> dialogList = new List<Dialog>();

            //index�� �´� ��ü ��ȭ ���� �����͸� ��������
            int index = int.Parse(dialogData[i]["index"].ToString());

            //Ư�� ���� ��ȭ�� ��ųʸ��� �߰�
            for (int j = 0; j < dialogData[i]["lines"].Count; j++)
            {
                Sprite portrait;
                string portraitName = dialogData[i]["lines"][j]["portrait"]?.ToString();

                //��Ʈ����Ʈ �̸��� �´� �ʻ�ȭ ���� �����ͼ� ����
                if (portraitName != null)
                {
                    if (!SpriteData.SpriteDictionary.ContainsKey(portraitName))
                    {
                        portrait = AssetLoader.Instance.Load<Sprite>($"Images/Portrait/{portraitName}");
                        SpriteData.SpriteDictionary[portraitName] = portrait;
                    }
                    portrait = SpriteData.SpriteDictionary[portraitName];
                }
                else
                {
                    portrait = null;
                }

                //�̸�, ��� �� �����ͼ� ����
                string name = dialogData[i]["lines"][j]["name"]?.ToString();
                string line = dialogData[i]["lines"][j]["line"].ToString();

                //��ü ��ȭ ����Ʈ�� �� �� �߰�
                dialogList.Add(new Dialog(portrait, name, line));
            }
            DialogDic.Add(index, dialogList);
        }
    }

    //��ȭ ��ųʸ����� Ư�� �ε�����, ���° �ٿ� �ش��ϴ� ��縦 �����´�.
    public Dialog GetLine(int index, int lineIndex)
    {
        if (lineIndex >= DialogDic[index].Count)
        {
            return null;
        }
        else
        {
            return DialogDic[index][lineIndex];
        }
    }
}
