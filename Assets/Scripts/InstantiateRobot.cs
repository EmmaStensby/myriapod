using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRobot : MonoBehaviour
{
    //public CameraFollow camera_follower;

    [Header("Controller settings")]
    [Tooltip("Step amplitude")]
    public float alpha_legs = 20f;
    [Tooltip("Lift amplitude")]
    public float alpha_feet = 30f;
    [Tooltip("Body contraction amplitude")]
    public float beta = 1f;
    [Tooltip("Body contraction stability")]
    public int contractions_buffer_length = 10;
    [Tooltip("Pressure sensor sensitivity")]
    public float gamma = 0.005f;
    [Tooltip("Controller modulation strength, angle")]
    public float sigma_a = 0.2f;
    [Tooltip("Controller modulation strength, translation")]
    public float sigma_t = 0f;
    [Tooltip("Walk speed")]
    public float omega = 3.14f;
    public float clamp_amount = 70f;
    public bool climb_mode = false;
    public float offset_variable;

    [Header("Morphology settings")]
    public bool ant_mode = false;
    public int middle_sections = 10;
    public float leg_length = 0.5f;
    public float leg_height = -0.5f;
    public float foot_length = 1f;
    public float point_mass = 0.05f;
    public float body_mass_modifier = 1f;
    public float drag = 0f;
    public float angularDrag = 0.05f;

    [Header("Segment prefabs")]
    public GameObject butt_prefab;
    public GameObject middle_prefab;
    public GameObject head_prefab;

    [Header("PD settings - Legs")]
    public float linear_spring_active_legs = 0f;
    public float linear_damper_active_legs = 0f;
    public float linear_maxforce_active_legs = 0f;
    public float rotation_spring_active_legs = 0f;
    public float rotation_damper_active_legs = 0f;
    public float rotation_maxforce_active_legs = 0f;

    [Header("PD settings - Feet")]
    public float linear_spring_active_feet = 0f;
    public float linear_damper_active_feet = 0f;
    public float linear_maxforce_active_feet = 0f;
    public float rotation_spring_active_feet = 0f;
    public float rotation_damper_active_feet = 0f;
    public float rotation_maxforce_active_feet = 0f;

    [Header("PD settings - Body:Stiff")]
    public float linear_spring_active_body1 = 0f;
    public float linear_damper_active_body1 = 0f;
    public float linear_maxforce_active_body1 = 0f;
    public float rotation_spring_active_body1 = 0f;
    public float rotation_damper_active_body1 = 0f;
    public float rotation_maxforce_active_body1 = 0f;

    
    [Header("PD settings - Body:Soft")]
    public float linear_spring_active_body2 = 0f;
    public float linear_damper_active_body2 = 0f;
    public float linear_maxforce_active_body2 = 0f;
    public float rotation_spring_active_body2 = 0f;
    public float rotation_damper_active_body2 = 0f;
    public float rotation_maxforce_active_body2 = 0f;

    [Header("PD settings - Body:Actuated")]
    public float linear_spring_active_body3 = 0f;
    public float linear_damper_active_body3 = 0f;
    public float linear_maxforce_active_body3 = 0f;
    public float rotation_spring_active_body3 = 0f;
    public float rotation_damper_active_body3 = 0f;
    public float rotation_maxforce_active_body3 = 0f;


    public void InstantiateModules(){
        // Create the head
        GameObject head_segment = GameObject.Instantiate(head_prefab, new Vector3(0f, 0f, 0f) + transform.position, Quaternion.identity, transform);
        SetPD(head_segment);
        SetLengths(head_segment);
        SetControllerParameters(head_segment, false);
        SetLegStartingPhase(head_segment);
        SetMasses(head_segment);
        SetPD(head_segment);
        InitializeLegs(head_segment);
        GameObject previous_segment = head_segment;
        // Create the middle
        GameObject current_segment;
        int i = 1;
        while(i <= middle_sections){
            current_segment = GameObject.Instantiate(middle_prefab, new Vector3(-i, 0f, 0f) + transform.position, Quaternion.identity, transform);
            SetPD(current_segment);
            ConnectSegments(previous_segment, current_segment, head_segment);
            SetLengths(current_segment);
            SetControllerParameters(current_segment, false);
            SetLegStartingPhase(current_segment);
            SetMasses(current_segment);
            InitializeLegs(current_segment);
            previous_segment = current_segment;
            i++;
        }
        // Create the butt
        current_segment = GameObject.Instantiate(butt_prefab, new Vector3(-i, 0f, 0f) + transform.position, Quaternion.identity, transform);
        SetPD(current_segment);
        ConnectSegments(previous_segment, current_segment, head_segment);
        SetLengths(current_segment);
        SetControllerParameters(current_segment, true);
        SetLegStartingPhase(current_segment);
        SetMasses(current_segment);
        InitializeLegs(current_segment);

        Physics.IgnoreLayerCollision(0, 0);

    }


    void ConnectSegments(GameObject previous_segment, GameObject current_segment, GameObject head_segment){
        // Connect segments with fixed joint
        ConfigurableJoint[] joints = previous_segment.transform.Find("SphereB").gameObject.GetComponents<ConfigurableJoint>();
        foreach (ConfigurableJoint joint in joints){
            if(joint.connectedBody == null){
                joint.connectedBody = current_segment.transform.Find("SphereF").gameObject.GetComponent<Rigidbody>();
            }
        }
        // Connect segments with PD systems
        string[] connections = {"SphereU", "SphereD", "SphereL", "SphereR"};
        foreach (string connection in connections){
            joints = previous_segment.transform.Find(connection).gameObject.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint joint in joints){
                if(joint.connectedBody == null){
                    joint.connectedBody = current_segment.transform.Find(connection).gameObject.GetComponent<Rigidbody>();
                }
            }
        }
        // Connect signal passing from PD systems to legs
        if(ant_mode){
            current_segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().connected_body_l_front = head_segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().connected_body_r_front = head_segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().connected_body_l_front = head_segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().connected_body_r_front = head_segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>();
        }
        else{
            current_segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().connected_body_l_front = previous_segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().connected_body_r_front = previous_segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().connected_body_l_front = previous_segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>();
            current_segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().connected_body_r_front = previous_segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>();
        }
        // Connect decorations
        previous_segment.transform.Find("Decorations").Find("CubeRR").gameObject.GetComponent<DisplayJoint>().p2 = current_segment.transform.Find("SphereR");
        previous_segment.transform.Find("Decorations").Find("CubeLL").gameObject.GetComponent<DisplayJoint>().p2 = current_segment.transform.Find("SphereL");
        previous_segment.transform.Find("Decorations").Find("CubeUU").gameObject.GetComponent<DisplayJoint>().p2 = current_segment.transform.Find("SphereU");
        previous_segment.transform.Find("Decorations").Find("CubeDD").gameObject.GetComponent<DisplayJoint>().p2 = current_segment.transform.Find("SphereD");
    }

    void SetControllerParameters(GameObject segment, bool skip_body){
        if(!skip_body){
            segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>().beta = beta;
            segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>().gamma = gamma;
            segment.transform.Find("SphereL").gameObject.GetComponent<BodyController>().contractions_buffer_length = contractions_buffer_length;
        }

        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().amplitude = alpha_legs;
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().sigma_a = sigma_a;
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().sigma_t = sigma_t;
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().angular_velocity = omega;
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().offset_variable = offset_variable;

        segment.transform.Find("SphereFootL").gameObject.GetComponent<FootController>().amplitude = alpha_feet;
        segment.transform.Find("SphereFootL").gameObject.GetComponent<FootController>().climb_mode = climb_mode;
        segment.transform.Find("SphereFootL").gameObject.GetComponent<FootController>().clamp_amount = clamp_amount;

        if(!skip_body){
            segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>().beta = beta;
            segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>().gamma = gamma;
            segment.transform.Find("SphereR").gameObject.GetComponent<BodyController>().contractions_buffer_length = contractions_buffer_length;
        }

        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().amplitude = alpha_legs;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().sigma_a = sigma_a;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().sigma_t = sigma_t;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().angular_velocity = omega;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().offset_variable = offset_variable;

        segment.transform.Find("SphereFootR").gameObject.GetComponent<FootController>().amplitude = alpha_feet;
        segment.transform.Find("SphereFootR").gameObject.GetComponent<FootController>().climb_mode = climb_mode;
        segment.transform.Find("SphereFootR").gameObject.GetComponent<FootController>().clamp_amount = clamp_amount;
    }

    void SetLegStartingPhase(GameObject segment){
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().starting_phase = Random.Range(0f, 1f)*Mathf.PI*2f;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().starting_phase = Random.Range(0f, 1f)*Mathf.PI*2f;
    }

    void SetMasses(GameObject segment){
        // Body
        string[] mass_points = {"SphereU", "SphereD", "SphereL", "SphereR", "SphereF", "SphereB"};
        foreach (string mass_point in mass_points){
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().mass = point_mass*body_mass_modifier;
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().drag = drag;
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().angularDrag = angularDrag;
        }
        // Legs
        string[] mass_points_2 = {"SphereLegL", "SphereFootL", "SphereLegR", "SphereFootR"};
        foreach (string mass_point in mass_points_2){
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().mass = point_mass;
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().drag = drag;
            segment.transform.Find(mass_point).gameObject.GetComponent<Rigidbody>().angularDrag = angularDrag;
        }      
    }
    void SetLengths(GameObject segment){
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().leg_length = leg_length;
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().leg_height = leg_height;
        segment.transform.Find("SphereFootL").gameObject.GetComponent<FootController>().foot_length = foot_length;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().leg_length = leg_length;
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().leg_height = leg_height;
        segment.transform.Find("SphereFootR").gameObject.GetComponent<FootController>().foot_length = foot_length;
    }

    void InitializeLegs(GameObject segment){
        segment.transform.Find("SphereLegL").gameObject.GetComponent<LegController>().InitializeLeg();
        segment.transform.Find("SphereLegR").gameObject.GetComponent<LegController>().InitializeLeg();
    }

    void SetPD(ConfigurableJoint joint, float lin_spring, float lin_damper, float lin_max, float rot_spring, float rot_damper, float rot_max){
        JointDrive jd = joint.angularXDrive;
        jd.positionSpring = rot_spring;
        jd.positionDamper = rot_damper;
        jd.maximumForce = rot_max;
        joint.angularXDrive = jd;
        jd = joint.angularYZDrive;
        jd.positionSpring = rot_spring;
        jd.positionDamper = rot_damper;
        jd.maximumForce = rot_max;
        joint.angularYZDrive = jd;
        jd = joint.xDrive;
        jd.positionSpring = lin_spring;
        jd.positionDamper = lin_damper;
        jd.maximumForce = lin_max;
        joint.xDrive = jd;
        jd = joint.yDrive;
        jd.positionSpring = lin_spring;
        jd.positionDamper = lin_damper;
        jd.maximumForce = lin_max;
        joint.yDrive = jd;
        jd = joint.zDrive;
        jd.positionSpring = lin_spring;
        jd.positionDamper = lin_damper;
        jd.maximumForce = lin_max;
        joint.zDrive = jd;
    }

    void SetPD(GameObject segment){
        ConfigurableJoint[] joints;
        string[] connections = {"SphereU", "SphereD", "SphereL", "SphereR"};
        foreach (string connection in connections){
            joints = segment.transform.Find(connection).gameObject.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint joint in joints){
                if(joint.connectedBody == null){
                    // Type: Body actuated (body 3)
                    SetPD(joint, linear_spring_active_body3, linear_damper_active_body3, linear_maxforce_active_body3, rotation_spring_active_body3, rotation_damper_active_body3, rotation_maxforce_active_body3);
                }
                else{
                    // Type: Body soft (body 2)
                    SetPD(joint, linear_spring_active_body2, linear_damper_active_body2, linear_maxforce_active_body2, rotation_spring_active_body2, rotation_damper_active_body2, rotation_maxforce_active_body2);
                }
            }
        }
        string[] connections_2 = {"SphereF", "SphereB"};
        foreach (string connection in connections_2){
            joints = segment.transform.Find(connection).gameObject.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint joint in joints){
                if(joint.connectedBody == null){
                }
                else{
                    // Type: Body stiff (body 1)
                    SetPD(joint, linear_spring_active_body1, linear_damper_active_body1, linear_maxforce_active_body1, rotation_spring_active_body1, rotation_damper_active_body1, rotation_maxforce_active_body1);
                }
            }
        }
        string[] connections_3 = {"SphereLegL", "SphereLegR"};
        foreach (string connection in connections_3){
            joints = segment.transform.Find(connection).gameObject.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint joint in joints){
                // Type legs (legs)
                SetPD(joint, linear_spring_active_legs, linear_damper_active_legs, linear_maxforce_active_legs, rotation_spring_active_legs, rotation_damper_active_legs, rotation_maxforce_active_legs);
            }
        }
        string[] connections_4 = {"SphereFootL", "SphereFootR"};
        foreach (string connection in connections_4){
            joints = segment.transform.Find(connection).gameObject.GetComponents<ConfigurableJoint>();
            foreach (ConfigurableJoint joint in joints){
                // Type feet (feet)
                SetPD(joint, linear_spring_active_feet, linear_damper_active_feet, linear_maxforce_active_feet, rotation_spring_active_feet, rotation_damper_active_feet, rotation_maxforce_active_feet);
            }
        }
    }
}
