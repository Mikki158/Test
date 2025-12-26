using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public SpeakerType speaker; // "NPC" или "Игрок"
    [TextArea(2, 5)]
    public string text;
}
