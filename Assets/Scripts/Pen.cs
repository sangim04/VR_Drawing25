using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UI; 

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

    [Header("UI Elements")]
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public Button applyColorButton; 
    public Image colorPreviewImage;
    public Slider penWidthSlider;

    // public LayerManager lm;

    private LineRenderer currentDrawing; // 현재 그리는 선
    private int index;
    private int currentColorIndex;

    private void Start()
    {
        currentColorIndex = 0;
        tipMaterial.color = penColors[currentColorIndex];

        if (applyColorButton != null)
            applyColorButton.onClick.AddListener(ApplyColorFromSlider); // 버튼에 이벤트 연결

        // 슬라이더 값이 바뀔 때마다 미리보기 색상도 변경
        if (redSlider != null) redSlider.onValueChanged.AddListener(_ => UpdatePreviewColor());
        if (greenSlider != null) greenSlider.onValueChanged.AddListener(_ => UpdatePreviewColor());
        if (blueSlider != null) blueSlider.onValueChanged.AddListener(_ => UpdatePreviewColor());

        UpdatePreviewColor(); // 초기 색도 미리 적용
        if (penWidthSlider != null)
            UpdatePenWidth(penWidthSlider.value); // 초기 펜 굵이 반영
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

        if (penWidthSlider != null)
            UpdatePenWidth(penWidthSlider.value);
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;
            currentDrawing = new GameObject("DrawingLine").AddComponent<LineRenderer>();
            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = tipMaterial.color;
            currentDrawing.startWidth = currentDrawing.endWidth = penWidth;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);
            // LayerManager의 AddLayer() 함수 호출
            // lm.AddLayer(currentDrawing.gameObject, 0);  // 예시
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

    //public void SwitchColor()
    //{
    //    currentColorIndex = (currentColorIndex + 1) % penColors.Length;
    //    tipMaterial.color = penColors[currentColorIndex];
    //}

    private void ApplyColorFromSlider()
    {
        if (redSlider == null || greenSlider == null || blueSlider == null) return;

        Color selectedColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        tipMaterial.color = selectedColor;
        drawingMaterial.color = selectedColor;
    }

    private void UpdatePreviewColor()
    {
        if (redSlider == null || greenSlider == null || blueSlider == null || colorPreviewImage == null) return;

        Color previewColor = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        colorPreviewImage.color = previewColor;
    }

    private void UpdatePenWidth(float value)
    {
        penWidth = value;
    }
}
