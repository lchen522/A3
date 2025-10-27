// StarFinalRise.cs
// 这个脚本让星星在被点击一次后，匀速移动到一个指定的目标位置。
// 到达后，它会停止响应任何后续点击。

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class StarFinalRise : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("在 Inspector 中拖拽一个空物体到这里，标记星星的最终位置")]
    public Transform targetPositionObject;

    [Tooltip("星星匀速上升的速度")]
    public float moveSpeed = 2.0f;

    [Header("Sound")]
    [Tooltip("飞升时播放的音效 (比如 'whoosh' 或 'magic' 声)")]
    public AudioClip riseSound;

    // --- 私有变量 ---
    private Vector3 targetPosition;
    private bool hasBeenClicked = false; // 关键的“一次性”锁
    private AudioSource audioSource;

    void Start()
    {
        // 1. 检查目标是否设置
        if (targetPositionObject == null)
        {
            Debug.LogError("错误：目标位置 (Target Position Object) 没有被设置！", this);
            return;
        }

        // 2. 记录目标位置
        targetPosition = targetPositionObject.position;

        // 3. 获取音效组件
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // 当点击星星时
    void OnMouseDown()
    {
        // 如果“锁”已经锁上了 (即已经点击过了)，则立即返回
        if (hasBeenClicked)
        {
            return;
        }

        // --- 锁上“锁” ---
        // 这是它第一次被点击，立即将锁设为 true
        // 这样从现在开始，这个 OnMouseDown 函数的开头就会直接 return
        hasBeenClicked = true;

        // 播放音效
        if (riseSound != null)
        {
            audioSource.PlayOneShot(riseSound);
        }

        // 开始协程，移动到目标位置
        StartCoroutine(MoveToTarget());
    }

    // --- 负责匀速移动的协程 ---
    private IEnumerator MoveToTarget()
    {
        // 只要当前位置还不是目标位置
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // 使用 MoveTowards 来实现匀速移动
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null; // 等待下一帧
        }

        // 移动结束，精确设置到最终位置
        transform.position = targetPosition;

        // 协程结束，星星现在会永远固定在这里。
    }
}