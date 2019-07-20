using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControls : MonoBehaviour
{
    public Transform theBall;
    private Rigidbody2D rb2d;
    public float balPosX;
    public float balVelY;
    public float random;
    public float speed = 35f;
    public float randomRange = 7f;
    public float boundX = 35.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        theBall = GameObject.FindGameObjectWithTag("Ball").transform;
    }

    // Update is called once per frame
    void Update()
    {
        balPosX = theBall.position.x + random;
        balVelY = theBall.GetComponent<Rigidbody2D>().velocity.y;
        Vector2 vel = rb2d.velocity;   

        // if the ball is moving towards computer paddle
        if (balVelY > 0) {
            if (balPosX > rb2d.position.x + 0.2){
                vel.x = speed;
            }
            else if (balPosX < rb2d.position.x - 0.2) {
                vel.x = -speed;
            }
            else {
                vel.x = 0;
            }
        } 
        // if the ball is moving away, computer paddle moving is slowed down
        else if (balVelY < 0) {
            if (balPosX > rb2d.position.x + 0.2){
                vel.x = speed * 0.5f;
            }
            else if (balPosX < rb2d.position.x - 0.2) {
                vel.x = -speed * 0.5f;
            }
            else {
                vel.x = 0;
            }
        }
        // Get the AI to move back towards the centre when a new round begins

        rb2d.velocity = vel;
        random = Random.Range(-randomRange, randomRange);

        // Limit paddle position on screen
        var pos = transform.position;
        if (pos.x > boundX) {
            pos.x = boundX;
        }
        else if (pos.x < -boundX) {
            pos.x = -boundX;
        }
        transform.position = pos;
    }
    void ResetPaddle()
    {
        var pos = transform.position;
        pos.x = 0;
        transform.position = pos;
    }
}