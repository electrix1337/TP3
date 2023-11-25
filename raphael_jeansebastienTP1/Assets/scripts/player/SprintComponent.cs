using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(playerMovement))]
public class SprintComponent : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAsset;
    [SerializeField] float sprintTime = 5;
    [SerializeField] float regenSpeed = 0.5f;
    [SerializeField] GameObject staminaBar;
    [SerializeField] GameObject textObject;
    TextMeshProUGUI text;
    float timeLeft;
    public bool sprinting
    {
        get;
        private set;
    }

    private void Start()
    {
        timeLeft = sprintTime;
        text = textObject.GetComponent<TextMeshProUGUI>();

        InputActionMap inputMap = inputAsset.FindActionMap("player");
        InputAction sprintAction = inputMap.FindAction("sprint");

        sprintAction.performed += Sprint;
        sprintAction.canceled += Sprint;
    }
    void Sprint(InputAction.CallbackContext action)
    {
        sprinting = action.ReadValue<float>() != 0;
    }
    void ShowStamina()
    {
        float stamina = timeLeft / sprintTime * 100;
        text.text = ((int)stamina).ToString() + "/100";
        staminaBar.transform.localPosition = new Vector3((stamina - 100) / 2, 0, 0);
        staminaBar.transform.localScale = new Vector3(timeLeft / sprintTime, 1, 1);
    }
    private void Update()
    {
        float time = Time.deltaTime;
        if (sprinting)//check if the sprint key is press
        {
            if (timeLeft - time >= 0)//check if the time left is enought to continu sprinting
                timeLeft -= time;
            else
            {
                sprinting = false;
                timeLeft = 0;
            }
            ShowStamina();
        }
        else if (timeLeft != sprintTime)//if not sprinting regen sprint time
        {
            timeLeft += regenSpeed * time;
            if (timeLeft > sprintTime)
            {
                timeLeft = sprintTime;
            }
            ShowStamina();
        }
    }
}
