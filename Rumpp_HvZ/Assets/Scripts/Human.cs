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
        // adds all forces clamps them and applies the force 
        for (int i = 0; i < GameManager.psgs.Count; i++)
        {
            uForce += Seek(GameManager.psgs[i]);
        }
        uForce += Flee(targetZombie);
        uForce += Flee(ObjectAvoidance());
        uForce = Vector3.ClampMagnitude(uForce, maxForce);
        uForce.y = 0;
        ApplyForce(uForce);
    }
    
    // if the zombies hit the humans they convert by being added to the list of zombies and are destroyed
    public void Convert()
    {
        GameManager.zombies.Add(Instantiate(zombie, new Vector3(gameObject.transform.position.x, zombie.GetComponent<BoxCollider>().size.y / 2, gameObject.transform.position.z), Quaternion.identity));
        GameManager.humans.Remove(gameObject);
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        //checks collisions between the treasure and human
        if (Vector3.Distance(position, GameManager.psgs[1].transform.position) < radius)
        {
            GameManager.psgs[1].GetComponent<Treasure>().OnGrab();
        }
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
