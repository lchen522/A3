// AspectRatioEnforcer.cs
// 这个脚本必须被挂载到“每一个”场景的 Main Camera (主摄像机) 上。
// 它会强制摄像机以 16:9 的宽高比渲染，并在多余的空间添加黑边 (Letterbox)。

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    // 我们的目标宽高比 (1920 / 1080 = 1.777...)
    private float targetAspect = 16.0f / 9.0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        UpdateAspectRatio();
    }

    // 在每一帧都更新，以确保 WebGL 窗口大小改变时也能立即响应
    void Update()
    {
        UpdateAspectRatio();
    }

    void UpdateAspectRatio()
    {
        // 1. 获取当前窗口的宽高比 (可能是任何值)
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 2. 比较窗口和目标的宽高比
        if (windowAspect < targetAspect)
        {
            // --- 窗口“太高” (比如 16:10) ---
            // 我们需要“Letterboxing” (上下加黑边)
            float scaledHeight = windowAspect / targetAspect;
            float letterboxHeight = (1.0f - scaledHeight) / 2.0f;
            mainCamera.rect = new Rect(0.0f, letterboxHeight, 1.0f, scaledHeight);
        }
        else
        {
            // --- 窗口“太宽” (比如 21:9) ---
            // 我们需要“Pillarboxing” (左右加黑边)
            float scaledWidth = targetAspect / windowAspect;
            float pillarboxWidth = (1.0f - scaledWidth) / 2.0f;
            mainCamera.rect = new Rect(pillarboxWidth, 0.0f, scaledWidth, 1.0f);
        }
    }
}