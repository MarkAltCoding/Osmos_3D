using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GetComponent<Rigidbody>().linearVelocity=new Vector3(Random.value,Random.value,Random.value);
        Instantiate(this);
    }
}
