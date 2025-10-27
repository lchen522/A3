// TypewriterEffect.cs (Legacy UI.Text Version)
// Creates a typewriter effect for a standard UI Text element.

using UnityEngine;
using UnityEngine.UI; // <-- IMPORTANT: Using this instead of TMPro
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    // Drag your standard UI Text component here in the Inspector
    public Text storyText; // <-- CHANGED: This is now 'Text'

    // Adjust the speed of the typing effect
    public float typingSpeed = 0.05f;

    private string fullText;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (storyText == null)
        {
            // Updated error message
            Debug.LogError("Story Text (UI.Text) is not assigned.");
            return;
        }

        // Store the full text from the text box
        fullText = storyText.text;

        // Clear the text box to start the effect
        storyText.text = "";

        // Start the typing effect
        typingCoroutine = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        // Loop through each character in the full text
        foreach (char c in fullText)
        {
            // Add one character at a time
            storyText.text += c;
            // Wait for 'typingSpeed' seconds before adding the next character
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}