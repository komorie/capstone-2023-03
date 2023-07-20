using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//맵 배열과, 스테이지의 특정 시점에서 실행될 이벤트를 싱글톤으로 저장
public class StageManager : Singleton<StageManager>
{

    private int roomInterval = 20; //방 사이 간격
    private Vector2 roomSize = new Vector2(10, 10); //방 크기
    private List<int> stageThemeList;

    public int StageLevel { get; set; } = 0; //현재 스테이지 레벨
    public Define.ThemeType StageTheme { get; set; } //현재 스테이지의 테마
    private int StageRoomCount { get; set; } //현재 스테이지의 방 숫자
    private Dictionary<int, Define.EventType> StageExRoomIndexes { get; set; } //현재 스테이지에서 특수 방이 될 번호들

    public List<Room> MapRooms { get; set; } //현재 스테이지 방 컴포넌트 배열
    public List<Vector2> MapRoomPoints { get; set; } //현재 스테이지 방 좌표 배열
    public List<List<int>> MapRoomEdges { get; set; } //현재 스테이지 방 연결관계 배열

    public event Action OnRoomClear;
    public event Action OnStageClear;

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
        OnStageClear?.Invoke();
    }


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void CreateStage()
    {
        StageLevel = 0;
        stageThemeList = Define.GenerateRandomNumbers((int)Define.ThemeType.Pirate, (int)Define.ThemeType.Final, 3); // 1 ~ 4 중에서 나올 스테이지 3개 지정
        CreateMap();
    }

    //이전 맵 파괴
    public void DestroyMap()
    {
        for (int i = 0; i < MapRooms.Count; i++)
        {
            AssetLoader.Instance.Destroy(MapRooms[i].gameObject);
        }

        MapRooms = null;
        MapRoomPoints = null;
        MapRoomEdges = null;
        StageExRoomIndexes = null;

        if(StageLevel == 4)
        {
            //엔딩 씬 호출
            SceneLoader.Instance.LoadScene("EdScene");
        }
    }

    //맵 생성
    public void CreateMap()
    {
        StageLevel += 1;

        switch (StageLevel)
        {
            case < 4:
                StageRoomCount = StageLevel + 11;
                StageTheme = (Define.ThemeType)stageThemeList[StageLevel - 1]; //현재 스테이지의 테마
                CreateSpecialRoomIndexes(); //특별한 방의 위치 지정
                break;
            case 4:
                //최종보스
                StageRoomCount = 1;
                StageTheme = Define.ThemeType.Final;
                break;
            default:
                break;
        }

        CreateMapRooms(); //방 생성 후 방 유형 지정
        CreateMapRoomPointsAndEdges(); //방들의 위치와 연결상태를 나타낸 그래프 생성
        PlaceMapRooms(); // 그래프대로 방 위치를 배치함
    }

    //특별 방이 저장될 인덱스들 생성
    private void CreateSpecialRoomIndexes()
    {
        int minRoomIndex = 1;
        int maxRoomIndex = StageRoomCount - 1;

        StageExRoomIndexes = new Dictionary<int, Define.EventType>();

        List<int> uniqueIndexes = Define.GenerateRandomNumbers(minRoomIndex, maxRoomIndex, 5);

        // 상점의 위치를 저장
        StageExRoomIndexes[uniqueIndexes[0]] = Define.EventType.Shop;

        // 휴식의 위치를 저장
        StageExRoomIndexes[uniqueIndexes[1]] = Define.EventType.Rest;

        // 이벤트의 위치를 저장
        StageExRoomIndexes[uniqueIndexes[2]] = Define.EventType.Event;
        StageExRoomIndexes[uniqueIndexes[3]] = Define.EventType.Event;
    }

    //특정 인덱스의 방이 무슨 방인지 타입을 리턴
    Define.EventType SelectRoomType(int node)
    {
        if (node == StageRoomCount - 1) return Define.EventType.Boss;
        if (node == 0) return Define.EventType.Start;

        //특별한 방이 있나 확인 후 가져오기
        if (StageExRoomIndexes.ContainsKey(node))
        {
            return StageExRoomIndexes[node];
        }

        return Define.EventType.Enemy;
    }

    //맵의 방 컴포넌트들을 참조하는 MapRooms 배열 생성
    private void CreateMapRooms()
    {
        MapRooms = new List<Room>(StageRoomCount);

        for (int node = 0; node < StageRoomCount; node++)
        {
            //방 타입 고르기
            Define.EventType roomType = SelectRoomType(node);

            //방 게임오브젝트 생성
            Room currentRoom = AssetLoader.Instance.Instantiate($"Prefabs/Room/Room{(int)StageTheme}", Map.transform).AddComponent<Room>();
            currentRoom.name = $"Room{node}";

            //멤버 변수 초기화
            currentRoom.Init(roomType);

            //배열에 추가
            MapRooms.Add(currentRoom);
        }

        if (StageLevel == 4) { MapRooms[0].Symbol.transform.position += new Vector3(-1.5f, 0, 1.5f); }
    }

    //큐를 이용해서 방의 위치와 방끼리의 연결 관계를 나타낸 그래프를 생성하는 bfs 변형 함수
    void CreateMapRoomPointsAndEdges()
    {
        int currentRoomCount = 1;
        int currentRoomIndex = 0;

        MapRoomPoints = new List<Vector2>(StageRoomCount);
        MapRoomEdges = InitializeMapRoomEdges();
        Queue<Vector2> roomQueue = new Queue<Vector2>();
        HashSet<Vector2> visitedRoomPoints = new HashSet<Vector2>(StageRoomCount);

        //시작점 추가
        MapRoomPoints.Add(Vector2.zero);
        visitedRoomPoints.Add(Vector2.zero);
        roomQueue.Enqueue(Vector2.zero);

        //맵 그래프 생성
        while (currentRoomCount < StageRoomCount)
        {
            //방 위치 가져오기
            Vector2 currentRoomPoint = roomQueue.Dequeue();

            //다음으로 탐색할 랜덤한 수만큼의 랜덤한 방향 생성.
            int nearRoomCount = Random.Range(1, 5);
            List<int> nearRoomDirections = Define.GenerateRandomNumbers(0, 4, nearRoomCount);

            for (int dir = 0; dir < nearRoomDirections.Count; dir++)
            {
                //방을 다 채웠을 경우 종료
                if (currentRoomCount >= StageRoomCount) break;

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
                        MapRoomPoints.Add(newRoomPoint);
                        MapRoomEdges[currentRoomIndex][dir] = currentRoomCount - 1;
                        MapRoomEdges[currentRoomCount - 1][(dir + 2) % 4] = currentRoomIndex;
                    }
                }
            }

            //드물게 갈 길이 막혔거나, 운이 좋지 않아서 방문한 곳에만 다시 방문한 경우
            //방을 다 못채웠는데 큐가 비는 경우가 생긴다. 
            //여러 가지 예외 상황을 상정하려다 보니 랜덤한 위치에서 다시 탐색을 실시하는 게 편한것 같아서 이렇게 설계
            if (roomQueue.Count == 0 && currentRoomCount < StageRoomCount)
            {
                int nextRoomIndex = Random.Range(0, MapRoomPoints.Count);
                currentRoomIndex = nextRoomIndex;
                roomQueue.Enqueue(MapRoomPoints[nextRoomIndex]);
            }
            else
            {
                currentRoomIndex++;
            }
        }
    }

    //실제 Room 게임오브젝트의 위치를 생성한 그래프에 맞게 조정
    void PlaceMapRooms()
    {
        for (int node = 0; node < MapRooms.Count; node++)
        {
            //mapRoomPoints의 좌표에 맞게 mapRooms에 속한 Room 오브젝트의 위치 조정
            MapRooms[node].transform.position = new Vector3(
                MapRoomPoints[node].x * (roomSize.x + roomInterval),
                0,
                MapRoomPoints[node].y * (roomSize.y + roomInterval)
            );
        }

        for (int node = 0; node < MapRooms.Count; node++)
        {
            //Room마다 연결된 방이 있는지 확인
            for (int dir = 0; dir < MapRoomEdges[node].Count; dir++)
            {
                //해당 방향에 연결된 방이 있는 경우
                if (MapRoomEdges[node][dir] != -1)
                {
                    //서로를 잇는 문 추가
                    MapRooms[node].SetDoor((Define.Direction)dir, MapRooms[MapRoomEdges[node][dir]]);
                }
            }
        }
    }

    //2차원 MapRoomEdges 배열 초기화
    List<List<int>> InitializeMapRoomEdges()
    {
        int dir = (int)Define.Direction.Count;

        List<List<int>> roomEdges = new List<List<int>>(StageRoomCount * dir);

        for (int i = 0; i < StageRoomCount; i++)
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
