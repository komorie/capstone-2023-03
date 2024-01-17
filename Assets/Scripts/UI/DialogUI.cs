using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DataStructs;
using System.Collections;

//대화창 UI 클래스
//1. 대화를 대화창 UI에 보여줌.
//2. 인풋을 통해 대화창을 다음 대화로 진행.

public class DialogUI : MonoBehaviour, IPointerDownHandler
{
    private bool isPrinting = false; // 출력 중
    private int dialogIndex;
    private int lineCount;
    private LineStruct currentLine;

    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text lineText;

    private Action CloseAction;

    private void OnEnable()
    {
        //엔터로 대화창 진행하는 함수 실행하게 이벤트 등록
        //인풋시스템에 이벤트 함수 등록할 때, 람다로 등록하지 않는 게 좋을 듯. 왠지는 모르겠는데 중복 실행 오류
        InputActions.keyActions.UI.Check.started += NextDialogByCheck;
    }

    private void OnDisable()
    {
        InputActions.keyActions.UI.Check.started -= NextDialogByCheck;
    }

    //마우스로 UI 클릭 시 작동
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPrinting) // 출력 중이지 않을 때만 다음 대화로 넘어감
            NextDialog();
    }

    public void NextDialogByCheck(InputAction.CallbackContext context)
    {
        if (!isPrinting) // 출력 중이지 않을 때만 다음 대화로 넘어감
            NextDialog();
    }

    //처음 대화창이 열릴 때 초기화
    public void Init(int index, Action CloseCallback = null)
    {
        dialogIndex = index;
        CloseAction = CloseCallback;
        lineCount = 0;
        NextDialog();
    }

/*    //대화창 초기화 - 문자열 인자를 받은 단문 대화 버전
    public void Init(string line, string portrait = null, string name = null, Action CloseCallback = null)
    {
        dialogIndex = -1;

        if (portrait == null || portrait == "")
        {
            this.portrait.gameObject.SetActive(false);
        }
        else
        {
            this.portrait.gameObject.SetActive(true);
            this.portrait.sprite = GameData.Instance.SpriteDic[portrait];
        }

        if (name == null || name == "")
        {
            nameText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(true);
            nameText.text = name;
        }

        lineText.text = line;
        isInited = true;
        CloseAction = CloseCallback; 

    }*/

    //대화창을 진행시키는 함수. 대화 데이터 딕셔너리에서 대화 내용을 가져오고, 가져올 내용이 더이상 없으면 대화창을 닫는다.
    //대화 내용에 따라 UI 구조와 포트레이트를 변경.
    public void NextDialog()
    {
        //다음 대화가 없으면 창 닫고 종료
        if (dialogIndex == -1 || GameData.Instance.DialogDic[dialogIndex].Count == lineCount) 
        {
            UIManager.Instance.HideUI("DialogUI");
            CloseAction?.Invoke();
            return;
        }

        //한 줄 가져오기
        currentLine = GameData.Instance.DialogDic[dialogIndex][lineCount];

        //이름, 초상화 등이 없는 경우는 이름, 초상화 창을 제거
        if (currentLine.portrait == null || currentLine.portrait == "")
        {
            portrait.gameObject.SetActive(false);   
        }
        else
        {
            portrait.gameObject.SetActive(true);
            portrait.sprite = GameData.Instance.SpriteDic[currentLine.portrait];
        }

        if(currentLine.name == null || currentLine.name == "")
        {
            nameText.transform.parent.gameObject.SetActive(false);  
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(true);
            nameText.text = currentLine.name;
        }
        StartCoroutine(TypeSentence(currentLine.line));
        lineCount++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isPrinting = true;
        lineText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            lineText.text += letter;
            yield return Define.WaitForSecondsDialog;
        }
        isPrinting = false; // 출력 완료
    }
}
