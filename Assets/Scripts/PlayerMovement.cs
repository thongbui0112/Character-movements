using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Text speedText;
    public Image sprintBar;
    [Header("Movement")]
    public float moveSpeed;
    public float jumpSpeed;
    public float airMutipiler;
    public float sprintSpeed;
    public float walkSpeed;

    public bool canSprint;
    public float timeSprint;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("CheckGround")]
    public LayerMask groundMask;
    private float groundDistance;
    public Transform groundCheck;
    public bool isGrounded;
    
    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    
    [Header("Stair")]
    public Transform rayUpper, rayLower;
    public bool onstair;
    public LayerMask stairMask;
    public enum MovementState
    {
        walking, 
        sprinting,
        airing
    }
    public MovementState state;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundDistance = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        StateControl();
        MyInput();
        SpeedControl();       
        CooldownSprinting();
        OnSlope();
        OnStair();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void StateControl()
    {
        if (Input.GetKey(KeyCode.RightShift)&&canSprint)
        {
            timeSprint -= 2 * Time.deltaTime;
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state=MovementState.airing;
        }
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void MovePlayer()
    {
        moveDirection = transform.TransformDirection(horizontalInput,0,verticalInput);
        moveDirection.Normalize();
        Jump();
        
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection()*moveSpeed * Time.deltaTime*800f,ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 500f* airMutipiler, ForceMode.Force);
        }
        else rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 500f, ForceMode.Force);
       // rb.useGravity = !OnSlope();
    }
    private void CooldownSprinting()
    {
        if (timeSprint >= 0 && timeSprint <= 10&&!Input.GetKey(KeyCode.RightShift))
        {
            timeSprint += 1/2f *Time.deltaTime;
        }
        if (timeSprint > 0) canSprint = true;
        else canSprint = false;
        if (timeSprint > 10) timeSprint = 10;
        if (timeSprint < 0) timeSprint = 0;
        sprintBar.fillAmount = Mathf.MoveTowards(sprintBar.fillAmount, timeSprint/10f, Time.deltaTime);
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
        }
    }
    private void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(groundCheck.position,-groundCheck.up,out hit, groundDistance, groundMask);
        SetDrag();
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(groundCheck.position,-groundCheck.up,out slopeHit, groundDistance, groundMask))
        {   
            float angle= Vector3.Angle(transform.up,slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private void SetDrag()
    {
        if (isGrounded) rb.drag = 5f;
        else rb.drag = 0f;
    }
    private void SpeedControl()
    {

        if (rb.velocity.x*rb.velocity.x+rb.velocity.z*rb.velocity.z > moveSpeed*moveSpeed)
        {
            float y = rb.velocity.y;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, moveSpeed);
            rb.velocity = new Vector3(rb.velocity.x,y,rb.velocity.z);            
        }
        float a = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
        speedText.text = "Speed : " + a.ToString();
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnStair()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(rayLower.position, rayLower.forward, out hitLower, 0.8f,stairMask) && moveDirection != Vector3.zero&&!OnSlope())   
        {float speed= 15*Time.deltaTime;
            RaycastHit hitUpper;
            if (!Physics.Raycast(rayUpper.position, rayUpper.forward, out hitUpper, 0.95f,stairMask))
            {
                RaycastHit hit;
                //   transform.position += new Vector3(0f, highStep, -highStep/10f);
                for (float i = 0; i <= 0.9f; i += 0.02f)
                {
                    if (!Physics.Raycast(rayLower.position + new Vector3(0, i, 0), rayLower.forward, out hit, 0.8f,stairMask))
                    { 
                     
                        onstair = true;
                        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up * i,speed);
                        return;
                    }
                }
            }
        }
        onstair = false;
    }

}
