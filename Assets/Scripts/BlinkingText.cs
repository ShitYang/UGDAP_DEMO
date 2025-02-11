using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Text text; // �ı����
    public float rotationSpeed = 30f; // ��ת�ٶȣ���/�룩
    public float scaleSpeed = 0.5f; // �����ٶ�
    public float maxScale = 1.5f; // ������ű���
    public float minScale = 1f; // ��С���ű���

    private Vector3 originalScale; // �ı��ĳ�ʼ����
    private bool isScalingUp = true; // �Ƿ����ڷŴ�

    private void Start()
    {
        // ��ȡ�ı��ĳ�ʼ����
        originalScale = text.rectTransform.localScale;
    }

    private void Update()
    {
        // ��ת�ı�
        RotateText();

        // �����ı�
        ScaleText();
    }

    private void RotateText()
    {
        // ��ʱ����ת
        text.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }

    private void ScaleText()
    {
        // ��ȡ��ǰ����
        Vector3 currentScale = text.rectTransform.localScale;

        // ����Ŀ������
        float targetScale = isScalingUp ? maxScale : minScale;

        // ��ֵ����
        currentScale = Vector3.Lerp(currentScale, originalScale * targetScale, scaleSpeed * Time.deltaTime);

        // ��������
        text.rectTransform.localScale = currentScale;

        // ����ӽ�Ŀ�����ţ��л�����
        if (Mathf.Abs(currentScale.magnitude - (originalScale * targetScale).magnitude) < 0.01f)
        {
            isScalingUp = !isScalingUp;
        }
    }
}
