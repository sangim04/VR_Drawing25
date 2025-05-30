using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Image colorWheel;      // �÷� �� Image ������Ʈ
    public Slider valueSlider;    // ��� ���� �����̴� (0~1)
    public Image previewImage;    // �̸����� �̹���

    private Texture2D wheelTexture;
    private Color hueColor = Color.white;  // ���� Hue ���� ����

    public event Action<Color> onColorChanged;

    void Start()
    {
        // �ؽ�ó ��������
        wheelTexture = colorWheel.sprite.texture;

        // �����̴� �⺻���� 1�� ���� (�ʱ⿣ �ִ� ���)
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

        // ���� Hue ���� ����
        hueColor = wheelTexture.GetPixelBilinear(px, py);

        // ��� �����ϰ� �̸����� & �ݹ�
        UpdatePreviewColor();
    }

    private void UpdatePreviewColor()
    {
        // ������hueColor ���̸� �����̴� ����ŭ ����
        Color finalColor = Color.Lerp(Color.black, hueColor, valueSlider.value);
        previewImage.color = finalColor;
        onColorChanged?.Invoke(finalColor);
    }
}
