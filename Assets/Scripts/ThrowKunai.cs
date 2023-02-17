using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKunai : MonoBehaviour
{
    public Transform kunaiPos;
    public Transform cam;
    public GameObject kunaiPrefabs;
    public float throwSpeed;
    public Vector3 hitPoint;
    public float maxDistance;
    public LayerMask layer;
    public float delayTime;
    //public float timer;
    public bool canThrow;
    public float countKunai;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && canThrow && countKunai > 0)
        {   canThrow = false; 
            StartThrow();
            Invoke("ResetTime",delayTime);
        }
        else CancelInvoke("StartThrow");
    }
    public void ResetTime()
    {
        canThrow = true;
    }
    private void StartThrow()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, layer))
        {
            hitPoint = hit.point;
        }
        else hitPoint = cam.position + cam.forward * maxDistance * Random.Range(0.6f,1f);
        CreatKunai();
    }
    public void CreatKunai()
    {
        GameObject kunai = Instantiate(kunaiPrefabs, kunaiPos.position, Quaternion.identity);
        kunai.transform.LookAt(hitPoint);
        rb=kunai.GetComponent<Rigidbody>();
        rb.AddForce((hitPoint-kunai.transform.position)*throwSpeed,ForceMode.Impulse);
        countKunai--;
        if(countKunai==0)
        {
            canThrow = false;
            CancelInvoke("StartThrow");
            Invoke("ResetCount",20f);

        }
    }
    public void ResetCount()
    {
        canThrow = true;
        countKunai = 100;
    }
}
