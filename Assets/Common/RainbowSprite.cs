using UnityEngine;

public class RainbowSprite : MonoBehaviour
{
    public float rainbowSpeed = 1f; // Adjust the speed of the rainbow effect

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("RainbowSprite script requires a SpriteRenderer component.");
            enabled = false; // Disable the script if no SpriteRenderer component is found
        }
    }

    private void Update()
    {
        // Update the sprite color with a rainbow effect
        Color rainbowColor = CalculateRainbowColor();
        spriteRenderer.color = rainbowColor;
    }

    private Color CalculateRainbowColor()
    {
        // Calculate a color based on time to create a rainbow effect
        float t = Time.time * rainbowSpeed;
        float r = Mathf.Sin(t) * 0.5f + 0.5f;
        float g = Mathf.Sin(t + 2f) * 0.5f + 0.5f;
        float b = Mathf.Sin(t + 4f) * 0.5f + 0.5f;

        return new Color(r, g, b);
    }
}

