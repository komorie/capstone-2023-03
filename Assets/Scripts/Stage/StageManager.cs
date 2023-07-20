using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//�� �迭��, ���������� Ư�� �������� ����� �̺�Ʈ�� �̱������� ����
public class StageManager : Singleton<StageManager>
{

    private int roomInterval = 20; //�� ���� ����
    private Vector2 roomSize = new Vector2(10, 10); //�� ũ��
    private List<int> stageThemeList;

    public int StageLevel { get; set; } = 0; //���� �������� ����
    public Define.ThemeType StageTheme { get; set; } //���� ���������� �׸�
    private int StageRoomCount { get; set; } //���� ���������� �� ����
    private Dictionary<int, Define.EventType> StageExRoomIndexes { get; set; } //���� ������������ Ư�� ���� �� ��ȣ��

    public List<Room> MapRooms { get; set; } //���� �������� �� ������Ʈ �迭
    public List<Vector2> MapRoomPoints { get; set; } //���� �������� �� ��ǥ �迭
    public List<List<int>> MapRoomEdges { get; set; } //���� �������� �� ������� �迭

    public event Action OnRoomClear;
    public event Action OnStageClear;

    //���� ��ġ�� �θ� ���ӿ�����Ʈ
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


    //�������鿡�� ���� ���� Ŭ����Ǿ����� �˸��� �Լ�.
    public void NotifyRoomClear()
    {
        OnRoomClear?.Invoke();
    }

