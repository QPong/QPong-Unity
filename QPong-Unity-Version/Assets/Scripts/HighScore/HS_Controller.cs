using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_Controller : MonoBehaviour
{
    HS_HUD hud;
    int selectedInitial;
    bool joystickReleased = true;
    void Start()
    {
        selectedInitial = 0;
        hud = gameObject.GetComponent<HS_HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyPressed();
    }

    void CheckKeyPressed() {
        if (Input.GetButtonDown("Start")) {
            // Done. Save score and back to main menu
            // TODO: Save Scores
            GameController.Instance.LoadMainMenu();
        }

        float hVvalue = Input.GetAxis ("Horizontal");
        float vValue = Input.GetAxis ("Vertical");

        if (hVvalue == 0 && vValue == 0) {
            joystickReleased = true;
            return;
        }

        if (!joystickReleased) return;

        // Check horizontal movement
        if (hVvalue > 0f) {
            // Select next initial
            selectedInitial += 1;
            selectedInitial = selectedInitial % 3;
        } else if (hVvalue < 0f) {
            // Select previous initial
            selectedInitial -= 1;
            selectedInitial = selectedInitial % 3;
        }

        // Check vertical movement
        if (vValue > 0f) {
            // Next letter
            hud.ChangeInitial(selectedInitial, +1);
        } else if (vValue < 0f) {
            // Previous letter
            hud.ChangeInitial(selectedInitial, -1);
        }
        joystickReleased = false;
    }

}
