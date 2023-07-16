using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    //싱글톤으로 유일한 게임 오브젝트 + 컴포넌트 생성
    //Singleton<T>를 상속받은 각각의 T 컴포넌트들은 자신을 가리키는 유일한 Instance를 가짐
    
    private static T instance;
    public static T Instance { get { Init(); return instance; } private set { instance = value; } }

    private static bool isDestroyed = false;


    protected virtual void Awake()
    {
        Init();
    }

    //유니티의 OnDisable(오브젝트가 비활성화) 함수는 Destroy(오브젝트가 파괴)될때도 호출된다.
    //이 코드 상, OnDisable 함수에서 싱글톤 인스턴스에 접근하려고 할 때, 오브젝트가 파괴된 상황이라면 싱글톤 오브젝트를 새로 생성해버린다.
    //그렇다고 자동으로 새로 생성하는 작업을 없애면 불편함.
    //그래서 싱글톤 오브젝트가 파괴되고 나서 접근하는 경우는 static bool을 사용해 재생성하지 않도록 함.
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
                go.AddComponent<T>();
            }

            instance = go.GetComponent<T>();
        }
    }
}
