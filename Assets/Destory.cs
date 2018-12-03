using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory : MonoBehaviour {


    // Update is called once per frame
    private void Start()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
