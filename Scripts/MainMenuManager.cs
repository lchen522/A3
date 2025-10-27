// MainMenuManager.cs
// This script manages UI navigation for button clicks.
// It handles loading scenes and quitting the application.
// ** UPDATED to play sound before scene change **

using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes
using System.Collections; // <-- 1. ������ӣ�����Э��

// 2. ȷ���������� AudioSource
[RequireComponent(typeof(AudioSource))]
public class MainMenuManager : MonoBehaviour
{
    // 3. �����Ч�Ĺ�������
    [Header("Sound Effect")]
    [Tooltip("��ק��ġ����顱��Ч������")]
    public AudioClip pageTurnSound;

    // 4. ˽�б���
    private AudioSource audioSource;
    private bool isLoading = false; // ��ֹ�ظ����

    void Awake()
    {
        // 5. ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // --- Function for the 'Next Page' Button ---
    public void OnNextPageClicked()
    {
        if (isLoading) return; // ������ڼ��أ�����Ӧ
        Debug.Log("Next Page button clicked. Loading scene: page1");
        // 6. ����Э��
        StartCoroutine(LoadSceneAfterSound("page1"));
    }

    // --- Function for the 'Exit' Button ---
    public void OnExitClicked()
    {
        if (isLoading) return;
        Debug.Log("Exit button clicked. Quitting application...");
        // 6. ����Э��
        StartCoroutine(QuitAfterSound());
    }

    // --- 7. �µ�Э�̣��������������س��� ---
    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        isLoading = true;
        float delay = 0f;

        // ��������
        if (audioSource != null && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
            delay = pageTurnSound.length; // ��ȡ��Чʱ��
        }

        // �ȴ���Ч�������
        yield return new WaitForSeconds(delay);

        // ���س���
        SceneManager.LoadScene(sceneName);
    }

    // --- 8. �µ�Э�̣������������˳� ---
    private IEnumerator QuitAfterSound()
    {
        isLoading = true;
        float delay = 0f;

        if (audioSource != null && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
            delay = pageTurnSound.length;
        }

        // �ȴ���Ч�������
        yield return new WaitForSeconds(delay);

        // �˳�����
        Application.Quit();
    }
}