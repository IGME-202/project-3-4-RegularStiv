using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    // terrain data
    public TerrainData terrain;
    //after grabbed by a human it moves to a new location
    public void OnGrab()
    {
        transform.position = new Vector3(Random.Range(0, terrain.size.x), gameObject.transform.localScale.y / 2, Random.Range(0, terrain.size.z));
    }
}
