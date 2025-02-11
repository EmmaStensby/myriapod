using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using System;
using TMPro;

public class ModularRobot : Agent
{
    public GameObject robot_prefab;
    private GameObject robot;
    private int id;

    public override void OnEpisodeBegin(){}

    private Vector3 first_pos;
    private bool first_pos_set = false;

    private float AngleDifference(float angle1, float angle2){
        float angle = angle1-angle2;
        while(angle > Mathf.PI) angle -= Mathf.PI*2f;
        while(angle < -Mathf.PI) angle += Mathf.PI*2f;
        return angle;
    }

    public override void CollectObservations(VectorSensor sensor){
        int i = 0;
        if(robot != null) {
            // Contractions
            List<float> contractions = new List<float>();
            foreach(Transform module in robot.transform){
                contractions.Add((module.Find("SphereF").position - module.Find("SphereB").position).magnitude);
            }
            // Contraction mean
            float contraction_mean = 0f;
            foreach(float val in contractions){
                contraction_mean += val/contractions.Count;
            }
            // Contraction variance
            float contraction_variance = 0f;
            foreach(float val in contractions){
                contraction_variance += (val-contraction_mean)*(val-contraction_mean)/contractions.Count;
            }
            contraction_variance = Mathf.Sqrt(contraction_variance);



            // Undulations
            List<float> undulations = new List<float>();
            Transform previous_module = null;
            foreach(Transform module in robot.transform){
                if(previous_module != null){
                    Vector3 this_vector = module.Find("SphereF").position - module.Find("SphereB").position;
                    Vector3 prev_vector = previous_module.Find("SphereF").position - previous_module.Find("SphereB").position;
                    float angle = Vector3.SignedAngle(prev_vector, this_vector, Vector3.up);
                    undulations.Add(Mathf.Abs(angle));
                }
                previous_module = module;
            }
            // Undulation mean
            float undulation_mean = 0f;
            foreach(float val in undulations){
                undulation_mean += val/undulations.Count;
            }
            // Undulation variance
            float undulation_variance = 0f;
            foreach(float val in undulations){
                undulation_variance += (val-undulation_mean)*(val-undulation_mean)/undulations.Count;
            }
            undulation_variance = Mathf.Sqrt(undulation_variance);



            // Phases transverse (ahead of each other)
            List<float> phases_transverse = new List<float>();
            previous_module = null;
            foreach(Transform module in robot.transform){
                if(previous_module != null){
                    float phase_offset_l = AngleDifference(module.Find("SphereLegL").GetComponent<LegController>().current_phase, previous_module.Find("SphereLegL").GetComponent<LegController>().current_phase);
                    float phase_offset_r = AngleDifference(module.Find("SphereLegR").GetComponent<LegController>().current_phase, previous_module.Find("SphereLegR").GetComponent<LegController>().current_phase);
                    phases_transverse.Add(phase_offset_l);
                    phases_transverse.Add(phase_offset_r);
                }
                previous_module = module;
            }
            // Phase transverse mean
            float phase_transverse_mean = 0f;
            foreach(float val in phases_transverse){
                phase_transverse_mean += val/phases_transverse.Count;
            }
            // Phase transverse variance
            float phase_transverse_variance = 0f;
            foreach(float val in phases_transverse){
                phase_transverse_variance += AngleDifference(val, phase_transverse_mean)*AngleDifference(val, phase_transverse_mean)/phases_transverse.Count;
            }
            phase_transverse_variance = Mathf.Sqrt(phase_transverse_variance);



            // Phases lateral (opposite sides)
            List<float> phases_lateral = new List<float>();
            foreach(Transform module in robot.transform){
                float phase_offset = AngleDifference(module.Find("SphereLegL").GetComponent<LegController>().current_phase, module.Find("SphereLegR").GetComponent<LegController>().current_phase);
                phases_lateral.Add(phase_offset);
            }
            // Phase lateral mean
            float phase_lateral_mean = 0f;
            foreach(float val in phases_lateral){
                phase_lateral_mean += val/phases_lateral.Count;
            }
            // Phase lateral variance
            float phase_lateral_variance = 0f;
            foreach(float val in phases_lateral){
                phase_lateral_variance += AngleDifference(val, phase_lateral_mean)*AngleDifference(val, phase_lateral_mean)/phases_lateral.Count;
            }
            phase_lateral_variance = Mathf.Sqrt(phase_lateral_variance);
 
            // Add any observations
            sensor.AddObservation(contraction_mean);
            sensor.AddObservation(contraction_variance);
            sensor.AddObservation(undulation_mean);
            sensor.AddObservation(undulation_variance);
            sensor.AddObservation(phase_transverse_mean);
            sensor.AddObservation(phase_transverse_variance);
            sensor.AddObservation(phase_lateral_mean);
            sensor.AddObservation(phase_lateral_variance);
            i += 8;
        }
        // Pad the rest
        for(; i < sensor.GetObservationSpec().Shape[0]; i++){
            sensor.AddObservation(0);
        }
    }

