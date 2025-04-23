using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LayerManager : MonoBehaviour
{
    public InputActionReference primaryButton;      // A 버튼 
    public InputActionReference secondaryButton;    // B 버튼 
    
    public int layerCount = 2;              // 레이어 갯수
    private int setactiveLayerIndex = 0;    // 가시성 count
    private int addLayerIndex = 0;          // 추가 count
    // 중간에 layer 추기 기능 아직 없음
    // 레이어 삭제 기능 아직 없음
    // HandController 안씀
    
    private static List<List<GameObject>> currentLayer = new List<List<GameObject>>();    // 현재 레이어

    public Text layerText;         // 현재 그려질 레이어 text, Inspector창에서 넣어줘야 함
    
    /*
     * setactiveLayerIndex는 몇번째 레이어가 보이게 할지 안보이게 할지
     * addLayerIndex는 몇번 레이어에 선을 추가할지
     */

    void Start()
    {
        primaryButton.action.started += OnPrimaryStarted; // A 버튼 
        secondaryButton.action.started += OnSecondaryStarted; // B 버튼 
        
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
                // obj.SetActive(!obj.activeSelf); // 보이면 안보이게, 안보이면 보이게
                obj.SetActive(false);   //걍 해당 레이어 무조건 안보이게
            }
        }
    }

    void OnPrimaryStarted(InputAction.CallbackContext context)
    {
        addLayerIndex++;
        if (addLayerIndex >= layerCount) addLayerIndex = 0;
        
        /* 레이어를 두개 사용할때만 정상 작동함 임의로 만든 기능, 향후 수정 필요 */
        if(addLayerIndex == 1) layerText.text = "Current Layer : B";
        else layerText.text = "Current Layer : A";
    }
    
    void OnSecondaryStarted(InputAction.CallbackContext context)
    {
        LayerSetActive();
    }
}

