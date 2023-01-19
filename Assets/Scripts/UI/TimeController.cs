using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private static float timer;

    private string minutesText, secondsText;

    private float minutes, seconds;  
    private float countdown = 3;    

    public bool gameStarted = true;

    public Text countdownText;
    public Text timerText;
    public Text lapsText;

    public GameObject car;

    void start()
    {
        
    }

    void Update()
    {
        StartGameTimer();

        if (gameStarted)    // If the countdown is finished, starts the LapTimer()
        {
            GameTimer();
            UpdateLaps();
        }        
    }


    private void StartGameTimer()  // Controls coutdown system
    {
        timerText.gameObject.SetActive(false);  // Hides the clock timer
        lapsText.gameObject.SetActive(false);

        countdown -= Time.time / 60; // Starts the coutdown timer

        StartCoroutine(RemoveAfterSeconds(4, countdownText)); // Starts the timer to deactivate the label

        countdownText.text = countdown.ToString("F0"); // Outputs time into label

        if (countdown <= 0) // If the timer has reached 0 or lower
        {
            countdownText.text = "GO!"; // Change the label text to GO!
            gameStarted = true; // Allows for game timer to start
        }
        

        IEnumerator RemoveAfterSeconds(int seconds, Text label) // Makes a timer to remove the countdown label
        {
            yield return new WaitForSeconds(seconds);
            label.gameObject.SetActive(false); ;
        }        
    }


    private void GameTimer() // Controls laps times
    {
        timerText.gameObject.SetActive(true);

        timer = Time.time;  // Starts lap timer

        minutes = Mathf.Floor(timer / 60);  // Sets minutes
        seconds = timer % 60;   // Sets seconds

        minutesText = minutes.ToString("F0");
        secondsText = seconds.ToString("F0");

        if (minutes < 10)   // If the number of minutes is smaller than 10
        {
            minutesText = "0" + minutes.ToString("F0"); // Adds a 0 to the number to make it visually appealing
        }
        else    // If not, the minutes go back to their normal
        {
            minutesText = minutes.ToString("F0");
        }

        if (seconds < 10)   // Same as minutes
        {
            secondsText = "0" + seconds.ToString("F2");
        }
        else
        {
            secondsText = seconds.ToString("F2");
        }

        timerText.text = minutesText + ":" + secondsText;   // For visual output
    }

    private void UpdateLaps()
    {
        lapsText.gameObject.SetActive(true);
        lapsText.text = car.GetComponent<Car_controller>().numLaps.ToString() + "/3 laps";        
    }
}
