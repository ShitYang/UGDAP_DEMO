using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Text text; // 文本组件
    public float rotationSpeed = 30f; // 旋转速度（度/秒）
    public float scaleSpeed = 0.5f; // 缩放速度
    public float maxScale = 1.5f; // 最大缩放比例
    public float minScale = 1f; // 最小缩放比例

    private Vector3 originalScale; // 文本的初始缩放
    private bool isScalingUp = true; // 是否正在放大

    private void Start()
    {
        // 获取文本的初始缩放
        originalScale = text.rectTransform.localScale;
    }

    private void Update()
    {
        // 旋转文本
        RotateText();

        // 缩放文本
        ScaleText();
    }

    private void RotateText()
    {
        // 逆时针旋转
        text.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    private void ScaleText()
    {
        // 获取当前缩放
        Vector3 currentScale = text.rectTransform.localScale;

        // 计算目标缩放
        float targetScale = isScalingUp ? maxScale : minScale;

        // 插值缩放
        currentScale = Vector3.Lerp(currentScale, originalScale * targetScale, scaleSpeed * Time.deltaTime);

        // 更新缩放
        text.rectTransform.localScale = currentScale;

        // 如果接近目标缩放，切换方向
        if (Mathf.Abs(currentScale.magnitude - (originalScale * targetScale).magnitude) < 0.01f)
        {
            isScalingUp = !isScalingUp;
        }
    }
}
