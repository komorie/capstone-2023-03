using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapRoom : MonoBehaviour
{
/*    //클릭으로 이동할 방
    public Room connectedRoom;

    //마우스 올리면 색이 바뀔 테두리 이미지
    [SerializeField]
    public Image roomOutline;*/

    //룸 타입에 따라 색이 바뀔 몸체 이미지
    [SerializeField]
    public Image roomBody;

    //연결 방향에 따라 표시/비표시될 선
    [SerializeField]
    public GameObject[] Doors = new GameObject[4];

    //캐릭터 아이콘
    [SerializeField]
    public GameObject player;

}
