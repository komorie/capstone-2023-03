using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Canvas�� ī�޶� BattleCamera�� ����, �׷� ī�޶� ���ٸ� ���� ī�޶�� ����
        Canvas canvas = GetComponent<Canvas>();
        Camera battleCamera = GameObject.Find("BattleCameraParent").transform.GetChild(0).GetComponent<Camera>();
        Camera mainCamera = Camera.main;
        if (battleCamera != null)
        {
            canvas.worldCamera = battleCamera;
            mainCamera.gameObject.SetActive(false);
            battleCamera.gameObject.SetActive(true);
        }
        else
        {
            canvas.worldCamera = mainCamera;
        }
    }

    // Update is called once per frame
}
