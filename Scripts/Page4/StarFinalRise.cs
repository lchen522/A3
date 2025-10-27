// StarFinalRise.cs
// ����ű��������ڱ����һ�κ������ƶ���һ��ָ����Ŀ��λ�á�
// ���������ֹͣ��Ӧ�κκ��������

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class StarFinalRise : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("�� Inspector ����קһ�������嵽���������ǵ�����λ��")]
    public Transform targetPositionObject;

    [Tooltip("���������������ٶ�")]
    public float moveSpeed = 2.0f;

    [Header("Sound")]
    [Tooltip("����ʱ���ŵ���Ч (���� 'whoosh' �� 'magic' ��)")]
    public AudioClip riseSound;

    // --- ˽�б��� ---
    private Vector3 targetPosition;
    private bool hasBeenClicked = false; // �ؼ��ġ�һ���ԡ���
    private AudioSource audioSource;

    void Start()
    {
        // 1. ���Ŀ���Ƿ�����
        if (targetPositionObject == null)
        {
            Debug.LogError("����Ŀ��λ�� (Target Position Object) û�б����ã�", this);
            return;
        }

        // 2. ��¼Ŀ��λ��
        targetPosition = targetPositionObject.position;

        // 3. ��ȡ��Ч���
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // ���������ʱ
    void OnMouseDown()
    {
        // ����������Ѿ������� (���Ѿ��������)������������
        if (hasBeenClicked)
        {
            return;
        }

        // --- ���ϡ����� ---
        // ��������һ�α����������������Ϊ true
        // ���������ڿ�ʼ����� OnMouseDown �����Ŀ�ͷ�ͻ�ֱ�� return
        hasBeenClicked = true;

        // ������Ч
        if (riseSound != null)
        {
            audioSource.PlayOneShot(riseSound);
        }

        // ��ʼЭ�̣��ƶ���Ŀ��λ��
        StartCoroutine(MoveToTarget());
    }

    // --- ���������ƶ���Э�� ---
    private IEnumerator MoveToTarget()
    {
        // ֻҪ��ǰλ�û�����Ŀ��λ��
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // ʹ�� MoveTowards ��ʵ�������ƶ�
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null; // �ȴ���һ֡
        }

        // �ƶ���������ȷ���õ�����λ��
        transform.position = targetPosition;

        // Э�̽������������ڻ���Զ�̶������
    }
}