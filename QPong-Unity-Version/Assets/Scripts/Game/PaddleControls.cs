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
    // Start is called before the first frame update
    void Start()
    {
        circuitGrid = GameObject.Find("CircuitGrid");
        circuitGridControlScript = circuitGrid.GetComponent<CircuitGridControl>();
        ballArray = circuitGridControlScript.ballArray;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().color.a > 0) {
            if (instantiateBallFlag) {
                instantiateBallFlag = false;
                Vector3 ballPosition = new Vector3(transform.position.x, transform.position.y+3, 0f);
                //ballObject = Instantiate(ball, ballPosition, Quaternion.identity);
                //ballObject.name = "ball"+name[name.Length-1];
                //ballObject.GetComponent<SuperposedBallControl>().stateProbability = GetComponent<SpriteRenderer>().color.a;
                int stateInDecimal = (int) Char.GetNumericValue(name[name.Length-1]);
                ballArray[stateInDecimal].transform.position = ballPosition;            
            }
        } 
        else {
            instantiateBallFlag = false;
        }
        
    }
}
