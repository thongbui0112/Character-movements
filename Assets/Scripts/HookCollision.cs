using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCollision : MonoBehaviour
{
    Grappling grapple;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grapple = FindObjectOfType<Grappling>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        grapple.HookCollision();
    }
}
