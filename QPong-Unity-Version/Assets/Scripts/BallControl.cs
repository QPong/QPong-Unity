using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public float speed = 30;
    public int startDirection;
    private Rigidbody2D rb2d;

    void GoBall(){
        float rand = Random.Range(-2f, 2f);
        float startDirection = Random.Range(-1f,1f);
        if (startDirection > 0) {
            rb2d.velocity = new Vector2(rand,-1).normalized * speed;
        } else {
            rb2d.velocity = new Vector2(rand,1).normalized * speed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Invoke("GoBall", 2); // wait for 2 seconds to give players time to get ready
    }

    void ResetBall(){
        rb2d.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void RestartGame(){
        ResetBall();
        Invoke("GoBall", 1);
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Hit the classical paddle?
        if (col.gameObject.CompareTag("ClassicalPaddle")) {
            // Calculate hit Factor
            float x = hitFactor(transform.position,
                            col.transform.position,
                            col.collider.bounds.size.x);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(x, -1).normalized;

            // Set Velocity with dir * speed
            rb2d.velocity = dir * rb2d.velocity.magnitude * 1.1f;
            Debug.Log("Hit Classical Paddle");
        }

        // Hit the quantum paddle?
        if (col.gameObject.CompareTag("QuantumPaddle")) {
            // Calculate hit Factor
            float x = hitFactor(transform.position,
                            col.transform.position,
                            col.collider.bounds.size.x);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(x, 1).normalized;

            // Set Velocity with dir * speed
            rb2d.velocity = dir * rb2d.velocity.magnitude * 1.1f;
            Debug.Log("Hit Quantum Paddle");
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth) {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.x - racketPos.x) / racketWidth;
    }
}
