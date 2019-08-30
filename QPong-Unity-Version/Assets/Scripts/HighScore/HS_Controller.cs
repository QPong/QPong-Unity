using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_Controller : MonoBehaviour
{
    public Animator animator;
    HS_HUD hud;
    int selectedInitial;

    void Start()
    {
        selectedInitial = 0;
        hud = gameObject.GetComponent<HS_HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyPressed();
        animator.SetInteger("Transition", selectedInitial);
    }

    void CheckKeyPressed() {
        if (Input.GetButtonDown("Start")) {
            // Done. Save score and back to main menu
            GameController.Instance.player.StoreNewHighScore(hud.UserName());
            GameController.Instance.LoadMainMenu();
        }

        if (Input.GetKeyDown(JoystickButtonMaps.left.ToString()) || Input.GetKeyDown(JoystickButtonMaps.a.ToString()))
        {
            // Select previous initial
            selectedInitial -= 1;
            selectedInitial = (3 + selectedInitial) % 3;
        }
        if (Input.GetKeyDown(JoystickButtonMaps.right.ToString()) || Input.GetKeyDown(JoystickButtonMaps.d.ToString()))
        {
            selectedInitial += 1;
            selectedInitial = (3 + selectedInitial) % 3;
        }
        if (Input.GetKeyDown(JoystickButtonMaps.up.ToString()) || Input.GetKeyDown(JoystickButtonMaps.w.ToString()))
        {
            // Previous letter
            hud.ChangeInitial(selectedInitial, -1);
        }
        if (Input.GetKeyDown(JoystickButtonMaps.down.ToString()) || Input.GetKeyDown(JoystickButtonMaps.z.ToString()))
        {
            // Next letter
            hud.ChangeInitial(selectedInitial, +1);
        }

    }

}
