// Page3_Navigation.cs
// ����ű�ר������ "Page3" �����İ�ť������
// ** UPDATED to play sound before scene change **

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 1. ���

[RequireComponent(typeof(AudioSource))] // 2. ���
public class Page3_Navigation : MonoBehaviour
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
    public void GoToPreviousPage()
    {
        if (isLoading) return;
        Debug.Log("���ڼ��س���: Page2");
        StartCoroutine(LoadSceneAfterSound("Page2")); // 5. ����Э��
    }

    // ������������ӵ� "NEXT" ��ť
    public void GoToNextPage()
    {
        if (isLoading) return;
        Debug.Log("���ڼ��س���: Page4");
        StartCoroutine(LoadSceneAfterSound("Page4")); // 5. ����Э��
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