using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



//LitJson �÷����� ��ƾ���
//��ȭâ UI���� ��ȭ ������ �������� �ϴ� ������ ���
public class DialogManager : Singleton<DialogManager>
{
    //�̸� JSON ���Ͽ��� ��縦 �����ͼ� ��ųʸ��� ����.
    public Dictionary<int, List<Dialog>> DialogDic { get; set; } = new Dictionary<int, List<Dialog>>(); 

    //���ĵ� �׸��� ��ųʸ��� ����.
    public Dictionary<string, Sprite> PortraitDic { get; set; } = new Dictionary<string, Sprite>();

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

            //��ȭ ����
            for (int j = 0; j < dialogData[i]["lines"].Count; j++)
            {
                Dialog dialog = new Dialog();

                dialog.portrait = dialogData[i]["lines"][j]["portrait"]?.ToString();
                dialog.name = dialogData[i]["lines"][j]["name"]?.ToString();
                dialog.line = dialogData[i]["lines"][j]["line"].ToString();
                dialogList.Add(dialog);
            }

            DialogDic.Add(index, dialogList);
        }
    }

    public Sprite GetSprite(string name)
    {
        Sprite portrait;
        if (!PortraitDic.ContainsKey(name))
        {
            portrait = AssetLoader.Instance.Load<Sprite>($"Images/Portrait/{name}");
            PortraitDic[name] = portrait;   
        }
        portrait = PortraitDic[name];
        return portrait;

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
}
