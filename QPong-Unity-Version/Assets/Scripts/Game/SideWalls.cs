using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour
{
    public float startSide;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.tag == "Ball"){
            string wallName = transform.name;
            gameManager.Score(wallName);
            if (wallName == "TopWall"){
                startSide = 1f;
            }
            else if (wallName == "BottomWall"){
                startSide = -1f;
            }
            hitInfo.gameObject.GetComponent<SuperposedBallControl>().RestartRound(startSide);
        }
    }

}
