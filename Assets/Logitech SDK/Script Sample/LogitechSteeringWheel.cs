using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LogitechSteeringWheel : MonoBehaviour
{
    public UnityStandardAssets.Vehicles.Car.CarController carControl;
    public Vector3 Arrows;
    LogitechGSDK.LogiControllerPropertiesData properties;
    private string actualState;
    private string activeForces;
    private string propertiesEdit;
    private string buttonStatus;
    private string forcesLabel;
    string[] activeForceAndEffect;
    [Header("Effects")]
    private int sideSwitch = 1;
    public int springCenter;
    public int springMagnitude;
    public int springFalloff;
    public int sideScale;
    public bool surfaceFX;
    public int surfaceMagMin;
    public int surfaceMagDelta;
    public int surfaceFreqMin;
    public int surfaceFreqDelta;
    [Header("Inputs")]
    public float wheel;
    public float accel;
    public float prev_accel;
    public float brake;
    private Rigidbody rb;

    //testbools --------------\
    bool downPress = false;
    bool upPress = false;
    bool[] gears = new bool[] { false, false, true, false };
    public int currIndex = 2;
    //-----------------------
    [Header("Test bools")]
    public bool neutral;
    public bool park;
    public bool pressed;
    public bool drive;
    public bool cruise;
    public bool reverse;
    public int currentgear = -1;
    public int prevGear = -1;
    public bool showGUI;
    public bool xButton;
    public bool yButton;
    public bool aButton;
    public bool bButton;
    public bool HomeButton;
    public float ignoreWheelUp;
    public float ignoreWheelDown;

    bool firstFrame = true;
    public int maxx = 0;
    public int minx = 0;
    public bool button4;
    public bool button5;
    public bool button6;
    public bool button7;
    public bool button8;
    public bool button9;
    public bool button10;
    public bool button11;
    public bool button12;
    public bool button13;
    public bool button14, button15, button16, button17;

    GameObject ryuk;
    public GameObject killIt;

    //-------bools for cruise control
    //public bool isPressed;
    [Header("Cruise Control")]
    public bool isP;
    public bool CC;
    public bool waiting;
    public bool paddlePress;
    public bool isSlightlyBraking;
    public bool isSlightlyGas;

    public float lockedSpeed;
    public bool leftPaddle;
    public bool rightPaddle;
    public bool speedCheck;
    public GameObject cruiseUI;
    public GameObject cruiseDisplay;

    [Header("Parking")]
    public bool isBraking; //true if brake value > 0.75
    public bool isParked; //true if parked, will enable park functions (no movement, etc)
    public bool parkGear; //true if park gear is on ([16])
    public bool falsePark, falseRelease;

    [Header("Teleport Spot")]
    Vector3 pos_road = new Vector3(542.212f, 295.072f, 166.249f);
    Vector3 rot_road = new Vector3(0.832f, -84.082f, -0.273f);


    // variables for dashboard gears

    public GameObject parkingImage;
    public GameObject neutralImage;
    public GameObject driveImage;
    public GameObject reverseImage;

    // Use this for initialization


    void Start()
    {
        /**
        if (keyboard)
        {
            neutral = true;
            drive = false;
            reverse = false;
            park = false;
        }
        **/

        //cruise vars
        rb = GetComponent<Rigidbody>();
        cruise = false;
        speedCheck = false;

        //------------
        currentgear = -1;

        isParked = false;
        falsePark = false;
        falseRelease = false;
        park = false;
        //pressed = false;
        //parkingImage.SetActive(false);
        //neutralImage.SetActive(false);
        //driveImage.SetActive(false);
        //reverseImage.SetActive(false);
        //activeForces = "";
        //propertiesEdit = "";
        //actualState = "";
        //buttonStatus = "";
        //activeForceAndEffect = new string[9];
        Debug.Log(LogitechGSDK.LogiSteeringInitialize(false));
        ryuk = GameObject.FindGameObjectWithTag("ryuk");


    }

    //    void OnGUI()
    //    {
    //        activeForces = GUI.TextArea(new Rect(10, 10, 180, 200), activeForces, 400);
    //        propertiesEdit = GUI.TextArea(new Rect(200, 10, 200, 200), propertiesEdit, 400);
    //        actualState = GUI.TextArea(new Rect(410, 10, 300, 200), actualState, 1000);
    //        buttonStatus = GUI.TextArea(new Rect(720, 10, 300, 200), buttonStatus, 1000);
    //        GUI.Label(new Rect(10, 400, 800, 400), forcesLabel);
    //    }
    public bool keyboard;
    // Update is called once per frame
    void Update()
    {

        /*
        if (keyboard)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                neutral = true;
                drive = false;
                reverse = false;
                park = false;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                neutral = false;
                drive = true;
                reverse = false;
                park = false;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                neutral = false;
                drive = false;
                reverse = true;
                park = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (brake > 0)
                {
                    park = true;
                    neutral = false;
                    drive = false;
                    reverse = false;
                }
            }
        }
        */
        isSlightlyBraking = Input.GetKey(KeyCode.C);

        park = gears[0];
        reverse = gears[1];
        neutral = gears[2];
        drive = gears[3];
        updateGear();
        if (prevGear != currentgear || firstFrame)
        {
            //call method
            GetComponentInChildren<GearDisplay>().UpdateGear(currentgear);
            prevGear = currentgear;
            firstFrame = false;
        }
        if (keyboard)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!upPress)
                {
                    if ((currIndex == 1 && brake > 0) || currIndex != 1)
                    {
                        --currIndex;
                        if (currIndex < 0)
                        {
                            currIndex = 0;
                        }
                        upPress = true;
                        for (int i = 0; i < 4; i++)
                        {
                            gears[i] = (i == currIndex);
                        }
                    }

                }
            }
            else
            {
                upPress = false;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!downPress)
                {
                    if ((currIndex == 0 && brake > 0) || currIndex != 0)
                    {
                        ++currIndex;
                        if (currIndex > 3)
                        {
                            currIndex = 3;
                        }
                        downPress = true;
                        for (int i = 0; i < 4; i++)
                        {
                            gears[i] = (i == currIndex);
                        }
                    }

                }
            }
            else
            {
                downPress = false;
            }

            if (CC) // or cruise
            {
                if (!speedCheck)
                {
                    if (accel <= 0.1f)
                    {
                        speedCheck = true;
                    }
                }
                //cruise on
                if (brake > 0 | (isSlightlyGas & speedCheck) | park | neutral | reverse) //disable cruise if brake is applied or no longer in drive
                {
                    isP = false;
                    cruise = false;
                    CC = false;
                    waiting = false;
                    paddlePress = false;
                    speedCheck = false;
                    //theoretical note: if user is holding abutton while triggering this block, cruise control will be exited, but will transition immediately to first isP state
                }
                else
                {
                    if (Input.GetKey(KeyCode.P))
                    {
                        if (isP)
                        {
                            //holding down, do nothing
                        }
                        else
                        {
                            //first frame of button being pressed
                            isP = true;
                        }

                    }
                    else
                    {
                        if (isP)
                        {
                            //button is now being released
                            //RESET
                            cruise = false;
                            CC = false;
                            isP = false;
                            //reset all stuff to false;

                        }
                        else
                        {
                            //nothing, button has not been pressed yet
                        }
                    }
                }




            }
            else //cruise off
            {
                if (Input.GetKey(KeyCode.P))
                {
                    if (waiting)
                    {
                        isP = false;

                    }
                    else
                    {
                        if (isP)
                        {
                            //button is being held down
                        }
                        else
                        {
                            isP = true;
                        }
                    }
                }
                else
                {
                    if (isP)
                    {
                        float currSpeed = carControl.CurrentSpeed * 2.23693629f;
                        if (Input.GetKey(KeyCode.RightBracket) & (currSpeed >= 25))
                        {
                            paddlePress = true;
                        }
                        else
                        {
                            if (paddlePress)
                            {
                                CC = true;
                                cruise = true;
                                waiting = false;
                                isP = false;
                                //transition to cruise
                            }
                            else
                            {
                                waiting = true;
                            }
                        }
                    }
                    else
                    {
                        if (waiting)
                        {
                            waiting = false;
                        }
                        //the default start state
                    }
                }
            }
            cruise = CC;
            if (!(cruise | isP))
            {
                cruiseDisplay.SetActive(false);
            }
            cruiseUI.SetActive(cruise | isP);


            /**
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (drive)
                {
                    neutral = true;
                    drive = false;
                    reverse = false;
                    park = false;
                }
                if (neutral)
                {
                    neutral = false;
                    drive = false;
                    reverse = true;
                    park = false;
                }
                if (reverse)
                {
                    if (brake > 0)
                    {
                        neutral = false;
                        drive = false;
                        reverse = false;
                        park = true;
                    }
                }
                
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (park)
                {
                    if (brake > 0)
                    {
                        neutral = false;
                        drive = false;
                        reverse = true;
                        park = false;
                    }
                }
                if (reverse)
                {
                    neutral = true;
                    drive = false;
                    reverse = false;
                    park = false;
                }
                if (neutral)
                {
                    neutral = false;
                    drive = true;
                    reverse = false;
                    park = false;
                }
            }
            **/
        }
        if (keyboard)
        {
            if (Input.GetKey(KeyCode.C)) accel += 0.01f;
            else accel = 0f;
            if (Input.GetKey(KeyCode.X)) brake += .1f;
            else brake = 0;
            wheel = (Input.mousePosition.x / 1535) - .5f;
            return;
        }
        //All the test functions are called on the first device plugged in(index = 0)
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {

            //CONTROLLER PROPERTIES
            StringBuilder deviceName = new StringBuilder(256);
            LogitechGSDK.LogiGetFriendlyProductName(0, deviceName, 256);
            //			propertiesEdit = "Current Controller : "+ deviceName + "\n";
            //            propertiesEdit += "Current controller properties : \n\n";
            //LogitechGSDK.LogiControllerPropertiesData actualProperties = new LogitechGSDK.LogiControllerPropertiesData();
            //LogitechGSDK.LogiGetCurrentControllerProperties(0, ref actualProperties);
            //            propertiesEdit += "forceEnable = " + actualProperties.forceEnable + "\n";
            //            propertiesEdit += "overallGain = " + actualProperties.overallGain + "\n";
            //            propertiesEdit += "springGain = " + actualProperties.springGain + "\n";
            //            propertiesEdit += "damperGain = " + actualProperties.damperGain + "\n";
            //            propertiesEdit += "defaultSpringEnabled = " + actualProperties.defaultSpringEnabled + "\n";
            //            propertiesEdit += "combinePedals = " + actualProperties.combinePedals + "\n";
            //            propertiesEdit += "wheelRange = " + actualProperties.wheelRange + "\n";
            //            propertiesEdit += "gameSettingsEnabled = " + actualProperties.gameSettingsEnabled + "\n";
            //            propertiesEdit += "allowGameSettings = " + actualProperties.allowGameSettings + "\n";
            //CONTROLLER STATE
            actualState = "Steering wheel current state : \n\n";
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            //            actualState += "x-axis position :" + rec.lX + "\n";
            //            actualState += "y-axis position :" + rec.lY + "\n";
            //            actualState += "z-axis position :" + rec.lZ + "\n";
            //            actualState += "x-axis rotation :" + rec.lRx + "\n";
            //            actualState += "y-axis rotation :" + rec.lRy + "\n";
            //            actualState += "z-axis rotation :" + rec.lRz + "\n";
            //            actualState += "extra axes positions 1 :" + rec.rglSlider[0] + "\n";
            //            actualState += "extra axes positions 2 :" + rec.rglSlider[1] + "\n";
            //

            //Button status :
            HomeButton = rec.rgbButtons[10] == 128;

            aButton = rec.rgbButtons[0] == 128;//a 0
            bButton = rec.rgbButtons[1] == 128;//b 1
            xButton = rec.rgbButtons[2] == 128;//x 2
            yButton = rec.rgbButtons[3] == 128;//y 3
            //test
            button4 = rec.rgbButtons[4] == 128; //right paddle shift
            button5 = rec.rgbButtons[5] == 128; //left paddle shift
            button6 = rec.rgbButtons[6] == 128; //right 3 lines button
            button7 = rec.rgbButtons[7] == 128; //left square  button
            button8 = rec.rgbButtons[8] == 128; //RSB
            button9 = rec.rgbButtons[9] == 128; //LSB
            button10 = rec.rgbButtons[10] == 128; //
            button11 = rec.rgbButtons[11] == 128; //
            button12 = rec.rgbButtons[12] == 128; //Top leftmost gear - alt reverse
            button13 = rec.rgbButtons[13] == 128; //Bottom leftmost - alt drive
            button14 = rec.rgbButtons[14] == 128; //Reverse Gear
            button15 = rec.rgbButtons[15] == 128; //Normal Drive gear
            button16 = rec.rgbButtons[16] == 128; //Top rightmost gear ------------ use for park in future, currently is reverse
            button17 = rec.rgbButtons[17] == 128; //Bottom rightmost gear - alt drive

            leftPaddle = button5;
            rightPaddle = button4;
            if (button9) //teleport to long road
            {
                transform.position = pos_road;
                transform.eulerAngles = rot_road;
                rb.velocity = Vector3.zero;


            }
            if (ryuk != null)
            {
                if (ryuk.transform.parent == this.transform && button7)
                {
                    Instantiate(killIt, ryuk.transform.position, Quaternion.identity);
                    Destroy(ryuk);
                }
            }

            if (keyboard == false)
            {
                parkGear = button16;
                drive = (button13 || button15 || button17);
            }
            //parkGear = (rec.rgbButtons[16] == 128); for when button debug values are removed\




            wheel = rec.lX / 32767f;
            if (wheel < ignoreWheelDown) wheel -= ignoreWheelDown;
            else if (wheel > ignoreWheelUp) wheel -= ignoreWheelUp;
            else wheel = 0;

            // normal acceleration ----------------
            //accel is [0, 1]

            //if (accel >= prev_accel) accel = ((-rec.lY/32767f)+1)/2f ;
            //switch acceleration to clutch

            if (accel >= prev_accel) accel = ((-rec.rglSlider[0] / 32767f) + 1) / 2f;

            else accel = prev_accel - Time.deltaTime / 10f;
            prev_accel = accel;
            checkExtremes(rec);
            //--------------------------
            //test acc ---------------
            //accel = -rec.lY / 32767f)+1)/ 2f;
            //--------------------------


            //switch control for acc pedal to brake pedal for testing----------------------------
            //use neutral gear to slow down

            //if (accel >= prev_accel) accel = (Mathf.Clamp((-rec.lRz*2f / 32767f), -1f, 1f) + 1) / 2f;

            //prev_accel = accel;
            //------------------------------------------------------------------------
            //testing axis
            //if (accel >= prev_accel) accel = ((-rec.lY/32767f)+1)/2f ;
            //else accel = prev_accel - Time.deltaTime / 10f;
            //test axis
            //if (accel >= prev_accel) accel = (Mathf.Clamp((-rec.lRz * 1.75f / 32767f), -1f, 1f) + 1) / 2f;

            brake = (-rec.lRz / 32767f + 1); //comment out this line when uncommenting pedal switch line
            //if(brake == 0.75f)
            //{
            //    isBraking = true;
            //}
            //else
            isBraking = (brake >= 0.5f);
            isSlightlyBraking = (brake >= 0.25f);
            isSlightlyGas = (accel > 0.85f);

            //brake = 0;
            //test---------------- to check for input from pedals
            //Debug.Log(Time.deltaTime); DIJOYSTATE2ENGINES
            Debug.Log("Z-axis pos.: " + rec.lRz);
            Debug.Log("X-axis pos.: " + rec.lX);
            Debug.Log("Y-axis pos.: " + rec.lY);
            Debug.Log("Mystery Axis: " + rec.rglSlider[0]);




            //---------------
            reverse = ((rec.rgbButtons[14] == 128) || (rec.rgbButtons[12] == 128));
            //park = rec.rgbButtons[16] == 128;
            neutral = true;
            for (int i = 12; i <= 17; i++)
            {
                if (rec.rgbButtons[i] == 128) neutral = false;
            }

            if (!isParked) //driving
            {
                if (parkGear)
                {
                    if (isBraking)
                    {
                        if (falsePark)
                        {
                            //do nothing, you tried to go park gear before braking
                        }
                        else
                        {
                            isParked = true; //transition
                            park = true;
                            //parkingImage.SetActive(true);
                            //driveImage.SetActive(false);
                            //reverseImage.SetActive(false);
                            //neutralImage.SetActive(false);
                        }
                    }
                    else
                    {
                        falsePark = true;
                    }
                }
                else
                {
                    if (falsePark)
                    {
                        falsePark = false;
                    }
                    else
                    {
                        if (button15)
                        {
                            //driveImage.SetActive(true);
                            //reverseImage.SetActive(false);
                            //eutralImage.SetActive(false);
                            //parkingImage.SetActive(false);
                        }
                        else if (button14)
                        {
                            //reverseImage.SetActive(true);
                            //driveImage.SetActive(false);
                            //neutralImage.SetActive(false);
                            //parkingImage.SetActive(false);
                        }
                        else if (neutral)
                        {
                            //neutralImage.SetActive(true);
                            //driveImage.SetActive(false);
                            //reverseImage.SetActive(false);
                            //parkingImage.SetActive(false);
                        }
                        //is driving
                    }
                }
            }
            else
            {
                if (parkGear)
                {
                    if (falseRelease)
                    {
                        falseRelease = false;
                    }
                    else
                    {
                        //normal drain
                    }
                }
                else
                {
                    if (isBraking)
                    {
                        if (falseRelease)
                        {
                            //nbad
                        }
                        else
                        {
                            isParked = false;
                            park = false;
                        }
                    }
                    else
                    {
                        falseRelease = true;
                    }
                }
            }
            park = isParked;

            updateGear();
            if (prevGear != currentgear || firstFrame)
            {
                //call method
                GetComponentInChildren<GearDisplay>().UpdateGear(currentgear);
                prevGear = currentgear;
                firstFrame = false;
            }


            //aButton
            //this is the script for the cruise control

            /*
            pressed = isPressed;
            if (aButton)
            {
                isPressed = true;
                pressed = true;
            }

            else
            {
                isPressed = false;
                pressed = false;
            }
            */
            //waiting
            //CC    
            //isP
            //paddlePress
            if (CC) // or cruise
            {
                if (!speedCheck)
                {
                    if (accel <= 0.1f)
                    {
                        speedCheck = true;
                    }
                }
                //cruise on
                if (isSlightlyBraking | (isSlightlyGas & speedCheck) | park | neutral | reverse) //disable cruise if brake is applied or no longer in drive
                {
                    isP = false;
                    cruise = false;
                    CC = false;
                    waiting = false;
                    paddlePress = false;
                    speedCheck = false;
                    //theoretical note: if user is holding abutton while triggering this block, cruise control will be exited, but will transition immediately to first isP state
                }
                else
                {
                    if (aButton)
                    {
                        if (isP)
                        {
                            //holding down, do nothing
                        }
                        else
                        {
                            //first frame of button being pressed
                            isP = true;
                        }

                    }
                    else
                    {
                        if (isP)
                        {
                            //button is now being released
                            //RESET
                            cruise = false;
                            CC = false;
                            isP = false;
                            //reset all stuff to false;

                        }
                        else
                        {
                            //nothing, button has not been pressed yet
                        }
                    }
                }




            }
            else //cruise off
            {
                if (aButton)
                {
                    if (waiting)
                    {
                        isP = false;

                    }
                    else
                    {
                        if (isP)
                        {
                            //button is being held down
                        }
                        else
                        {
                            isP = true;
                        }
                    }
                }
                else
                {
                    if (isP)
                    {
                        float currSpeed = carControl.CurrentSpeed * 2.23693629f;
                        if (button4 & (currSpeed >= 25))
                        {
                            paddlePress = true;
                        }
                        else
                        {
                            if (paddlePress)
                            {
                                CC = true;
                                cruise = true;
                                waiting = false;
                                isP = false;
                                //transition to cruise
                            }
                            else
                            {
                                waiting = true;
                            }
                        }
                    }
                    else
                    {
                        if (waiting)
                        {
                            waiting = false;
                        }
                        //the default start state
                    }
                }
            }
            cruise = CC;
            if (!(cruise | isP))
            {
                cruiseDisplay.SetActive(false);
            }
            cruiseUI.SetActive(cruise | isP);



            /*
            // Enabling/Disabling Gear Symbol on Dashboard
            if (isParked)
            {
                parkingImage.SetActive(true);
            }
            else
            {
                parkingImage.SetActive(false);
            }

            if (neutral)
            {
                neutralImage.SetActive(true);
            }
            else
            {
                neutralImage.SetActive(false);
            }

            if (button15)
            {
                driveImage.SetActive(true);
            }
            else
            {
                driveImage.SetActive(false);
            }
            */

            Debug.Log(rec.rgbButtons[15]);

            switch (rec.rgdwPOV[0])
            {
                case (0): Arrows.x += 1f; break; // Up
                                                 //                case (4500): actualState += "POV : UP-RIGHT\n"; break;
                case (9000): Arrows.y += 1f; ; break; // Right
                                                      //                case (13500): actualState += "POV : DOWN-RIGHT\n"; break;
                case (18000): Arrows.x -= 1f; break; // Down
                                                     //                case (22500): actualState += "POV : DOWN-LEFT\n"; break;
                case (27000): Arrows.y -= 1f; break; // Left
                                                     //                case (31500): actualState += "POV : UP-LEFT\n"; break;
                default: actualState += "POV : CENTER\n"; break;
            }








            //            buttonStatus = "Button pressed : \n\n";
            //            for (int i = 0; i < 128; i++)
            //            {
            //                if (rec.rgbButtons[i] == 128)
            //                {
            //                    buttonStatus += "Button " + i + " pressed\n";
            //                }
            //
            //            }

            /* THIS AXIS ARE NEVER REPORTED BY LOGITECH CONTROLLERS 
             * 
             * actualState += "x-axis velocity :" + rec.lVX + "\n";
             * actualState += "y-axis velocity :" + rec.lVY + "\n";
             * actualState += "z-axis velocity :" + rec.lVZ + "\n";
             * actualState += "x-axis angular velocity :" + rec.lVRx + "\n";
             * actualState += "y-axis angular velocity :" + rec.lVRy + "\n";
             * actualState += "z-axis angular velocity :" + rec.lVRz + "\n";
             * actualState += "extra axes velocities 1 :" + rec.rglVSlider[0] + "\n";
             * actualState += "extra axes velocities 2 :" + rec.rglVSlider[1] + "\n";
             * actualState += "x-axis acceleration :" + rec.lAX + "\n";
             * actualState += "y-axis acceleration :" + rec.lAY + "\n";
             * actualState += "z-axis acceleration :" + rec.lAZ + "\n";
             * actualState += "x-axis angular acceleration :" + rec.lARx + "\n";
             * actualState += "y-axis angular acceleration :" + rec.lARy + "\n";
             * actualState += "z-axis angular acceleration :" + rec.lARz + "\n";
             * actualState += "extra axes accelerations 1 :" + rec.rglASlider[0] + "\n";
             * actualState += "extra axes accelerations 2 :" + rec.rglASlider[1] + "\n";
             * actualState += "x-axis force :" + rec.lFX + "\n";
             * actualState += "y-axis force :" + rec.lFY + "\n";
             * actualState += "z-axis force :" + rec.lFZ + "\n";
             * actualState += "x-axis torque :" + rec.lFRx + "\n";
             * actualState += "y-axis torque :" + rec.lFRy + "\n";
             * actualState += "z-axis torque :" + rec.lFRz + "\n";
             * actualState += "extra axes forces 1 :" + rec.rglFSlider[0] + "\n";
             * actualState += "extra axes forces 2 :" + rec.rglFSlider[1] + "\n";
             */

            int shifterTipe = LogitechGSDK.LogiGetShifterMode(0);
            string shifterString = "";
            if (shifterTipe == 1) shifterString = "Gated";
            else if (shifterTipe == 0) shifterString = "Sequential";
            else shifterString = "Unknown";
            actualState += "\nSHIFTER MODE:" + shifterString;

            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CONSTANT));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DAMPER));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DIRT_ROAD));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SLIPPERY_ROAD));
            //Debug.Log(LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_CAR_AIRBORNE));

            LogitechGSDK.LogiStopDamperForce(0);
            LogitechGSDK.LogiStopBumpyRoadEffect(0);
            LogitechGSDK.LogiStopDirtRoadEffect(0);
            LogitechGSDK.LogiStopSpringForce(0);
            LogitechGSDK.LogiPlaySpringForce(0, springCenter, springMagnitude, springFalloff);
            //LogitechGSDK.LogiStopSurfaceEffectEffect(0);
            if (surfaceFX)
            {
                var surfaceMag = surfaceMagMin + surfaceMagDelta * carControl.CurrentSpeed / carControl.MaxSpeed;
                var surfaceFreq = surfaceFreqMin + surfaceFreqDelta * carControl.CurrentSpeed / carControl.MaxSpeed;
                LogitechGSDK.LogiPlaySurfaceEffect(0, 0, (int)surfaceMag, (int)surfaceFreq);
            }
            LogitechGSDK.LogiPlayDamperForce(0, 0);
            //			sideSwitch *= -1; 
            //			int sideMag = (int) (carControl.CurrentSpeed/sideScale);
            //			LogitechGSDK.LogiPlaySideCollisionForce(0,sideSwitch*sideMag);
            //Debug.Log(sideSwitch);

        }
        else if (!LogitechGSDK.LogiIsConnected(0))
        {
            actualState = "PLEASE PLUG IN A STEERING WHEEL OR A FORCE FEEDBACK CONTROLLER";
        }
        else
        {
            actualState = "THIS WINDOW NEEDS TO BE IN FOREGROUND IN ORDER FOR THE SDK TO WORK PROPERLY";
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }
    // test methods -----------------------------------------------------------------------------------------------------

    void checkExtremes(LogitechGSDK.DIJOYSTATE2ENGINES rec)
    {
        if (rec.lX > maxx)
        {
            maxx = rec.lX;
        }
        else if (rec.lX < minx)
        {
            minx = rec.lX;
        }
    }
    void updateGear()
    {
        if (park)
        {
            currentgear = 0;
        }
        else if (reverse)
        {
            currentgear = 1;
        }
        else if (neutral)
        {
            currentgear = 2;
        }
        else if (drive)
        {
            currentgear = 3;
        }
        else
        {
            Debug.Log("You must have really messed up somehow.");
        }
    }

}
