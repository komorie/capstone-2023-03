using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField]
    //���� ���� ������Ʈ
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
            //ǥ���� �� ������ ��������
            Room room = Stage.Instance.MapRooms[i]; 
            Vector2 roomPoint = Stage.Instance.MapRoomPoints[i];
            List<int> roomEdge = Stage.Instance.MapRoomEdges[i];

            //�̴ϸ� �� ������Ʈ ��ȯ�ϰ�, ���� ��ǥ�� ���� ��ġ ����
            MiniMapRoom miniRoom = AssetLoader.Instance.Instantiate("Prefabs/UIElement/MiniMapRoom", miniMap.transform).GetComponent<MiniMapRoom>();
            miniRoom.GetComponent<RectTransform>().anchoredPosition = new Vector2(roomPoint.x * 100 , roomPoint.y * 100);

            //���� Ÿ�Կ� ���� ���� �ٲٱ�
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

            //����� ���� ������ �� ���⿡�� ���� �׸���
            for (int j = 0; j < roomEdge.Count; j++)
            {
                if (roomEdge[j] != -1)
                {
                    miniRoom.Doors[j].SetActive(true);
                }
            }

            //Ŭ�������� ���� + ���� ���� ���� �̴ϸʻ󿡼� ��Ȱ��ȭ
            if(!room.IsCleared && !room.IsEntered)
            {
                miniRoom.gameObject.SetActive(false);
            }

            //���� �� ���� ��� �÷��̾� ������ Ȱ��ȭ
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
