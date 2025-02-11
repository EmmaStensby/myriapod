using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSensor : MonoBehaviour
{
    public float in_contact = 0.0f;

    void OnCollisionStay(Collision collisionInfo){
        if(collisionInfo.collider.tag == "floor"){
            in_contact = (collisionInfo.impulse.magnitude/Time.fixedDeltaTime);
        }

        if(in_contact <= 0f) transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(255f/255f, 0f/255f, 0f/255f);
        else transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(0f/255f, 0f/255f, 0f/255f);
    }
}
