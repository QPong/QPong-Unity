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
    public string ballType;
    // store the gateString at the moment of collision with quantum paddle for measurement
    public string gateString;

    public void GoBall(){
        float rand = Random.Range(-2f, 2f);
        if (startDirection > 0) {
            rb2d.velocity = new Vector2(rand,-1).normalized * speed;
        } else {
            rb2d.velocity = new Vector2(rand,1).normalized * speed;
        }
    }
    // always start the ball upwards when the ball hits quantum paddle to avoid getting stuck
    public void StartQuantumBall(){
        float rand = Random.Range(-2f, 2f);
        rb2d.velocity = new Vector2(rand,1).normalized * speed;
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
        // make the ball visible and enable collider
        GetComponent<SuperposedBallControl>().ballType = "ClassicalBall";
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 1f, 1f);
        GetComponent<BoxCollider2D>().enabled = true;
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
            // if quantum state has no superposition
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
            // if quantum state has superposition
            else {
                Debug.Log("State Probability: "+stateProbability);
                for (int i = 0; i < 8; i++)
                {
                    /*
                    // generate a ball in all paddles with finite probability, except the paddle in contact with the ball
                    if (col.gameObject.name != circuitGridControlScript.paddleArray[i].name) {
                        circuitGridControlScript.paddleArray[i].GetComponent<PaddleControls>().instantiateBallFlag = true;
                    }
                    */
                    // hide the incoming ball and disable collider
                    ballType = "HiddenBall";
                    GetComponent<SpriteRenderer>().color = new Color(10, 1, 1, 0.3f);
                    GetComponent<BoxCollider2D>().enabled = false;
                    // generate a ball in all paddles with finite probability
                    circuitGridControlScript.paddleArray[i].GetComponent<PaddleControls>().instantiateBallFlag = true;
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
