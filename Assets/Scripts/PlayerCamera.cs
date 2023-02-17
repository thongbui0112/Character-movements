using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitive;
    public float ySensitive;

    public Transform player;





    private float xRotation;
    private float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X")*Time.deltaTime*xSensitive;
        float mouseY = Input.GetAxisRaw("Mouse Y")*Time.deltaTime *ySensitive;

        xRotation -= mouseY;
        xRotation= Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation ,0,0);
        player.transform.Rotate(mouseX * Vector3.up);

    }
}
