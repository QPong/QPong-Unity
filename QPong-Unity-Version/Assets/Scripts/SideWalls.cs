using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalls : MonoBehaviour
{
    public float startSide;
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            string wallName = transform.name;
            GameManager.Score(wallName);
            if (wallName == "TopWall"){
                startSide = 1f;
            }
            else if (wallName == "BottomWall"){
                startSide = -1f;
            }
            hitInfo.gameObject.GetComponent<BallControl>().RestartGame(startSide);
        }
    }

}
