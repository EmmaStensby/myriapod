using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour
{
    public bool right_leg = false;
    public float starting_phase = 0f;
    public float amplitude = 0f;
    public float leg_length = 0f;
    public float leg_height = 0f;
    public float offset_variable;

    public float sigma_a = 0f;
    public float sigma_t = 0f;
    public float angular_velocity = 0f; // Angle per second

    public BodyController connected_body_l;
    public BodyController connected_body_r;
    public BodyController connected_body_l_front;
    public BodyController connected_body_r_front;

    public Transform foot;

    public float current_phase;
    private ConfigurableJoint cj;
    private float prev_T = 0f;

    public void InitializeLeg()
    {
        // Disconnect joint
        cj = GetComponent<ConfigurableJoint>();
        Rigidbody rb = cj.connectedBody;
        cj.connectedBody = null;
        
        // Move the leg
        float middle_z;
        if(right_leg) middle_z = transform.position.z + 1.5f;
        else middle_z = transform.position.z - 1.5f;
        if(right_leg) transform.position = new Vector3(transform.position.x, transform.position.y+leg_height, middle_z-(leg_length+0.5f));
        else transform.position = new Vector3(transform.position.x, transform.position.y+leg_height, middle_z+leg_length+0.5f);
        if(right_leg) foot.transform.position = new Vector3(foot.transform.position.x, foot.transform.position.y+leg_height, middle_z-(leg_length+1.5f));
        else foot.transform.position = new Vector3(foot.transform.position.x, foot.transform.position.y+leg_height, middle_z+leg_length+1.5f);
        
        // Move the anchor
        if(right_leg) cj.anchor = new Vector3(0f, 0f, (leg_length));
        else cj.anchor = new Vector3(0f, 0f, -(leg_length));

        // Reconnect the joint
        cj.connectedBody = rb;

        // Position the foot
        foot.gameObject.GetComponent<FootController>().FromLegStart();

        // Move the leg to stating position
        float starting_angle = -amplitude*Mathf.Cos(starting_phase);
        if(right_leg) transform.RotateAround(rb.transform.position, rb.transform.up, -starting_angle);
        else transform.RotateAround(rb.transform.position, rb.transform.up, starting_angle);
        if(right_leg) foot.RotateAround(rb.transform.position, rb.transform.up, -starting_angle);
        else foot.RotateAround(rb.transform.position, rb.transform.up, starting_angle);

        // Set the joint rotation
        if(right_leg) cj.targetRotation = Quaternion.Euler(new Vector3(0f, starting_angle, 0f));
        else cj.targetRotation = Quaternion.Euler(new Vector3(0f, -starting_angle, 0f));

        // Set phase
        current_phase = starting_phase;
    }

    void FixedUpdate()
    {
        float length_l = 1f;
        float length_r = 1f;
        float length_l_front = 1f;
        float length_r_front = 1f;

        if(connected_body_l != null) length_l = connected_body_l.length;
        if(connected_body_r != null) length_r = connected_body_r.length;
        if(connected_body_l_front != null) length_l_front = connected_body_l_front.length;
        if(connected_body_r_front != null) length_r_front = connected_body_r_front.length;

        float A = length_r + length_r_front - length_l - length_l_front;
        float T = length_r + length_l - length_r_front - length_l_front;
        float DT = T - prev_T;
        prev_T = T;

        // Calculate new phase
        if(right_leg) current_phase += angular_velocity*Time.fixedDeltaTime + sigma_a*Time.fixedDeltaTime*A*Mathf.Cos(current_phase) - sigma_t*Time.fixedDeltaTime*T*Mathf.Cos(current_phase + offset_variable*Mathf.PI);
        else current_phase += angular_velocity*Time.fixedDeltaTime - sigma_a*Time.fixedDeltaTime*A*Mathf.Cos(current_phase) - sigma_t*Time.fixedDeltaTime*T*Mathf.Cos(current_phase + offset_variable*Mathf.PI);

        current_phase = current_phase%(2*Mathf.PI);

        if(current_phase > Mathf.PI) transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(255f/255f, 0f/255f, 0f/255f);
        else transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(0f/255f, 0f/255f, 0f/255f);

        // Calculate new angle
        float angle = -amplitude*Mathf.Cos(current_phase);

        // Set the joint rotation
        if(right_leg) cj.targetRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));
        else cj.targetRotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }
}
