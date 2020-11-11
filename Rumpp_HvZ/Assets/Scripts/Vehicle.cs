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


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = Vector3.right;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        ClacSteeringForce();
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
    }

    // makes the force applied realistic
    protected void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    // debug lines
    protected virtual void OnRenderObject()
    {
        green.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(transform.position);
        GL.Vertex((velocity * 2 + transform.position));
        GL.End();
        blue.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(transform.position);
        GL.Vertex(Quaternion.Euler(0, 90, 0) * velocity.normalized * 2   + transform.position );
        GL.End();
    }

    protected abstract void ClacSteeringForce();

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
}
