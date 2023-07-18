using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera BattleCamera;
    RectTransform rectTransform;
    public Vector3 DefaultPos;

    public float fixedZValue = 43.25f;

    public void Start()
    {
        BattleCamera = GameObject.Find("BattleCameraParent").transform.GetChild(0).GetComponent<Camera>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = this.transform.position;
    }

    //드래그할 때 마우스를 따라 움직임

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, fixedZValue);
        this.transform.position = BattleCamera.ScreenToWorldPoint(mousePos);
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public void OnEndDrag(PointerEventData eventData) 
    {
        this.transform.position = DefaultPos;
        this.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }
}
