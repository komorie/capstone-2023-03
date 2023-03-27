/*using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;


//LitJson �÷����� ��ƾ���
//��ȭ ������ ����� Ŭ����
//LineData: �� ���� ��ȭ ������ ����� Ŭ����
//DialogData: JSON���� ������, ��ü ��ȭ ������ �����. UI ��� ������ �� �ְ� ��.

public class LineData
{
    public Sprite portrait;
    public string name;
    public string currentLine;

    public LineData(Sprite portrait, string name, string currentLine)
    {
        this.portrait = portrait;
        this.name = name;
        this.currentLine = currentLine;
    }
}

public class DialogData : Singleton<DialogData>
{
    //�̸� JSON ���Ͽ��� ��縦 �����ͼ� ��ųʸ��� ����.
    //I/O�� �׶��׋� �ϴ� �� �ð� �Ҹ� ũ�Ƿ�, �޸𸮿� ���� �÷����� ���
    private Dictionary<int, List<LineData>> DialogDic { get; set; } = new Dictionary<int, List<LineData>>(); 

    //���� ������ ��� �����־�� ��.
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        //JSON �����͸� �����ͼ�, ��ųʸ��� ����.
        //Index���� �����Ͽ� �����ؼ� ���߿� Index�� ��ȭ ������ �������� ��.
        JsonData dialogData = DataManager.Instance.LoadJson("LineData");

        for (int i = 0; i < dialogData.Count; i++)
        {
            List<LineData> dialogList = new List<LineData>();

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
                string currentLine = dialogData[i]["lines"][j]["currentLine"].ToString();

                //��ü ��ȭ ����Ʈ�� �� �� �߰�
                dialogList.Add(new LineData(portrait, name, currentLine));
            }
            DialogDic.Add(index, dialogList);
        }
    }

    //��ȭ ��ųʸ����� Ư�� �ε�����, ���° �ٿ� �ش��ϴ� ��縦 �����´�.
    public LineData GetLine(int index, int lineIndex)
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
*/