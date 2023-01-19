using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GlobalPos : MonoBehaviour
{
    private GameObject tracker;

    private Vector3 carPos;


    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.FindGameObjectWithTag("Tracker");
    }

    // Update is called once per frame
    void Update()
    {
        carPos = transform.position;
        tracker.transform.position = new Vector3(transform.position.x, transform.position.y + 120, transform.position.z);
    }
}
