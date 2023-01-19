using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_controller : MonoBehaviour
{
    public GameObject wheel;
    private Car_controller car;

    public Material gold;

    public Renderer entity1;
    public Renderer entity2;
    public Renderer entity3;
    public Renderer entity4;
    public Renderer entity5;

    private float speed = 100f;
    private float bounceSpeed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<Car_controller>();

        entity1.material = gold;
        entity2.material = gold;
        entity3.material = gold;
        entity4.material = gold;
        entity5.material = gold;
    }

    void Update() {
        Rotate();
    }

    private void Rotate()   // Adds and upwards and downards motion + a spinning moting
    {
        wheel.transform.Rotate(Vector3.up * (speed * Time.deltaTime));

        Vector3 pos = wheel.transform.position;

        float y = Mathf.Sin(Time.time * bounceSpeed) / 70;

        wheel.transform.position = new Vector3(pos.x, y + wheel.transform.position.y, pos.z);
    }


    private void OnTriggerEnter(Collider obj)
    {
        car.itemAcquired = true;
        Destroy(wheel);
    }
}
