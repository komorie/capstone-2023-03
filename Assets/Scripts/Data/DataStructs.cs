using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructs
{
    [System.Serializable]
    public class CardStruct
    {
        public int index;
        public string name;
        public string description; //����
        public int cost;
        public int rarity;
        public string type; //����, ��ų, ��û�� ��
        public string attack_type; // ����, ����
        public string attribute; //�Ӽ�
        public string target; //Ÿ��
        public int damage; //���ݷ�
        public int times; //Ƚ��
        public string special; //Ư��ȿ��
    }

    [System.Serializable]
    public class LineStruct
    {
        public string portrait;
        public string name;
        public string line;
    }

    [System.Serializable]
    public class SettingStruct
    {
        public bool tutorial;
        public int bgm;
        public int effect;
    }

    [System.Serializable]
    public class RewardStruct
    {
        public int viewers;
        public int money;
    }

}
