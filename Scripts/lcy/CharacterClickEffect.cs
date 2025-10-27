using UnityEngine;
using System.Collections;

public class CharacterClickEffect : MonoBehaviour
{
    [Header("References")]
    public GameObject soundWaveObject; // ������������
    public AudioClip stomachSound;     // ���ӹ��๾����Ч

    [Header("Settings")]
    public float fadeDuration = 0.5f; // ���뵭��ʱ��
    public float waveScale = 1.2f;    // ���ƷŴ���

    private AudioSource audioSource;
    private bool isPlaying = false;
    private Vector3 originalScale;

    void Start()
    {
        // ȷ����ʼ״̬��ȷ
        soundWaveObject.SetActive(false);
        originalScale = soundWaveObject.transform.localScale;

        // ���AudioSource���
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

        // ����Ч��
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

        // ������Ч
        audioSource.Play();

        // �ȴ���Ч�������
        yield return new WaitForSeconds(stomachSound.length);

        // ����Ч��
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