using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Pen : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip; // �� �� ��ġ
    public Material drawingMaterial; //�� ����
    public Material tipMaterial; // �� ����

    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f; //�� �ʺ�
    public Color[] penColors; // ���� �迭

    [Header("XR Components")]
    public XRGrabInteractable grabbable; // XR Interaction Toolkit�� ��� ���
    public HandController rightHandController; // ������ ��Ʈ�ѷ�
    public HandController leftHandController; // �޼� ��Ʈ�ѷ�

    private LineRenderer currentDrawing; // ���� �׸��� ��
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
