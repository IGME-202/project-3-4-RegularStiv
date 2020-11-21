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
        if (targetHuman != null)
        {
            black.SetPass(3);
            GL.Begin(GL.LINES);
            GL.Vertex(gameObject.transform.position);
            GL.Vertex(targetHuman.transform.position);
            GL.End();
        }

    }
    // applies forces to the zombies to move around 
    protected override void ClacSteeringForce()
    {
        if (GameManager.humans.Count == 0)
        {
            Vector3 wanderForce = new Vector3(Random.Range(-maxForce, maxForce), 0, Random.Range(-maxForce, maxForce));
            Vector3 uforce = Vector3.zero;
            uforce += Seek(wanderForce);
            uforce += ObjectAvoidance() * avoidanceForce;
            uforce = Vector3.ClampMagnitude(uforce, maxForce);
            uforce.y = 0;
            ApplyForce(uforce);
        }
        else
        {
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
            Vector3 uforce = Vector3.zero;
            if (transform.position.x > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10));
            }
            if (transform.position.x < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10));
            }
            if (transform.position.z > terrainRadius)
            {
                uforce += Seek(new Vector3(10, 0, 10));
            }
            if (transform.position.z < 2.5)
            {
                uforce += Seek(new Vector3(10, 0, 10));
            }
            uforce += Seek(targetHuman);
            uforce += ObjectAvoidance() * avoidanceForce;
            uforce.y = 0;
            uforce = Vector3.ClampMagnitude(uforce, maxForce);
            ApplyForce(uforce);
            ApplyFriction(.2f);
        }
        // center seeking force to keep zombies on terrain
        
    }
    protected override Vector3 ObjectAvoidance()
    {
        return base.ObjectAvoidance();
    }
}
