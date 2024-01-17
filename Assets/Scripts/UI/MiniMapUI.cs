using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField]
    //맵이 붙을 오브젝트
    public GameObject miniMap;

    private void Awake()
    {
        UpdateMiniMap();
    }

    private void OnEnable()
    {
        UpdateMiniMap();
        InputActions.keyActions.UI.MiniMap.started += Close;
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.MiniMap.started -= Close;
    }

    public void UpdateMiniMap()
    {
        for(int i = 0; i < Stage.Instance.MapRooms.Count; i++)
        {
            //표시할 방 정보들 가져오기
            Room room = Stage.Instance.MapRooms[i]; 
            Vector2 roomPoint = Stage.Instance.MapRoomPoints[i];
            List<int> roomEdge = Stage.Instance.MapRoomEdges[i];

            //미니맵 방 오브젝트 소환하고, 방의 좌표에 따라 위치 지정
            MiniMapRoom miniRoom = AssetLoader.Instance.Instantiate("Prefabs/UIElement/MiniMapRoom", miniMap.transform).GetComponent<MiniMapRoom>();
            miniRoom.GetComponent<RectTransform>().anchoredPosition = new Vector2(roomPoint.x * 100 , roomPoint.y * 100);

            //방의 타입에 따라 색깔 바꾸기
            switch(room.Type)
            {
                case Define.EventType.Start:
                    miniRoom.roomBody.color = Color.gray;
                    break;
                case Define.EventType.Shop:
                    miniRoom.roomBody.color = Define.HexToColor("EDF13C");
                    break;
                case Define.EventType.Rest:
                    miniRoom.roomBody.color = Define.HexToColor("8AFF9A");
                    break;
                case Define.EventType.Event:
                    miniRoom.roomBody.color = Define.HexToColor("B05EEA");
                    break;
                case Define.EventType.Enemy:
                    miniRoom.roomBody.color = Define.HexToColor("768AFD");
                    break;
                case Define.EventType.Boss:
                    miniRoom.roomBody.color = Define.HexToColor("FF5050");
                    break;
            }

            //연결된 방이 있으면 그 방향에만 선을 그리기
            for (int j = 0; j < roomEdge.Count; j++)
            {
                if (roomEdge[j] != -1)
                {
                    miniRoom.Doors[j].SetActive(true);
                }
            }

            //클리어하지 않은 + 들어가지 않은 방은 미니맵상에서 비활성화
            if(!room.IsCleared && !room.IsEntered)
            {
                miniRoom.gameObject.SetActive(false);
            }

            //현재 들어간 방일 경우 플레이어 아이콘 활성화
            if(room.IsEntered)
            {
                miniRoom.player.SetActive(true);
            }
        }
    }

    public void ExitClick()
    {
        UIManager.Instance.HideUI("MiniMapUI");
    }

    private void Close(InputAction.CallbackContext context)
    {
        ExitClick();
    }
}
