using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Speedometer_controller : MonoBehaviour
{
    // Variables and References
    private const float maxSpeedAngle = -102.127f;
    private const float minSpeedAngle = 103.575f;

    private float maxSpeed = 260f;
    private float speed = 0f;

    private Rigidbody car;
    public Text speedLabel;    

    private Transform needleTrans; 

    private void Start()
    {
        needleTrans = transform.Find("Needle"); // Searches for the attached gameObject named "Needle"

        car = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate() // Takes the car's current speed and displays it in a speedometer
    {
        speed = car.velocity.magnitude * 3f; // Gets the speed of the car

        if (speedLabel != null)     // Changes the speedLabel text
            speedLabel.text = ((int)speed) + " km/h";

        if (needleTrans != null)    // Changes the rotation angle of the Needle
            needleTrans.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedAngle, maxSpeedAngle, speed / maxSpeed));
    }
}
