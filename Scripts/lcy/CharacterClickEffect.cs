using UnityEngine;
using System.Collections;

public class CharacterClickEffect : MonoBehaviour
{
    [Header("References")]
    public GameObject soundWaveObject; // 声音波纹物体
    public AudioClip stomachSound;     // 肚子咕噜咕噜音效

    [Header("Settings")]
    public float fadeDuration = 0.5f; // 淡入淡出时间
    public float waveScale = 1.2f;    // 波纹放大倍数

    private AudioSource audioSource;
    private bool isPlaying = false;
    private Vector3 originalScale;

    void Start()
    {
        // 确保初始状态正确
        soundWaveObject.SetActive(false);
        originalScale = soundWaveObject.transform.localScale;

        // 添加AudioSource组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = stomachSound;
    }

    void OnMouseDown()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayEffect());
        }
    }

    IEnumerator PlayEffect()
    {
        isPlaying = true;

        // 淡入效果
        soundWaveObject.SetActive(true);
        soundWaveObject.transform.localScale = Vector3.zero;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            soundWaveObject.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale * waveScale, progress);
            yield return null;
        }

        // 播放音效
        audioSource.Play();

        // 等待音效播放完毕
        yield return new WaitForSeconds(stomachSound.length);

        // 淡出效果
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            soundWaveObject.transform.localScale = Vector3.Lerp(originalScale * waveScale, Vector3.zero, progress);
            yield return null;
        }

        soundWaveObject.SetActive(false);
        isPlaying = false;
    }
}