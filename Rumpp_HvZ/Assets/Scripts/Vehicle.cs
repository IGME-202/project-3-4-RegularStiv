using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    //vectors
    protected Vector3 position;
    protected Vector3 direction;
    protected Vector3 velocity;
    protected Vector3 acceleration;
    protected Vector3 wanderTarget;
    protected List<GameObject> avoidList;
    public TerrainData terrain;
    public bool debugLinesOn = true;
    //debug materials
    public Material blue;
    public Material black;
    public Material green;
    public Material red;
    public Material purple;


    //speeds and forces
    [Min(0.0001f)]
    public float mass = 1f;
    public float radius = 1f;
    public float maxSpeed = 1f;
    public float minSpeed = 1f;
    public float maxForce = 5f;
    public float avoidanceForce = 1f;
    public float terrainRadius = 17.5f;
    public float wanderRadius = 2f;

    public float time = 0;

    public int angle;


    // Start is called before the first frame update
    void Start()
    {
        avoidList = new List<GameObject>();
        position = transform.position;
        direction = Vector3.right;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        angle = Random.Range(0, 360);
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        ClacSteeringForce();
        ApplyFriction(.25f);
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugLinesOn = !debugLinesOn;
        }
        time += Time.deltaTime;
    }

    // makes the force applied realistic
    protected void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    // debug lines
    protected virtual void OnRenderObject()
    {
        if (debugLinesOn)
        {
            Vector3 debugLocation = transform.position;
            debugLocation.y = debugLocation.y + 1;
            green.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(debugLocation);
            GL.Vertex((direction * 2 + debugLocation));
            GL.End();
            blue.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(debugLocation);
            GL.Vertex(Quaternion.Euler(0, 90, 0) * velocity.normalized * 2 + debugLocation);
            GL.End();
        }
    }

    protected abstract void ClacSteeringForce();

    protected virtual Vector3 ObjectAvoidance()
    {
        Vector3 avoidForce = Vector3.zero;
        Vector3 right = Vector3.Cross(velocity, Vector3.up);
        Vector3 distance = new Vector3(1000, 1000, 1000);
        avoidList.Clear();
        for (int i = 0; i < GameManager.obstacles.Count; i++)
        {
            Vector3 toOther = GameManager.obstacles[i].transform.position - transform.position;
            float dot = Vector3.Dot(velocity, toOther);
            if (dot >= 0)
            {
                if (Vector3.Distance(GameManager.obstacles[i].transform.position, transform.position) < 2 + GameManager.obstacles[i].GetComponent<Obstacle>().radius)
                {
                    dot = Vector3.Dot(right, toOther);
                    if (Mathf.Abs(dot) <= radius + GameManager.obstacles[i].GetComponent<Obstacle>().radius)
                    {
                        avoidList.Add(GameManager.obstacles[i]);
                        if (dot >= 0)
                        {
                            avoidForce += -right.normalized * maxSpeed;
                        }
                        else
                        {
                            avoidForce += right.normalized * maxSpeed;
                        }
                    }
                }
            }
        }
        return avoidForce;
    }

    public void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        acceleration += friction;
    }

    public static bool AABBCollision(BoxCollider a, BoxCollider b)
    {
        if (a.bounds.min.x < b.bounds.max.x && a.bounds.max.x > b.bounds.min.x && a.bounds.max.z > b.bounds.min.z && a.bounds.min.z < b.bounds.max.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector3 Wander()
    {
        if (time > .5f)
        {
            Vector3 distanceAhead = transform.position + velocity;
            int angleChange = Random.Range(-30, 30);
            angle += angleChange;
            Vector3 target = Vector3.zero;
            target.x = distanceAhead.x + Mathf.Cos(angle) * wanderRadius;
            target.z = distanceAhead.z + Mathf.Sin(angle) * wanderRadius;
            time = 0;
            wanderTarget = target;
            return Seek(target);
        }
        return Seek(wanderTarget);
    }

    //seek
    #region
    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - position;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 seekingForce = desiredVelocity - velocity;

        seekingForce.y = 0;

        return seekingForce;
    }

    public Vector3 Seek(GameObject obj)
    {
        return Seek(obj.transform.position);
    }
    #endregion

    //flee
    #region
    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 fleeingForce = desiredVelocity - velocity;

        fleeingForce.y = 0;

        return fleeingForce;
    }

    public Vector3 Flee(GameObject obj)
    {
        return Flee(obj.transform.position);
    }
    #endregion

    public Vector3 Pursue(GameObject target)
    {
        Vector3 distanceBetween = target.transform.position - transform.position;

        float scale = (int)(distanceBetween.magnitude / maxSpeed);

        Vector3 futurePos = target.transform.position + target.GetComponent<Vehicle>().velocity * scale;

        futurePos.y = 0;

        return Seek(futurePos);
    }

    public Vector3 Evade(GameObject target)
    {
        Vector3 distanceBetween = target.transform.position - transform.position;

        int scale = (int)(distanceBetween.magnitude / maxSpeed);

        Vector3 futurePos = target.transform.position + target.GetComponent<Vehicle>().velocity * scale;

        futurePos.y = 0;

        return Flee(futurePos);
    }
}
