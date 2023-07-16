using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//자동 구현 프로퍼티 사용 이유
//1. 함수라서 디버깅에 걸린다.
public class Room : MonoBehaviour
{
    public bool IsCleared { get; set; } = false; //방 깼다
    public bool IsEntered { get; set; } = false; //방 들어왔다

    public Define.EventType Type { get; set; }
    public RoomSymbol Symbol { get; set; } = null;

    //갖고있는 방향-문 딕셔너리
    public Dictionary<Define.Direction, Door> Doors { get; set; } = new Dictionary<Define.Direction, Door>((int)Define.Direction.Count);

    private void OnTriggerEnter(Collider collider)
    {
        IsEntered = true;

        if (IsCleared == false)
        { 
            //처음 들어올 때 문 닫기
            ActivateDoors(false);

            //적이 없는 방이면 그냥 클리어 처리
            if(Type != Define.EventType.Enemy && Type != Define.EventType.Event)
            {
                IsCleared = true;
                ActivateDoors(true);
                StageManager.Instance.NotifyRoomClear();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        //심볼이 없거나 파괴되었을 때, 혹은 적이나 보스가 아닐 때 방 클리어 처리 (나중에 교체 가능)
        if (Symbol == null && IsCleared == false)
        {
            IsCleared = true;
            ActivateDoors(true);
            StageManager.Instance.NotifyRoomClear();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IsEntered = false;  
    }

    //멤버 변수 초기화
    public void Init(Define.EventType type)
    {
        //Type 지정
        Type = type;
        //Symbol 소환

        int offset; 

        switch (type)
        {
            //심볼을 소환할 때, 각각 랜덤으로 인덱스를 배정하고, 인덱스에 따라 다른 대화 내용과 전투, 아이템 획득 등을 하게 할 예정
            case Define.EventType.Enemy:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/EnemySymbol", transform).AddComponent<EnemySymbol>();
                
                int choice = Random.Range(0, 2); //일반 잡몹 대화문일지 보스 잡몹 대화문일지
                offset = choice == 0 ? 0 : (int)StageManager.Instance.Theme;
                Symbol.Init(offset, type); 

                break;
            case Define.EventType.Rest:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/RestSymbol", transform)
                    .AddComponent<RestSymbol>();
                Symbol.Init(Define.REST_INDEX, type);
                break;
            case Define.EventType.Shop:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/ShopSymbol", transform)
                    .AddComponent<ShopSymbol>();
                Symbol.Init(Define.SHOP_INDEX, type);
                break;
            case Define.EventType.Event:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/EventSymbol", transform)
                    .AddComponent<EventSymbol>();
                offset = Random.Range(1, 8);
                Symbol.Init(Define.EVENT_INDEX + offset, type);
                break;
            case Define.EventType.Boss:
                Symbol = AssetLoader.Instance.Instantiate($"Prefabs/RoomSymbol/BossSymbol", transform)
                    .AddComponent<BossSymbol>();
                Symbol.Init(Define.BOSS_INDEX + (int)StageManager.Instance.Theme - 1, type);
                break;
            default:
                return;
        }
        Symbol.transform.position = new Vector3(0, 1, 0);
    }

    //이 방에서 특정 방향에 있는 문을 목적지 위치와 연결하고, 소유한 Doors 딕셔너리에 추가한다.
    public void SetDoorsDictionary(Define.Direction direction, Room destination)
    {
        //일단 소유한 문 딕셔너리에 <방향-문> 추가
        Doors[direction] = transform.Find("Doors").GetChild((int)direction).GetComponent<Door>();
        Doors[direction].gameObject.SetActive(true);

        //현재 방 중심에서 문까지 가는 벡터의 반대 벡터
        Vector3 oppositeVector = (transform.position - Doors[direction].transform.position) * 0.8f;
        oppositeVector.y = 0;

        //문의 목적지가 될 벡터값 설정
        Doors[direction].Destination = destination.transform.position + oppositeVector;    
    }

    //문들 전부 활성화/비활성화
    public void ActivateDoors(bool isActivated)
    {
        foreach(KeyValuePair<Define.Direction, Door> door in Doors) 
        {
            door.Value.gameObject.SetActive(isActivated);
        }
    }

}
