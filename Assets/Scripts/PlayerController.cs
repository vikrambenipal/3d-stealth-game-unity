using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed; 


    // Update is called once per frame
    void Update()
    {
        // normalize since this is a direction 
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // if the vector has magnitude of 0 the player can't move 
        float inputMagnitude = inputDirection.magnitude; 

        // angle of rotation based on x and z axis 
        float targetAngle =  Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        // rotate player along the y-axis 
        transform.eulerAngles = Vector3.up * targetAngle;

        // moves object automatically along the z-axis (direction * distance * magnitude) 
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * inputMagnitude, Space.World);
    }
}
