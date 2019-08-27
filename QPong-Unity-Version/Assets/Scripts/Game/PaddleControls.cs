using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControls : MonoBehaviour
{
    public GameObject ball;
    public GameObject ballObject;
    public GameObject[] ballArray;
    public bool instantiateBallFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().color.a > 0) {
            if (instantiateBallFlag) {
                instantiateBallFlag = false;
                Vector2 ballPosition = new Vector2(transform.position.x, transform.position.y+3);
                ballObject = Instantiate(ball, ballPosition, Quaternion.identity);
                ballObject.name = "ball"+name[name.Length-1];
                ballObject.GetComponent<SuperposedBallControl>().stateProbability = GetComponent<SpriteRenderer>().color.a;
            }
        } 
        else {
            instantiateBallFlag = false;
        }
        
    }
}