    public override void OnActionReceived(ActionBuffers actions){
        if(robot == null) SetReward(0);
        else {
            if(!first_pos_set){
                first_pos = robot.transform.GetChild(0).Find("SphereF").position;
                first_pos_set = true;
            }
            if(robot.GetComponent<InstantiateRobot>().climb_mode) SetReward(robot.transform.GetChild(0).Find("SphereF").position.y - first_pos.y);
            else SetReward(Vector3.Distance(robot.transform.GetChild(0).Find("SphereF").position, first_pos));
        }
    }

    public void CreateRobot(RobotConfigChannel rc){
        robot = GameObject.Instantiate(robot_prefab);
        InstantiateRobot ir = robot.GetComponent<InstantiateRobot>();
        ir.alpha_legs = rc.alpha_legs;
        ir.alpha_feet = rc.alpha_feet;
        ir.beta = rc.beta;
        ir.contractions_buffer_length = rc.contractions_buffer_length;
        ir.gamma = rc.gamma;
        ir.sigma_a = rc.sigma_a;
        ir.sigma_t = rc.sigma_t;
        ir.omega = rc.omega;
        ir.middle_sections = rc.middle_sections;
        ir.leg_length = rc.leg_length;
        ir.leg_height = rc.leg_height;
        ir.foot_length = rc.foot_length;
        ir.point_mass = rc.point_mass;
        ir.body_mass_modifier = rc.body_mass_modifier;
        ir.climb_mode = rc.climb_mode;
        ir.clamp_amount = rc.clamp_amount;
        ir.offset_variable = rc.offset_variable;
        ir.ant_mode = rc.ant_mode;

        ir.linear_spring_active_legs = rc.linear_spring_active_legs;
        ir.linear_damper_active_legs = rc.linear_damper_active_legs;
        ir.rotation_spring_active_legs = rc.rotation_spring_active_legs;
        ir.rotation_damper_active_legs = rc.rotation_damper_active_legs;
        ir.linear_spring_active_feet = rc.linear_spring_active_feet;
        ir.linear_damper_active_feet = rc.linear_damper_active_feet;
        ir.rotation_spring_active_feet = rc.rotation_spring_active_feet;
        ir.rotation_damper_active_feet = rc.rotation_damper_active_feet;
        ir.linear_spring_active_body1 = rc.linear_spring_active_body1;
        ir.linear_damper_active_body1 = rc.linear_damper_active_body1;
        ir.rotation_spring_active_body1 = rc.rotation_spring_active_body1;
        ir.rotation_damper_active_body1 = rc.rotation_damper_active_body1;
        ir.linear_spring_active_body2 = rc.linear_spring_active_body2;
        ir.linear_damper_active_body2 = rc.linear_damper_active_body2;
        ir.rotation_spring_active_body2 = rc.rotation_spring_active_body2;
        ir.rotation_damper_active_body2 = rc.rotation_damper_active_body2;
        ir.linear_spring_active_body3 = rc.linear_spring_active_body3;
        ir.linear_damper_active_body3 = rc.linear_damper_active_body3;
        ir.rotation_spring_active_body3 = rc.rotation_spring_active_body3;
        ir.rotation_damper_active_body3 = rc.rotation_damper_active_body3;

        ir.InstantiateModules();
        
        if(rc.climb_mode){
            robot.transform.position = new Vector3(0f, 35f, 0f);
            robot.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }
}