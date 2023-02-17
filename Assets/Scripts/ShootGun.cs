using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGun : MonoBehaviour
{
    public float dame = 10f;
    public Transform cam;
    public float maxdistance;
    public KeyCode shoot;
    private void Update()
    {
        if(Input.GetKeyDown(shoot) )   {
            Shoot();
        }
    }
    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxdistance))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(dame);
            }
        }
    }
}
