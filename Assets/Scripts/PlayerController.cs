using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8;

    float angle; 
    float smoothInputMagnitude;
    float smoothMoveVelocity; 


    // Update is called once per frame
    void Update()
    {
        // normalize since this is a direction 
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // if the vector has magnitude of 0 the player can't move 
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

        // angle of rotation based on x and z axis 
        float targetAngle =  Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        // multiply by inputMagnitude so it will stop interpolating the angle once the player lets go of the key
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude); 

        // rotate player along the y-axis 
        transform.eulerAngles = Vector3.up * angle;

        // moves object automatically along the z-axis (direction * distance * magnitude) 
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude, Space.World);
    }
}
