using UnityEngine;

// [RequireComponent] ��ȷ������ű������ӵ��Ķ�����
// ����ͬʱӵ�� Rigidbody2D, Collider2D �� AudioSource ���
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // <-- �������Զ���� AudioSource ���
public class PineconeInteraction : MonoBehaviour
{
    [Header("����Ч��")]
    [Tooltip("���ʱʩ�ӵ����Ĵ�С")]
    public float forceAmount = 5.0f;

    [Header("��Ч")]
    [Tooltip("��������Ҫ���ŵĵ����Ч�ļ�")]
    public AudioClip clickSound; // <-- ���������ڴ����Ч

    // ˽�б���
    private Rigidbody2D rb;
    private AudioSource audioSource; // <-- ���������ڲ�����Ч���������

    void Start()
    {
        // ��ȡ������ͬһ�������ϵ����
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // <-- ��������ȡ AudioSource ���

        // ȷ����ʼʱ�� Kinematic ״̬����������Ӱ��
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // ���û������������������ϵ��κ� Collider 2D ʱ�����ͻᱻ�Զ�����
    private void OnMouseDown()
    {
        // --- 0. ������Ч (ÿ�ε����ִ��) ---

        // ����û��Ƿ��� Inspector �����ָ������ƵƬ��
        if (clickSound != null)
        {
            // PlayOneShot ������Ч�ص����ţ�����û��������㣩���ǳ��ʺ�������Ч
            audioSource.PlayOneShot(clickSound);
        }

        // --- 1. �������� (����һ�ε��ʱִ��) ---

        // �������Ƿ��Դ��� Kinematic ״̬
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            // �����������л�Ϊ Dynamic (��̬)
            rb.bodyType = RigidbodyType2D.Dynamic;

            // ȷ������Ϊ0
            rb.gravityScale = 0f;

            // ȷ��û�н�����(angularDrag)��������������ת
            rb.angularDrag = 0f;
        }

        // --- 2. ʩ��������� (ÿ�ε����ִ��) ---

        // ��ȡһ�������������
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // ʩ��һ��˲ʱ���� (����)
        rb.AddForce(randomDirection * forceAmount, ForceMode2D.Impulse);
    }
}