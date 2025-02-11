using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootController : MonoBehaviour
{
    public LegController leg;
    public bool right_leg = false;
    public float amplitude = 0f;
    public float foot_length = 1f;
    public bool climb_mode;
    public float clamp_amount;

    private float current_phase;
    private ConfigurableJoint cj;

    public void FromLegStart()
    {
        cj = GetComponent<ConfigurableJoint>();
        Rigidbody rb = cj.connectedBody;
        cj.connectedBody = null;

        // Move the foot
        float leg_z = leg.transform.position.z;
        if(right_leg) transform.position = new Vector3(transform.position.x, transform.position.y, leg_z-foot_length);
        else transform.position = new Vector3(transform.position.x, transform.position.y, leg_z+foot_length);
        
        // Move the anchor
        if(right_leg) cj.anchor = new Vector3(0f, 0f, (foot_length));
        else cj.anchor = new Vector3(0f, 0f, -(foot_length));

        // Reconnect the joint
        cj.connectedBody = rb;

        // Set phase
        current_phase = leg.current_phase;

        // Calculate starting angle
        float starting_angle = amplitude*Mathf.Sin(current_phase); // - amplitude/2f;
        if(climb_mode) starting_angle -= clamp_amount;

        // Set the joint rotation
        if(right_leg) cj.targetRotation = Quaternion.Euler(new Vector3(-starting_angle, 0f, 0f));
        else cj.targetRotation = Quaternion.Euler(new Vector3(starting_angle, 0f, 0f));
    }

    void FixedUpdate()
    {
        // Set phase
        current_phase = leg.current_phase;

        // Calculate new angle
        float angle = amplitude*Mathf.Sin(current_phase); // - amplitude/2f;
        if(climb_mode) angle -= clamp_amount;

        // Set the joint rotation
        if(right_leg) cj.targetRotation = Quaternion.Euler(new Vector3(-angle, 0f, 0f));
        else cj.targetRotation = Quaternion.Euler(new Vector3(angle, 0f, 0f));
    }
}
