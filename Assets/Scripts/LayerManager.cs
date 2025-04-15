using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public List<GameObject> layersZero;    // 0번 레이어
    private List<GameObject> layersOne;     // 1번 레이어
    private bool layersZeroFlag = true;     // 초기값은 보이게
    private bool layersOneFlag = true;
    // public bool test;

    void Start()
    {
        layersZero = new List<GameObject>();
        layersOne = new List<GameObject>();
    }

    void FixedUpdate()
    {
        /*
        if (test)
        {
            test = false;
            LayerSetActive(0);
        }
        */
    }

    public void AddLayer(GameObject layer, int layerNum)
    {
        switch (layerNum)
        {
            case 0:
                layersZero.Add(layer);
                break;
            case 1:
                layersOne.Add(layer);
                break;
            default:
                Debug.Log("Layer Number Not Found");
                break;
        }
    }

    public void LayerSetActive(int layerNum)
    {
        switch (layerNum)
        {
            case 0:
                foreach (var obj in layersZero)
                {
                    obj.SetActive(!layersZeroFlag);
                }
                layersZeroFlag = !layersZeroFlag;
                break;
            case 1:
                foreach (var obj in layersOne)
                {
                    obj.SetActive(!layersOneFlag);
                }
                layersOneFlag = !layersOneFlag;
                break;
            default:
                Debug.Log("Layer Number Not Found");
                break;
        }
    }
}

