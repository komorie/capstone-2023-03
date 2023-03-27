using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߿� ����ȭ�� �� �� �� �ֱ� ������ �ϴ� �� Ŭ������ �ѹ� ���ؼ� �ҷ����� �Ѵ�
//����/�������� �ҷ��� �� ��� ����
public class AssetLoader : Singleton<AssetLoader>
{
    private Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    //ĳ�ø� �ؼ� ������ �ҷ����� �ʵ��� ��
    public GameObject Load(string path)
    {
        if (!cache.ContainsKey(path))
        {
            cache.Add(path, Resources.Load<GameObject>(path));
        }

        return cache[path];
    }

    //�������� (�Ⱥҷ������� �ҷ�����) �����ؼ� ��ȯ
    //AssetLoader.Instance.Instantiate("Prefabs/BossDoor"); ������ ���
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load(path);
        GameObject go = Object.Instantiate(prefab, parent);

        go.name = prefab.name;

        return go;
    }

    //�ٸ� ����
    public GameObject Instantiate(string path, Vector3 postion, Quaternion rotation, Transform parent = null)
    {
        GameObject go = Instantiate(path, parent);

        go.transform.position = postion;
        go.transform.rotation = rotation;

        return go;
    }

    //���� ������Ʈ �ı�
    public void Destroy(GameObject go)
    {
        Object.Destroy(go);
    }


}
