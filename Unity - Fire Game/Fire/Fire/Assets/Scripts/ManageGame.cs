using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour {
    public int lives = 3;
    public int platforms = 5;

    public GameObject L1Platforms, L1Platforms2, playerPrefab, firePrefab, cloneFire, clonePlayer, groundPrefab, cloneGround, spikePrefab, cloneSpikes, rocksPrefab, cloneRocks, bladesPrefab, cloneBlades;
    public AudioClip fire, hit, lose, win;
    public Text livesText;
    public GameObject endText;
    public static ManageGame instance;

    private new AudioSource audio;
    public bool hasPlayed = false;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        Setup();
	}
	
    void Setup() {
        //clone player, ground, and fire at startup
        cloneGround = Instantiate(groundPrefab, groundPrefab.transform.position, Quaternion.identity);
        clonePlayer = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        clonePlayer.GetComponent<PlayerController>().SetInstance(instance);
        cloneFire = Instantiate(firePrefab, firePrefab.transform.position, Quaternion.identity);
        clonePlayer.GetComponent<PlayerController>().playerHit = false;
        MoveFire.canMove = false;

        //make platforms and spikes at startup
        Instantiate(L1Platforms, L1Platforms.transform.position, Quaternion.identity);
        Instantiate(L1Platforms2, L1Platforms2.transform.position, Quaternion.identity);
        cloneSpikes = Instantiate(spikePrefab, spikePrefab.transform.position, Quaternion.identity);
        cloneBlades = Instantiate(bladesPrefab, bladesPrefab.transform.position, Quaternion.identity);
        cloneRocks = Instantiate(rocksPrefab, rocksPrefab.transform.position, Quaternion.identity);
        audio.clip = fire;
        audio.Play();
    }

    public void LoseLife()
    {
        if (clonePlayer.GetComponent<PlayerController>().playerHit == true)
        {
            //keep track of lives
            lives--;
            livesText.text = "Lives: " + lives;
            //if player hit by fire or spike, destroy
            audio.clip = hit;
            audio.Play();
            Destroy(clonePlayer);
            CheckGameOver();
            //if game not over, reset stage
            Destroy(cloneFire);
            Destroy(cloneRocks);
            Destroy(cloneBlades);
            Destroy(cloneSpikes);
            Camera.main.transform.position = new Vector3(0, 0, -10);
            if (lives > 0)
            {
                Invoke("NextLife", 1);
            }
        }
    }

    void NextLife()
    {
        //sets up after a death
        Setup();
    }

    void CheckGameOver()
    {
        //is game complete or is player dead
        if (lives < 1)
        {
            endText.GetComponent<Text>().text = "Game Over!!!";
            endText.SetActive(true);
            audio.clip = lose;
            audio.Play();
        }
    }

    public void WinGame()
    {
        endText.GetComponent<Text>().text = "You Win!!!";
        endText.SetActive(true);
        Destroy(cloneFire);
        Destroy(cloneSpikes);
        Destroy(cloneRocks);
        Destroy(cloneBlades);
        if (hasPlayed == false) {
            audio.clip = win;
            audio.Play();
            hasPlayed = true;
        }
    }
}
