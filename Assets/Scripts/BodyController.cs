using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public TouchSensor foot;
    public float beta = 0f;
    public float gamma = 0f;
    public float length = 0f;
    private ConfigurableJoint cj;
    private Transform connected_transform;
    private float[] contractions_buffer;
    public int contractions_buffer_length = 1;
    private int contractions_index = 0;

    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
        connected_transform = cj.connectedBody.transform;
        contractions_buffer = new float[contractions_buffer_length];
    }

    void FixedUpdate()
    {
        float contraction = beta*((float)System.Math.Tanh((double)(gamma*foot.in_contact)));
        contractions_buffer[contractions_index] = contraction;
        contractions_index++;
        if(contractions_index >= contractions_buffer_length) contractions_index = 0;

        float actual_contraction = 0f;

        for(int i = 0; i < contractions_buffer_length; i++){
            actual_contraction += contractions_buffer[i]/contractions_buffer_length;
        }

        cj.targetPosition = new Vector3(actual_contraction, 0f, 0f);
        length = Vector3.Distance(transform.position, connected_transform.position);
    }
}
