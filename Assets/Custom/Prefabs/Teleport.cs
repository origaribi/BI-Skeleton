using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    GameObject driver;
    bool chase = false;
    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        driver = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!chase)
        {
            if(Vector3.Distance(transform.position, driver.transform.position) < 20f)
            {
                attach();
            }
        }
    }
    void attach()
    {
        Vector3 newrot = new Vector3(0.487f, 200.869f, 0.612f);
        transform.eulerAngles = newrot;
        

        transform.parent = driver.transform;
        Vector3 newpos = camera.transform.position;
        Vector3 subtract = new Vector3(-0.23f, 1.6f, -0.75f);
        newpos -= subtract;
        transform.position = newpos;
        //transform.LookAt(driver.transform.position);
        //Vector3 newrot = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        //transform.eulerAngles.x = 0f;
        
    }

}

