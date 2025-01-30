using System.Collections;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public float displayDuration = 5f;

    private void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(DisableInstructionsAfterTime());
    }

    private IEnumerator DisableInstructionsAfterTime()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}