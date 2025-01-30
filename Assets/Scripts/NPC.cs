using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int _index;
    private bool _canSkip = false;
    private Coroutine typingCoroutine;

    public float wordSpeed;
    private bool _playerIsClose;
    public bool isSpecial = false;

    private PlayerController _player;
    private ProjectileShooter _playerShooter;
    public GameObject _newBullet;

    private bool _hasTalked = false;

    private void Start()
    {
        _hasTalked = PlayerPrefs.GetInt(gameObject.name + "_talked", 0) == 1;
    }

    private void Update()
    {
        if (_hasTalked) return;

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

    private void ZeroText()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    private IEnumerator Typing()
    {
        dialogueText.text = "";
        foreach (char letter in dialogue[_index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void NextLine()
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
            FinishDialogue();
        }
    }

    private void FinishDialogue()
    {
        ZeroText();
        _index = 0;
        RefillPlayerHealth();
        SetCheckpoint();

        if (isSpecial)
        {
            _playerShooter.projectilePrefab = _newBullet;
        }

        _hasTalked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasTalked)
        {
            _playerIsClose = true;
            _player = other.GetComponent<PlayerController>();
            _playerShooter = other.GetComponent<ProjectileShooter>();
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
        _player?.Heal();
    }

    private void SetCheckpoint()
    {
        _player?.Checkpoint(gameObject.transform);
    }
}
