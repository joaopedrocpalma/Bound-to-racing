using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    private Car_controller player;
    private AI_controller Ai;

    public Text text1;
    public Text text2;
    public Text text3;

    public string aiName;

    void Start()
    {
        player = GameObject.FindObjectOfType<Car_controller>();
        Ai = GameObject.FindObjectOfType<AI_controller>();
    }

    void Update()
    {
        checkPlace();
        checKWin();
    }

    private void checkPlace()
    {
        if (player.numCheckpoints > Ai.numCheckpoints)
        {

        }
    }

    private void checKWin()
    {
        if(player.numLaps == 3) // If the number of laps matches the amount need
        {
            text1.text = "Congratulations!!";   // Displays a couple of phrases
            text2.text = "You won!!!";
            text1.gameObject.SetActive(true);
            text2.gameObject.SetActive(true);
        }

        else if (Ai.numLaps == 3)
        {
            Debug.Log("AI " + aiName + " won"); // tries to get the AI's name and displays it for the player.
            text3.text = "You finished in";
            text3.gameObject.SetActive(true);
        }
    }
}
