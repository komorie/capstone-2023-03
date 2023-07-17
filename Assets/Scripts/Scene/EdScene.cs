using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SoundManager.Instance.Play("Sounds/OpBgm", Sound.Effect);
        UIManager.Instance.ShowUI("FullDialogUI").GetComponent<FullDialogUI>().Init(
            6001, () => { SceneLoader.Instance.LoadScene("TitleScene"); }
        );
    }
}
