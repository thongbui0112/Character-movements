using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public AnimationCurve curve;
    public Transform checkpoint;
    public float maxDistance;
    public LayerMask layerMask;
    public Vector3 hitPoint;
    public Transform leftHook;
    public Transform leftHookPos;
    public float hookSpeed;
    public float moveToHookSpeed;
    public float acceleration;
    private float a;
    Rigidbody rb, rbLeft;
    public bool canHook;
    public bool grappled;
    public bool isParent;
    public bool hookCollision;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {   
        canHook = true;
        grappled = false;
        isParent = true;
        hookCollision = false;
        rb=GetComponent<Rigidbody>();
        rbLeft = leftHook.GetComponent<Rigidbody>();
        a = acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl)&&canHook) StartGrapple();
        Grappled();
       // HookCollision();
        if (hookCollision) Invoke("MoveToHook", 0.2f);
        Reparent();
    }
    private void LateUpdate()
    {
        SetLine();
    }
    private void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, maxDistance, layerMask)&&canHook){
            hitPoint = hit.point;
            canHook = false;
            grappled =true;
            rb.velocity = Vector3.zero;
        }
    }
    public void Grappled()
    {
        if (grappled)
        {

            leftHook.parent = null;
            leftHook.LookAt(hitPoint);
            rbLeft.isKinematic = false;
            rbLeft.AddForce((hitPoint - leftHook.position)*hookSpeed,ForceMode.Impulse);
            lineRenderer.enabled = true;
            isParent = false;
            grappled=false;
        }
    }
    public void HookCollision()
    {
            hookCollision = true;
    }
    private void MoveToHook()
    {
        if (hookCollision)
        {
            //rb.isKinematic = true;
            rb.useGravity = false;
            moveToHookSpeed = Vector3.Distance(transform.position, hitPoint) * Time.deltaTime * a;
            transform.position = Vector3.MoveTowards(transform.position, hitPoint+2*transform.forward, moveToHookSpeed);
            if(a>3f) a =a - 2.2f*a * Time.deltaTime;
        }
    }
    private void Reparent()
    {
        if (!isParent && Vector3.Distance(transform.position,hitPoint)<2f)
        {
            hookCollision = false;
            leftHook.parent = leftHookPos;
            leftHook.localPosition = Vector3.zero;
            leftHook.localRotation = Quaternion.Euler(Vector3.zero);
            leftHook.localScale = new Vector3(1.5f, 1.5f, 1);
            lineRenderer.enabled = false;
            isParent = true;
            //  rb.isKinematic=false;
            rb.useGravity = true;
            Invoke("ResetGrapple", 0.2f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Reparent();
    }
    private void ResetGrapple()
    {
        canHook = true;
        a = acceleration;
    }
    public void SetLine()
    {
        lineRenderer.SetPosition(0, leftHookPos.position);
        lineRenderer.SetPosition(1, leftHook.position);

    }
}
