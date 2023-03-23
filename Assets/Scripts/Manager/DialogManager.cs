using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;


//LitJson �÷����� ��ƾ���
//1. ��ȭâ�� ��ȭ ������ �������� �ϴ� ����.
//2. ��ȭâ UI�� ��ȯ�ϴ� �Լ�.

public class DialogManager : Singleton<DialogManager>
{
    private DialogUI dialogUI;

    //�̸� JSON ���Ͽ��� ��縦 �����ͼ� ��ųʸ��� ����.
    private Dictionary<int, List<Dialog>> DialogDic { get; set; } = new Dictionary<int, List<Dialog>>(); 

    //�̸��� �´� ���ĵ� �׸��� ��ųʸ��� ����.
    private Dictionary<string, Sprite> PortraitDic { get; set; } = new Dictionary<string, Sprite>();

    //���� ������ ��� �����־�� �ϹǷ� �̱���
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

            int index = int.Parse(dialogData[i]["index"].ToString());

            //Ư�� ���� ��ȭ�� ��ųʸ��� �߰�
            for (int j = 0; j < dialogData[i]["lines"].Count; j++)
            {
                Dialog dialog = new Dialog();

                Sprite portrait;
                string portraitName = dialogData[i]["lines"][j]["portrait"]?.ToString();

                //��Ʈ����Ʈ �̸��� �´� �ʻ�ȭ ���� �����ͼ� ����
                if (portraitName != null)
                {
                    if (!PortraitDic.ContainsKey(portraitName))
                    {
                        portrait = AssetLoader.Instance.Load<Sprite>($"Images/Portrait/{portraitName}");
                        PortraitDic[portraitName] = portrait;
                    }
                    dialog.portrait = PortraitDic[portraitName];
                }
                else
                {
                    dialog.portrait = null;
                }

                //�̸�, ��� �� �����ͼ� ����
                dialog.name = dialogData[i]["lines"][j]["name"]?.ToString();
                dialog.line = dialogData[i]["lines"][j]["line"].ToString();
                dialogList.Add(dialog);
            }

            DialogDic.Add(index, dialogList);
        }
    }

    //��ȭ ��ųʸ����� Ư�� �ε�����, ���° �ٿ� �ش��ϴ� ��縦 �����´�.
    public Dialog GetLine(int index, int lineIndex)
    {
        if (lineIndex == DialogDic[index].Count)
        {
            return null;
        }
        else
        {
            return DialogDic[index][lineIndex];
        }
    }

    //��ȭâ ����
    public void OpenDialog(int index, Action CloseCallback = null)
    {
        dialogUI = PanelManager.Instance.ShowPanel("DialogUI", false, CloseCallback).GetComponent<DialogUI>();
        dialogUI.FirstDialog(index);
    }
}
