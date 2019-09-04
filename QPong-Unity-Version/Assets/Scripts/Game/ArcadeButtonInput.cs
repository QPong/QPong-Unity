using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ArcadeButtonInput : MonoBehaviour
{

    public ArcadeButtonGates isButtonPressed()
    {

        foreach (string buttonName in System.Enum.GetNames(typeof(ArcadeButtonGates)))
        {
            if (buttonName == "None") { continue; }
            if (Input.GetButtonDown(buttonName.ToUpper()))
            {
                ArcadeButtonGates button;
                System.Enum.TryParse<ArcadeButtonGates>(buttonName, out button);
                print("button " + button);
                return button;
            }
        }

        return ArcadeButtonGates.None;
    }
}