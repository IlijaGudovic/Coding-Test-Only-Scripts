using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [Header("Manual Settings")]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float meteorSpeed = 5f;
    [SerializeField] private List<GameObject> meteorPrefabs = new List<GameObject>();
    [SerializeField] private BuildingManager buildingScript = null;

    private void Start()
    {

        StartCoroutine(meteorSpawner());

    }

    IEnumerator meteorSpawner()
    {

        yield return new WaitForSeconds(Random.Range(fireRate - 0.2f, fireRate + 0.2f));

        int randomInt = Random.Range(0, 100);

        if (randomInt < 69) // 70 % chance for single spawn 
        {
            spawnMeteor();
        }
        else // 30 % for duble
        {
            spawnMeteor();
            spawnMeteor();
        }

        StartCoroutine(meteorSpawner());

    }

    private void spawnMeteor()
    {

        int randomHorizontal = Random.Range(-90, 90);

        Vector2 spawnPosition = new Vector2((float)randomHorizontal / 10, 6);

        GameObject spawnedMeteor = Instantiate(meteorPrefabs[Random.Range(0, 2)], spawnPosition, Quaternion.identity);

        orderRandomDestination(spawnedMeteor);

    }

    private void orderRandomDestination(GameObject meteor)
    {

        int randomInt = Random.Range(0, buildingScript.allBuildings.Count);

        if (buildingScript.allBuildings[randomInt] == null)
        {

            Destroy(meteor);
            return;

        }

        Vector2 desiredPosition = buildingScript.allBuildings[randomInt].transform.position;

        Vector2 direction = desiredPosition - (Vector2)meteor.transform.position;

        meteor.transform.up = direction;
        meteor.GetComponent<Rigidbody2D>().velocity = meteor.transform.up * meteorSpeed;

    }



}
