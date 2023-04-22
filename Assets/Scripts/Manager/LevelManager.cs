using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ Ư�� �������� ����� �̺�Ʈ�� �̱������� ����
//�̰Ŷ� ���� �� ������ ���� �� �� �ִ�
public class LevelManager : Singleton<LevelManager>
{
    public Room CurrentRoom { get; set; }

    public event Action<Room> RoomCleared;

    public event Action LevelCleared;

    public List<Room> Rooms { get; set; }   
    public List<Vector2> RoomPoints { get; set; }
    public List<List<int>> RoomEdges { get; set; }

    public void OnRoomClear()
    {
        RoomCleared?.Invoke(CurrentRoom);
    }

    //��������Ʈ�� ������ �Լ��鿡�� ������ Ŭ����Ǿ����� �˸��� �Լ� -> ���� ����
    public void OnLevelClear()
    {
        LevelCleared?.Invoke();
    }
}
