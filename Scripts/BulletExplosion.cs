using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{

    public float timeForDestroy = 1.2f;


    private void Start()
    {

        Invoke(nameof(selfDestroy), timeForDestroy);
        
    }

    private void selfDestroy()
    {

        Destroy(gameObject);

    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {

            Destroy(collision.gameObject);

        }

    }




}
