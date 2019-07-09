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
    public float speed = 50f;
    public float randomRange = 5f;

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
        else {
            if (rb2d.position.x > 0.1 && rb2d.position.x < -0.1) {
                vel.x = -speed;
            } else if (rb2d.position.x < -0.1 && rb2d.position.x < 0.1) {
                vel.x = speed;
            }
        }
        rb2d.velocity = vel;
        random = Random.Range(-randomRange, randomRange);
    }
}