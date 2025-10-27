// EndSceneNavigation.cs
// ����ű�ר������ "EndScene" (������Ա����) ������
// ��ֻ����һ�����ܣ�������ҳ (SampleScene)��
// ** UPDATED to play sound before scene change **

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 1. ���

[RequireComponent(typeof(AudioSource))] // 2. ���
public class EndSceneNavigation : MonoBehaviour
{
    // 3. �����Ч����
    [Header("Sound Effect")]
    [Tooltip("��ק��ġ����顱��Ч������")]
    public AudioClip pageTurnSound;

    private AudioSource audioSource;
    private bool isLoading = false;

    void Awake()
    {
        // 4. ��ȡ���
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // ������������ӵ� "BACK" ��ť
    public void GoToHomepage()
    {
        if (isLoading) return;
        Debug.Log("���ڼ��س���: SampleScene");
        StartCoroutine(LoadSceneAfterSound("SampleScene")); // 5. ����Э��
    }

    // --- 6. �µ�Э�� ---
    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        isLoading = true;
        float delay = 0f;

        if (audioSource != null && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
            delay = pageTurnSound.length;
        }

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}