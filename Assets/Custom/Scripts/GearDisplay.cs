using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class GearDisplay : MonoBehaviour
{
    //public LogitechSteeringWheel Logitech;
    //public Outline outline;
    public int currentGear = -1;

    //public GameObject parkText;         // - 0
    //public GameObject reverseText;      // - 1
    //public GameObject neutralText;      // - 2
    //public GameObject driveText;        // - 3
    public GameObject[] gears;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateGear(int gear)
    {
        currentGear = gear;
        ChangeGear();
    }
    
    void ChangeGear()
    {
        foreach(GameObject gear in gears)
        {
            if (gear.GetComponent<GearSwitch>().isOn)
            {
                gear.GetComponent<GearSwitch>().TurnOff();
            }
        }
        gears[currentGear].GetComponent<GearSwitch>().TurnOn();

    }
}
