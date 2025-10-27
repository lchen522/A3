// SparkleInteraction.cs
// REVISED VERSION 4.1 - Added Click Volume Control

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class SparkleInteraction : MonoBehaviour
{
    [Header("Swimming AI (Fish-like)")]
    [Tooltip("How fast the star 'swims' forward.")]
    public float moveSpeed = 1.0f; // Slower speed is better for bouncing

    [Tooltip("How fast the star 'steers' or rotates.")]
    public float turnSpeed = 1.5f;

    [Tooltip("How *smoothly* the star changes direction.")]
    public float wanderSmoothness = 0.2f;

    [Header("Click Feedback")]
    [Tooltip("The 'ding' sound to play on click")]
    public AudioClip dingSound;

    // --- ����������±��� ---
    [Tooltip("�����Ч����� (0.0 �� 1.0)")]
    [Range(0f, 1f)]
    public float clickSoundVolume = 1.0f; // <-- ����
    // -------------------------

    [Tooltip("How much force to apply on click (as an impulse).")]
    public float clickForceMagnitude = 1.0f;

    [Tooltip("The color the star will flash when clicked")]
    public Color glowColor = Color.white;

    // ... (Component References �� Private State ���ֲ���) ...
    // --- Component References ---
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // --- Private State ---
    private Color originalColor;
    private float perlinSeed;
    private bool isClickable = true;

    // ... (Start() �� FixedUpdate() ���ֲ���) ...
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store original state
        originalColor = spriteRenderer.color;

        // --- 1. Setup Rigidbody for bouncing ---
        // (Remember to set Drag and Angular Drag to 0 in the Inspector!)
        rb.gravityScale = 0; // No gravity

        // --- 2. Setup Audio ---
        audioSource.playOnAwake = false;

        // --- 3. Setup Perlin Noise ---
        perlinSeed = Random.Range(0f, 1000f);

        // Give it an initial push to start swimming
        rb.AddForce(Random.insideUnitCircle.normalized * moveSpeed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        // --- AI Swimming Logic ---
        Wander();

        // --- Boundary Logic (REMOVED) ---
        // The [ScreenEdges] EdgeCollider2D and Bouncy_Material now handle all boundaries.
        // No code needed here for that.
    }

    private void Wander()
    {
        // Update the Perlin seed to get a new "smooth" random value
        perlinSeed += Time.fixedDeltaTime * wanderSmoothness;

        // Use Perlin noise to get a value between -1 and 1
        float steerAmount = (Mathf.PerlinNoise(perlinSeed, 0f) - 0.5f) * 2f;

        // --- Apply continuous rotation (steering) ---
        rb.AddTorque(steerAmount * turnSpeed);

        // --- Apply continuous forward movement ---
        // (We must re-apply force constantly, otherwise it would stop)

        // Push in the direction the star is facing
        rb.AddForce(transform.up * moveSpeed);

        // Optional: Clamp velocity so it doesn't get too fast
        // rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeed * 1.5f);
    }

    // --- This handles the click event ---
    void OnMouseDown()
    {
        if (!isClickable) return; // Cooldown active

        isClickable = false;

        // Apply Click Force (as requested: 'push' in random direction)
        rb.velocity = Vector2.zero; // Reset velocity for a clean "push"
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDirection * clickForceMagnitude, ForceMode2D.Impulse);

        // Play Sound & Glow
        if (dingSound != null)
        {
            // --- �޸���һ�� ---
            audioSource.PlayOneShot(dingSound, clickSoundVolume); // <-- �޸�
            // ------------------

            spriteRenderer.color = glowColor;
            StartCoroutine(ClickCooldown(dingSound.length));
        }
        else
        {
            StartCoroutine(ClickCooldown(0.5f)); // Default cooldown
        }
    }

    // --- This handles the Cooldown ---
    private IEnumerator ClickCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isClickable = true;
        spriteRenderer.color = originalColor;
    }
}