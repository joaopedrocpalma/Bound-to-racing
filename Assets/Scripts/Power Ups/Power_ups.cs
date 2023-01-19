using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to the power up boxes

public class Power_ups : MonoBehaviour
{
    // Variables
    private int num = 0;

    private float speed = 100f;
    private float bounceSpeed = 2f;

    private bool buff = false;

    private GameObject player;

    private GameObject AI;

    public Material material;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AI = GameObject.FindGameObjectWithTag("Ai");
    }

    void Update()
    {
        ChangeMaterial();
        Rotate();
    }

    // When the entity hits the power up box, it will create a random num between 1 - 3 for power up
    private void OnTriggerEnter (Collider collider)
    {
        num = UnityEngine.Random.Range(0, 3);
        buff = true;

        if (collider.gameObject.tag == "Player")    // If the object that collides is the player, it will modify the Buff_controller variables and set the box to innactive
        {
            player.GetComponent<Buff_controller>().buffNum = num;   // Tells the Buff controller which buff has been acquired
            player.GetComponent<Buff_controller>().buffAvailable = buff; // Tells buff controller there is a buff available

            this.gameObject.SetActive(false);   // Hides the box if it has been hit by a car
        }    
        
        if(collider.gameObject.tag == "AI")
        {
            this.gameObject.SetActive(false);   // Hides the box if it has been hit by a car

            AI.GetComponent<Buff_controllerAI>().buffNum = num;
            AI.GetComponent<Buff_controllerAI>().buffAvailable = buff;

        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * (speed * Time.deltaTime));

        Vector3 pos = transform.position;

        float y = Mathf.Sin(Time.time * bounceSpeed) / 5 + 1;

        transform.position = new Vector3(pos.x, y, pos.z);
    }

    private void ChangeMaterial()
    {
        gameObject.GetComponent<Renderer>().material = material;
    }
}
