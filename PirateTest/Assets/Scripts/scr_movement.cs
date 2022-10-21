using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_movement : MonoBehaviour
{
    public float speed;
    public float speedMult;
    public float maxSpeed;
    public float DashVelocity;
    public float DashCD;
    public float DashTime;
    public float JumpForce;
    float RemainingDashTime = 0f;
    float RemainingDashCD = 0f;
    float hInput;
    float vInput;

    public Transform Orientation;

    Vector3 Direction;

    Rigidbody RB;

    public LayerMask GroundMask;
    public float Height;
    public float GroundDrag;

    bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        RB.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        grounded = CheckGrounded();
        CheckDash();
        CheckJump();
        TrackMovement();
    }

    void UpdateInput()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
    }

    bool CheckGrounded()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, Height * 0.5f + 0.2f, GroundMask))
        {
            RB.drag = GroundDrag;
            return true;
        }
        else
        {
            RB.drag = 0f;
            return false;
        }
    }

    void CheckDash()
    {
        if (RemainingDashTime > 0f)
        {
            RemainingDashTime -= 1f;
        }

        if (RemainingDashCD > 0f)
        {
            RemainingDashCD -= 1f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            RB.AddForce(Orientation.forward * DashVelocity * speedMult, ForceMode.Impulse);

            RemainingDashCD = DashCD;
            RemainingDashTime = DashTime;
        }
    }

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RB.velocity = new Vector3(RB.velocity.x, 0f, RB.velocity.z);
            RB.AddForce(transform.up * JumpForce, ForceMode.Impulse);
        }
    }

    void TrackMovement()
    {
        if (RemainingDashTime <= 0f)
        {
            Direction = Orientation.forward * vInput + Orientation.right * hInput;
            RB.AddForce(Direction.normalized * speed * speedMult, ForceMode.Force);

            if (grounded)
            {
                LimitSpeed();
            }
        }
    }

    void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(RB.velocity.x, 0, RB.velocity.z);

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 velCap = flatVel.normalized * maxSpeed;
            RB.velocity = new Vector3(velCap.x, RB.velocity.y, velCap.z);
        }
    }
}
