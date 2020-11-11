using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    // fix camera glitch
    public GameObject zombie;
    protected override void ClacSteeringForce()
    {
        GameObject targetZombie = GameManager.zombies[0];
        float distance = 100;
        for (int i = 0; i < GameManager.zombies.Count; i++)
        {
            if (Vector3.Distance(gameObject.transform.position, GameManager.zombies[i].transform.position) < distance)
            {
                distance = Vector3.Distance(gameObject.transform.position, GameManager.zombies[i].transform.position);
                targetZombie = GameManager.zombies[i];
            }
        }
        Vector3 uForce = Vector3.zero;

        for (int i = 0; i < GameManager.psgs.Count; i++)
        {
            uForce += Seek(GameManager.psgs[i]);
        }
        uForce += Flee(targetZombie);

        uForce = Vector3.ClampMagnitude(uForce, maxForce);
        uForce.y = 0;
        ApplyForce(uForce);
    }
    public void Convert()
    {
        GameManager.zombies.Add(Instantiate(zombie, new Vector3(gameObject.transform.position.x, zombie.GetComponent<BoxCollider>().size.y / 2, gameObject.transform.position.z), Quaternion.identity));
        GameManager.humans.Remove(gameObject);
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(position, GameManager.psgs[0].transform.position) < radius)
        {
            GameManager.psgs[0].GetComponent<Treasure>().OnGrab();
        }
        for (int i = 0; i < GameManager.zombies.Count; i++)
        {
            if (Vector3.Distance(position, GameManager.zombies[i].transform.position) < radius)
            {
                Convert();
                break;
            }
        }
    }
    protected override void OnRenderObject()
    {
        base.OnRenderObject();

    }
}
