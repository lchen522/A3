using UnityEngine;

// [RequireComponent] 能确保这个脚本被附加到的对象上
// 必须同时拥有 Rigidbody2D, Collider2D 和 AudioSource 组件
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // <-- 新增：自动添加 AudioSource 组件
public class PineconeInteraction : MonoBehaviour
{
    [Header("物理效果")]
    [Tooltip("点击时施加的力的大小")]
    public float forceAmount = 5.0f;

    [Header("音效")]
    [Tooltip("拖入你想要播放的点击音效文件")]
    public AudioClip clickSound; // <-- 新增：用于存放音效

    // 私有变量
    private Rigidbody2D rb;
    private AudioSource audioSource; // <-- 新增：用于播放音效的组件引用

    void Start()
    {
        // 获取附加在同一个对象上的组件
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // <-- 新增：获取 AudioSource 组件

        // 确保开始时是 Kinematic 状态，不受物理影响
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // 当用户用鼠标点击了这个对象上的任何 Collider 2D 时，它就会被自动调用
    private void OnMouseDown()
    {
        // --- 0. 播放音效 (每次点击都执行) ---

        // 检查用户是否在 Inspector 面板中指定了音频片段
        if (clickSound != null)
        {
            // PlayOneShot 允许音效重叠播放（如果用户快速连点），非常适合用于音效
            audioSource.PlayOneShot(clickSound);
        }

        // --- 1. 激活物理 (仅第一次点击时执行) ---

        // 检查刚体是否仍处于 Kinematic 状态
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            // 将刚体类型切换为 Dynamic (动态)
            rb.bodyType = RigidbodyType2D.Dynamic;

            // 确保重力为0
            rb.gravityScale = 0f;

            // 确保没有角阻力(angularDrag)，让它能自由旋转
            rb.angularDrag = 0f;
        }

        // --- 2. 施加随机的力 (每次点击都执行) ---

        // 获取一个随机方向向量
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 施加一个瞬时的力 (冲量)
        rb.AddForce(randomDirection * forceAmount, ForceMode2D.Impulse);
    }
}