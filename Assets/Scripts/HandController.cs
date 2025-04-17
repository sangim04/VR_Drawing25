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
    public bool isPrimaryPressed; // A ��ư ���� ����

    public Pen pen;
    
    public GameObject player;

    private void Start()
    {
        triggerButton.action.performed += OnTriggerPressed;
        triggerButton.action.canceled += OnTriggerReleased;

        primaryButton.action.performed += OnPrimaryPressed; // A ��ư 
        primaryButton.action.canceled += OnPrimaryReleased; // A ��ư 

        isPrimaryPressed = false;
        isTrigger = false;
        
        player = GameObject.FindGameObjectWithTag("Player");    // XR Origin�� player�±� �����ؾ���
    }

    private void FixedUpdate()
    {
        if (isPrimaryPressed)
        {
            // player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);  // ���ǵ� ���� x (����)
            
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
