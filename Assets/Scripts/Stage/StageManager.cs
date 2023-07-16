using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

//맵 배열과, 스테이지의 특정 시점에서 실행될 이벤트를 싱글톤으로 저장
public class StageManager : Singleton<StageManager>
{

    private readonly int roomInterval = 20; //방 사이 간격
    private Vector2 roomSize = new Vector2(10, 10); //방 크기

    private List<int> ThemeIndexes { get; set; }

    public int Stage { get; set; } = 0; //현재 스테이지 레벨
    public Define.ThemeType Theme { get; set; } //현재 스테이지의 테마

    private int roomCount; //현재 스테이지의 방 숫자

    private Dictionary<int, Define.EventType> specialRoomIndexes; //현재 스테이지에서 특수 방이 될 번호들 모음

    public List<Room> Rooms { get; set; } //현재 스테이지 방 컴포넌트 배열
    public List<Vector2> RoomPoints { get; set; } //현재 스테이지 방 좌표 배열
    public List<List<int>> RoomEdges { get; set; } //현재 스테이지 방 연결관계 배열

    public event Action OnRoomClear;
    public event Action OnLevelClear;

    //맵이 배치될 부모 게임오브젝트
    public GameObject Map
    {
        get
        {
            GameObject map = GameObject.Find("Map");
            if (map == null)
            {
                map = new GameObject();
                map.name = "Map";
            }
            return map;
        }
    }


    //옵저버들에게 현재 방이 클리어되었음을 알리는 함수.
    public void NotifyRoomClear()
    {
        OnRoomClear?.Invoke();
    }

    //옵저버들에게 현재 스테이지가 클리어되었음을 알리는 함수.
    public void NotifyLevelClear()
    {
        DestroyMap();
        CreateMap();
        OnLevelClear?.Invoke();
    }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void CreateStage()
    {
        Stage = 0;
        CreateMap();
    }

