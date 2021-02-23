using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : MonoBehaviour
{

    [Header("Buildings Manual Settings")]
    [SerializeField] private GameObject basicBuildingPrefab = null;
    [SerializeField] private GameObject rifleBuildingPrefab = null;

    [SerializeField] private Vector2 offsetBetweenBuilldings = new Vector2();

    [Header("Buildings Store")]
    [SerializeField] private GameObject buildingsFolder = null;
    private List<GameObject> rifleBuildings = new List<GameObject>();
    public List<GameObject> allBuildings;

    [Header("Rifle Settings")]
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private GameObject explosionPrefab = null;
    [SerializeField] private float bulletSpeed = 10;
    public List<bool> rifleCooldown = new List<bool>();
    private Vector2 mousePosition;
    [SerializeField] GameObject shootIndicator = null;


    void Start()
    {

        spawnBuildings();

        for (int i = 0; i < rifleCooldown.Count; i++)
        {
            rifleCooldown[i] = true;
        }
        
    }


    private void spawnBuildings()
    {

        float horizontalSpawnPosition = 0 - 9 / 2 * offsetBetweenBuilldings.x; //Calculating horizontal start spawn position
        Vector2 startingSpawnPosition = new Vector2(horizontalSpawnPosition, -4.5f);

        for (int i = 0; i < 9; i++)
        {

            GameObject spawnedBuilding;

            if (i % 4 == 0) //Spawn Rifle Building
            {

                Vector2 spawnPosition = new Vector2(startingSpawnPosition.x + i * offsetBetweenBuilldings.x, startingSpawnPosition.y);

                spawnedBuilding = Instantiate(rifleBuildingPrefab, spawnPosition, Quaternion.identity);

                spawnedBuilding.GetComponent<BuildingHitIndicator>().buildingManagerScript = this;

                rifleBuildings.Add(spawnedBuilding);

            }
            else //Spawn Basic Builiding
            {

                Vector2 spawnPosition = new Vector2(startingSpawnPosition.x + i * offsetBetweenBuilldings.x, startingSpawnPosition.y);

                spawnedBuilding = Instantiate(basicBuildingPrefab, spawnPosition, Quaternion.identity);

                spawnedBuilding.GetComponent<BuildingHitIndicator>().buildingManagerScript = this;

            }

            allBuildings.Add(spawnedBuilding);

            if (buildingsFolder != null)
            {
                spawnedBuilding.transform.SetParent(buildingsFolder.transform);
            }

        }

        #region Swich Place

        GameObject saveBuilding = rifleBuildings[0];

        rifleBuildings[0] = rifleBuildings[1];

        rifleBuildings[1] = rifleBuildings[2];

        rifleBuildings[2] = saveBuilding;

        #endregion

    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            tryToShoot(mousePosition);

            shootIndicator.transform.position = mousePosition;

        }

    }

    private void tryToShoot(Vector2 desiredBulletDestination)
    {

        for (int i = 0; i < rifleCooldown.Count; i++)
        {

            if (rifleCooldown[i] == true)
            {

                rifleCooldown[i] = false;

                if (rifleBuildings[i] != null)
                {

                    //Start Shooting
                    int buildingIndex = i;
                    shootTowards(desiredBulletDestination, buildingIndex);

                    break;

                }

            }
            
        }

    }

    private void shootTowards(Vector2 desiredBulletDestination, int index)
    {

        Vector2 spawnPosition = new Vector2(rifleBuildings[index].transform.position.x , rifleBuildings[index].transform.position.y + 0.5f); // building position (x, y + offsets)

        GameObject spawnedBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        Vector2 direction = desiredBulletDestination - spawnPosition;

        float distance = Vector2.Distance(desiredBulletDestination, spawnPosition);

        spawnedBullet.transform.up = direction;
        spawnedBullet.GetComponent<Rigidbody2D>().velocity = spawnedBullet.transform.up * bulletSpeed;

        float calculateTimeForStop = distance / bulletSpeed;

        StartCoroutine(resetColdown(calculateTimeForStop, index, spawnedBullet)); // Invoke Stop Move

    }


    IEnumerator resetColdown(float timeForReset, int buildingIndex, GameObject bullet)
    {

        yield return new WaitForSeconds(timeForReset);

        Instantiate(explosionPrefab, bullet.transform.position, Quaternion.identity);

        Destroy(bullet);

        yield return new WaitForSeconds(1.2f);

        rifleCooldown[buildingIndex] = true;

    }

    public void checkForGameRestart()
    {

        if (allBuildings.Count == 0)
        {
            //Restart Game
            Debug.Log("Restart Game");

            SceneManager.LoadScene(0);

        }

    }

    



}
