// Page2_Navigation.cs
// 这个脚本专门用于 "Page2" 场景的按钮导航。
// ** UPDATED to play sound before scene change **

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 1. 添加

[RequireComponent(typeof(AudioSource))] // 2. 添加
public class Page2_Navigation : MonoBehaviour
{
    // 3. 添加音效变量
    [Header("Sound Effect")]
    [Tooltip("拖拽你的“翻书”音效到这里")]
    public AudioClip pageTurnSound;

    private AudioSource audioSource;
    private bool isLoading = false;

    void Awake()
    {
        // 4. 获取组件
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // 这个函数将连接到 "BACK" 按钮
    public void GoToPreviousPage()
    {
        if (isLoading) return;
        Debug.Log("正在加载场景: Page1");
        StartCoroutine(LoadSceneAfterSound("Page1")); // 5. 启动协程
    }

    // 这个函数将连接到 "NEXT" 按钮
    public void GoToNextPage()
    {
        if (isLoading) return;
        Debug.Log("正在加载场景: Page3");
        StartCoroutine(LoadSceneAfterSound("Page3")); // 5. 启动协程
    }

    // --- 6. 新的协程 ---
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