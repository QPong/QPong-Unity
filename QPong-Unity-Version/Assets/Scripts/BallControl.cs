using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public float speed = 30;
    private Rigidbody2D rb2d;

    void GoBall(){
        float rand = Random.Range(0, 2);
        if (rand < 1) {
            rb2d.velocity = new Vector2(1,1) * speed;
        } else {
            rb2d.velocity = new Vector2(-1,1) * speed;
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

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.collider.CompareTag("Player")){
            Vector2 vel;
            vel.x = rb2d.velocity.x * 1.1f;
            vel.y = (rb2d.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
            rb2d.velocity = vel;
        }
    }
}