    //이전 맵 파괴
    public void DestroyMap()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            AssetLoader.Instance.Destroy(Rooms[i]?.gameObject);
        }
        Rooms = null;
        RoomPoints = null;
        RoomEdges = null;
        specialRoomIndexes = null;

        if(Stage == 4)
        {
            //엔딩 씬 호출
            SceneLoader.Instance.LoadScene("EdScene");
        }
    }

    //맵 생성
    public void CreateMap()
    {

        if(Stage == 0)
        {
            ThemeIndexes = Define.GenerateRandomNumbers((int)Define.ThemeType.Pirate, (int)Define.ThemeType.Final, 3); // 1 ~ 4 중에서 나올 스테이지 3개 지정
        }

        Stage += 1;

        //스테이지 생성
        if (Stage < 4)
        {
            roomCount = Stage + 11;
            Theme = (Define.ThemeType)ThemeIndexes[Stage - 1]; //현재 스테이지의 테마
            CreateSpecialRoomIndexes(); //특별한 방의 위치 지정
            CreateMapRooms(); //방 생성 후 방 유형 지정
            CreateMapRoomPointsAndEdges(); //방들의 위치와 연결상태를 나타낸 그래프 생성
            PlaceMapRooms(); // 그래프대로 방 위치를 배치함
        }
        else if (Stage == 4)
        {
            //최종보스
            roomCount = 1;
            Theme = Define.ThemeType.Final;
            CreateMapRooms(); //방 생성 후 방 유형 지정
            CreateMapRoomPointsAndEdges(); //방들의 위치와 연결상태를 나타낸 그래프 생성
            PlaceMapRooms(); // 그래프대로 방 위치를 배치함
        }
    }

    //특별 방이 저장될 인덱스들 생성
    private void CreateSpecialRoomIndexes()
    {
        int minRoomIndex = 1;
        int maxRoomIndex = roomCount - 1;

        specialRoomIndexes = new Dictionary<int, Define.EventType>();

        List<int> uniqueIndexes = Define.GenerateRandomNumbers(minRoomIndex, maxRoomIndex, 5);

        // 상점의 위치를 저장
        specialRoomIndexes[uniqueIndexes[0]] = Define.EventType.Shop;

        // 휴식의 위치를 저장
        specialRoomIndexes[uniqueIndexes[1]] = Define.EventType.Rest;

        // 이벤트의 위치를 저장
        specialRoomIndexes[uniqueIndexes[2]] = Define.EventType.Event;
        specialRoomIndexes[uniqueIndexes[3]] = Define.EventType.Event;
    }

    //특정 인덱스의 방이 무슨 방인지 타입을 리턴
    Define.EventType SelectRoomType(int node)
    {
        if (node == roomCount - 1) return Define.EventType.Boss;
        if (node == 0) return Define.EventType.Start;

        //특별한 방이 있나 확인 후 가져오기
        if (specialRoomIndexes.ContainsKey(node))
        {
            return specialRoomIndexes[node];
        }

        return Define.EventType.Enemy;
    }

    //맵의 방을 저장하는 Rooms 배열 생성
    private void CreateMapRooms()
    {
        Rooms = new List<Room>(roomCount);

        for (int node = 0; node < roomCount; node++)
        {
            //방 타입 고르기
            Define.EventType roomType = SelectRoomType(node);

            //방 게임오브젝트 생성
            Room currentRoom = AssetLoader.Instance.Instantiate($"Prefabs/Room/Room{(int)Theme}", Map.transform).AddComponent<Room>();
            currentRoom.name = $"Room{node}";

            //멤버 변수 초기화
            currentRoom.Init(roomType);

            //배열에 추가
            Rooms.Add(currentRoom);
        }

        if (Stage == 4) { Rooms[0].Symbol.transform.position += new Vector3(-1.5f, 0, 1.5f); }
    }

    //큐를 이용해서 방의 위치와 연결 관계를 나타낸 그래프를 생성하는 bfs 변형 함수
    void CreateMapRoomPointsAndEdges()
    {
        int currentRoomCount = 1;
        int currentRoomIndex = 0;

        RoomPoints = new List<Vector2>(roomCount);
        RoomEdges = InitializeMapRoomEdges();
        Queue<Vector2> roomQueue = new Queue<Vector2>();
        HashSet<Vector2> visitedRoomPoints = new HashSet<Vector2>(roomCount);

        //시작점 추가
        RoomPoints.Add(Vector2.zero);
        visitedRoomPoints.Add(Vector2.zero);
        roomQueue.Enqueue(Vector2.zero);

        //맵 그래프 생성
        while (currentRoomCount < roomCount)
        {
            //방 위치 가져오기
            Vector2 currentRoomPoint = roomQueue.Dequeue();

            //다음으로 탐색할 랜덤한 수만큼의 랜덤한 방향 생성.
            int nearRoomCount = Random.Range(1, 5);
            List<int> nearRoomDirections = Define.GenerateRandomNumbers(0, 4, nearRoomCount);

            foreach (int dir in nearRoomDirections)
            {
                //방을 다 채웠을 경우 종료
                if (currentRoomCount >= roomCount) break;

                //방향에 대응하는 벡터값을 더해 새로 탐색할 방의 벡터값 리턴
                Vector2 newRoomPoint = currentRoomPoint + Define.directionVectors[(Define.Direction)dir];
                
                if (newRoomPoint.x >= -5 && newRoomPoint.x <= 5 && newRoomPoint.y >= -5 && newRoomPoint.y <= 5) // 맵이 너무 길쭉해지는거 방지
                {
                    //방문하지 않은 곳일 경우 방문 처리
                    if (!visitedRoomPoints.Contains(newRoomPoint))
                    {
                        visitedRoomPoints.Add(newRoomPoint);
                        roomQueue.Enqueue(newRoomPoint);
                        currentRoomCount++;

                        //방 위치, 해당 방과 연결된 다른 방 추가
                        //roomEdges는 [인덱스1: 방의 번호, 인덱스2: 방향] 에다가 그 방향에 [연결된 다른 방의 인덱스]를 저장
                        RoomPoints.Add(newRoomPoint);
                        RoomEdges[currentRoomIndex][dir] = currentRoomCount - 1;
                        RoomEdges[currentRoomCount - 1][(dir + 2) % 4] = currentRoomIndex;
                    }
                }
            }

            //드물게 갈 길이 막혔거나, 운이 좋지 않아서 방문한 곳에만 다시 방문한 경우
            //방을 다 못채웠는데 큐가 비는 경우가 생긴다. 이때 지금까지 생성한 방 중 랜덤한 위치를 큐에 넣고 다시 탐색을 실시.
            //아니면 방을 생성하고 배열에 넣기
            if (roomQueue.Count == 0 && currentRoomCount < roomCount)
            {
                int nextRoomIndex = Random.Range(0, RoomPoints.Count);
                currentRoomIndex = nextRoomIndex;
                roomQueue.Enqueue(RoomPoints[nextRoomIndex]);
            }
            else
            {
                currentRoomIndex++;
            }
        }
    }

    //실제 Room 오브젝트의 위치 조정 + 문 연결
    void PlaceMapRooms()
    {
        for (int node = 0; node < Rooms.Count; node++)
        {
            //mapRoomPoints의 좌표에 맞게 mapRooms에 속한 Room 오브젝트의 위치 조정
            Rooms[node].transform.position = new Vector3(
                RoomPoints[node].x * (roomSize.x + roomInterval),
                0,
                RoomPoints[node].y * (roomSize.y + roomInterval)
            );
        }

        for (int node = 0; node < Rooms.Count; node++)
        {
            //Room마다 연결된 방이 있는지 확인
            for (int dir = 0; dir < RoomEdges[node].Count; dir++)
            {
                //해당 방향에 연결된 방이 있는 경우
                if (RoomEdges[node][dir] != -1)
                {
                    //서로를 잇는 문 추가
                    Rooms[node].SetDoorsDictionary((Define.Direction)dir, Rooms[RoomEdges[node][dir]]);
                }
            }
        }
    }

    //2차원 RoomEdges 배열 초기화
    List<List<int>> InitializeMapRoomEdges()
    {
        int dir = (int)Define.Direction.Count;

        List<List<int>> roomEdges = new List<List<int>>(roomCount * dir);

        for (int i = 0; i < roomCount; i++)
        {
            roomEdges.Add(new List<int>(dir));

            for (int j = 0; j < dir; j++)
            {
                roomEdges[i].Add(-1);
            }
        }
        return roomEdges;
    }
}
