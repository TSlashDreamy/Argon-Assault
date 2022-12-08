using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=== new input system ===//
//using UnityEngine.InputSystem;
//[SerializeField] InputAction movement;

//void OnEnable()
//{
//    movement.Enable();   
//}

//void OnDisable()
//{
//    movement.Disable();    
//}

// on Update
//=== new input system ===//
//float horizontalThrow = movement.ReadValue<Vector2>().x;
//float verticalThrow = movement.ReadValue<Vector2>().y;
//=== ~new input system ===//

public class PlayerControls : MonoBehaviour
{
    [Header("General setup settings")]
    [Tooltip("How fast player is moving (left/right)(up/down)")][SerializeField] float controlSpeed = 30f;
    [Tooltip("Range for ship to reach (left/right)")][SerializeField] float xRange = 8.5f;
    [Tooltip("Range for ship to reach (up/down)")][SerializeField] float yRange = 6.5f;

    [Header("Laser gun array")]
    [Tooltip("Input all laser gameobjects here")][SerializeField] GameObject[] lasers;

    [Header("Screen position based tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Screen input based tuning")]
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float controlRollFactor = -20f;

    float xThrow, yThrow;


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessShooting();
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessTranslation()
    {
        //=== old input system ===//
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");
        //=== ~old input system ===//

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * controlSpeed * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessShooting()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    void SetLaserActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

    
}
