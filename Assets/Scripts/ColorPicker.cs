using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Image colorWheel;      // 컬러 휠 Image 컴포넌트
    public Slider valueSlider;    // 밝기 조절 슬라이더 (0~1)
    public Image previewImage;    // 미리보기 이미지

    private Texture2D wheelTexture;
    private Color hueColor = Color.white;  // 순수 Hue 색상 저장

    public event Action<Color> onColorChanged;

    void Start()
    {
        // 텍스처 가져오기
        wheelTexture = colorWheel.sprite.texture;

        // 슬라이더 기본값을 1로 설정 (초기엔 최대 밝기)
        valueSlider.value = 1f;
        valueSlider.onValueChanged.AddListener(_ => UpdatePreviewColor());
    }

    public void OnPointerDown(PointerEventData evt) => PickColor(evt);
    public void OnDrag(PointerEventData evt) => PickColor(evt);

    private void PickColor(PointerEventData evt)
    {
        RectTransform rt = colorWheel.rectTransform;
        Vector2 localPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, evt.position, evt.pressEventCamera, out localPos))
            return;

        float px = localPos.x / rt.rect.width + 0.5f;
        float py = localPos.y / rt.rect.height + 0.5f;
        if (px < 0 || px > 1 || py < 0 || py > 1) return;

        // 순수 Hue 색상 저장
        hueColor = wheelTexture.GetPixelBilinear(px, py);

        // 밝기 적용하고 미리보기 & 콜백
        UpdatePreviewColor();
    }

    private void UpdatePreviewColor()
    {
        // 검정↔hueColor 사이를 슬라이더 값만큼 보간
        Color finalColor = Color.Lerp(Color.black, hueColor, valueSlider.value);
        previewImage.color = finalColor;
        onColorChanged?.Invoke(finalColor);
    }
}
