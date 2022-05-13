using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public static Joystick instance;//Use this reference to access the joystick from other Scripts

    [Header("Buttons Parameters")]
    [Space(10, order = 1)]

    [SerializeField]
    GameObject leftButtonObject;
    [SerializeField]
    GameObject rightButtonObject;
    [SerializeField]
    GameObject interactionButtonObject;
    [SerializeField]
    float scaleFactorWhenPressed = 0.75f;

    [Space(10)]
    [Header("Internal States")]
    [SerializeField]
    bool showConsoleMessages;
    [SerializeField]
    bool movingRight;
    [SerializeField]
    bool movingLeft;
    [SerializeField]
    bool interactingDown;
    [SerializeField]
    bool interactingUp;
    [SerializeField]
    bool interactionButtonPressed;
    [SerializeField]
    float horizontalAxis;

    bool rightButtonDown;
    bool leftButtonDown;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        interactingDown = false;
        interactingUp = false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            LeftButtonDown();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RightButtonDown();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractButtonDown();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            LeftButtonUp();

        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            RightButtonUp();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            InteractButtonUp();
        }

        if (showConsoleMessages)
        {
            if (movingRight)
            {
                Debug.Log("Moving Right");
            }
            if (movingLeft)
            {
                Debug.Log("Moving Left");
            }

        }


    }


    public float GetHorizontalAxis()
    {
        return horizontalAxis;
    }

    public bool IsInteractionDown()
    {
        return interactingDown;

    }
    

    public bool IsInteractionUp()
    {
        return interactingUp;

    }

    public void LeftButtonDown()
    {
        if (!leftButtonDown)
        {
            leftButtonDown = true;
            leftButtonObject.transform.localScale = Vector3.one * scaleFactorWhenPressed;
            if (horizontalAxis > -1f)
            {
                horizontalAxis -= 1f;
            }
            horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);
        }
        

    }
    public void LeftButtonUp()
    {
        if (leftButtonDown)
        {
            leftButtonDown = false;
            leftButtonObject.transform.localScale = Vector3.one;
            horizontalAxis += 1f;
            horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);
        }

    }
    public void RightButtonDown()
    {
        if (!rightButtonDown)
        {
            rightButtonDown = true;
            rightButtonObject.transform.localScale = Vector3.one * scaleFactorWhenPressed;
            horizontalAxis += 1f;
            horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);
        }

    }

    public void RightButtonUp()
    {
        if (rightButtonDown)
        {
            rightButtonDown = false;
            rightButtonObject.transform.localScale = Vector3.one;
            horizontalAxis -= 1f;
            horizontalAxis = Mathf.Clamp(horizontalAxis, -1f, 1f);

        }

    }

    public void InteractButtonDown()
    {
        interactionButtonObject.transform.localScale = Vector3.one * scaleFactorWhenPressed;
        //Interaction Button Pressed
        interactingDown = true;
        interactionButtonPressed = true;
        if (showConsoleMessages)
        {
            Debug.Log("Interaction Button Down");
        }
    }
    public void InteractButtonUp()
    {
        interactionButtonObject.transform.localScale = Vector3.one ;
        //Interaction Button Released
        interactingUp = true;
        interactionButtonPressed = false;
        if (showConsoleMessages)
        {
            Debug.Log("Interaction Button Up");
        }
    }



}
