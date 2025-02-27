using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public GameObject controlsCanvas; 
    private bool isActive = false; 

    void Start()
    {
        controlsCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isActive = !isActive;
            controlsCanvas.SetActive(isActive);
        }
    }
}
