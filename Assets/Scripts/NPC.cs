using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int _index;
    private bool _canSkip = false;
    private Coroutine typingCoroutine;

    public float wordSpeed;
    private bool _playerIsClose;

    private PlayerController _playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy && _canSkip)
            {
                NextLine();
            }
            else if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                typingCoroutine = StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[_index])
        {
            _canSkip = true;
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue[_index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        _canSkip = false;
        if (_index < dialogue.Length - 1)
        {
            _index++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogueText.text = "";
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
            _index = 0;
            RefillPlayerHealth();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsClose = true;
            _playerHealth = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsClose = false;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            ZeroText();
        }
    }

    private void RefillPlayerHealth()
    {
        _playerHealth.Heal();
    }
}
