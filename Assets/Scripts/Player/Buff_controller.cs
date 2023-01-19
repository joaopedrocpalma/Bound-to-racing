using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

// Attach this script to the player to manage the buffs

public class Buff_controller : MonoBehaviour
{
    // These variables are public so that the power ups can control if the player has one available and which
    [Header("Prefabs")]
    public GameObject missile;
    public GameObject shield;    
    public GameObject clone;

    [Header("Powers Icons")]
    public Image powerUp;

    public Sprite empty;
    public Sprite boost;
    public Sprite jump;
    public Sprite attack;
    public Sprite defend;


    private GameObject shieldRef;

    private Rigidbody rb;

    private Vector3 pos;

    public bool buffAvailable = false;
    public bool isGrounded;

    public float buffNum;
    private float jumpForce = 3800f;
    private float distance = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        DisplayPower();
    }


    // Get player input, if the player has an available buff he can press the mouse button 0 to activate it
    private void GetInput()
    {
        if (buffAvailable)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) // If the player presses LMB
            {
                buffAvailable = false;

                if (buffNum == 0)
                {
                    Boost();
                }

                if (buffNum == 1 && isGrounded) // If the buff Num is the same as the jump and the car is on the ground
                {
                    Jump(); // Makes the player jump
                }

                if (buffNum == 2)
                {
                    Attack();
                }

                if (buffNum == 3)
                {
                    Defend();
                }
            }
        }        
    }


    // Gives a temporary speed boost of 8 seconds to the player and controls the time the boost is activated
    private void Boost()
    {
        GetComponent<Car_controller>().maxAccel += 50f;
        Invoke(nameof(StopBoost), 8);
    }

    // Once the time of the boost is up, it resets the speed to it's original value
    private void StopBoost()
    {
        GetComponent<Car_controller>().maxAccel -= 50f;
    }


    // Makes the player jump when he gets this power up
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    // Sends out a projectile that will destroy and oponent
    private void Attack()
    {
        pos = transform.position + transform.forward * distance; // Updates the missile position to always shoot from the front
        clone = Instantiate(missile, pos, transform.rotation); // Creates and instance of the missile
    }


    // Creates a shield around the player for 5 seconds
    private void Defend()
    {
        shieldRef = Instantiate(shield, pos, transform.rotation);
        Invoke(nameof(StopDefend), 4);
    }

    private void StopDefend()
    {
        Destroy(shieldRef);
    }

    private void DisplayPower()
    {
        if(buffNum == 0)
        {
            powerUp.sprite = boost;
        }
        else if (buffNum == 1)
        {
            powerUp.sprite = jump;
        }
        else if (buffNum == 2)
        {
            powerUp.sprite = attack;
        }
        else if (buffNum == 3)
        {
            powerUp.sprite = defend;
        }

        if(!buffAvailable)
        {
            powerUp.sprite = empty;
        }
    }
}
