// MainMenuManager.cs
// This script manages UI navigation for button clicks.
// It handles loading scenes and quitting the application.
// ** UPDATED to play sound before scene change **

using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes
using System.Collections; // <-- 1. 必须添加，用于协程

// 2. 确保物体上有 AudioSource
[RequireComponent(typeof(AudioSource))]
public class MainMenuManager : MonoBehaviour
{
    // 3. 添加音效的公共变量
    [Header("Sound Effect")]
    [Tooltip("拖拽你的“翻书”音效到这里")]
    public AudioClip pageTurnSound;

    // 4. 私有变量
    private AudioSource audioSource;
    private bool isLoading = false; // 防止重复点击

    void Awake()
    {
        // 5. 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // --- Function for the 'Next Page' Button ---
    public void OnNextPageClicked()
    {
        if (isLoading) return; // 如果正在加载，则不响应
        Debug.Log("Next Page button clicked. Loading scene: page1");
        // 6. 启动协程
        StartCoroutine(LoadSceneAfterSound("page1"));
    }

    // --- Function for the 'Exit' Button ---
    public void OnExitClicked()
    {
        if (isLoading) return;
        Debug.Log("Exit button clicked. Quitting application...");
        // 6. 启动协程
        StartCoroutine(QuitAfterSound());
    }

    // --- 7. 新的协程：播放声音并加载场景 ---
    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        isLoading = true;
        float delay = 0f;

        // 播放声音
        if (audioSource != null && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
            delay = pageTurnSound.length; // 获取音效时长
        }

        // 等待音效播放完毕
        yield return new WaitForSeconds(delay);

        // 加载场景
        SceneManager.LoadScene(sceneName);
    }

    // --- 8. 新的协程：播放声音并退出 ---
    private IEnumerator QuitAfterSound()
    {
        isLoading = true;
        float delay = 0f;

        if (audioSource != null && pageTurnSound != null)
        {
            audioSource.PlayOneShot(pageTurnSound);
            delay = pageTurnSound.length;
        }

        // 等待音效播放完毕
        yield return new WaitForSeconds(delay);

        // 退出程序
        Application.Quit();
    }
}