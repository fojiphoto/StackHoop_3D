using UnityEngine;
using UnityEngine.UI;

public class RainbowText : MonoBehaviour
{
    public float rainbowSpeed = 1f; // Adjust the speed of the rainbow effect

    private Text textComponent;

    private void Start()
    {
        textComponent = GetComponent<Text>();

        if (textComponent == null)
        {
            Debug.LogError("RainbowText script requires a Text component.");
            enabled = false; // Disable the script if no Text component is found
        }
    }

    private void Update()
    {
        // Update the text color with a rainbow effect
        Color rainbowColor = CalculateRainbowColor();
        textComponent.color = rainbowColor;
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
