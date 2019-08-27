using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControls : MonoBehaviour
{
    public GameObject theBall;
    private Rigidbody2D rb2d;
    private Vector2 ballPos;
    private float balVelY;
    public float speed = 20f;
    public float boundX = 35.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // find the closes ball
        theBall = FindClosestEnemy();
        balVelY = theBall.GetComponent<Rigidbody2D>().velocity.y; 

        // if the ball is moving towards computer paddle
        if (balVelY > 0) {
            ballPos = theBall.transform.localPosition;

            if (transform.localPosition.x > -boundX && ballPos.x < transform.localPosition.x) {
                transform.localPosition += new Vector3 (-speed * Time.deltaTime, 0, 0);
            }

            if (transform.localPosition.x < boundX && ballPos.x > transform.localPosition.x) {
                transform.localPosition += new Vector3 (speed * Time.deltaTime, 0, 0);
            }
        } 
    }
    public void ResetPaddle()
    {
        var pos = transform.position;
        pos.x = 0;
        transform.position = pos;
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Ball");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        Debug.Log("The Closest Ball: "+closest.name);
        return closest;
    }
}