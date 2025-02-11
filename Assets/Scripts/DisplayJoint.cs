using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayJoint : MonoBehaviour
{
    public Transform p1;
    public Transform p2;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = p1.position + ((p2.position - p1.position)/2f);
        transform.position = pos;
        transform.LookAt(p1);
        transform.rotation = transform.rotation*Quaternion.AngleAxis(90f, Vector3.up);
    }
}
