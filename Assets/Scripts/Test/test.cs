using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = new GameObject();

        AssetLoader.Instance.Instantiate("Prefabs/BossDoor");
        AssetLoader.Instance.Instantiate("Prefabs/Door", obj.transform);

        AssetLoader.Instance.Instantiate("Prefabs/Player", Vector3.forward, Quaternion.Euler(0, 20, 20));
        AssetLoader.Instance.Instantiate("Prefabs/Player", Vector3.back, Quaternion.Euler(0, 20, 20), obj.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
