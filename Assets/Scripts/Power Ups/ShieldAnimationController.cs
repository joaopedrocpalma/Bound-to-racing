using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShieldAnimationController : MonoBehaviour
{
    private GameObject car;

    private Renderer shield;

    private float posX, posY, posZ;

    private float scaleX, scaleY;   

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");

        shield = GetComponent<Renderer>();
    }

    void Update()
    {
        posX = car.transform.eulerAngles.x;
        posY = car.transform.eulerAngles.y;
        posZ = car.transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(-90, posY, posZ);

        scaleX = Mathf.Cos(Time.time) * 100 - 10;
        scaleY = Mathf.Cos(Time.time) * 100 - 10;
        shield.material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));

        transform.position = car.transform.position;
    }
}
