// RabbitJump.cs
// This script makes the rabbit hop in place and play a sound on click.
// It uses a Coroutine to handle the motion and click cooldown.

using UnityEngine;
using System.Collections; // Required for Coroutines

// These components are necessary for the script to work
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))] // Or CircleCollider2D, etc.

public class RabbitJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [Tooltip("How high the rabbit jumps (in Unity units)")]
    public float jumpHeight = 1.5f;

    [Tooltip("The total duration of the jump (up and down)")]
    public float jumpDuration = 0.5f;

    [Header("Sound")]
    [Tooltip("The sound effect to play on jump")]
    public AudioClip jumpSound;

    // --- Private Variables ---
    private AudioSource audioSource;
    private Vector3 originalPosition; // To store the starting position
    private bool isJumping = false;   // Cooldown flag

    void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();

        // Store the rabbit's starting position
        originalPosition = transform.position;

        // Ensure audio doesn't play on start
        audioSource.playOnAwake = false;
    }

    // This function is called when the object's collider is clicked
    void OnMouseDown()
    {
        // Check the cooldown flag. If already jumping, do nothing.
        if (isJumping)
        {
            return;
        }

        // Start the jump!
        StartCoroutine(DoJump());
    }

    // --- The Jump Coroutine ---
    // This function runs over multiple frames
    private IEnumerator DoJump()
    {
        // 1. Set Cooldown
        isJumping = true;

        // 2. Play Sound
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        // --- 3. Animate the Jump (Up and Down) ---
        float timer = 0f;

        while (timer < jumpDuration)
        {
            // Advance the timer
            timer += Time.deltaTime;

            // Calculate 't' (a value from 0 to 1)
            float t = timer / jumpDuration;

            // --- The "Arc" using Mathf.Sin ---
            // A sine wave goes from 0 -> 1 -> 0 over 0 to Pi.
            // This perfectly mimics a jump arc.
            float heightOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // Apply the offset to the original position
            transform.position = originalPosition + new Vector3(0, heightOffset, 0);

            // Wait for the next frame
            yield return null;
        }

        // --- 4. Finish the Jump ---

        // Just to be safe, snap back to the exact original position
        transform.position = originalPosition;

        // Reset the cooldown flag so we can jump again
        isJumping = false;
    }
}