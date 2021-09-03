using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSwitch : MonoBehaviour
{
    public GameObject gearOn;
    public GameObject gearOff;
    public bool isOn;


    // Start is called before the first frame update
    void Start()
    {
        gearOn.SetActive(false);
        gearOff.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnOn()
    {
        gearOn.SetActive(true);
        gearOff.SetActive(false);
        isOn = true;
    }
    public void TurnOff()
    {
        gearOn.SetActive(false);
        gearOff.SetActive(true);
        isOn = false;
    }
}
