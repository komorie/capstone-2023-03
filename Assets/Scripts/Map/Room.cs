using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�ڵ� ���� ������Ƽ ��� ����
//1. �Լ��� ����뿡 �ɸ���.
public class Room : MonoBehaviour
{
    private bool IsCleared { get; set; } = false;

    private Define.EventType Type;
    public RoomSymbol Symbol { get; set; } = null;

    //�����ִ� ����-�� ��ųʸ�
    public Dictionary<Define.Direction, Door> Doors { get; set; } = new Dictionary<Define.Direction, Door>((int)Define.Direction.Count);

    private void OnTriggerEnter(Collider collider)
    {
        //���� �� ��ġ ����
        MapManager.Instance.CurrentRoom = this;

        if (IsCleared == false)
        { 
            //ó�� ���� �� �� �ݱ�
            ActivateDoors(false);

            //���� ���� ���̸� �׳� Ŭ���� ó��
            if(Type != Define.EventType.Enemy && Type != Define.EventType.Boss && Type != Define.EventType.Event)
            {
                IsCleared = true;
                ActivateDoors(true);
                MapManager.Instance.OnRoomClear();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        //�ɺ��� ���ų� �ı��Ǿ��� ��, Ȥ�� ���̳� ������ �ƴ� �� �� Ŭ���� ó�� (���߿� ��ü ����)
        if (Symbol == null && IsCleared == false)
        {
            IsCleared = true;
            ActivateDoors(true);
            MapManager.Instance.OnRoomClear();
        }
    }

    //��� ���� �ʱ�ȭ
    public void Init(Define.EventType type)
    {
        //Type ����
        this.Type = type;
        //Symbol ��ȯ
        switch (type)
        {
            //�ɺ��� ��ȯ�� ��, ���� �������� �ε����� �����ϰ�, �ε����� ���� �ٸ� ��ȭ ����� ����, ������ ȹ�� ���� �ϰ� �� ����
            //�̰� �ֵ� ������Ʈ�� �� ���� �����տ� �ٿ��� �ɵ�...?
            case Define.EventType.Enemy:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/EnemySymbol", transform)
                    .AddComponent<EnemySymbol>();
                Symbol.Index = Random.Range(1, 4);
                break;
            case Define.EventType.Rest:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/RestSymbol", transform)
                    .AddComponent<RestSymbol>();
                Symbol.Index = 3001;
                break;
            case Define.EventType.Shop:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/ShopSymbol", transform)
                    .AddComponent<ShopSymbol>();
                Symbol.Index = 2001;
                break;
            case Define.EventType.Event:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/EventSymbol", transform)
                    .AddComponent<EventSymbol>();
                Symbol.Index = 2001;
                break;
            case Define.EventType.Boss:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/BossSymbol", transform)
                    .AddComponent<BossSymbol>();
                Symbol.Index = 1001;
                break;
            default:
                return;
        }
        Symbol.Type = type;
        Symbol.transform.position = new Vector3(0, 1, 0);
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
