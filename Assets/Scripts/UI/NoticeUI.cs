using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeUI : MonoBehaviour
{
    [Header("SubNotice")]
    public GameObject noticeUI;
    public TextMeshProUGUI NoticeText;
    public Animator SubAni;

    private WaitForSeconds Delay1 = new WaitForSeconds(2.0f);
    private WaitForSeconds Delay2 = new WaitForSeconds(1.0f);

    // Start is called before the first frame update
    void Start()
    {
        noticeUI.SetActive(false);
    }

    // Update is called once per frame
    public void ShowNotice(string text)
    {
        noticeUI.SetActive(false);
        NoticeText.text = text;
        StopAllCoroutines();
        StartCoroutine(SubDelay());
    }

    IEnumerator SubDelay()
    {
        noticeUI.SetActive(true);
        SubAni.SetBool("isOn", true);
        yield return Delay1;
        SubAni.SetBool("isOn", false);
        yield return Delay2;
        noticeUI.SetActive(false);
    }


}
