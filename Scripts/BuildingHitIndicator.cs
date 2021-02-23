using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHitIndicator : MonoBehaviour
{

    public BuildingManager buildingManagerScript = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {

            buildingManagerScript.allBuildings.Remove(this.gameObject);

            buildingManagerScript.checkForGameRestart();

            Destroy(collision.gameObject);

            Destroy(gameObject);

        }

    }

}
