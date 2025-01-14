using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject on;
    public GameObject off;
    private bool isOn;
    public PickUp_Controller pickUp;

    void Start()
    {
        on.SetActive(false);
        off.SetActive(true);
        isOn = false;
    }

    void Update()
    {
        if (pickUp.equipped)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isOn)
                {
                    on.SetActive(false);
                    off.SetActive(true);
                }

                if (!isOn)
                {
                    on.SetActive(true);
                    off.SetActive(false);
                }

                isOn = !isOn;
            }
        }
    }
}
