using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float dribbleSpeedMultiplier;
    public float torque;
    public float timeLeft = 2.0f;
    public string forwardButton = "Forward_P1";
    public string rotateButton = "Rotate_P1";
    public string throwButton = "Throw_P1";
    public string team = "Team_1";
    public string opponent = "Team_1";

    private float speedMultiplier = 1;
    private float countdown;
    private Rigidbody2D rb;
    private FixedJoint2D fixedJoint;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        fixedJoint = GetComponent<FixedJoint2D>();
        fixedJoint.enabled = false;
        countdown = timeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.CompareTag("Dribbling"))
        {
            countdown -= Time.deltaTime;
            if (countdown < 0)
            {
                RandomDropBall(0.3f);
            }
        }        
    }


    void FixedUpdate()
    {
        float moveForward = Input.GetAxis(forwardButton);
        float rotation = Input.GetAxis(rotateButton);

        Vector2 movement = new Vector2(moveForward, 0f);
        rb.AddRelativeForce(movement * speed * speedMultiplier);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * speedMultiplier;
        }
        rb.AddTorque(rotation * torque);

        if (Input.GetButtonDown(throwButton) && fixedJoint.enabled)
        {
            DropBall(1.0f);          

            //Vector2 dir = (fixedJoint.connectedBody.position -  new Vector2(transform.position.x, transform.position.y));
            //dir.Normalize();
            //fixedJoint.connectedBody.AddForce(dir * 1.5f);
        }
    }

    private void PickUpBall(Rigidbody2D ball)
    {
        transform.tag = "Dribbling";
        fixedJoint.enabled = true;
        fixedJoint.connectedBody = ball;
        speedMultiplier = dribbleSpeedMultiplier;
    }

    private void DropBall(float force)
    {
        transform.tag = team;
        fixedJoint.enabled = false;
        speedMultiplier = 1.0f;

        Vector2 dir = (fixedJoint.connectedBody.position - rb.position);
        dir.Normalize();
        fixedJoint.connectedBody.AddForce(dir * force);
        fixedJoint.connectedBody.tag = "FreeBall";

        countdown = timeLeft;
    }

    private void RandomDropBall(float force)
    {
        transform.tag = team;
        fixedJoint.enabled = false;
        speedMultiplier = 1.0f;

        Vector2 randomdir = new Vector2(Random.value, Random.value).normalized * 0.5f;
        Vector2 dir = (fixedJoint.connectedBody.position - rb.position);
        dir.Normalize();
        fixedJoint.connectedBody.AddForce( (dir + randomdir).normalized * force );
        fixedJoint.connectedBody.tag = "FreeBall";

        countdown = timeLeft;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FreeBall"))
        {
            collision.gameObject.tag = "Ball";
            PickUpBall( collision.gameObject.GetComponent<Rigidbody2D>() );
        }
        else if (collision.gameObject.CompareTag(opponent))
        {
            Vector2 dir = (collision.gameObject.GetComponent<Rigidbody2D>().position - rb.position);
            dir.Normalize();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 50.0f);

            if (transform.CompareTag("Dribbling"))
            {
                DropBall(0.2f);
            }  
        }
    }
}
