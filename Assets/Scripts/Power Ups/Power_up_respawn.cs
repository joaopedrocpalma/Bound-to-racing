using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Power_up_respawn : MonoBehaviour
{
    public GameObject pUp;  // Creating variables to receive references
    public GameObject pUp1;
    public GameObject pUp2;

    private float timer = 10f;  // internal timer for respawn

    private bool startTimer = false;    // Flag to start timer

    void Update()
    {
        if (pUp.activeInHierarchy == false || pUp1.activeInHierarchy == false || pUp2.activeInHierarchy == false)   // Searches for the 3 GameObjects in the hierarchy to see if they are active
        {
            startTimer = true;  // If yes, starts timer
        }

        if (startTimer)
        {
            timer -= Time.deltaTime;    // Decreases through -Time.deltaTime

            if (timer <= 0) // When the timer reaches 0
            {
                timer = 10f;    // Restarts the variable
                startTimer = false; // Stops the timer

                if (pUp.activeInHierarchy == false) // makes the GameObject active again
                {
                    pUp.SetActive(true);
                }

                if (pUp1.activeInHierarchy == false)
                {
                    pUp1.SetActive(true);
                }

                if (pUp2.activeInHierarchy == false)
                {
                    pUp2.SetActive(true);
                }

            }
        }        
    }
}
