using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Zombie : Vehicle
{
    private GameObject targetHuman;
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
    protected override void ClacSteeringForce()
    {
        if (GameManager.humans.Count == 0)
        {
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            return;
        }

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

        Vector3 uforce = Seek(GameManager.psgs[1].transform.position);
        uforce += Seek(targetHuman);
        uforce = Vector3.ClampMagnitude(uforce, maxForce);
        uforce.y = 0;
        ApplyForce(uforce);
    }
}
