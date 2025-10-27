using UnityEngine;

// ȷ�����Ӵ˽ű��Ķ���һ����һ�� Collider 2D �� AudioSource ���
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))] // <-- �������Զ���� AudioSource
public class DraggableObject : MonoBehaviour
{
    [Header("��Ч")]
    [Tooltip("��������ץȡʱ���ŵ���Ч")]
    public AudioClip grabSound; // <-- ���������ڴ����Ч

    // ˽�б���
    private Vector3 offset;
    private float zCoord;
    private AudioSource audioSource; // <-- ���������ڲ�����Ч���������

    // Awake �� Start ֮ǰ���У��ʺ�������ȡ�������
    void Awake()
    {
        // ��ȡ������ͬһ�������ϵ� AudioSource ���
        audioSource = GetComponent<AudioSource>(); // <-- ����
    }

    // ����갴��ʱ����
    void OnMouseDown()
    {
        // --- 1. ������Ч ---

        // ����û��Ƿ��� Inspector ����������Ч
        if (grabSound != null) // <-- ����
        {
            // ����һ����Ч
            audioSource.PlayOneShot(grabSound); // <-- ����
        }

        // --- 2. ������ק�߼� ---

        // 1. ��¼����� Z ��λ��
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // 2. ���㲢�洢ƫ����
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    // ����갴ס���϶�ʱ��ÿһ֡�������
    void OnMouseDrag()
    {
        // 3. ���������λ��
        transform.position = GetMouseWorldPos() + offset;
    }

    // ������������ȡ�������������ϵ�е�λ��
    private Vector3 GetMouseWorldPos()
    {
        // ��ȡ�������Ļ�ϵ� X, Y ��������
        Vector3 mousePoint = Input.mousePosition;

        // �� Z ��������Ϊ����֮ǰ�洢���������
        mousePoint.z = zCoord;

        // ����Ļ���꣨���أ�ת��Ϊ�������꣨Unity��λ��
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}