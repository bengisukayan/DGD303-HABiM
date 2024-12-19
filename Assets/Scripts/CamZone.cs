using Cinemachine;
using UnityEngine;

public class CamZone : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCam = null;

    void Start()
    {
        virtualCam.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCam.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCam.enabled = false;
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
