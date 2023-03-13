using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ���� ��ҵ�, ������ Ư�� �������� ����� �̺�Ʈ�� �̱������� ����
public class GameManager : Singleton<GameManager>
{
    public Map Map { get; set; }
    public GameObject Player { get; set; }
    public Vector3 StartPosition { get; set; } = Vector3.zero;

    public event Action<Room> onRoomClear;
    public event Action onLevelClear;

    protected override void Awake()
    {
        base.Awake();

        Map = new GameObject("Map").AddComponent<Map>();
        Player = Instantiate(Resources.Load<GameObject>("Prefabs/Player"), StartPosition, Quaternion.identity); 
    }


    //�̷��� �̺�Ʈ�� �۷ι��� �����Ѵٸ�, �� Ŭ���� �̺�Ʈ �����ʵ��� ����� ��, '� ���� Ŭ����Ǿ�����' �� �� �� �־��
    //Ư�� �游 ���� �����ٴ��� �ϴ� �� �����ѵ�... 
    //��ųʸ��� ��� ������?? �ϸ� �ǳ�?
    public void OnRoomClear(Room room)
    {
        onRoomClear?.Invoke(room);
    }

    //��������Ʈ�� ������ �Լ��鿡�� ������ Ŭ����Ǿ����� �˸��� �Լ� -> ���� ����
    public void OnLevelClear()
    {
        onLevelClear?.Invoke();
    }
}
