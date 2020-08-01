using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private bool canJump;
    [SerializeField] private float maxDistanceToGround;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpMultiplier;

    PlayerInput myInput;
    Rigidbody myRigidbody;

    [SerializeField] Image successbar;
    
    [Range(1.0f, 100.0f)]public float movementSpeed;
    RaycastHit groundCheck;
    public void Start()
    {       
        myInput = GetComponent<PlayerInput>();
        myRigidbody = GetComponent<Rigidbody>();
        if (myInput == null)
            Debug.LogError("We didn't find a PlayerInput component!");
        if (myRigidbody == null)
            Debug.LogError("We didn't find a Rigidbody component!");
    }

    public void Update()
    {
        Physics.Raycast(transform.position, Vector3.down, out groundCheck);
        canJump = groundCheck.distance < maxDistanceToGround;
        GameObject.FindGameObjectsWithTag("Paintable");
        successbar.fillAmount = (myInput.jumpCharge()-0.2f)/6f * 10f;
    }

    
    public void FixedUpdate()
    {
        jumpMultiplier = myInput.jumpMultiplier();
        
        myRigidbody.AddForce(Vector3.up * jumpForce * jumpMultiplier *(canJump? 1f:0f), ForceMode.Force);
        myRigidbody.AddForce( new Vector3 (myInput.horizontal(), 0f, myInput.vertical()) * movementSpeed);

    }
    
}
