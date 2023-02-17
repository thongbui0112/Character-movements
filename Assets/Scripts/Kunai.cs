using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject kunaiPar;
    Rigidbody rb;
    //ThrowKunai throw;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject par = Instantiate(kunaiPar,gameObject.transform.position,Quaternion.identity);
       //kunaiPar.transform.position = collision.gameObject.transform.position;
        par.SetActive(true);
        rb.velocity = Vector3.zero;
        Destroy(gameObject,0.2f);
        Destroy(par,0.2f);
    }
    public void Death()
    {
        Destroy(gameObject,4f);
    }
}
