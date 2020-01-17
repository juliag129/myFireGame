using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFire : MonoBehaviour {

    public Transform fireTransform;
    public Transform playerTransform;
    public static bool canMove = false;

    //use this for initialization
    void Start () {
        StartCoroutine(DelayedStart());
        Camera.main.GetComponent<CameraController>().SetFireTransform(fireTransform.transform);
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(10);
        canMove = true;
    }

    //update is called once per frame
    void LateUpdate () {
        if (canMove) {
            fireTransform.transform.position += Vector3.up * Time.deltaTime;
        }
    }
}