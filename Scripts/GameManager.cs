// GameManager.cs
// 这是一个“单例”管理器，它会在游戏启动时运行，
// 负责处理黑场淡入和贯穿所有场景的背景音乐。
// ** 已更新：修复了返回主页时的黑屏BUG，并添加了分辨率锁定 **

using UnityEngine;
using UnityEngine.SceneManagement; // <-- 需要这个来监听场景加载
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("【重要】你的主菜单场景的文件名 (必须完全匹配)")]
    public string mainMenuSceneName = "SampleScene";

    [Tooltip("【重要】你的淡入/淡出黑色 Image 物体的名字")]
    public string fadeScreenObjectName = "FadeScreen";

    [Header("Background Music")]
    [Tooltip("拖拽你的背景音乐(mp3/wav)到这里")]
    public AudioClip backgroundMusic;

    [Tooltip("在 Inspector 中调节背景音乐的音量")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Header("Scene Fade-In")]
    [Tooltip("从黑场淡入到场景需要多长时间")]
    public float fadeDuration = 1.5f;

    // --- 单例模式 ---
    public static GameManager instance;

    // --- 私有变量 ---
    private AudioSource audioSource;
    private Image fadeScreen; // 我们不再在 Inspector 中设置它

    void Awake()
    {
        // --- 1. 单例模式实现 ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // --- 2. 订阅“场景加载完毕”事件 ---
            // 'OnSceneLoaded' 是我们稍后创建的一个函数
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // --- 3. (新功能) 锁定分辨率 ---
        // 强制游戏以 1920x1080 窗口模式运行
        // (如果你想要全屏，把 FullScreenMode.Windowed 改为 FullScreenMode.ExclusiveFullScreen)
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // --- 4. 设置背景音乐 (和以前一样) ---
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        // 音乐只在 Start() 中播放一次
        // (淡入效果已经移到 OnSceneLoaded 中了)
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // --- 5. (新功能) 场景加载时的处理函数 ---
    // 每当一个新场景加载完成时，这个函数就会被自动调用
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 我们检查加载的场景是否是主菜单
        if (scene.name == mainMenuSceneName)
        {
            // 在新场景中，通过名字查找 FadeScreen 物体
            GameObject fadeScreenObj = GameObject.Find(fadeScreenObjectName);

            if (fadeScreenObj != null)
            {
                // 找到了！获取它的 Image 组件
                fadeScreen = fadeScreenObj.GetComponent<Image>();

                // 确保它在开始时是激活的且不透明 (全黑)
                fadeScreen.gameObject.SetActive(true);
                fadeScreen.color = Color.black;

                // 开始淡入协程
                StartCoroutine(FadeIn());
            }
            else
            {
                // 如果找不到，在控制台给提示
                Debug.LogWarning("GameManager: 在 " + scene.name + " 场景中没有找到名为 '" + fadeScreenObjectName + "' 的物体。");
            }
        }
    }

    // --- 6. 淡入协程 (和以前一样) ---
    private IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float smoothAlpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeScreen.color = new Color(0, 0, 0, smoothAlpha);
            yield return null;
        }

        fadeScreen.color = Color.clear; // 设为完全透明
        fadeScreen.gameObject.SetActive(false); // 禁用
    }

    // (OnValidate 保持不变，用于在 Inspector 中调节音量)
    void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    // (当 GameManager 被销毁时，取消订阅事件，防止内存泄漏)
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
