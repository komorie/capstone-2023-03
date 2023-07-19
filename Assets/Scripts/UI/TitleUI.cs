using TMPro;
using UnityEngine;


public class TitleUI : MonoBehaviour
{
    public void StartButtonClick()
    {
        SceneLoader.Instance.LoadScene("OpScene");
    }

    public void LibraryButtonClick()
    {
        UIManager.Instance.ShowUI("LibraryUI")
            .GetComponent<LibraryUI>()
            .Init(LibraryMode.Library);
    }

    public void SettingButtonClick()
    {
        UIManager.Instance.ShowUI("SettingUI");
    }

    public void ExitButtonClick()
    {
        UIManager.Instance.ShowUI("SelectUI", false)
            .GetComponent<SelectUI>()
            .Init(
                "정말로 종료하시겠습니까?",
                () => { Application.Quit(); },
                () => { UIManager.Instance.HideUI("SelectUI"); }
            );
    }
}
