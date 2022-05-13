using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    private Joystick joystick;

    public bool useRigidbodyForMovement;

    public float speed;
    public float jumpSpeed;

    private Rigidbody rigidbody;

    float currentSpeed;
    void Start()
    {
        joystick = Joystick.instance;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        currentSpeed = speed * joystick.GetHorizontalAxis();

        if (useRigidbodyForMovement)
        {
            rigidbody.velocity = new Vector3(currentSpeed, rigidbody.velocity.y, 0);
        }
        else
        {
            transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0) ;
        }

        

        if (joystick.IsInteractionDown())
        {

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0);
        }

        if (joystick.IsInteractionUp())
        {
            //rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpSpeed, 0);
        }  
    }

}
