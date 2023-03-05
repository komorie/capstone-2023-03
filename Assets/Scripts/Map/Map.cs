using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int stage = 0;
    private int roomCount; //���� �� ����
    private int roomInterval = 2; //���� �� ���� ����
    
    private Vector2 roomSize = new Vector2(10, 10); //���� ���� �� ũ��;
    
    private List<Room> mapRooms; //�ʿ� �ִ� ����� �迭
    private List<Vector2> mapRoomPoints; //��� ��ǥ�� �迭
    private List<List<int>> mapRoomEdges; //��� ������ ���� ���踦 ��Ÿ�� �迭
    private List<int> specialRoomIndexes; //Ư�� ���� �� �迭 �ε����� ����

    private void Awake()
    {
        CreateStage();
    }

    private void OnEnable()
    {
        LevelManager.Instance.LevelClear += CreateStage;
    }

    public void OnDisable()
    {
        LevelManager.Instance.LevelClear -= CreateStage;
    }

    //������ �°� �������� ����
    public void CreateStage()
    {
        //���� �������� �ı�
        if (stage != 0)
        {
            for (int i = 0; i < mapRooms.Count; i++)
            {
                Destroy(mapRooms[i].gameObject);
            }
            mapRooms = null;
            mapRoomPoints = null;
            mapRoomEdges = null;
        }

        if (stage < 3)
        {
            stage += 1;
            roomCount = stage * 3 + 10;
            specialRoomIndexes = Define.GenerateRandomNumbers(2, roomCount - 1, (int)Define.RoomEventType.Count - 3);
            CreateMapRooms();
            CreateMapRoomPointsAndEdges();
            PlaceMapRooms();
        }
        else if (stage == 3)
        {
            //��������
        }
        else
        {
            //����
        }
    }

    //�����ϱ�
    private Dictionary<Define.RoomEventType, int> CreateSpecialRoomIndexes()
    {

        return null;
    }

    //Ư�� ��ȣ�� ���� ���� ������ Ÿ���� ����
    Define.RoomEventType SelectRoomType(int node)
    {
        if (node == 0) return Define.RoomEventType.Start;
        if (node == roomCount - 1) return Define.RoomEventType.Boss; 

        for (int i = 0; i < specialRoomIndexes.Count; i++)
        {
            if (specialRoomIndexes[i] == node)
            {
                return (Define.RoomEventType)(i + 3);
            }
        }
        return Define.RoomEventType.Normal;
    }

    //2���� mapRoomEdges �迭 �ʱ�ȭ
   List<List<int>> InitializeMapRoomEdges()
    {
        int dir = (int)Define.Direction.Count;

        List<List<int>> roomEdges = new List<List<int>>(roomCount * dir);

        for (int i = 0 ; i < roomCount; i++)
        {
            roomEdges.Add(new List<int>(dir));

            for(int j = 0; j < dir; j++)
            {
                roomEdges[i].Add(-1);
            }
        }
        return roomEdges;
    }

    //���� ���� �����ϴ� mapRooms �迭 ����
    private void CreateMapRooms()
    {
        mapRooms = new List<Room>(roomCount);

        for (int node = 0; node < roomCount; node++)
        {
            //�� Ÿ�� ����
            Define.RoomEventType roomType = SelectRoomType(node);

            //�� ���ӿ�����Ʈ ����
            Room currentRoom = Instantiate(
                AssetLoader.Instance.Load($"Prefabs/Room/{roomType}Room"),
                transform
            ).AddComponent<Room>();
            currentRoom.name = $"Room{node}";

            //��� ���� �ʱ�ȭ
            currentRoom.Init(roomType);

            //�迭�� �߰�
            mapRooms.Add(currentRoom);
        }
    }

    //ť�� �̿��ؼ� ���� ��ġ�� ���� ���踦 ������ ����Ʈ�� �����ϴ� bfs ����
    void CreateMapRoomPointsAndEdges()
    {
        int currentRoomCount = 1;
        int currentRoomIndex = 0;

        mapRoomPoints = new List<Vector2>(roomCount);
        mapRoomEdges = InitializeMapRoomEdges();
        Queue<Vector2> roomQueue = new Queue<Vector2>();
        HashSet<Vector2> visitedRoomPoints = new HashSet<Vector2>(roomCount);

        //������ �߰�
        mapRoomPoints.Add(Vector2.zero);
        visitedRoomPoints.Add(Vector2.zero);
        roomQueue.Enqueue(Vector2.zero);

        //�� �׷��� ����
        while (currentRoomCount < roomCount)
        {
            //�� ��ġ ��������
            Vector2 currentRoomPoint = roomQueue.Dequeue();

            //�������� Ž���� ������ ����ŭ�� ������ ���� ����.
            int nearRoomCount = Random.Range(1, 5);
            List<int> nearRoomDirections = Define.GenerateRandomNumbers(0, 4, nearRoomCount);

            foreach (int dir in nearRoomDirections)
            {
                //���� �� ä���� ��� ����
                if (currentRoomCount >= roomCount) break;

                //���⿡ �����ϴ� ���Ͱ��� ���� ���� Ž���� ���� ���Ͱ� ����
                Vector2 newRoomPoint = currentRoomPoint + Define.directionVectors[(Define.Direction)dir];
                
                //�湮���� ���� ���� ��� �湮 ó��
                if (!visitedRoomPoints.Contains(newRoomPoint))
                {
                    visitedRoomPoints.Add(newRoomPoint);
                    roomQueue.Enqueue(newRoomPoint);
                    currentRoomCount++;

                    //�� ��ġ, �ش� ��� ����� �ٸ� �� �߰�
                    //roomEdges�� [�ε���1: ���� ��ȣ, �ε���2: ����] ���ٰ� �� ���⿡ [����� �ٸ� ���� �ε���]�� ����
                    mapRoomPoints.Add(newRoomPoint);
                    mapRoomEdges[currentRoomIndex][dir] = currentRoomCount - 1;
                    mapRoomEdges[currentRoomCount - 1][(dir + 2) % 4] = currentRoomIndex;
                }
            }

            //�幰�� �� ���� �����ų�, ���� ���� �ʾƼ� �湮�� ������ �ٽ� �湮�� ���
            //���� �� ��ä���µ� ť�� ��� ��찡 �����. �̶� ���ݱ��� ������ �� �� ������ ��ġ�� ť�� �ְ� �ٽ� Ž���� �ǽ�.
            //�ƴϸ� ���� �����ϰ� �迭�� �ֱ�
            if (roomQueue.Count == 0 && currentRoomCount < roomCount)
            {
                int nextRoomIndex = Random.Range(0, mapRoomPoints.Count);
                currentRoomIndex = nextRoomIndex;
                roomQueue.Enqueue(mapRoomPoints[nextRoomIndex]);
            }
            else
            {
                currentRoomIndex++;
            }
        }
    }

    //���� Room ������Ʈ�� ��ġ ���� + �� ����
    void PlaceMapRooms()
    {
        for (int node = 0; node < mapRooms.Count; node++)
        {
            //mapRoomPoints�� ��ǥ�� �°� mapRooms�� ���� Room ������Ʈ�� ��ġ ����
            mapRooms[node].transform.position = new Vector3(
                mapRoomPoints[node].x * (roomSize.x + roomInterval),
                0,
                mapRoomPoints[node].y * (roomSize.y + roomInterval)
            );
        }

        for (int node = 0; node < mapRooms.Count; node++)
        {
            //Room���� ����� ���� �ִ��� Ȯ��
            for (int dir = 0; dir < mapRoomEdges[node].Count; dir++)
            {
                //�ش� ���⿡ ����� ���� �ִ� ���
                if (mapRoomEdges[node][dir] != -1)
                {
                    //���θ� �մ� �� �߰�
                    mapRooms[node].SetDoorsDictionary((Define.Direction)dir, mapRooms[mapRoomEdges[node][dir]]);
                }
            }
        }
    }
}
