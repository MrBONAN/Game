using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private float speed;
    private float amount;
    // Start is called before the first frame update
    void Start()
    {
        speed = 30;
        amount = 1;
    }

    void FixedUpdate()
    {
         //how much it shakes
         transform.position = new Vector3(Mathf.Sin(Time.time * speed) * amount, Mathf.Cos(Time.time * speed) * amount, 0);
    }
}
