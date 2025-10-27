using UnityEngine;

// 确保附加此脚本的对象一定有一个 Collider 2D 和 AudioSource 组件
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // <-- 新增：自动添加 AudioSource
public class DraggableObject : MonoBehaviour
{
    [Header("音效")]
    [Tooltip("拖入点击并抓取时播放的音效")]
    public AudioClip grabSound; // <-- 新增：用于存放音效

    // 私有变量
    private Vector3 offset;
    private float zCoord;
    private AudioSource audioSource; // <-- 新增：用于播放音效的组件引用

    // Awake 在 Start 之前运行，适合用来获取组件引用
    void Awake()
    {
        // 获取附加在同一个对象上的 AudioSource 组件
        audioSource = GetComponent<AudioSource>(); // <-- 新增
    }

    // 当鼠标按下时调用
    void OnMouseDown()
    {
        // --- 1. 播放音效 ---

        // 检查用户是否在 Inspector 中拖入了音效
        if (grabSound != null) // <-- 新增
        {
            // 播放一次音效
            audioSource.PlayOneShot(grabSound); // <-- 新增
        }

        // --- 2. 计算拖拽逻辑 ---

        // 1. 记录物体的 Z 轴位置
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // 2. 计算并存储偏移量
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    // 当鼠标按住并拖动时，每一帧都会调用
    void OnMouseDrag()
    {
        // 3. 更新物体的位置
        transform.position = GetMouseWorldPos() + offset;
    }

    // 辅助函数：获取鼠标在世界坐标系中的位置
    private Vector3 GetMouseWorldPos()
    {
        // 获取鼠标在屏幕上的 X, Y 像素坐标
        Vector3 mousePoint = Input.mousePosition;

        // 将 Z 坐标设置为我们之前存储的物体深度
        mousePoint.z = zCoord;

        // 将屏幕坐标（像素）转换为世界坐标（Unity单位）
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}