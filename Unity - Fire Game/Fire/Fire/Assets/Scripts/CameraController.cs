using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject playerPrefab, firePrefab;
    //private Vector3 offset;

    public Transform playerTransform;
    public Transform fireTransform;
    public Transform targetTransform;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public bool fireInView = true;
    public bool playerInView = true;

    // Use this for initialization
    void Start() {
        //offset = transform.position - firePrefab.transform.position;
        StartCoroutine(DelayedStart());
    }

    public void SetPlayerTransform(Transform target) {
        playerTransform = target;
    }

    public void SetFireTransform(Transform target)
    {
        fireTransform = target;
    }

    IEnumerator DelayedStart()
    {
        //wait til player escapes or fire moves after 10 sec
        yield return new WaitForSeconds(10);
    }

    //LateUpdate is called after Update each frame
    void LateUpdate()
    { 
        if (playerTransform && PlayerController.hitTop == true)
        {
            //define a target position above and behind the target transform
            Vector3 targetPosition = playerTransform.TransformPoint(new Vector3(0, 0, -10));
            targetPosition.x = 0;
            //smoothly move the camera towards player
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        if (fireTransform && fireTransform.transform.position.y >= (Camera.main.ScreenToWorldPoint(new Vector3(0, 90, -10)).y))
        {
            transform.position += Vector3.up * Time.deltaTime;
            PlayerController.hitTop = false;
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0.0f, 105.0f), transform.position.z);
    }
}