using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public float stepX = 5.0f;
    public float stepY = 5.0f;
    public float boundX = 5.0f;
    public float boundY = 35.0f;
    public float centerX = 56.0f;

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;

        if (Input.GetKeyDown(moveUp)) {
            pos.y += stepY;
        }
        else if (Input.GetKeyDown(moveDown)) {
            pos.y += -stepY;
        }
        else if (Input.GetKeyDown(moveLeft)) {
            pos.x += -stepX;
        }
        else if (Input.GetKeyDown(moveRight)) {
            pos.x += stepX;
        }

        if (pos.y > boundY) {
            pos.y = boundY;
        }
        else if (pos.y < -boundY) {
            pos.y = -boundY;
        }

        if (pos.x > centerX + boundX) {
            pos.x = centerX + boundX;
        }
        else if (pos.x < centerX - boundX) {
            pos.x = centerX - boundX;
        }

        transform.position = pos;
    }
}
