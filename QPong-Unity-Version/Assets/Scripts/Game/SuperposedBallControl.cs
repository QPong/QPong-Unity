using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperposedBallControl : MonoBehaviour
{
    public float speed = 30;
    public float startDirection = -1f;
    public float startPosition = 30;
    public float startPositionYOffset = 8;
    private Rigidbody2D rb2d;
    public GameObject ball;
    public GameObject[] ballArray;
    public GameObject[] paddleArray;
    public int ballNumber=2;
    GameObject circuitGrid;
    CircuitGridControl circuitGridControlScript;
    public float stateProbability;

    void GoBall(){
        float rand = Random.Range(-2f, 2f);
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

        circuitGrid = GameObject.Find("CircuitGrid");
        circuitGridControlScript = circuitGrid.GetComponent<CircuitGridControl>();
        paddleArray = circuitGridControlScript.paddleArray;
        //RestartRound(startDirection);
        //GoBall();
    }

    public void ResetBall(float startSide){
        rb2d.velocity = Vector2.zero;
        if (startSide > 0){
            transform.position = new Vector2(0, startPosition + startPositionYOffset);
        }
        else if (startSide < 0){
            transform.position = new Vector2(0, -startPosition + startPositionYOffset);
        }
        
    }

    public void RestartRound(float startSide){
        startDirection = startSide;
        ResetBall(startDirection);
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
            float stateProbability = col.gameObject.GetComponent<SpriteRenderer>().color.a;
            if (stateProbability == 1) {
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
            else {
                Debug.Log("State Probability: "+stateProbability);
                for (int i = 0; i < 8; i++)
                {
                    // generate a ball in all paddles with finite probability, except the paddle in contact with the ball
                    if (col.gameObject.name != circuitGridControlScript.paddleArray[i].name) {
                        circuitGridControlScript.paddleArray[i].GetComponent<PaddleControls>().instantiateBallFlag = true;
                    }
                }
                
            }
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
