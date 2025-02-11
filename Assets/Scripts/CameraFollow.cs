using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject follow_object;
    public List<GameObject> follow_objects;
    private Vector3 offset = new Vector3(0f, 0f, 0f);
    private Vector3 orig;
    private Quaternion orig_rotation;
    private Vector3 orig_side_view = new Vector3(-10f, 1f, 0f);
    private bool following = true;
    public bool top_view = true;

    private Vector3 no_turn;

    void Start(){
        orig = transform.position;
        orig_rotation = transform.rotation;
        if(!top_view){
            transform.rotation = Quaternion.identity;
            offset = new Vector3(0f, 0f, 0f);
        }
    }

    void Update()
    {
        Vector3 mass_middle_point_1 = new Vector3(0f, 0f, 0f);
        for(int i = 0; i < (int)(follow_objects.Count/2); i++){
            mass_middle_point_1 += follow_objects[i].transform.Find("Cube/Bottom").position/((int)(follow_objects.Count/2));
        }
        Vector3 mass_middle_point_2 = new Vector3(0f, 0f, 0f);
        for(int i = (int)(follow_objects.Count/2); i < follow_objects.Count; i++){
            mass_middle_point_2 += follow_objects[i].transform.Find("Cube/Bottom").position/(follow_objects.Count - (int)(follow_objects.Count/2));
        }
        Vector3 mass_middle_point = (mass_middle_point_1 + mass_middle_point_2)/2f;
        Vector3 dir = (Quaternion.AngleAxis(90, Vector3.up)*(mass_middle_point_1-mass_middle_point_2)).normalized;
        if(no_turn == null && follow_object != null)
            no_turn = follow_object.transform.right*10f;
        else if(follow_object != null)
            no_turn += (follow_object.transform.right*10f - no_turn)*0.01f;

        if(follow_object != null && following){
            if(top_view)
                transform.position = new Vector3(orig.x + offset.x  + follow_object.transform.position.x, orig.y + offset.y, orig.z + offset.z + follow_object.transform.position.z);
            else{
                transform.position = mass_middle_point + dir*10f;
            }
        }
        else{
            if(top_view)
                transform.position = new Vector3(orig.x + offset.x, orig.y + offset.y, orig.z + offset.z);
            else
                transform.position = new Vector3(orig_side_view.x + offset.x, orig_side_view.y + offset.y, orig_side_view.z + offset.z);
        }

        if(Input.GetKeyDown("space")){
            following = !following;
            if(following){
                if(top_view)
                    offset = new Vector3(0f, offset.y, 0f);
                else
                    offset = new Vector3(0f, 0f, 0f);
            }
            else{
                if(top_view)
                    offset = new Vector3(offset.x + follow_object.transform.position.x, offset.y, offset.z + follow_object.transform.position.z);
                else
                    offset = new Vector3(offset.x + follow_object.transform.position.x, offset.y, offset.z + follow_object.transform.position.z);
            }
        }

        if(Input.GetKeyDown("r")){
            top_view = !top_view;
            if(top_view){
                transform.rotation = orig_rotation;
            }
            else{
                transform.rotation = Quaternion.identity;
            }
            if(following){
                if(top_view)
                    offset = new Vector3(0f, offset.y, 0f);
                else
                    offset = new Vector3(0f, 0f, 0f);
            }
            else{
                if(top_view)
                    offset = new Vector3(offset.x + follow_object.transform.position.x, offset.y, offset.z + follow_object.transform.position.z);
                else
                    offset = new Vector3(offset.x + follow_object.transform.position.x, offset.y, offset.z + follow_object.transform.position.z);
            }
        }

        Vector3 move_input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(!top_view && follow_object != null){
            transform.LookAt(follow_object.transform);
        }
        offset += move_input;
    }
}
