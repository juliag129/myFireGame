using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollider : MonoBehaviour {

    //use this for initialization
    void Start()
    {

    }

    //update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 100, 10));
    }
}
