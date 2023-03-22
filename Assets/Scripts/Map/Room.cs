using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�ڵ� ���� ������Ƽ ��� ����
//1. �Լ��� ����뿡 �ɸ���.
public class Room : MonoBehaviour
{
    private bool IsCleared { get; set; } = false;

    private Define.EventType type;
    public RoomSymbol Symbol { get; set; } = null;

    //�����ִ� ����-�� ��ųʸ�
    public Dictionary<Define.Direction, Door> Doors { get; set; } = new Dictionary<Define.Direction, Door>((int)Define.Direction.Count);

    private void OnTriggerEnter(Collider collider)
    {
        if(IsCleared == false)
        { 
            //ó�� ���� �� �� ��Ȱ��ȭ
            ActivateDoors(false);

            //���Ͱ� �ִ� ���� �ƴϸ� �ٷ� Ŭ���� ó��
            if ((type != Define.EventType.Enemy && type != Define.EventType.Boss) && collider.gameObject.tag == "Player")
            {
                IsCleared = true;
                ActivateDoors(true);
                LevelManager.Instance.OnRoomClear();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        //���� �ִ� ���̸�, �¸� Ȥ�� ���� �� �� �ɺ��� �ı��ǰ�? �̶� Ŭ���� ó��
        if (Symbol != null && !Symbol.isActiveAndEnabled && IsCleared == false)
        {
            IsCleared = true;
            ActivateDoors(true);
            LevelManager.Instance.OnRoomClear();
        }
    }

    //��� ���� �ʱ�ȭ
    public void Init(Define.EventType type)
    {
        //Type ����
        this.type = type;
        //Symbol ��ȯ
        switch (type)
        {
            //�ɺ��� ��ȯ�� ��, ���� �������� �ε����� �����ϰ�, �ε����� ���� �ٸ� ��ȭ ����� ����, ������ ȹ�� ���� �ϰ� �� ����
            case Define.EventType.Enemy:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/EnemySymbol", transform).AddComponent<EnemySymbol>();
                Symbol.Index = Random.Range(1, 4);
                break;
            case Define.EventType.Item:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/ItemSymbol", transform).AddComponent<ItemSymbol>();
                Symbol.Index = 2;
                break;
            case Define.EventType.Shop:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/ShopSymbol", transform).AddComponent<ShopSymbol>();
                Symbol.Index = 3;
                break;
            case Define.EventType.Boss:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/BossSymbol", transform).AddComponent<BossSymbol>();
                Symbol.Index = 4;
                break;
            default:
                return;
        }
        Symbol.SymbolType = type;
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
