using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UI;

public enum PenCapStyle
{
    Rounded,
    //Pointed
}

public class Pen : MonoBehaviour
{
    [Header("Pen Properties")]
    public Transform tip; // 펜 끝 위치
    public Material drawingMaterial; //펜 재질
    public Material tipMaterial; // 펜 색상
    public Material dotMaterial; //펜 재질

    [Range(0.01f, 0.1f)]
    public float penWidth = 0.01f; //펜 너비

    [Header("XR Components")]
    public XRGrabInteractable grabbable; // XR Interaction Toolkit의 잡기 기능
    public HandController rightHandController; // 오른손 컨트롤러
    public HandController leftHandController; // 왼손 컨트롤러

    [Header("UI Elements")]
    public ColorPicker colorPicker;
    public Slider penWidthSlider;

    [Header("Pen Style")]
    public PenCapStyle capStyle = PenCapStyle.Rounded;

    public LayerManager lm;

    public bool isDotted;
    private float scrollSpeed = 1f;
    List<LineRenderer> lines = new List<LineRenderer>();

    private LineRenderer currentDrawing; // 현재 그리는 선
    private int index;

    private void Start()
    {
        if (colorPicker != null)
        {
            colorPicker.onColorChanged += ApplyPickedColor;
            ApplyPickedColor(colorPicker.previewImage.color);
        }

        if (penWidthSlider != null) { 
            penWidthSlider.onValueChanged.AddListener(UpdatePenWidth);
            UpdatePenWidth(penWidthSlider.value);
        }
    }

    private void Update()
    {
        bool isGrabbed = grabbable.isSelected;
        bool isDrawing = ((rightHandController != null && rightHandController.isTrigger) || (leftHandController != null && leftHandController.isTrigger));

        if (isGrabbed && isDrawing)
        {
            Draw();
        }
        else if (currentDrawing != null)
        {
            // 점선으로 변경
            /*
            if (isDotted)
            {
                currentDrawing.material.color = tipMaterial.color;
                currentDrawing.material = dotMaterial;
                currentDrawing.textureMode = LineTextureMode.Tile;
                currentDrawing.material.mainTextureScale = new Vector2(currentDrawing.positionCount * 0.01f, 1f);
                lines.Add(currentDrawing);
            }
            */
            currentDrawing = null;
        }
        
        // animation
        if (lines != null)
        {
            foreach (var lineRenderer in lines)
            {
                Vector2 offset = lineRenderer.material.mainTextureOffset;
                offset.x += Time.deltaTime * scrollSpeed;
                lineRenderer.material.mainTextureOffset = offset;
                //lineRenderer.material.mainTextureOffset.x += Time.deltaTime * scrollSpeed;
            }
        }
    }

    private void Draw()
    {
        if (currentDrawing == null)
        {
            index = 0;
            currentDrawing = new GameObject("DrawingLine").AddComponent<LineRenderer>();
            currentDrawing.material = drawingMaterial;
            currentDrawing.startColor = currentDrawing.endColor = tipMaterial.color;

            // 선 스타일 설정
            if (capStyle == PenCapStyle.Rounded)
            {
                
                currentDrawing.startWidth = currentDrawing.endWidth = penWidth;

                int capSegments = Mathf.Clamp(Mathf.RoundToInt(penWidth * 1000f), 4, 20);
                currentDrawing.numCapVertices = capSegments;
                currentDrawing.numCornerVertices = Mathf.Clamp(capSegments / 2, 2, 10);
            }
            /*
            else if (capStyle == PenCapStyle.Pointed)
            {
                currentDrawing.startWidth = penWidth;
                currentDrawing.endWidth = penWidth;

                // 선 양 끝을 뾰족하게, 중간은 두껍게 (AnimationCurve로 컨트롤)
                AnimationCurve taperCurve = new AnimationCurve(
                    new Keyframe(0f, 0f),                // 시작점 뾰족
                    new Keyframe(0.2f, 1f),              // 빠르게 두꺼워짐
                    new Keyframe(0.8f, 1f),              // 중간은 유지
                    new Keyframe(1f, 0f)                 // 끝점 뾰족
                );

                currentDrawing.widthCurve = taperCurve;
                currentDrawing.widthMultiplier = penWidth;

                currentDrawing.numCapVertices = 0;
                currentDrawing.numCornerVertices = 0;
            }
            */

            currentDrawing.alignment = LineAlignment.TransformZ;
            currentDrawing.positionCount = 1;
            currentDrawing.SetPosition(0, tip.position);

            // LayerManager의 AddLayer() 함수 호출
            lm.AddLayer(currentDrawing.gameObject);
            
            if (isDotted)
            {
                currentDrawing.material.color = tipMaterial.color;
                currentDrawing.material = dotMaterial;
                currentDrawing.textureMode = LineTextureMode.Tile;
                currentDrawing.material.mainTextureScale = new Vector2(currentDrawing.positionCount * 0.01f, 1f);
                lines.Add(currentDrawing);
            }
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

    private void ApplyPickedColor(Color c)
    {
        tipMaterial.color = c;
        drawingMaterial.color = c;
    }

    private void UpdatePenWidth(float value)
    {
        penWidth = value;
    }
    
    public static void UpdateTextureScale(LineRenderer line, float textureScaleFactor = 1f)
    {
        if (line == null || line.material == null || line.positionCount < 2)
            return;

        float length = 0f;
        for (int i = 1; i < line.positionCount; i++)
        {
            length += Vector3.Distance(line.GetPosition(i - 1), line.GetPosition(i));
        }

        // 텍스처 타일링 조정
        Vector2 scale = line.material.mainTextureScale;
        scale.x = length * textureScaleFactor;
        line.material.mainTextureScale = scale;
    }


}
