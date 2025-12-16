using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach this to your UI Text or TextMeshProUGUI GameObject.
// Assign either the `uiText` (Unity UI) or `tmpText` (TextMeshPro) field in the inspector.
public class TimerDisplay : MonoBehaviour
{
    public Text uiText;
    public TextMeshPro tmpText;

    void Update()
    {
        if (GLOBALTimer.Instance == null)
            return;

        string formatted = GLOBALTimer.Instance.GetFormattedTime();

        if (tmpText != null)
            tmpText.text = formatted;
        else if (uiText != null)
            uiText.text = formatted;
    }
}
