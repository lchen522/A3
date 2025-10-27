// OwlToggleMovement.cs
// 这个脚本让物体（猫头鹰）在它的初始位置和一个设定的目标位置之间来回平移。

using UnityEngine;
using System.Collections; // 必须有，因为我们要用协程

// 确保物体有碰撞体，这样才能被点击
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))] // 顺便加上音效

public class OwlToggleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("在 Inspector 中拖拽一个空物体到这里，用来标记猫头鹰的目标位置")]
    public Transform targetPositionObject;

    [Tooltip("猫头鹰平移到目标位置需要多长时间")]
    public float moveDuration = 1.0f;

    [Header("Sound")]
    [Tooltip("移动时播放的音效 (比如 'whoosh' 声)")]
    public AudioClip moveSound;

    // --- 私有变量 ---
    private Vector3 originalPosition; // 猫头鹰的初始位置（树上）
    private Vector3 targetPosition;   // 目标位置（树下）

    private bool isAtOriginalPosition = true; // 状态标记：是否在初始位置？
    private bool isMoving = false;            // 冷却标记：是否正在移动中？

    private AudioSource audioSource;

    void Start()
    {
        // 1. 检查目标位置是否设置
        if (targetPositionObject == null)
        {
            Debug.LogError("错误：目标位置 (Target Position Object) 没有被设置！", this);
            return;
        }

        // 2. 记录初始位置和目标位置
        originalPosition = transform.position;
        targetPosition = targetPositionObject.position;

        // 3. 获取音效组件
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // 当点击猫头鹰的碰撞体时
    void OnMouseDown()
    {
        // 如果正在移动中，则不响应点击（防止连续点击）
        if (isMoving)
        {
            return;
        }

        // 播放音效
        if (moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        // 根据当前状态，决定要去哪里
        if (isAtOriginalPosition)
        {
            // 当前在树上 -> 移动到树下
            StartCoroutine(MoveToPosition(targetPosition));
            isAtOriginalPosition = false; // 更新状态
        }
        else
        {
            // 当前在树下 -> 移动回树上
            StartCoroutine(MoveToPosition(originalPosition));
            isAtOriginalPosition = true; // 更新状态
        }
    }

    // --- 负责平移的协程 ---
    private IEnumerator MoveToPosition(Vector3 endPosition)
    {
        isMoving = true; // 开始移动，开启“冷却”

        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            // 计算平滑的插值 (t 从 0 变化到 1)
            float t = timer / moveDuration;
            t = t * t * (3f - 2f * t); // 这是一个 "SmoothStep" 计算，使移动两头慢，中间快

            // 更新位置
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null; // 等待下一帧
        }

        // 移动结束，精确设置到最终位置
        transform.position = endPosition;

        isMoving = false; // 结束移动，解除“冷却”
    }
}