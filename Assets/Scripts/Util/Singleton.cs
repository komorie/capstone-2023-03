using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    //�̱������� ������ ���� ������Ʈ + ������Ʈ ����
    //Singleton<T>�� ��ӹ��� ������ T ������Ʈ���� �ڽ��� ����Ű�� ������ Instance�� ����
    
    private static T instance;
    public static T Instance { get { Init(); return instance; } private set { instance = value; } }

    private static bool isDestroyed = false;


    protected virtual void Awake()
    {
        Init();
    }

    //����Ƽ�� OnDisable(������Ʈ�� ��Ȱ��ȭ) �Լ��� Destroy(������Ʈ�� �ı�)�ɶ��� ȣ��ȴ�.
    //�� �ڵ�󿡼� GET���� ������ �� �̱��� �ν��Ͻ��� �������� ���� ��� �����ϴ� ����� ����. (���� �ñ⸦ �Ű澲�� �ʰ� ������� �ǵ�)
    //�׷��� �ٸ� ������Ʈ�� OnDisable �Լ����� �̱��� �ν��Ͻ��� �����Ϸ��� �� ��(�̺�Ʈ ���� ��), ������Ʈ�� �ı��� ��Ȳ�̶�� �̱��� ������Ʈ�� ���� �����ع�����.
    //�׷��� �̱��� ������Ʈ�� �ı��ǰ� ���� �����ϴ� ���� static bool�� ����� ��������� �ʵ��� ��.
    //�̷��� �ϸ� ������ �Ϻη� �̱��� ������Ʈ�� �ı��ߴٰ� �Ǵٽ� �����Ϸ��� ����? 
    //������ �ڵ带 © ���� ������ �� �ڵ����� �����ϴ� �� ���� �ݵ�� ��������� ����/�Ҹ��ϵ��� �ϴ°� ��������...
    private void OnDestroy()
    {
        isDestroyed = true;
    }

    protected static void Init()
    {
        if(isDestroyed == true) return;

        if (instance == null)
        {
            GameObject go = GameObject.Find(typeof(T).Name);

            if(go == null)
            { 
                go = new GameObject(typeof(T).Name);
            }

            if(go.TryGetComponent(out instance) == false)
            {
                Instance = go.AddComponent<T>();
            }
        }
    }
}
