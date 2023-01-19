using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class AI_controller : MonoBehaviour
{
    // Private variables
    private Checkpoint_controller cPoint;
    private TimeController timer;
    private Buff_controllerAI buff;   

    private Rigidbody rb;

    private Vector3 centerMass = new Vector3(0, 0, -1f);

    private List<Transform> nodes;
    [SerializeField] private List<Wheel> wheels;

    private float maxSteerAngle = 20f;
    private float topSpeed = 100;
    private float currentSpeed = 0;
    private int currentNode, numWheels = 0;
    private bool steering = false;

    // Public variables
    public Transform path;

    public float maxAccel = 35.0f;
    public float health = 100f;


    [Header("Checkpoints")]
    public int numCheckpoints = 0;

    public int numLaps = 0;
    public bool hitCPoint = false;

    private int numCheckPointsNeeded;

    [Header("Sensors")] // A header to separate variables in inspector
    private float sensorLength = 5f;
    private float frontSensorStart = 2.25f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerMass;

        timer = GameObject.Find("UI").GetComponent<TimeController>();    // References the UI to get the TimeController script, for the gameStarted fla

        buff = GetComponent<Buff_controllerAI>();
        cPoint = GameObject.FindObjectOfType<Checkpoint_controller>();

        GetPathNodes();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (timer.gameStarted)  // If the game has started
        {
            Sensors();
            CheckNodeDistance();
            RotateWheels();
            Turning();
            Accelerate();            
            Grounded();
            HealthController();
            Brake();
            CheckLaps();
            ChangeColor();
        }
    }


    private void GetPathNodes() // Adds the path nodes to a private list, so we can use it to guide the AI
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++) // Checks for each individual transform in the pathTransforms
        {
            if (pathTransforms[i] != path.transform)  // If it's not the repeated transform
            {
                nodes.Add(pathTransforms[i]);   // Adds the Transform to the node list, thus creating the path
            }
        }
    }


    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos.z += frontSensorStart;
        sensorStartPos.y += 0.15f;

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawRay(sensorStartPos, hit.point);
        }
    }


    private void CheckNodeDistance()    // Checks the distance to the next node and changes it, if the AI is close enough
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 10f)   // If the distance to the next node is smaller than 10f
        {
            if (currentNode == nodes.Count - 1) // If the currentNode is the last one, set it to the first (reset the loop)
            {
                currentNode = 0;
            }
            else
            {
                currentNode += 1;
            }
        }
    }

    private void RotateWheels() // Applies a rotation visual to the wheels
    {
        Quaternion rot;
        Vector3 pos;

        foreach (var wheel in wheels)
        {
            wheel.collider.GetWorldPose(out pos, out rot);
            wheel.model.transform.position = pos;
            wheel.model.transform.rotation = rot;
        }
    }


    private void Turning()  // Calculates the next node direction
    {
        Vector3 nextNodeDir = transform.InverseTransformPoint(nodes[currentNode].position);
        nextNodeDir /= nextNodeDir.magnitude;
        float newSteer = (nextNodeDir.x / nextNodeDir.magnitude) * maxSteerAngle; // This is the distance between the car and the next node

        foreach (var wheel in wheels)   // Steers the wheels in that direction
        {
            if (wheel.axel == Axel.Front)
            {
                wheel.collider.steerAngle = newSteer;
            }            
        }
        steering = true;
    }


    private void Accelerate()   // Accelerates the wheels
    {
        currentSpeed = rb.velocity.magnitude * 3f;

        foreach (var wheel in wheels)   // For each individual wheel in the wheels list
        {
            // Accelerates the wheels making the car move
            wheel.collider.motorTorque = 1f * maxAccel * 500 * Time.deltaTime;
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


    private void Brake()    // Applies braking when the speed is too fast and the AI is steering
    {
        currentSpeed = rb.velocity.magnitude * 3f;

        if (steering && currentSpeed >= 100)   // To make turns easier, he slows down 
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Rear)
                {
                    wheel.collider.brakeTorque = Mathf.Infinity;
                }
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.collider.brakeTorque = 0;
            }
        }
    }

    private void RestoreBrakes()    // Restores the braking values to 0, so the ecar can move again
    {
        foreach (var wheel in wheels)
        {
            wheel.collider.brakeTorque = 0;
        }
    }


    private void Grounded() // Checks if the car is touching the ground with the 4 wheels
    {
        foreach(var wheel in wheels)
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


    private void HealthController()
    {
        if (health <= 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.collider.brakeTorque = Mathf.Infinity;
            }
            Invoke(nameof(RestoreBrakes), 3);
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

        if (numCheckpoints == numCheckPointsNeeded) // If the AI hits all checkpoint in the map
        {
            numCheckpoints = 0;
            numLaps += 1;
        }
    }

    private void ChangeColor()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.Find("body").GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
