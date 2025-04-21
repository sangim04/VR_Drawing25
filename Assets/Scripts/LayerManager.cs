using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

public class LayerManager : MonoBehaviour
{
    public InputActionReference primaryButton;      // A 버튼 
    public InputActionReference secondaryButton;    // B 버튼 
    
    public int layerCount;                  // 레이어 갯수
    private int setactiveLayerIndex = 0;    // 가시성 count
    private int addLayerIndex = 0;          // 추가 count
    // 중간에 layer 추기 기능 아직 없음
    // 레이어 삭제 기능 아직 없음
    
    private static List<List<GameObject>> currentLayer = new List<List<GameObject>>();    // 현재 레이어
    
    /*
     * setactiveLayerIndex는 몇번째 레이어가 보이게 할지 안보이게 할지
     * addLayerIndex는 몇번 레이어에 선을 추가할지
     */

    void Start()
    {
        primaryButton.action.started += OnPrimaryStarted; // A 버튼 
        //primaryButton.action.canceled += OnPrimaryReleased; // A 버튼 
        secondaryButton.action.started += OnSecondaryStarted; // B 버튼 
        //secondaryButton.action.canceled += OnSecondaryReleased; // B 버튼 
        
        for (int i = 0; i < layerCount; i++)    // layerCount만큼 레이어 생성
        {
            currentLayer.Add(new List<GameObject>());
        }
    }

    public void AddLayer(GameObject layer)  // 현재 레이어 List에 추가
    {
        currentLayer[addLayerIndex].Add(layer);
    }

    private void LayerSetActive()   // 레이어 가시성
    {
        setactiveLayerIndex++;
        if(setactiveLayerIndex >= currentLayer.Count) setactiveLayerIndex = 0;
        
        if (setactiveLayerIndex == 0)   // 전부
        {
            foreach (var var in currentLayer)
            {
                foreach (var obj in var)
                {
                    obj.SetActive(!obj.activeSelf); // 보이면 안보이게, 안보이면 보이게
                }
            }
        }
        else    // setactiveLayerIndex번째 레이어
        {
            foreach (var obj in currentLayer[setactiveLayerIndex])
            {
                obj.SetActive(!obj.activeSelf); // 보이면 안보이게, 안보이면 보이게
            }
        }
    }

    void OnPrimaryStarted(InputAction.CallbackContext context)
    {
        addLayerIndex++;
        if (addLayerIndex >= layerCount) addLayerIndex = 0;
    }

    void OnPrimaryReleased(InputAction.CallbackContext context)
    {
        //Debug.Log("Primary Released");
    }
    
    void OnSecondaryStarted(InputAction.CallbackContext context)
    {
        LayerSetActive();
    }

    void OnSecondaryReleased(InputAction.CallbackContext context)
    {
        //Debug.Log("Secondary Released");
    }
}

