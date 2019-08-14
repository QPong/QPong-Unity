using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Player needs to hit the Start button to start the game instead of any button
        if (Input.GetButtonDown("Start")) {
            GameController.Instance.StartGame();
        }
    }
}
