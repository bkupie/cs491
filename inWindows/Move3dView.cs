using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3dView : MonoBehaviour
{
    public float maxAcceleration = 5f;

    private float acceleration = 0.05f;
    private float movementSpeed = 0f;
    private float movementSpeed2 = 0f;
    private Vector2 touchAxis;
    private float triggerAxis;
    private Rigidbody rb;
    private Vector3 defaultPosition;

    public void SetTouchAxis(Vector2 data)
    {
        touchAxis = data;
    }

    public void SetTriggerAxis(float data)
    {
        triggerAxis = data;
    }

    public void ResetPos()
    {
        transform.position = defaultPosition;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultPosition = transform.position;
    }

    private void FixedUpdate()
    {
        CalculateSpeed();
        Move();
    }

    private void CalculateSpeed()
    {
        if (touchAxis.y != 0f)
        {
            movementSpeed += (acceleration * touchAxis.y);
            //movementSpeed = Mathf.Clamp(movementSpeed, -maxAcceleration, maxAcceleration);
        }
        else
        {
            Decelerate();
        }


        if (touchAxis.x != 0f)
        {
            movementSpeed2 += (acceleration * touchAxis.x);
            //movementSpeed2 = Mathf.Clamp(movementSpeed2, -maxAcceleration, maxAcceleration);
        }
        else
        {
            Decelerate2();
        }
    }

    private void Decelerate()
    {
        if (movementSpeed > 0)
        {
            //movementSpeed -= Mathf.Lerp(acceleration, maxAcceleration, 0f);
            movementSpeed = 0;
        }
        else if (movementSpeed < 0)
        {
            //movementSpeed += Mathf.Lerp(acceleration, -maxAcceleration, 0f);
            movementSpeed = 0;
        }
        else
        {
            movementSpeed = 0;
        }
    }

    private void Decelerate2()
    {
        if (movementSpeed2 > 0)
        {
            //movementSpeed2 -= Mathf.Lerp(acceleration, maxAcceleration, 0f);
            movementSpeed2 = 0;
        }
        else if (movementSpeed2 < 0)
        {
            //movementSpeed2 += Mathf.Lerp(acceleration, -maxAcceleration, 0f);
            movementSpeed2 = 0;
        }
        else
        {
            movementSpeed2 = 0;
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
        Vector3 movement2 = transform.right * movementSpeed2 * Time.deltaTime;
        rb.MovePosition(rb.position + movement + movement2);
    }

    //private void OnTriggerStay(Collider collider)
    //{
    //    isJumping = false;
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    isJumping = true;
    //}
}