    //�������鿡�� ���� ���������� Ŭ����Ǿ����� �˸��� �Լ�.
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
        stageThemeList = Define.GenerateRandomNumbers((int)Define.ThemeType.Pirate, (int)Define.ThemeType.Final, 3); // 1 ~ 4 �߿��� ���� �������� 3�� ����
        CreateMap();
    }

    //���� �� �ı�
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
            //���� �� ȣ��
            SceneLoader.Instance.LoadScene("EdScene");
        }
    }

    //�� ����
    public void CreateMap()
    {
        StageLevel += 1;

        switch (StageLevel)
        {
            case < 4:
                StageRoomCount = StageLevel + 11;
                StageTheme = (Define.ThemeType)stageThemeList[StageLevel - 1]; //���� ���������� �׸�
                CreateSpecialRoomIndexes(); //Ư���� ���� ��ġ ����
                break;
            case 4:
                //��������
                StageRoomCount = 1;
                StageTheme = Define.ThemeType.Final;
                break;
            default:
                break;
        }

        CreateMapRooms(); //�� ���� �� �� ���� ����
        CreateMapRoomPointsAndEdges(); //����� ��ġ�� ������¸� ��Ÿ�� �׷��� ����
        PlaceMapRooms(); // �׷������ �� ��ġ�� ��ġ��
    }

    //Ư�� ���� ����� �ε����� ����
    private void CreateSpecialRoomIndexes()
    {
        int minRoomIndex = 1;
        int maxRoomIndex = StageRoomCount - 1;

        StageExRoomIndexes = new Dictionary<int, Define.EventType>();

        List<int> uniqueIndexes = Define.GenerateRandomNumbers(minRoomIndex, maxRoomIndex, 5);

        // ������ ��ġ�� ����
        StageExRoomIndexes[uniqueIndexes[0]] = Define.EventType.Shop;

        // �޽��� ��ġ�� ����
        StageExRoomIndexes[uniqueIndexes[1]] = Define.EventType.Rest;

        // �̺�Ʈ�� ��ġ�� ����
        StageExRoomIndexes[uniqueIndexes[2]] = Define.EventType.Event;
        StageExRoomIndexes[uniqueIndexes[3]] = Define.EventType.Event;
    }

    //Ư�� �ε����� ���� ���� ������ Ÿ���� ����
    Define.EventType SelectRoomType(int node)
    {
        if (node == StageRoomCount - 1) return Define.EventType.Boss;
        if (node == 0) return Define.EventType.Start;

        //Ư���� ���� �ֳ� Ȯ�� �� ��������
        if (StageExRoomIndexes.ContainsKey(node))
        {
            return StageExRoomIndexes[node];
        }

        return Define.EventType.Enemy;
    }

    //���� �� ������Ʈ���� �����ϴ� MapRooms �迭 ����
    private void CreateMapRooms()
    {
        MapRooms = new List<Room>(StageRoomCount);

        for (int node = 0; node < StageRoomCount; node++)
        {
            //�� Ÿ�� ����
            Define.EventType roomType = SelectRoomType(node);

            //�� ���ӿ�����Ʈ ����
            Room currentRoom = AssetLoader.Instance.Instantiate($"Prefabs/Room/Room{(int)StageTheme}", Map.transform).AddComponent<Room>();
            currentRoom.name = $"Room{node}";

            //��� ���� �ʱ�ȭ
            currentRoom.Init(roomType);

            //�迭�� �߰�
            MapRooms.Add(currentRoom);
        }

        if (StageLevel == 4) { MapRooms[0].Symbol.transform.position += new Vector3(-1.5f, 0, 1.5f); }
    }

    //ť�� �̿��ؼ� ���� ��ġ�� �波���� ���� ���踦 ��Ÿ�� �׷����� �����ϴ� bfs ���� �Լ�
    void CreateMapRoomPointsAndEdges()
    {
        int currentRoomCount = 1;
        int currentRoomIndex = 0;

        MapRoomPoints = new List<Vector2>(StageRoomCount);
        MapRoomEdges = InitializeMapRoomEdges();
        Queue<Vector2> roomQueue = new Queue<Vector2>();
        HashSet<Vector2> visitedRoomPoints = new HashSet<Vector2>(StageRoomCount);

        //������ �߰�
        MapRoomPoints.Add(Vector2.zero);
        visitedRoomPoints.Add(Vector2.zero);
        roomQueue.Enqueue(Vector2.zero);

        //�� �׷��� ����
        while (currentRoomCount < StageRoomCount)
        {
            //�� ��ġ ��������
            Vector2 currentRoomPoint = roomQueue.Dequeue();

            //�������� Ž���� ������ ����ŭ�� ������ ���� ����.
            int nearRoomCount = Random.Range(1, 5);
            List<int> nearRoomDirections = Define.GenerateRandomNumbers(0, 4, nearRoomCount);

            for (int dir = 0; dir < nearRoomDirections.Count; dir++)
            {
                //���� �� ä���� ��� ����
                if (currentRoomCount >= StageRoomCount) break;

                //���⿡ �����ϴ� ���Ͱ��� ���� ���� Ž���� ���� ���Ͱ� ����
                Vector2 newRoomPoint = currentRoomPoint + Define.directionVectors[(Define.Direction)dir];
                
                if (newRoomPoint.x >= -5 && newRoomPoint.x <= 5 && newRoomPoint.y >= -5 && newRoomPoint.y <= 5) // ���� �ʹ� ���������°� ����
                {
                    //�湮���� ���� ���� ��� �湮 ó��
                    if (!visitedRoomPoints.Contains(newRoomPoint))
                    {
                        visitedRoomPoints.Add(newRoomPoint);
                        roomQueue.Enqueue(newRoomPoint);
                        currentRoomCount++;

                        //�� ��ġ, �ش� ��� ����� �ٸ� �� �߰�
                        //roomEdges�� [�ε���1: ���� ��ȣ, �ε���2: ����] ���ٰ� �� ���⿡ [����� �ٸ� ���� �ε���]�� ����
                        MapRoomPoints.Add(newRoomPoint);
                        MapRoomEdges[currentRoomIndex][dir] = currentRoomCount - 1;
                        MapRoomEdges[currentRoomCount - 1][(dir + 2) % 4] = currentRoomIndex;
                    }
                }
            }

            //�幰�� �� ���� �����ų�, ���� ���� �ʾƼ� �湮�� ������ �ٽ� �湮�� ���
            //���� �� ��ä���µ� ť�� ��� ��찡 �����. 
            //���� ���� ���� ��Ȳ�� �����Ϸ��� ���� ������ ��ġ���� �ٽ� Ž���� �ǽ��ϴ� �� ���Ѱ� ���Ƽ� �̷��� ����
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

    //���� Room ���ӿ�����Ʈ�� ��ġ�� ������ �׷����� �°� ����
    void PlaceMapRooms()
    {
        for (int node = 0; node < MapRooms.Count; node++)
        {
            //mapRoomPoints�� ��ǥ�� �°� mapRooms�� ���� Room ������Ʈ�� ��ġ ����
            MapRooms[node].transform.position = new Vector3(
                MapRoomPoints[node].x * (roomSize.x + roomInterval),
                0,
                MapRoomPoints[node].y * (roomSize.y + roomInterval)
            );
        }

        for (int node = 0; node < MapRooms.Count; node++)
        {
            //Room���� ����� ���� �ִ��� Ȯ��
            for (int dir = 0; dir < MapRoomEdges[node].Count; dir++)
            {
                //�ش� ���⿡ ����� ���� �ִ� ���
                if (MapRoomEdges[node][dir] != -1)
                {
                    //���θ� �մ� �� �߰�
                    MapRooms[node].SetDoor((Define.Direction)dir, MapRooms[MapRoomEdges[node][dir]]);
                }
            }
        }
    }

    //2���� MapRoomEdges �迭 �ʱ�ȭ
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
