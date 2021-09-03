using UnityEngine;
using System.Collections;
using System.Text;


public class BlinkerController : MonoBehaviour
{
    //copypasted variables ------------------

    private string actualState;
    public bool xButton;
    //public bool yButton;
    //public bool aButton;
    public bool bButton;
    public bool yButton;
    public bool HomeButton;
   

    // new variables ----------------------
    public GameObject rightBlinker;
    public GameObject leftBlinker;
    public GameObject rightBackBlinker;
    public GameObject leftBackBlinker;
    public bool rightIsBlinking;
    public bool leftIsBlinking;
    //public bool allAreBlinking;
    public bool delayRight;
    public bool delayLeft;
    //public bool delayAll;
    //private IEnumerator coroutine;
    //public int numButton = 0;
    //public bool canTurnOff;
    //public bool wasOn;
    //public bool canBlink = false;


    // Blinker Arrows
    public GameObject leftBlinkerArrow;
    public GameObject rightBlinkerArrow;

    public bool canTurnRightOff;
    public bool canTurnLeftOff;
    public bool canTurnAllOff;
    public AudioSource blinkerSound;
    public bool allAreBlinking;

    public GameObject rightSound;
    public GameObject leftSound;
    public GameObject hazardSound;

    public bool leftComplete = false;
    public bool rightComplete = false;

    //public bool rightIs



