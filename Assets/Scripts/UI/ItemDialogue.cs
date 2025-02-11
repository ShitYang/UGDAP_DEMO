using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class ItemDialogue : MonoBehaviour {

    public TextMeshProUGUI dialogueText;
    
    [TextArea(2, 5)]
    public string[] dialogueLines;
    public float typingSpeed = 0.05f;
    public float displayDuration = 3f;
    
    public void EnterDialogue()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        foreach (string line in dialogueLines)
        {
            yield return StartCoroutine(TypeSentence(line));
            yield return new WaitForSeconds(displayDuration);
        }
        gameObject.SetActive(false);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void Update() {
        transform.rotation = Quaternion.identity;
    }
}
