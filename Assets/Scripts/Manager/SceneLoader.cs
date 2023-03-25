using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    string nextScene = null;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //�ε�â �θ���, ���� �� �ε��ϴ� �ڷ�ƾ �����ϴ� �Լ�
    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        StartCoroutine(LoadProcess());
        SceneManager.LoadScene("LoadingScene");
    }

    //�ε� ����
    IEnumerator LoadProcess()
    {
        Debug.Log("2");
        yield return null;
        Debug.Log("3");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("4");
        SceneManager.LoadScene(nextScene);
        yield return null;
    }
}
