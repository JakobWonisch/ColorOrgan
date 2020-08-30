using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RepositioningListener : MonoBehaviour
{

    /*
     * Waits for double clicks on gripaction to reposition keyboard, cartridge slot, etc...
     */

    public SteamVR_Action_Boolean gripAction;
    public SteamVR_Action_Vibration vibrationAction;

    public Transform sustainSwitch, cartridgeSlot, keyboard;
    public Transform sustainTarget, slotTarget, keyboardTarget; // where the objects should be placed

    private float interval = 0.2f;

    private bool hasClicked;
    private float lastInput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hasClicked && Time.time - lastInput > interval)
            hasClicked = false;

        if (gripAction.GetStateUp(SteamVR_Input_Sources.Any) || Input.GetKeyUp(KeyCode.P))
        {
            if (hasClicked)
            {
                // reposition
                Debug.Log("Repositioning");

                sustainSwitch.position = sustainTarget.position;
                sustainSwitch.rotation = sustainTarget.rotation;

                cartridgeSlot.position = slotTarget.position;
                cartridgeSlot.rotation = slotTarget.rotation;

                keyboard.position = keyboardTarget.position;
                keyboard.rotation = keyboardTarget.rotation;

                vibrationAction.Execute(0, 0.05f, 100, 0.2f, SteamVR_Input_Sources.Any);

                hasClicked = false;
            }
            else
            {
                lastInput = Time.time;
                hasClicked = true;
            }
        }
    }
}
