using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D> ();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Invoke("Fall", 4);
        }
    }

    void Fall()
    {
        rb.isKinematic = false;
        Invoke("Done", 1);
    }

    void Done()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController player = GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController;
        if (col.gameObject.tag == "Player" && player.GetComponent<PlayerController>().playerHit != true)
        {
            //player hit by rock
            player.GetComponent<PlayerController>().playerHit = true;
            ManageGame temp = GameObject.FindObjectOfType(typeof(ManageGame)) as ManageGame;
            temp.LoseLife();
        }
    }
}
