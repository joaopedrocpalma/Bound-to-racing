using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

// Attach this script to a cube to make it hold the camera for the player

public class Camera_controller : MonoBehaviour
{
    private GameObject car;

    public Camera dCam;
    public Camera bCam;
    public Camera nCam;
    public Camera fCam;

    private float posX;
    private float posY;
    private float posZ;

    private int camNum = 1; 

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");

        // Activates the default camera when the game starts
        dCam.enabled = true;
        nCam.enabled = false;
        fCam.enabled = false;
        bCam.enabled = false;
    }

    void Update() // Gets the ever changing position of the player and applies it to the camera position
    {
        posX = car.transform.eulerAngles.x;
        posY = car.transform.eulerAngles.y;
        posZ = car.transform.eulerAngles.z;

        dCam.transform.eulerAngles = new Vector3(posX - posX, posY, posZ - posZ); // Stabilizes default camera
        fCam.transform.eulerAngles = new Vector3(posX - posX, posY, posZ - posZ); // Stabilizes far camera

        GetInput();
    }


    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.C)) { // When the player presses C, hcanges the camera
            if (camNum == 0) // Normal distance
            {
                dCam.enabled = true;
                nCam.enabled = false;
                fCam.enabled = false;
                bCam.enabled = false;
                camNum += 1; // Increments the camera number to know which camera is being used in the rotation
            }
            else if (camNum == 1) // Near distance
            {
                dCam.enabled = false;
                nCam.enabled = true;
                fCam.enabled = false;
                bCam.enabled = false;
                camNum += 1;
            }

            else // Far distance
            {
                fCam.transform.eulerAngles = new Vector3(posX - posX, posY, posZ - posZ);

                dCam.enabled = false;
                nCam.enabled = false;
                fCam.enabled = true;
                bCam.enabled = false;
                camNum = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2)) // If the player presses the middle mouse button for the back camera
        {
            bCam.enabled = true;
            dCam.enabled = false;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse2)) // If the player stops pressing the mouse button it sets the active to default
        {
            bCam.enabled = false;
            dCam.enabled = true;
        }        
    }


    // I tried making a camera searcher which would take the name of the camera as input, it would then activate it and deactivate all others
    /*private void FindCamera(string camName)
    {
        foreach (Camera c in Camera.allCameras) // for each camera in existing
        {
            if (camName == c.name) // If the camera matches the name given
            {
                Debug.Log(c.name);
                if (c.gameObject.active != true) // If it's not active
                {
                    c.gameObject.SetActive(true); // Activate it
                }                
            }
            else // For all the other cameras that dont match the name
            {
                if(c.gameObject.active != false) // If they aren't deactivated
                {
                    c.gameObject.SetActive(false); // Deactivate them
                }                
            }
        }
    }*/
}
