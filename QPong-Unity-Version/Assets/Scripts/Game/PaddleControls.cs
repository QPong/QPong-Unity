using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

public class PaddleControls : MonoBehaviour
{
    public GameObject ball;
    public GameObject ballObject;
    public GameObject[] ballArray;
    public bool instantiateBallFlag;
    GameObject circuitGrid;
    CircuitGridControl circuitGridControlScript;
    CircuitGridClient circuitGridClientScript;
    public string gateString;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        circuitGrid = GameObject.Find("CircuitGrid");
        circuitGridControlScript = circuitGrid.GetComponent<CircuitGridControl>();
        circuitGridClientScript = circuitGrid.GetComponent<CircuitGridClient>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ballArray = gameManager.ballArray;
    }

    // Update is called once per frame
    void Update()
    {
        // generate ball if the state probability > 0
        if (GetComponent<SpriteRenderer>().color.a > 0) {
            if (instantiateBallFlag) {
                instantiateBallFlag = false;
                Vector3 ballPosition = new Vector3(transform.position.x, transform.position.y+3, 0f);

                int stateInDecimal = (int) Char.GetNumericValue(name[name.Length-1]);
                ballArray[stateInDecimal].transform.position = ballPosition;
                // change sprite, color and enable collider
                ballArray[stateInDecimal].GetComponent<SuperposedBallControl>().ballType = "QuantumBall";

                // save gateString at the moment of collision with quantum paddle for measurement later
                gateString = circuitGridClientScript.gateString;
                Debug.Log("Gate String for ball "+gateString);

                ballArray[stateInDecimal].GetComponent<SuperposedBallControl>().gateString = gateString;
                ballArray[stateInDecimal].GetComponent<SpriteRenderer>().color = new Color(0.2f, 1f, 1f, 0.3f);
                ballArray[stateInDecimal].GetComponent<BoxCollider2D>().enabled = true;
                // kick the ball
                ballArray[stateInDecimal].GetComponent<SuperposedBallControl>().StartQuantumBall();            
            }
        } 
        else {
            instantiateBallFlag = false;
        }
        
    }
}
