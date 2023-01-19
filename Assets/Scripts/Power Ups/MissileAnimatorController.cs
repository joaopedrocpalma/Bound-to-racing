using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR.WSA;
using Vector3 = UnityEngine.Vector3;

public class MissileAnimatorController : MonoBehaviour
{
    private Buff_controller buff;
    private Car_controller car;
    private AI_controller ai;

    private float speed = 200f;

    private int damage = 100;

    // Start is called before the first frame update
    void Start()
    {
        buff = GameObject.FindGameObjectWithTag("Player").GetComponent<Buff_controller>();
        car = GameObject.FindGameObjectWithTag("Player").GetComponent<Car_controller>();
        ai = GameObject.FindGameObjectWithTag("Ai").GetComponent<AI_controller>();

        transform.Rotate(90, 0, 0); // To amke the missile face correctly
    }

    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter(Collider obj) // If the gameObject it colldies with is either a player or an AI
    {
        Destroy(gameObject);

        if (obj.gameObject.tag == "Player")
        {
            car.health -= damage; // Deals Damage
        }

        if (obj.gameObject.tag == "AI")
        {
            ai.health -= damage;
        }

        if (gameObject.activeSelf)
        {
            Invoke(nameof(DestroyMissile), 8);
        }
    }

    private void Movement() // Animates the missile with a routation and makes it go forward
    {        
        transform.Translate(0, (speed / 2) * Time.deltaTime, 0);
        transform.Rotate(Vector3.up * (speed * Time.deltaTime));
    }

    private void DestroyMissile()
    {
        Destroy(gameObject);
    }
}
