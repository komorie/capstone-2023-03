using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�ڵ� ���� ������Ƽ ��� ����
//1. �Լ��� ����뿡 �ɸ���.
public class Room : MonoBehaviour
{
    private bool IsCleared { get; set; } = false;
    private Define.RoomEventType type;
    public EventSymbol RoomSymbol { get; set; } = null;

    //�����ִ� ����-�� ��ųʸ�
    public Dictionary<Define.Direction, Door> Doors { get; set; } = new Dictionary<Define.Direction, Door>((int)Define.Direction.Count);

    private void OnTriggerEnter(Collider collider)
    {
        if(IsCleared == false)
        { 
            //ó�� ���� �� �� ��Ȱ��ȭ
            ActivateDoors(false);

            //���Ͱ� �ִ� ���� �ƴϸ� �ٷ� Ŭ���� ó��
            if ((type != Define.RoomEventType.Normal && type != Define.RoomEventType.Boss) && collider.gameObject.tag == "Player")
            {
                IsCleared = true;
                ActivateDoors(true);
                GameManager.Instance.OnRoomClear();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        //���� �ִ� ���̸�, �¸� Ȥ�� ���� �� �� �ɺ��� �ı��ǰ�? �̶� Ŭ���� ó��
        if (RoomSymbol != null && !RoomSymbol.isActiveAndEnabled && IsCleared == false)
        {
            IsCleared = true;
            ActivateDoors(true);
            GameManager.Instance.OnRoomClear();
        }
    }

    //��� ���� �ʱ�ȭ
    public void Init(Define.RoomEventType type)
    {
        //Type ����
        this.type = type;

        //RoomSymbol ��ȯ
        switch (type)
        {
            case Define.RoomEventType.Normal:
                RoomSymbol = AssetLoader.Instance.Instantiate($"Prefabs/EventSymbol/EnemySymbol", transform).AddComponent<EnemySymbol>();
                break;
            case Define.RoomEventType.Item:
                RoomSymbol = AssetLoader.Instance.Instantiate($"Prefabs/EventSymbol/ItemSymbol", transform).AddComponent<ItemSymbol>();
                break;
            case Define.RoomEventType.Shop:
                RoomSymbol = AssetLoader.Instance.Instantiate($"Prefabs/EventSymbol/ShopSymbol", transform).AddComponent<ShopSymbol>();
                break;
            case Define.RoomEventType.Boss:
                RoomSymbol = AssetLoader.Instance.Instantiate($"Prefabs/EventSymbol/BossSymbol", transform).AddComponent<BossSymbol>();
                break;
            default:
                return;
        }
        RoomSymbol.transform.position = new Vector3(0, 1, 0);
    }

    //�� �濡�� Ư�� ���⿡ �ִ� ���� ������ ��ġ�� �����ϰ�, ������ Doors ��ųʸ��� �߰��Ѵ�.
    public void SetDoorsDictionary(Define.Direction direction, Room destination)
    {
        //�ϴ� ������ �� ��ųʸ��� <����-��> �߰�
        Doors[direction] = transform.Find("Doors").GetChild((int)direction).GetComponent<Door>();
        Doors[direction].gameObject.SetActive(true);

        //���� �� �߽ɿ��� ������ ���� ������ �ݴ� ����
        Vector3 oppositeVector = (transform.position - Doors[direction].transform.position) * 0.8f;
        oppositeVector.y = 0;

        //���� �������� �� ���Ͱ� ����
        Doors[direction].Destination = destination.transform.position + oppositeVector;    
    }

    //���� ���� Ȱ��ȭ/��Ȱ��ȭ
    public void ActivateDoors(bool isActivated)
    {
        foreach(KeyValuePair<Define.Direction, Door> door in Doors) 
        {
            door.Value.gameObject.SetActive(isActivated);
        }
    }

}
