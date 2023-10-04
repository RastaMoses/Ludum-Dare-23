using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrb : MonoBehaviour
{
    //Serialize Params
    [SerializeField] float healAmount = 10;
    [SerializeField] float pickupRange = 3f;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float maxLifeTime = 6f;
    [SerializeField] float randomSpawnRange = 1;
    [SerializeField] GameObject popVFX;

    //Cached Comps
    GameObject player;

    //State
    bool inRange;
    float lifeTime = 0;
    float distance;
    Vector3 randomSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<FPS_Controller>().gameObject;
        //Establish Random Point to travel to
        randomSpawnPos = transform.position + new Vector3(Random.Range(-randomSpawnRange,randomSpawnRange), Random.Range(-randomSpawnRange, randomSpawnRange), Random.Range(-randomSpawnRange, randomSpawnRange));
    }

    // Update is called once per frame
    void Update()
    {
        if (!inRange)
        {
            //Move to random spot after spawn
            transform.position = Vector3.Lerp(transform.position, randomSpawnPos, moveSpeed * Time.deltaTime);

            //Calc Player Distance
            distance = Vector3.Distance(transform.position, player.transform.position);

            //If in range follow player
            if(distance <= pickupRange)
            {
                FollowPlayer();
            }
            else
            {
                //Lifetime calc
                lifeTime += Time.deltaTime;

                if(lifeTime >= maxLifeTime)
                {
                    Instantiate(popVFX, transform);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            //Follow Player
            transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void FollowPlayer()
    {
        inRange = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<HP>().Heal(healAmount);
            Instantiate(popVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
