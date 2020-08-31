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
        Reposition();
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
                Reposition();

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

    private void Reposition()
    {
        Debug.Log("Repositioning");
        Quaternion baseRot = Quaternion.Euler(0, sustainTarget.parent.rotation.y, 0);

        sustainSwitch.position = sustainTarget.position;
        sustainSwitch.rotation = baseRot * sustainSwitch.localRotation;

        cartridgeSlot.position = slotTarget.position;
        cartridgeSlot.rotation = baseRot * slotTarget.localRotation;

        keyboard.position = keyboardTarget.position;
        keyboard.rotation = baseRot * keyboardTarget.localRotation;

    }
}
