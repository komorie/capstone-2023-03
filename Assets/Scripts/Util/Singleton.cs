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
    //이 코드상에서 GET으로 접근할 시 싱글톤 인스턴스가 생성되지 않은 경우 생성하는 기능이 있음. (생성 시기를 신경쓰지 않고 만들려는 의도)
    //그런데 다른 오브젝트의 OnDisable 함수에서 싱글톤 인스턴스에 접근하려고 할 때(이벤트 해제 등), 오브젝트가 파괴된 상황이라면 싱글톤 오브젝트를 새로 생성해버린다.
    //그래서 싱글톤 오브젝트가 파괴되고 나서 접근하는 경우는 static bool을 사용해 재생성하지 않도록 함.
    //이렇게 하면 문제는 일부러 싱글톤 오브젝트를 파괴했다가 또다시 생성하려는 경우는? 
    //다음에 코드를 짤 때는 접근할 때 자동으로 생성하는 건 빼고 반드시 명시적으로 생성/소멸하도록 하는게 나을지도...
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
