using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���߿� ��巹����� �ٲܼ��� �ֱ� ������ �ϴ� �Ŵ��� Ŭ������ �ѹ� ���ؼ� �ҷ����� �Ѵ�
//ĳ���� ���� ���ҽ� �ҷ����� �ð� ����
//�ٵ� instantiate�� ����� �ʹ� Ŀ�� Ƽ�� �ȳ���...�� update������ ���� �θ��ų� �ϸ� ����
public class AssetLoader : Singleton<AssetLoader>
{
    private Dictionary<string, GameObject> cache = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public GameObject Load(string path)
    {
        if (!cache.ContainsKey(path))
        {
            cache.Add(path, Resources.Load<GameObject>(path));
        }

        return cache[path];
    }


}
