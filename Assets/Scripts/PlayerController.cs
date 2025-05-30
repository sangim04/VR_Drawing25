using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class PlayerController : MonoBehaviour
{
    public InputActionReference primaryButton;
    public bool isPrimaryPressed; // X 버튼 상태 변수
    public GameObject player;

    private float upSpeed = 0.02f;
    private bool isDownDelay;
    private bool isDown;
    
    void Start()
    {
        primaryButton.action.started += OnPrimaryStarted;   // X 버튼 
        primaryButton.action.canceled += OnPrimaryReleased; // X 버튼 
        isPrimaryPressed = false;
        
        player = GameObject.FindGameObjectWithTag("Player");    // XR Origin에 player태그 설정해야함
    }

    private void FixedUpdate()
    {
        if (isPrimaryPressed)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + (isDown ? -upSpeed:upSpeed), player.transform.position.z);
            /*
             * 위 아래 이동은 하는데 왼쪽 썸스틱으로 이동하면 떨어져요 : 밑을 보시오
             * Locomotion  ->  Move  ->  Dynamic Move Provider 스크립트  ->  Awake 함수 안에 'useGravity = false;'를 넣어주세요
             * Continuous Move Provider에 중력 계산이 있고 Dynamic Move Provider은 그걸 상속받습니다.
             */
        }
    }
    
    void OnPrimaryStarted(InputAction.CallbackContext context)
    {
        isPrimaryPressed = true;
        if (isDownDelay) isDown = true;         // isDownDelay가 true일 경우 내려감
        else StartCoroutine(UpDown());    // isDownDelay가 false일 경우 n초동안 true가 되게 만드는 코루틴 호출
    }

    void OnPrimaryReleased(InputAction.CallbackContext context)
    {
        isPrimaryPressed = false;
        isDown = false;
    }

    IEnumerator UpDown()    // 0.3f초동안 isDownDelay가 유지됨
    {
        isDownDelay = true;
        yield return new WaitForSeconds(0.3f);
        isDownDelay = false;
    }


}
