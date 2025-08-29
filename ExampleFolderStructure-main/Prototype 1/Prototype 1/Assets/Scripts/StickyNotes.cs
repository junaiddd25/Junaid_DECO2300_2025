using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    public TMP_Text label;
    [TextArea] public string initialText = "New note";

    void Start()
    {
        if (!label) label = GetComponentInChildren<TMP_Text>();
        if (label) label.text = initialText;
    }
}