    // Start is called before the first frame update
    void Start()
    {
        // canTurnOff = false;
        allAreBlinking = false;
         
        canTurnRightOff = false;
        canTurnLeftOff = false;
        //canTurnAllOff = false; 
        rightBlinker.SetActive(false);
        leftBlinker.SetActive(false);
        rightBackBlinker.SetActive(false);
        leftBackBlinker.SetActive(false);
        rightIsBlinking = false;
        leftIsBlinking = false;
        //allAreBlinking = false;
        delayRight = false;
        delayLeft = false;
        //delayAll = false;

        Debug.Log(LogitechGSDK.LogiSteeringInitialize(false));

        InvokeRepeating("blinkRightLight", 0f, 0.25f);
        InvokeRepeating("blinkLeftLight", 0f, 0.25f);
        InvokeRepeating("blinkHazard", 0f, 0.25f);
        //InvokeRepeating("blinkAllLights", 0f, 0.15f);

        leftSound.SetActive(false);
        rightSound.SetActive(false);

        leftBlinkerArrow.SetActive(false);
        rightBlinkerArrow.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {

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
            HomeButton = rec.rgbButtons[10] == 128;

            //aButton = rec.rgbButtons[0] == 128;//a 0
            bButton = rec.rgbButtons[1] == 128;//b 1
            xButton = rec.rgbButtons[2] == 128;//x 2
            yButton = rec.rgbButtons[3] == 128;//y 3
            //yButton = rec.rgbButtons[3] == 128;//y 3
            /**
            if (bButton)
            {
                if (!rightIsBlinking)
                {
                    rightIsBlinking = true;
                    numButton++;
                    Debug.Log("B button pressed for" + numButton + "frames");
                    
                    canBlink = true;
                    coroutine = blinkLights();
                    StartCoroutine(coroutine);
                    Invoke("StopRightBlinker", 5f);
                }
                else if(canTurnOff)
                {
                    Debug.Log("Blinker manually turned off");
                    rightIsBlinking = false;
                    Invoke("StopRightBlinker", 0f);
                    canTurnOff = false;
                }
            }
            else if (rightIsBlinking)
            {
                canTurnOff = true;
            }
    **/
        /*
            if (yButton)
            {
                //if left is on and right is pressed, turn left off first
                if (leftIsBlinking && rightIsBlinking)
                {
                    leftIsBlinking = false;
                    rightIsBlinking = false;
                    //allAreBlinking = false;
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    canTurnLeftOff = false;
                    canTurnRightOff = false;
                    //canTurnAllOff = false;
                }
                //if right is off, turn right on
                if (!rightIsBlinking && !canTurnRightOff && !leftIsBlinking && !canTurnLeftOff)
                {
                    rightIsBlinking = true;
                    leftIsBlinking = true;
                    //allAreBlinking = true;
                }
                //if button is pressed and Off variable is on, turn right off
                else if (canTurnRightOff && canTurnLeftOff)
                {
                    rightIsBlinking = false;
                    leftIsBlinking = false;
                    //allAreBlinking = false;
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    //canTurnRightOff = false;
                }
            }
            //if right is on and button isnt pressed, turn Off variable on
            else
            {
                if (rightIsBlinking && leftIsBlinking)
                {
                    canTurnLeftOff = true;
                    canTurnRightOff = true;
                    //canTurnAllOff = true;
                }
                else if (canTurnRightOff && canTurnLeftOff)
                {
                    canTurnLeftOff = false;
                    canTurnRightOff = false;
                    //canTurnAllOff = false;
                }
            }
            */
            //right blinker ==---------------------------------------------
            if (bButton)
            {
                //if left is on and right is pressed, turn left off first
                if (allAreBlinking)
                {
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    canTurnAllOff = false;
                    leftSound.SetActive(false);
                    rightSound.SetActive(false);
                    leftBlinkerArrow.SetActive(false);
                    rightBlinkerArrow.SetActive(false);
                }
                if (leftIsBlinking)
                {
                    leftIsBlinking = false;
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    canTurnLeftOff = false;
                    leftSound.SetActive(false);
                    leftBlinkerArrow.SetActive(false);
                }
                //if right is off, turn right on
                if (!rightIsBlinking && !canTurnRightOff)
                {
                    rightIsBlinking = true;
                    rightSound.SetActive(true);
                    rightBlinkerArrow.SetActive(true);
                }
                //if button is pressed and Off variable is on, turn right off
                else if (canTurnRightOff)
                {
                    rightIsBlinking = false;
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    rightSound.SetActive(false);
                    //canTurnRightOff = false;
                    rightBlinkerArrow.SetActive(false);

                }
            }
            //if right is on and button isnt pressed, turn Off variable on
            else
            {
                if (rightIsBlinking)
                {
                    canTurnRightOff = true;
                }
                else if (canTurnRightOff)
                {
                    canTurnRightOff = false;
                }
            }
            //left blinker ----------------------------------------------
            if (xButton)
            {
                if (allAreBlinking)
                {
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    canTurnAllOff = false;
                    leftSound.SetActive(false);
                    rightSound.SetActive(false);
                    leftBlinkerArrow.SetActive(false);
                    rightBlinkerArrow.SetActive(false);
                }
                if (rightIsBlinking)
                {
                    rightIsBlinking = false;
                    rightBlinker.SetActive(false);
                    rightBackBlinker.SetActive(false);
                    canTurnRightOff = false;
                    rightSound.SetActive(false);
                    rightBlinkerArrow.SetActive(false);
                }
                if (!leftIsBlinking && !canTurnLeftOff)
                {
                    leftIsBlinking = true;
                    leftSound.SetActive(true);
                }
                else if (canTurnLeftOff)
                {
                    leftIsBlinking = false;
                    leftBlinker.SetActive(false);
                    leftBackBlinker.SetActive(false);
                    leftSound.SetActive(false);
                    leftBlinkerArrow.SetActive(false);
                    //canTurnLeftOff = false;
                }
            }
            else
            {
                if (leftIsBlinking)
                {
                    canTurnLeftOff = true;
                }
                else if (canTurnLeftOff)
                {
                    canTurnLeftOff = false;
                }
            }
            
            //hazard lights  ---------------------------------------------
            if (yButton)
            {
                //first frame
                if (!allAreBlinking && !canTurnAllOff)
                {
                    TurnOffLeft();
                    TurnOffRight();
                    allAreBlinking = true;
                    leftSound.SetActive(true);
                    rightSound.SetActive(true);

                }
                //first frame when button press while blinking
                else if(canTurnAllOff)
                {
                    allAreBlinking = false;
                    TurnOffRight();
                    TurnOffLeft();



                }

            }
            //release button
            else
            {
                //release button while blinker is on
                if (allAreBlinking)
                {
                    canTurnAllOff = true;
                }
                //default branch when blinkers arent used
                else if (canTurnAllOff)
                {
                    canTurnAllOff = false;
                }
            }
            CheckBlinkers(rec);
            



        }


    }
    /**
    IEnumerator blinkLights()
    {
        //rightIsBlinking = true;
        while (canBlink)
        {
            rightBlinker.SetActive(true);
        
            
            yield return new WaitForSeconds(0.1f);
            rightBlinker.SetActive(false);
            
            
            yield return new WaitForSeconds(0.1f);
        }


    }
    **/

