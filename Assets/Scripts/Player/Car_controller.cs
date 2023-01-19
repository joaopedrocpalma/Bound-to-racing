using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Experimental.TerrainAPI;

public enum Axel    //This is a class but it’s variables are always public
{
    Front,
    Rear
}

// To be accessible on the editor I serialized it
[Serializable]
public struct Wheel // Creates a struct which will save the wheel's models and colliders in their respective axels
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class Car_controller : MonoBehaviour
{
    public Renderer entity1;
    public Renderer entity2;
    public Renderer entity3;
    public Renderer entity4;
    public Renderer entity5;

    public Material gold;

    public bool itemAcquired = false;

    // Private variables
    [SerializeField] private List<Wheel> wheels;

    private Vector3 centerMass;

    private Rigidbody rb;

    private Checkpoint_controller cPoint;
    private TimeController timer;
    private Buff_controller buff;

    private GlobalPos pos;

    public GameObject Menu;

    public bool inMenu = false;    

    [Header("Car Attributes")]
    public float maxAccel = 35.0f;
    public float health = 100f;
    public int numLaps = 0;

    private float hInput, vInput, currentSpeed;
    private float turnSensitivity = 0.5f;
    private float maxSteerAngle = 40f;
    private float topSpeed = 260;
    private int numWheels;
    private bool moving = false;


    [Header("CheckPoints")]
    public int numCheckPointsNeeded = 0;
    public int numCheckpoints = 0;
    public string currentCheckPoint;
    public bool spawnPoint = false;
    public Vector3 cPointPos, cPointRot;    // Saves checkpoint coordinates

    // Functions
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerMass;

        timer = GameObject.Find("UI").GetComponent<TimeController>();    // References the UI to get the TimeController script, for the gameStarted flag

        buff = GetComponent<Buff_controller>();
        cPoint = GameObject.FindObjectOfType<Checkpoint_controller>();
    }


    private void Update() // Gets player inputs, animates the wheels and changes their direction
    {
        if (timer.gameStarted)  // If the game has started
        {
            CheckLaps();
            GetInput();
            RotateWheels();
            Turning();
            Grounded();
            HealthController();
            if (itemAcquired)
            {
                Debug.Log("Here");
                PaintWheels();
            }
        }
    }


    private void LateUpdate() // Makes a late call for the funtion Accelerate after all Update functions have been ran
    {
        Accelerate();
    }


    private void GetInput() // Controls player inputs, direction, breaking
    {
        // Gets both inputs for X and Y axis
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) // To stop the car from always accelerating
        {
            moving = true;
            vInput = Input.GetAxis("Vertical");
        }
        else
        {
            vInput = 0; // Resets the input to stop the car from accelerating
            moving = false;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) // To prevent the car from going all over the place in directions
        {
            hInput = Input.GetAxis("Horizontal");
        }
        else
        {
            hInput = 0; // Resets the input to stop the car from turning
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inMenu) // If the pause menu is open
            {
                ResumeGame();
                Menu.SetActive(false);
                inMenu = false;
            }
            else    // If the player want to pause the game
            {
                PauseGame();
                Menu.SetActive(true);
                inMenu = true;
            }            
        }

        BrakingController(moving);  // controlls the brakign system

        // Checks if the player is using the hand break
        Respawn(); 
    }


    private void Respawn()  // Controlls the respawning of player
    {
        if (spawnPoint)
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.position = cPointPos; // Makes the player go to the correct position
                transform.rotation = Quaternion.LookRotation(cPointRot);    // Rorates the player in the direction of the race

                rb.velocity = new Vector3(0, 0, 0); // Stops the car movement when using checkpoint
            }
        }
    }


    private void BrakingController(bool moving) // Stops the wheels motion and the car movement
    {
        if (moving) // If the player is in movement makes stops using the brakes
        {
            RestoreBrakes();
        }
        else if(Input.GetKey(KeyCode.Space)) // Otherwise constantly brakes
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Rear)
                {
                    wheel.collider.brakeTorque = Mathf.Infinity;
                    wheel.collider.motorTorque = 0;
                }
            }
        }
        else if(moving == false)    // If the player isn't pressing any buttons the car should stop by itself
        {
            foreach (var wheel in wheels)
            {
                wheel.collider.brakeTorque = 20000 * Time.deltaTime;
            }
        }
    }

    private void RestoreBrakes()
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque = 0;
        }
    }


    private void RotateWheels()  // Transforms the wheels visual to the correct turning position
    {
        foreach (var wheel in wheels)
        {
            // Define the wheel position and the rotation
            Quaternion rot;
            Vector3 pos;

            // Apply for rot and pos to change the wheels turning direction
            wheel.collider.GetWorldPose(out pos, out rot);  // Gets the world position and rotation values and sends them into pos and rot
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;       
        }
    }


    private void Turning() // Applies transformation to the wheels to make the car turn
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)    // For the front wheels only
            {
                // Sets the current steering angle of the wheels
                var steeringAngle = hInput * turnSensitivity * maxSteerAngle;
                // Chooses value between steeringAngle and 0.5f to make the wheel turn
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, steeringAngle, turnSensitivity);
            }
        }
    }


    private void Accelerate() // Applies transformation of movement to the car
    {
        currentSpeed = rb.velocity.magnitude * 3f;

        foreach (var wheel in wheels)   // For each individual wheel in the wheels list
        {
            // Accelerates the wheels making the car move
            wheel.collider.motorTorque = vInput * maxAccel * 500 * Time.deltaTime;
        }

        if (currentSpeed >= topSpeed) // If the car reaches the threshold it wil lstop accelerating
        {
            foreach (var wheel in wheels)   // For each individual wheel in the wheels list
            {
                // Stops accelerating
                wheel.collider.motorTorque = 0;
            }
        }
    }


    private void Grounded() // Checks if the player is in the air
    {
        foreach (var wheel in wheels)
        {
            if (wheel.collider.isGrounded)
            {
                numWheels += 1;
            }
        }

        if (numWheels == 4)
        {
            buff.isGrounded = true;     // Variable needed to be set to true to use the shield
            numWheels = 0;
        }
        else
        {
            buff.isGrounded = false;
            numWheels = 0;
        }
    }


    private void CheckLaps() // Checks for win condition
    {
        if (numLaps == 0)
        {
            numCheckPointsNeeded = 4;
        }
        else
        {
            numCheckPointsNeeded = 3;
        }

        if (numCheckpoints == numCheckPointsNeeded) // If the player hits all checkpoint in the map
        {
            numCheckpoints = 0;
            numLaps += 1;
        }
    }


    private void HealthController()
    {
        if (health <= 0)
        {
            BrakingController(false);
            Invoke(nameof(RestoreBrakes), 3);
        }
    }

    private void PaintWheels()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            entity1.material = gold;
            entity2.material = gold;
            entity3.material = gold;
            entity4.material = gold;
            entity5.material = gold;
        }       
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}