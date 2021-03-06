using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool disableControl;
    [Range(0.05f, 0.5f)]
    public float movementSpeed = 0.15f;
    [Range(5, 10)]
    public float rotationSpeed = 5;
    [Range(10, 25)]
    public float wiggleSpeed = 20;
    [Range(1, 5)]
    public float wiggleAmplitude = 3;
    [Range(1, 5)]
    public float idleFactor = 2.5f;
    public Transform vehicle;
    public bool playSound = true;
    public AudioClip idleSound, engineSound;
    [Range(0, 1)]
    public float idleVolume = 0.2f;
    [Range(0, 1)]
    public float engineVolume = 0.6f;
    [Range(1, 20)]
    public float min3DSoundDistance = 10;

    private bool isOnGround;
    private Vector2 inputVal;
    private Vector3 isoUp, isoRight;
    private bool isUp, isDown, isRight, isLeft;
    private Transform vehicleModel, cam;
    private float idleSpeed, idleAmplitude;
    private Rigidbody rb;
    private AudioSource idleSource, engineSource;
    private TerrainSpawner terrainSpawner;
    private GameController controller;

    private Vector2 lastCoordinate;

    private void Start()
    {
        isoUp = new Vector3(-1, 0, 1);
        isoRight = new Vector3(1, 0, 1);

        vehicleModel = vehicle.transform.GetChild(0);
        idleSpeed = wiggleSpeed / idleFactor;
        idleAmplitude = wiggleAmplitude / idleFactor;

        rb = GetComponent<Rigidbody>();

        idleSource = gameObject.AddComponent<AudioSource>();
        idleSource.loop = true;
        idleSource.volume = idleVolume;
        idleSource.clip = idleSound;
        idleSource.minDistance = min3DSoundDistance;
        idleSource.spatialBlend = 1;

        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.loop = true;
        engineSource.volume = engineVolume;
        engineSource.clip = engineSound;
        engineSource.minDistance = min3DSoundDistance;
        engineSource.spatialBlend = 1;

        if (playSound)
        {
            idleSource.Play();
            engineSource.Play();
        }

        terrainSpawner = FindObjectOfType<TerrainSpawner>();
        controller = FindObjectOfType<GameController>();
    }

    private void OnMove(InputValue input)
    {
        if (!disableControl)
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
    }

    private void OnInteract()
    {
        controller.triggerPOI();
    }

    private void Update()
    {
        if (!disableControl)
        {
            transform.rotation = Quaternion.identity;

            Vector2 coordinate = new Vector2(Mathf.Round(transform.position.x / 100f), Mathf.Round(transform.position.z / 100f));

            if (!lastCoordinate.Equals(coordinate))
            {
                terrainSpawner.spawnAround(coordinate);
            }
        }
    }

    private void FixedUpdate()
    {
        float tempSpeed, tempAmplitude;

        if (inputVal != Vector2.zero)
        {
            if (engineSource.volume < engineVolume + 0.3f)
            {
                engineSource.volume += 0.01f;
            }

            if (engineSource.pitch < 1.2f)
            {
                engineSource.pitch += 0.05f;
            }

            tempSpeed = wiggleSpeed;
            tempAmplitude = wiggleAmplitude;

            if (!disableControl)
            {
                if (isOnGround)
                {
                    rb.position += ((isoRight * inputVal.x) + (isoUp * inputVal.y)) * movementSpeed;
                }

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
        else
        {
            if (engineSource.volume > engineVolume)
            {
                engineSource.volume -= 0.01f;
            }

            if (engineSource.pitch > 1)
            {
                engineSource.pitch -= 0.01f;
            }

            tempSpeed = idleSpeed;
            tempAmplitude = idleAmplitude;
        }

        vehicleModel.transform.localRotation = Quaternion.Euler(Vector3.up * tempAmplitude * Mathf.Sin(tempSpeed * Time.time));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!disableControl)
        {
            if (collision.gameObject.layer == 6)
            {
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }
        }
    }
}
