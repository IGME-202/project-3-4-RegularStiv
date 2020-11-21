using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    // zombie object
    public GameObject zombie;
    protected override void ClacSteeringForce()
    {
        // sets the zombie object
        GameObject targetZombie = GameManager.zombies[0];
        float distance = 100;
        //compares the distance of all the zombies and runs from the closest one 
        for (int i = 0; i < GameManager.zombies.Count; i++)
        {
            if (Vector3.Distance(gameObject.transform.position, GameManager.zombies[i].transform.position) < distance)
            {
                distance = Vector3.Distance(gameObject.transform.position, GameManager.zombies[i].transform.position);
                targetZombie = GameManager.zombies[i];
            }
        }
        Vector3 uForce = Vector3.zero;

        if (transform.position.x > terrainRadius)
        {
            uForce += Seek(new Vector3(10, 0, 10));
        }
        if (transform.position.x < 2.5)
        {
            uForce += Seek(new Vector3(10, 0, 10));
        }
        if (transform.position.z > terrainRadius)
        {
            uForce += Seek(new Vector3(10, 0, 10));
        }
        if (transform.position.z < 2.5)
        {
            uForce += Seek(new Vector3(10, 0, 10));
        }
        // adds all forces clamps them and applies the force 
        if (Vector3.Distance(targetZombie.transform.position, gameObject.transform.position) < 6)
        {
            uForce += Flee(targetZombie);
        }
        uForce += ObjectAvoidance() * avoidanceForce;
        uForce.y = 0;
        uForce = Vector3.ClampMagnitude(uForce, maxForce);
        ApplyForce(uForce);
        ApplyFriction(.2f);
    }
    
    // if the zombies hit the humans they convert by being added to the list of zombies and are destroyed
    public void Convert()
    {
        GameManager.zombies.Add(Instantiate(zombie, new Vector3(gameObject.transform.position.x, zombie.GetComponent<BoxCollider>().size.y, gameObject.transform.position.z), Quaternion.identity));
        GameManager.humans.Remove(gameObject);
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        // collisions between humans and zombies 
        for (int i = 0; i < GameManager.zombies.Count; i++)
        {
            if (Vector3.Distance(position, GameManager.zombies[i].transform.position) < radius)
            {
                Convert();
                break;
            }
        }
    }
    protected override Vector3 ObjectAvoidance()
    {
        return base.ObjectAvoidance();
    }
    // debug lines
    protected override void OnRenderObject()
    {
        base.OnRenderObject();

    }
}
