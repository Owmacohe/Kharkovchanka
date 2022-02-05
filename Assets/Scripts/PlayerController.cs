using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Range(0.05f, 0.5f)]
    public float movementSpeed = 0.15f;
    public float rotationSpeed;
    public Transform vehicle;

    private Vector2 inputVal;
    private Vector3 isoUp, isoRight;
    private bool isUp, isDown, isRight, isLeft;

    private void Start()
    {
        isoUp = new Vector3(-1, 0, 1);
        isoRight = new Vector3(1, 0, 1);
    }

    private void OnMove(InputValue input)
    {
        inputVal = input.Get<Vector2>();

        if (inputVal.y > 0)
        {
            isUp = true;
            isDown = false;
        }
        else if (inputVal.y < 0)
        {
            isUp = false;
            isDown = true;
        }
        else
        {
            isUp = false;
            isDown = false;
        }

        if (inputVal.x > 0)
        {
            isRight = true;
            isLeft = false;
        }
        else if (inputVal.x < 0)
        {
            isRight = false;
            isLeft = true;
        }
        else
        {
            isRight = false;
            isLeft = false;
        }
    }

    private void FixedUpdate()
    {
        if (inputVal != Vector2.zero)
        {
            transform.position += ((isoRight * inputVal.x) + (isoUp * inputVal.y)) * movementSpeed;

            float tempAngle = vehicle.transform.localRotation.eulerAngles.y;
            float targetAngle = 0;

            if (isUp)
            {
                if (isRight) { targetAngle = 270; }
                else if (isLeft) { targetAngle = 180; }
                else { targetAngle = 225; }
            }
            else if (isDown)
            {
                if (isRight) { targetAngle = 1; }
                else if (isLeft) { targetAngle = 90; }
                else { targetAngle = 45; }
            }
            else if (isRight)
            {
                if (isUp) { targetAngle = 270; }
                else if (isDown) { targetAngle = 1; }
                else { targetAngle = 315; }
            }
            else if (isLeft)
            {
                if (isUp) { targetAngle = 180; }
                else if (isDown) { targetAngle = 90; }
                else { targetAngle = 135; }
            }

            if (tempAngle > 225)
            {
                if (targetAngle > tempAngle || targetAngle <= 45)
                {
                    if (targetAngle > 45 && tempAngle + rotationSpeed > targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle += rotationSpeed;
                    }
                }
                else if (targetAngle < tempAngle)
                {
                    if (tempAngle - rotationSpeed < targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle -= rotationSpeed;
                    }
                }
            }
            else if (tempAngle <= 225 && tempAngle > 45)
            {
                if (targetAngle > tempAngle)
                {
                    if (tempAngle + rotationSpeed > targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle += rotationSpeed;
                    }
                }
                else
                {
                    if (tempAngle - rotationSpeed < targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle -= rotationSpeed;
                    }
                }
            }
            else
            {
                if (targetAngle < tempAngle || targetAngle > 225)
                {
                    if (targetAngle <= 225 && tempAngle - rotationSpeed < targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle -= rotationSpeed;
                    }
                }
                else if (targetAngle > tempAngle)
                {
                    if (tempAngle + rotationSpeed > targetAngle)
                    {
                        tempAngle = targetAngle;
                    }
                    else
                    {
                        tempAngle += rotationSpeed;
                    }
                }
            }

            if (tempAngle == 45)
            {
                tempAngle = 44;
            }

            vehicle.transform.localRotation = Quaternion.Euler(Vector3.up * tempAngle);
        }
    }
}
