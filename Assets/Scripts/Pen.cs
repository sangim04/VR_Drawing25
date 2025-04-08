using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Pen : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip; // 펜 끝 위치
    public Material drawingMaterial; //펜 재질
    public Material tipMaterial; // 펜 색상

    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f; //펜 너비
    public Color[] penColors; // 색상 배열

    [Header("XR Components")]
    public XRGrabInteractable grabbable; // XR Interaction Toolkit의 잡기 기능
    public HandController rightHandController; // 오른손 컨트롤러
    public HandController leftHandController; // 왼손 컨트롤러

    private LineRenderer currentDrawing; // 현재 그리는 선
    private int index;
    private int currentColorIndex;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];
    }

    private void Update()
    {
        bool isGrabbed = grabbable.isSelected;
        bool isDrawing = ((rightHandController != null && rightHandController.isTrigger) || (leftHandController != null && leftHandController.isTrigger)); 

        if (isGrabbed && isDrawing)
        {
            Draw();
            Debug.Log($"[Pen] isGrabbed: {grabbable.isSelected}, leftTrigger: {leftHandController?.isTrigger}");
        }
        else if (currentDrawing != null)
        {
            currentDrawing = null;
        }

        if (rightHandController != null && rightHandController.isPrimaryPressed)
        {
            SwitchColor();
        }
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;
            currentDrawing = new GameObject("DrawingLine").AddComponent<LineRenderer>();
            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = penColors[currentColorIndex];
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);
        }
        else
        {
            var currentPos = currentDrawing.GetPosition(index);
            if (Vector3.Distance(currentPos, tip.position) > 0.01f)
            {
                index++;
                currentDrawing.positionCount = index + 1;
                currentDrawing.SetPosition(index, tip.position);
            }
        }
    }

    public void SwitchColor()
    {
        currentColorIndex = (currentColorIndex + 1) % penColors.Length;
        tipMaterial.color = penColors[currentColorIndex];
    }
}
