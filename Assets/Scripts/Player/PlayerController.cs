using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController s_Instance;       
    
    [SerializeField]
    [Tooltip("Speed to move the player on the ground")]
    private float speed = 10f;

    [SerializeField]
    [Tooltip("Speed to move the player while is jumping")]
    private float jumpSpeed = 5f;

    [SerializeField]
    [Tooltip("Force to applied when player wants jump")]
    private float jumpForce = 10f;

    [SerializeField]
    [Tooltip("Fall speed to used when player is in the air")]
    private float fallSpeed = 10f;    

    [SerializeField]
    [Tooltip("Speed when player is moving the mouse through the screen")]
    private float rotationSpeed = 10f;                          

    [Header("Ground Raycast")]
    [SerializeField]
    [Tooltip("Layer to detect the ground")]
    private LayerMask groundLayer = 8;

    [SerializeField]
    [Tooltip("Raycast distance")]
    private float raycastDistance = 0.01f;

    private Rigidbody rb;                                   //Component used to apply force at the object
    private InputMaster controls;                           //Input controls get from the keyboard
    

    //WASD keyboard
    private Vector2 inputVector;                            //Vector2 values from the inputs        
    
    //used to rotate player
    private Vector2 currentMousePosition;                           
    private Vector2 lastMousePosition;
    
    public bool CanMove { get; set; } = true;

    public bool IsLive { get; private set; } = true;

    public Vector3 PlayerPosition { get
        {
            return transform.position;
        } }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();                                                                                                           

        controls = new InputMaster();                                                                                //Create a new instance from the Inputs, called too InputMaster (Name given by me)
        controls.Player.Movement.performed += ctx => inputVector = ctx.ReadValue<Vector2>();                         //Ever the key is pressed its called one action called performed. When is action is called, its returned a vector2 value to the InputVector         
        controls.Player.LookRotation.performed += ctx => currentMousePosition = ctx.ReadValue<Vector2>();            //Ever the mouse is moved to the right/left, it's called the action and save the value in the lookRotation
        controls.Player.Jump.performed += ctx => Jump();                                                             //Ever the jump key is pressed, call the method to jump                        

        s_Instance = this;
    }
    
    private void FixedUpdate()
    {
        IsLive = EllenHealth.Instance.Health > 0;

        if (IsLive)
        {
            //If isn't in the air, force fall more fast
            if (!IsGrounded())
                rb.AddForce(Vector3.down * fallSpeed * Time.fixedDeltaTime);

            //Only rotation the player if the mouse is moving
            if (currentMousePosition.x != lastMousePosition.x)
                Turn();

            if (CanMove)
                Move();
        }

        Debug.Log("Is Live " + IsLive);
    }

    //Control the Movement
    private void Move()
    {                 
        //If is pressing keys to move forward
        if (inputVector.y != 0.0f)
        {
            float h = inputVector.x;                                                                       //Get horizontal value given from the Input (W ; S)
            float f = inputVector.y;                                                                       //Get the vertical value given from the Input (A ; D)              
            float vel = IsGrounded() ? speed : jumpSpeed;

            Vector3 forwardForce = transform.forward * f;                                                 //Get the current forward and multply by the forward input value given by the W key and/or S is pressed
            Vector3 horizontalForce = transform.right * h;                                                //Get the current horizontal and multply by the horizontal input value given when the D key and/or A key is pressed

            Vector3 moveForce = (forwardForce + horizontalForce) * vel * Time.fixedDeltaTime;            //Plus the two forces and multiply by the speed and the time passed (Fixed delta time) 

            rb.AddForce(moveForce);                                                                      //Add the force      
        }
        else
        {
            if (IsGrounded())
            {
                rb.velocity = Vector3.zero;
            }
        }                    
    }    

    //Control the Rotation
    private void Turn()
    {
        //If the X axis from the current mouse positions is greate than the last mouse position axis X, then H values is positive, else, is negative
        float h = currentMousePosition.x > lastMousePosition.x ? Mathf.Abs(currentMousePosition.x - lastMousePosition.x) : -(lastMousePosition.x - currentMousePosition.x);        

        //Rotation only in the Y axis. 
        Vector2 axisY = Vector2.up * h * rotationSpeed;

        //Apply rotation
        transform.Rotate(axisY);

        //Take the current position how last
        lastMousePosition = currentMousePosition;
    }

    //Control the jump 
    private void Jump()
    {
        if (IsGrounded())
        {
            //Only add force in the Y axis            
            rb.AddForce(jumpForce * Vector3.up);            
        }
    }

    private void OnEnable()
    {
        //Enable the controls when the object is enabled
        controls.Enable();        
    }

    private void OnDisable()
    {
        //Disable the controls when the object is disabled
        controls.Disable();
    }

    /// <summary>
    /// Check if the player is touching the ground
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
    }

    public void StopMovement()
    {
        CanMove = false;
        rb.velocity = Vector3.zero;
    }

    public void ReturnMovement()
    {
        CanMove = true;
    }   

    /// <summary>
    /// Restore the life and other parements. It's called when player death and respawn again
    /// </summary>
    public void Respawn(Vector3 checkPointPosition)
    {
        if (!IsLive)
        {
            IsLive = true;
            transform.position = checkPointPosition;

            EllenHealth.Instance.RestoreHealth();
        }
    }

    private void OnDrawGizmos()
    {
        if (IsGrounded())
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, Vector3.down * raycastDistance);
    }

}
