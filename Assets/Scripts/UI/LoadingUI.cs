using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    private string nextScene = null;

    [SerializeField]
    public Image progressBar; //�ε� ������

    public void Init(string sceneName)
    {
        nextScene = sceneName;
        StartCoroutine(LoadProcess());
    }

    IEnumerator LoadProcess()
    {
        yield return null; //ó�� �ڷ�ƾ ������ �����ӿ��� ����

        // �񵿱� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        // �ε��� �Ϸ�� ������ ��ٸ�
        while (!asyncLoad.isDone)
        {
            // ������� ����ϰ� progressBar �̹����� fillAmount �Ӽ��� �Ҵ�
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.fillAmount = progress/2;

            // ���� �����ӱ��� ��ٸ�
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
    }
}
