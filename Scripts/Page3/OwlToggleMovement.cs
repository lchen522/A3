// OwlToggleMovement.cs
// ����ű������壨èͷӥ�������ĳ�ʼλ�ú�һ���趨��Ŀ��λ��֮������ƽ�ơ�

using UnityEngine;
using System.Collections; // �����У���Ϊ����Ҫ��Э��

// ȷ����������ײ�壬�������ܱ����
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))] // ˳�������Ч

public class OwlToggleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("�� Inspector ����קһ�������嵽����������èͷӥ��Ŀ��λ��")]
    public Transform targetPositionObject;

    [Tooltip("èͷӥƽ�Ƶ�Ŀ��λ����Ҫ�೤ʱ��")]
    public float moveDuration = 1.0f;

    [Header("Sound")]
    [Tooltip("�ƶ�ʱ���ŵ���Ч (���� 'whoosh' ��)")]
    public AudioClip moveSound;

    // --- ˽�б��� ---
    private Vector3 originalPosition; // èͷӥ�ĳ�ʼλ�ã����ϣ�
    private Vector3 targetPosition;   // Ŀ��λ�ã����£�

    private bool isAtOriginalPosition = true; // ״̬��ǣ��Ƿ��ڳ�ʼλ�ã�
    private bool isMoving = false;            // ��ȴ��ǣ��Ƿ������ƶ��У�

    private AudioSource audioSource;

    void Start()
    {
        // 1. ���Ŀ��λ���Ƿ�����
        if (targetPositionObject == null)
        {
            Debug.LogError("����Ŀ��λ�� (Target Position Object) û�б����ã�", this);
            return;
        }

        // 2. ��¼��ʼλ�ú�Ŀ��λ��
        originalPosition = transform.position;
        targetPosition = targetPositionObject.position;

        // 3. ��ȡ��Ч���
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // �����èͷӥ����ײ��ʱ
    void OnMouseDown()
    {
        // ��������ƶ��У�����Ӧ�������ֹ���������
        if (isMoving)
        {
            return;
        }

        // ������Ч
        if (moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }

        // ���ݵ�ǰ״̬������Ҫȥ����
        if (isAtOriginalPosition)
        {
            // ��ǰ������ -> �ƶ�������
            StartCoroutine(MoveToPosition(targetPosition));
            isAtOriginalPosition = false; // ����״̬
        }
        else
        {
            // ��ǰ������ -> �ƶ�������
            StartCoroutine(MoveToPosition(originalPosition));
            isAtOriginalPosition = true; // ����״̬
        }
    }

    // --- ����ƽ�Ƶ�Э�� ---
    private IEnumerator MoveToPosition(Vector3 endPosition)
    {
        isMoving = true; // ��ʼ�ƶ�����������ȴ��

        float timer = 0f;
        Vector3 startPosition = transform.position;

        while (timer < moveDuration)
        {
            timer += Time.deltaTime;

            // ����ƽ���Ĳ�ֵ (t �� 0 �仯�� 1)
            float t = timer / moveDuration;
            t = t * t * (3f - 2f * t); // ����һ�� "SmoothStep" ���㣬ʹ�ƶ���ͷ�����м��

            // ����λ��
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null; // �ȴ���һ֡
        }

        // �ƶ���������ȷ���õ�����λ��
        transform.position = endPosition;

        isMoving = false; // �����ƶ����������ȴ��
    }
}