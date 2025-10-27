// AspectRatioEnforcer.cs
// ����ű����뱻���ص���ÿһ���������� Main Camera (�������) �ϡ�
// ����ǿ��������� 16:9 �Ŀ�߱���Ⱦ�����ڶ���Ŀռ���Ӻڱ� (Letterbox)��

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    // ���ǵ�Ŀ���߱� (1920 / 1080 = 1.777...)
    private float targetAspect = 16.0f / 9.0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    // ��ÿһ֡�����£���ȷ�� WebGL ���ڴ�С�ı�ʱҲ��������Ӧ
    void Update()
    {
        UpdateAspectRatio();
    }

    void UpdateAspectRatio()
    {
        // 1. ��ȡ��ǰ���ڵĿ�߱� (�������κ�ֵ)
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 2. �Ƚϴ��ں�Ŀ��Ŀ�߱�
        if (windowAspect < targetAspect)
        {
            // --- ���ڡ�̫�ߡ� (���� 16:10) ---
            // ������Ҫ��Letterboxing�� (���¼Ӻڱ�)
            float scaledHeight = windowAspect / targetAspect;
            float letterboxHeight = (1.0f - scaledHeight) / 2.0f;
            mainCamera.rect = new Rect(0.0f, letterboxHeight, 1.0f, scaledHeight);
        }
        else
        {
            // --- ���ڡ�̫�� (���� 21:9) ---
            // ������Ҫ��Pillarboxing�� (���ҼӺڱ�)
            float scaledWidth = targetAspect / windowAspect;
            float pillarboxWidth = (1.0f - scaledWidth) / 2.0f;
            mainCamera.rect = new Rect(pillarboxWidth, 0.0f, scaledWidth, 1.0f);
        }
    }
}