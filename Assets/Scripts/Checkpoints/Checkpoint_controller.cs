using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_controller : MonoBehaviour
{
    private Car_controller player;  // References for use later
    private AI_controller ai;
    private WinCondition win;

    private string cPointName;  // stores the current checkpoint's name

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Car_controller>(); // Pointing references to objects
        ai = GameObject.FindGameObjectWithTag("Ai").GetComponent<AI_controller>();
        win = GetComponent<WinCondition>();
    }

    // When the player collides with the checkpoint, it saves his current position and sets spawnPoint to true
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")    // If the obj that collided with the checkpoint is the player
        {
            cPointName = gameObject.name;   // stores the gameObject's name on the variable

            if (cPointName != player.currentCheckPoint) // If the name isn't the same as the one stored in the player
            {
                player.numCheckpoints += 1; // Increases the player's checkpoint cuunter

                player.cPointPos = transform.position;  // Gives him the checkpoint's coordinates and orientation
                player.cPointRot = transform.forward;
                player.spawnPoint = true;

                player.currentCheckPoint = cPointName;  // Changes the player's current checkpoint
            }
        }

        if(obj.tag == "AI") // Checks if the bot hit a checkpoint
        {
            ai.numCheckpoints += 1; // Increases the number of his checkpoints

            win.aiName = obj.name;  // Stores the bot's name in the WinCondition script for when the bot wins
        }
    }
}
