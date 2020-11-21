using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // public objects
    public GameObject human;
    public GameObject zombie;
    public GameObject treasure;
    public GameObject obstacle;
    public Terrain terrain;

    //lists 
    public static List<GameObject> humans;
    public static List<GameObject> zombies;
    public static List<GameObject> obstacles;

    // Start is called before the first frame update
    void Awake()
    {
        //initializes all objects and lists
        humans = new List<GameObject>();
        zombies = new List<GameObject>();
        obstacles = new List<GameObject>();
        terrain.terrainData.size = new Vector3(20, 1, 20);
        for (int i = 0; i < 3; i++)
        {
            obstacles.Add(Instantiate(obstacle, new Vector3(Random.Range(0, terrain.terrainData.size.x), .5f, Random.Range(0, terrain.terrainData.size.z)), Quaternion.identity));
        }
        for (int i = 0; i < 5; i++)
        {
            humans.Add(Instantiate(human, new Vector3(Random.Range(0, terrain.terrainData.size.x), human.GetComponent<BoxCollider>().size.y / 2, Random.Range(0, terrain.terrainData.size.z)), Quaternion.identity));
        }
        zombies.Add(Instantiate(zombie, new Vector3(Random.Range(0, terrain.terrainData.size.x), zombie.GetComponent<BoxCollider>().size.y  , Random.Range(0, terrain.terrainData.size.z)), Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
