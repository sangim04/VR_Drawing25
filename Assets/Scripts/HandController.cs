using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandController : MonoBehaviour
{
    public InputActionReference triggerButton;
    public InputActionReference primaryButton;

    public bool isTrigger;
    public bool isPrimaryPressed; // A 버튼 상태 변수

    public Pen pen;
    
    public GameObject player;

    private void Start()
    {
        triggerButton.action.performed += OnTriggerPressed;
        triggerButton.action.canceled += OnTriggerReleased;

        primaryButton.action.performed += OnPrimaryPressed; // A 버튼 
        primaryButton.action.canceled += OnPrimaryReleased; // A 버튼 

        isPrimaryPressed = false;
        isTrigger = false;
        
        player = GameObject.FindGameObjectWithTag("Player");    // XR Origin에 player태그 설정해야함
    }

    private void FixedUpdate()
    {
        if (isPrimaryPressed)
        {
            // player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);  // 스피드 변수 x (임의)
            
        }
    }

    void OnTriggerPressed(InputAction.CallbackContext context)
    {
        isTrigger = true;
        Debug.Log($"[{gameObject.name}] Trigger pressed: {context.control}");
    }

    void OnTriggerReleased(InputAction.CallbackContext context)
    {
        isTrigger = false;
    }

    void OnPrimaryPressed(InputAction.CallbackContext context)
    {
        isPrimaryPressed = true;
    }

    void OnPrimaryReleased(InputAction.CallbackContext context)
    {
        isPrimaryPressed = false;
    }
}
