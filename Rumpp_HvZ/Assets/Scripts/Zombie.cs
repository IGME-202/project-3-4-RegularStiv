using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Zombie : Vehicle
{
    private GameObject targetHuman;
    // debug lines 
    protected override void OnRenderObject()
    {
        base.OnRenderObject();
        if (targetHuman != null && debugLinesOn)
        {
            // shows the zombies target and future position
            Vector3 debugLocation = transform.position;
            debugLocation.y = debugLocation.y + 1;
            black.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(debugLocation);
            Vector3 chaseTarget = targetHuman.transform.position;
            chaseTarget.y = chaseTarget.y + 1;
            GL.Vertex(chaseTarget);
            GL.End();
            red.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(debugLocation);
            GL.Vertex(debugLocation + velocity);
            GL.End();
        }

    }
    // applies forces to the zombies to move around 
    protected override void ClacSteeringForce()
    {
        Vector3 uforce = Vector3.zero;
        for (int i = 0; i < GameManager.zombies.Count; i++)
        {
            if (gameObject != GameManager.zombies[i] && AABBCollision(gameObject.GetComponent<BoxCollider>(),GameManager.zombies[i].GetComponent<BoxCollider>()))
            {
                uforce += Flee(GameManager.zombies[i]);
            }
        }
        if (GameManager.humans.Count == 0)
        {
           // keeps the zombie on the terrain
            if (transform.position.x > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            if (transform.position.x < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            if (transform.position.z > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            if (transform.position.z < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            // wanders if there are no humans 
            uforce += Wander();
            //applies other forces
            uforce += ObjectAvoidance() * avoidanceForce;
            uforce = Vector3.ClampMagnitude(uforce, maxForce);
            uforce.y = 0;
            ApplyForce(uforce);
        }
        else
        {
            //finds the closest human 
            targetHuman = GameManager.humans[0];
            float distance = 100;
            for (int i = 0; i < GameManager.humans.Count; i++)
            {
                if (Vector3.Distance(gameObject.transform.position, GameManager.humans[i].transform.position) < distance)
                {
                    distance = Vector3.Distance(gameObject.transform.position, GameManager.humans[i].transform.position);
                    targetHuman = GameManager.humans[i];
                }
            }
            // keeps the zombie on the terrain
            if (transform.position.x > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            if (transform.position.x < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10))* 1.5f;
            }
            if (transform.position.z > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            if (transform.position.z < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10)) * 1.5f;
            }
            // chases human and applies other forces
            uforce += Pursue(targetHuman);
            uforce += ObjectAvoidance() * avoidanceForce;
            uforce.y = 0;
            uforce = Vector3.ClampMagnitude(uforce, maxForce);
            ApplyForce(uforce);
        }
        // center seeking force to keep zombies on terrain
        
    }
    protected override Vector3 ObjectAvoidance()
    {
        return base.ObjectAvoidance();
    }
}
