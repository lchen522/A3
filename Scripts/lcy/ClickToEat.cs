using UnityEngine;

// ������Ի�ȷ���κι��ش˽ű������嶼�Զ����һ��Collider2D���
// ������ OnMouseDown ��Ч�������
[RequireComponent(typeof(Collider2D))]
public class ClickToEat : MonoBehaviour
{
    // ��Unity�༭���У�����ġ����Ե�������Ч�ļ��ϵ�����
    public AudioClip eatSound;

    // ����������������� Collider2D ʱ����������ᱻ�Զ�����
    void OnMouseDown()
    {
        // 1. ������Ч
        // ����ʹ�� PlayClipAtPoint��������ָ��λ�ô���һ����ʱ����Դ������������
        // �������ĺô��ǣ���ʹ����������Ͼ���ʧ�ˣ�����Ҳ�������ز�����ϡ�
        if (eatSound != null)
        {
            AudioSource.PlayClipAtPoint(eatSound, transform.position);
        }

        // 2. ��������ʧ
        // gameObject.SetActive(false) ���������壬֮�󻹿�����������ʾ
        // �����ϣ�����屻����ɾ����Ҳ����ʹ�� Destroy(gameObject);
        gameObject.SetActive(false);
    }
}