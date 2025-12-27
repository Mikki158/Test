using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager2D : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public SpriteRenderer portraitImage;

    public SpeakerData[] speakers;
    private DialogueLine[] currentLines;
    private DialogueNPC DialogueNPC;

    public float delay = 0.06f;

    public int currentLine = 0;
    private bool dialogueActive = false;
    private bool justClosed = false;
    public bool stopTyping = false;

    public bool IsDialogueActive => dialogueActive;
    public bool JustClosed => justClosed;
    public bool StopTyping => stopTyping;
    private bool justStarted = false;
    bool skip;

    void Update()
    {
        if (!dialogueActive) return;

        if (justStarted)
        {
            justStarted = false;
            return;
        }

        if (Keyboard.current?.eKey.wasPressedThisFrame == true && stopTyping)
        {
            NextLine();
        }

        //if (Keyboard.current?.eKey.wasPressedThisFrame == true)
        //    skip = true;
    }

    public void StartDialogue(DialogueData dialogue, DialogueNPC currentNPC)
    {
        currentLines = dialogue.lines;
        dialogueActive = true;
        currentLine = 0;
        justStarted = true; 
        DialogueNPC = currentNPC;

        dialoguePanel.SetActive(true);
        ShowLine();
        //Time.timeScale = 0f;
    }

    void ShowLine()
    {
        Debug.Log(currentLine);
        skip = false;
        DialogueLine line = currentLines[currentLine];
        SpeakerData speaker = GetSpeakerData(line.speaker);

        nameText.text = speaker.displayName;
        //dialogueText.text = line.text;
        portraitImage.sprite = speaker.portrait;

        StopAllCoroutines();
        StartCoroutine(TypeText(line.text));
    }

    SpeakerData GetSpeakerData(SpeakerType type)
    {
        foreach (var s in speakers)
        {
            if (s.type == type)
                return s;
        }

        Debug.LogError($"Speaker {type} not found!");
        return null;
    }

    void NextLine()
    {
        currentLine++;
        stopTyping = false;
        

        if (currentLine >= currentLines.Length)
        {
            dialogueActive = false;
            justClosed = false;
            stopTyping = false;
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        justClosed = true;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        currentLine = -1;
        StartCoroutine(NextLevel());
        StartCoroutine(ResetJustClosed());
    }

    System.Collections.IEnumerator ResetJustClosed()
    {
        yield return null;
        justClosed = false;
        //if (DialogueNPC.endLevel)
        //{
        //    SceneManager.LoadScene(DialogueNPC.nextLevel);
        //}
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1f);
        DialogueNPC.CloseDialogue();
    }

    IEnumerator TypeText(string text)
    {
        
        dialogueText.text = "";
        skip = false;
        stopTyping = false;

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);

            //if (skip)
            //{
            //    dialogueText.text = text;
            //    stopTyping = true;
            //    skip = false;
            //    yield break;
            //}
        }

        stopTyping = true;
    }
}
