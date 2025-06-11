using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a single dice in the game. Handles rolling and value display.
/// </summary>
public class Dice : MonoBehaviour
{
    [Header("Dice Face Sprites (1-6)")]
    public Sprite[] faceSprites; // Assign 6 sprites in inspector
    public SpriteRenderer spriteRenderer; // Assign in inspector

    [Header("Animation Settings")]
    public float rollDuration = 0.5f;
    public int rollCycles = 10;

    private Coroutine rollCoroutine;

    /// <summary>
    /// Plays the roll animation and updates the visual to show the rolled value.
    /// </summary>
    public void ShowRoll(int value, System.Action onComplete = null)
    {
        if (rollCoroutine != null)
            StopCoroutine(rollCoroutine);
        rollCoroutine = StartCoroutine(RollAnimation(value, onComplete));
    }

    private IEnumerator RollAnimation(int finalValue, System.Action onComplete)
    {
        int faceCount = faceSprites.Length;
        for (int i = 0; i < rollCycles; i++)
        {
            int randomFace = Random.Range(0, faceCount);
            // Add a small random rotation for 3D effect
            transform.localEulerAngles = new Vector3(
                Random.Range(-30f, 30f),
                Random.Range(-30f, 30f),
                Random.Range(-30f, 30f)
            );
            yield return new WaitForSeconds(rollDuration / rollCycles);
        }
        // Clamp value to 1-6
        int index = Mathf.Clamp(finalValue - 1, 0, faceCount - 1);
        // Snap to a fixed rotation for the final value (simulate a real dice face up)
        transform.localEulerAngles = GetRotationForValue(finalValue);
        rollCoroutine = null;
        onComplete?.Invoke();
    }

    // Returns a fixed rotation for each dice value (1-6)
    private Vector3 GetRotationForValue(int value)
    {
        switch (value)
        {
            case 1: return new Vector3(-90, 0, 0);
            case 2: return new Vector3(0, 0, 0);
            case 3: return new Vector3(0, 0, -90);
            case 4: return new Vector3(0, 0, 90);
            case 5: return new Vector3(180, 0, 0);
            case 6: return new Vector3(90, 0, 0);
            default: return Vector3.zero;
        }
    }
}
