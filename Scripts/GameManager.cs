// GameManager.cs
// ����һ��������������������������Ϸ����ʱ���У�
// ������ڳ�����͹ᴩ���г����ı������֡�
// ** �Ѹ��£��޸��˷�����ҳʱ�ĺ���BUG��������˷ֱ������� **

using UnityEngine;
using UnityEngine.SceneManagement; // <-- ��Ҫ�����������������
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("����Ҫ��������˵��������ļ��� (������ȫƥ��)")]
    public string mainMenuSceneName = "SampleScene";

    [Tooltip("����Ҫ����ĵ���/������ɫ Image ���������")]
    public string fadeScreenObjectName = "FadeScreen";

    [Header("Background Music")]
    [Tooltip("��ק��ı�������(mp3/wav)������")]
    public AudioClip backgroundMusic;

    [Tooltip("�� Inspector �е��ڱ������ֵ�����")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;

    [Header("Scene Fade-In")]
    [Tooltip("�Ӻڳ����뵽������Ҫ�೤ʱ��")]
    public float fadeDuration = 1.5f;

    // --- ����ģʽ ---
    public static GameManager instance;

    // --- ˽�б��� ---
    private AudioSource audioSource;
    private Image fadeScreen; // ���ǲ����� Inspector ��������

    void Awake()
    {
        // --- 1. ����ģʽʵ�� ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // --- 2. ���ġ�����������ϡ��¼� ---
            // 'OnSceneLoaded' �������Ժ󴴽���һ������
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // --- 3. (�¹���) �����ֱ��� ---
        // ǿ����Ϸ�� 1920x1080 ����ģʽ����
        // (�������Ҫȫ������ FullScreenMode.Windowed ��Ϊ FullScreenMode.ExclusiveFullScreen)
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // --- 4. ���ñ������� (����ǰһ��) ---
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        // ����ֻ�� Start() �в���һ��
        // (����Ч���Ѿ��Ƶ� OnSceneLoaded ����)
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // --- 5. (�¹���) ��������ʱ�Ĵ����� ---
    // ÿ��һ���³����������ʱ����������ͻᱻ�Զ�����
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���Ǽ����صĳ����Ƿ������˵�
        if (scene.name == mainMenuSceneName)
        {
            // ���³����У�ͨ�����ֲ��� FadeScreen ����
            GameObject fadeScreenObj = GameObject.Find(fadeScreenObjectName);

            if (fadeScreenObj != null)
            {
                // �ҵ��ˣ���ȡ���� Image ���
                fadeScreen = fadeScreenObj.GetComponent<Image>();

                // ȷ�����ڿ�ʼʱ�Ǽ�����Ҳ�͸�� (ȫ��)
                fadeScreen.gameObject.SetActive(true);
                fadeScreen.color = Color.black;

                // ��ʼ����Э��
                StartCoroutine(FadeIn());
            }
            else
            {
                // ����Ҳ������ڿ���̨����ʾ
                Debug.LogWarning("GameManager: �� " + scene.name + " ������û���ҵ���Ϊ '" + fadeScreenObjectName + "' �����塣");
            }
        }
    }

    // --- 6. ����Э�� (����ǰһ��) ---
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

        fadeScreen.color = Color.clear; // ��Ϊ��ȫ͸��
        fadeScreen.gameObject.SetActive(false); // ����
    }

    // (OnValidate ���ֲ��䣬������ Inspector �е�������)
    void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    // (�� GameManager ������ʱ��ȡ�������¼�����ֹ�ڴ�й©)
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
