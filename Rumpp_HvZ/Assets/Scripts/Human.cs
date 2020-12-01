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
        Vector3 uForce = Vector3.zero;
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
        //pushes the other humans away if colliding 
        for (int i = 0; i < GameManager.humans.Count; i++)
        {
            if (gameObject != GameManager.humans[i] && AABBCollision(gameObject.GetComponent<BoxCollider>(), GameManager.humans[i].GetComponent<BoxCollider>()))
            {
                uForce += Flee(GameManager.humans[i]);
            }
        }
       
        // pushes the object back towards the center of the terrain if it is too close to one side 
        if (transform.position.x > terrainRadius)
        {
            uForce += Seek(new Vector3(10, 0, 10)) * 1.5f;
        }
        if (transform.position.x < 2.5)
        {
            uForce += Seek(new Vector3(10, 0, 10)) * 1.5f;
        }
        if (transform.position.z > terrainRadius)
        {
            uForce += Seek(new Vector3(10, 0, 10)) * 1.5f;
        }
        if (transform.position.z < 2.5)
        {
            uForce += Seek(new Vector3(10, 0, 10)) * 1.5f;
        }

        // adds all forces clamps them and applies the force 
        if (Vector3.Distance(targetZombie.transform.position, gameObject.transform.position) < 6)
        {
            uForce += Evade(targetZombie);
        }
        // wanders if there are no zombies around 
        else
        {
            uForce += Wander();
        }
        // applies other forces
        uForce += ObjectAvoidance() * avoidanceForce;
        uForce.y = 0;
        uForce = Vector3.ClampMagnitude(uForce, maxForce);
        ApplyForce(uForce);
    }
    
    // if the zombies hit the humans they convert by being added to the list of zombies and are destroyed
    public void Convert()
    {
        GameObject newZombie = Instantiate(zombie, new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), Quaternion.identity);
        newZombie.GetComponent<Zombie>().debugLinesOn = debugLinesOn;
        GameManager.zombies.Add(newZombie);
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
        // shows the future pos as a line
        if (debugLinesOn)
        {
            base.OnRenderObject();
            Vector3 debugLocation = transform.position;
            debugLocation.y = debugLocation.y + 1;
            purple.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(debugLocation);
            GL.Vertex(debugLocation + velocity);
            GL.End();
        }
    }
}