    void blinkAllLights()
    {
        if (rightIsBlinking && leftIsBlinking)
        {
            if (!rightBlinker.activeInHierarchy && !leftBlinker.activeInHierarchy)
            {
                rightBlinker.SetActive(true);
                rightBackBlinker.SetActive(true);
                leftBlinker.SetActive(true);
                leftBackBlinker.SetActive(true);
                leftBlinkerArrow.SetActive(true);
                rightBlinkerArrow.SetActive(true);
            }
            else
            {
                rightBlinker.SetActive(false);
                rightBackBlinker.SetActive(false);
                leftBlinker.SetActive(false);
                leftBackBlinker.SetActive(false);
                leftBlinkerArrow.SetActive(false);
                rightBlinkerArrow.SetActive(false);
            }
        }

    }
    
    void blinkRightLight()
    {
        if (rightIsBlinking)
        {
            if (!rightBlinker.activeInHierarchy)
            {
                rightBlinker.SetActive(true);
                rightBackBlinker.SetActive(true);
                rightBlinkerArrow.SetActive(true);
            }
            else
            {
                rightBlinker.SetActive(false);
                rightBackBlinker.SetActive(false);
                rightBlinkerArrow.SetActive(false);
            }
        }
        
    }
    void blinkLeftLight()
    {
        if (leftIsBlinking)
        {
            if (!leftBlinker.activeInHierarchy)
            {
                leftBlinker.SetActive(true);
                leftBackBlinker.SetActive(true);
                leftBlinkerArrow.SetActive(true);

            }
            else
            {
                leftBlinker.SetActive(false);
                leftBackBlinker.SetActive(false);
                leftBlinkerArrow.SetActive(false);
            }
        }
    }
    void blinkHazard()
    {
        if (allAreBlinking)
        {
            if (!leftBlinker.activeInHierarchy && !rightBlinker.activeInHierarchy)
            {
                leftBlinker.SetActive(true);
                leftBackBlinker.SetActive(true);
                rightBlinker.SetActive(true);
                rightBackBlinker.SetActive(true);
                leftBlinkerArrow.SetActive(true);
                rightBlinkerArrow.SetActive(true);
            }
            else
            {
                leftBlinker.SetActive(false);
                leftBackBlinker.SetActive(false);
                rightBlinker.SetActive(false);
                rightBackBlinker.SetActive(false);
                leftBlinkerArrow.SetActive(false);
                rightBlinkerArrow.SetActive(false);
            }
        }
    }

    /*
    void TurnOffAll()
    {
        rightIsBlinking = false;
        leftIsBlinking = false;
        canTurnRightOff = false;
        canTurnLeftOff = false;
        //allAreBlinking = false;
        //canTurnAllOff = false;
    }
    */
    void TurnOffRight()
    {
        rightIsBlinking = false;
        canTurnRightOff = false;
        rightBlinker.SetActive(false);
        rightBackBlinker.SetActive(false);
        rightSound.SetActive(false);
        rightBlinkerArrow.SetActive(false);
    }
    void TurnOffLeft()
    {
        leftIsBlinking = false;
        canTurnLeftOff = false;
        leftBlinker.SetActive(false);
        leftBackBlinker.SetActive(false);
        leftSound.SetActive(false);
        leftBlinkerArrow.SetActive(false);
    }
    
    /**
    void StopRightBlinker()
    {
        if (rightIsBlinking)
        {
            Debug.Log("turned off");
            canTurnOff = false;
            canBlink = false;
            rightIsBlinking = false;
            StopCoroutine(coroutine);
            rightBlinker.SetActive(false);
        }

    }
    **/

    void CheckBlinkers(LogitechGSDK.DIJOYSTATE2ENGINES rec)
    {
        if (leftIsBlinking)
        {
            if(rec.lX < -12000)
            {
                leftComplete = true;
            }
            else if((rec.lX > -2000 && rec.lX < 2000) && leftComplete)
            {
                TurnOffLeft();
                leftComplete = false;
            }
        }

        if (rightIsBlinking)
        {
            if (rec.lX > 12000)
            {
                rightComplete = true;
            }
            else if ((rec.lX > -2000 && rec.lX < 2000) && rightComplete)
            {
                TurnOffRight();
                rightComplete = false;
            }
        }


    }
}
