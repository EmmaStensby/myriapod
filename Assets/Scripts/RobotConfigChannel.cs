using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.SideChannels;
using System.Text;
using System;

public class RobotConfigChannel : SideChannel
{
    public int seed;
    public float alpha_legs;
    public float alpha_feet;
    public float beta;
    public int contractions_buffer_length;
    public float gamma;
    public float sigma_a;
    public float sigma_t;
    public float omega;
    public int middle_sections;
    public float leg_length;
    public float leg_height;
    public float foot_length;
    public float point_mass;
    public float body_mass_modifier;
    public float clamp_amount;
    public float offset_variable;

    public bool climb_mode = false;
    public bool ant_mode = false;

    public float linear_spring_active_legs = 0f;
    public float linear_damper_active_legs = 0f;
    public float linear_maxforce_active_legs = 0f;
    public float rotation_spring_active_legs = 0f;
    public float rotation_damper_active_legs = 0f;
    public float rotation_maxforce_active_legs = 0f;
    public float linear_spring_active_feet = 0f;
    public float linear_damper_active_feet = 0f;
    public float linear_maxforce_active_feet = 0f;
    public float rotation_spring_active_feet = 0f;
    public float rotation_damper_active_feet = 0f;
    public float rotation_maxforce_active_feet = 0f;
    public float linear_spring_active_body1 = 0f;
    public float linear_damper_active_body1 = 0f;
    public float linear_maxforce_active_body1 = 0f;
    public float rotation_spring_active_body1 = 0f;
    public float rotation_damper_active_body1 = 0f;
    public float rotation_maxforce_active_body1 = 0f;
    public float linear_spring_active_body2 = 0f;
    public float linear_damper_active_body2 = 0f;
    public float linear_maxforce_active_body2 = 0f;
    public float rotation_spring_active_body2 = 0f;
    public float rotation_damper_active_body2 = 0f;
    public float rotation_maxforce_active_body2 = 0f;
    public float linear_spring_active_body3 = 0f;
    public float linear_damper_active_body3 = 0f;
    public float linear_maxforce_active_body3 = 0f;
    public float rotation_spring_active_body3 = 0f;
    public float rotation_damper_active_body3 = 0f;
    public float rotation_maxforce_active_body3 = 0f;

    public RobotConfigChannel(){
        ChannelId = new Guid("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
    }

    protected override void OnMessageReceived(IncomingMessage msg){
        seed = msg.ReadInt32();
        alpha_legs = msg.ReadFloat32();
        alpha_feet = msg.ReadFloat32();
        beta = msg.ReadFloat32();
        contractions_buffer_length = msg.ReadInt32();
        gamma = msg.ReadFloat32();
        sigma_a = msg.ReadFloat32();
        sigma_t = msg.ReadFloat32();
        omega = msg.ReadFloat32();
        middle_sections = msg.ReadInt32();
        leg_length = msg.ReadFloat32();
        leg_height = msg.ReadFloat32();
        foot_length = msg.ReadFloat32();
        point_mass = msg.ReadFloat32();
        body_mass_modifier = msg.ReadFloat32();
        if(msg.ReadInt32() > 0) climb_mode = true;
        clamp_amount = msg.ReadFloat32();
        offset_variable = msg.ReadFloat32();
        if(msg.ReadInt32() > 0) ant_mode = true;

        linear_spring_active_legs = msg.ReadFloat32();
        linear_damper_active_legs = msg.ReadFloat32();
        rotation_spring_active_legs = msg.ReadFloat32();
        rotation_damper_active_legs = msg.ReadFloat32();
        linear_spring_active_feet = msg.ReadFloat32();
        linear_damper_active_feet = msg.ReadFloat32();
        rotation_spring_active_feet = msg.ReadFloat32();
        rotation_damper_active_feet = msg.ReadFloat32();
        linear_spring_active_body1 = msg.ReadFloat32();
        linear_damper_active_body1 = msg.ReadFloat32();
        rotation_spring_active_body1 = msg.ReadFloat32();
        rotation_damper_active_body1 = msg.ReadFloat32();
        linear_spring_active_body2 = msg.ReadFloat32();
        linear_damper_active_body2 = msg.ReadFloat32();
        rotation_spring_active_body2 = msg.ReadFloat32();
        rotation_damper_active_body2 = msg.ReadFloat32();
        linear_spring_active_body3 = msg.ReadFloat32();
        linear_damper_active_body3 = msg.ReadFloat32();
        rotation_spring_active_body3 = msg.ReadFloat32();
        rotation_damper_active_body3 = msg.ReadFloat32();
    }

    public void SendMessage(){
        OutgoingMessage msg = new OutgoingMessage();
        QueueMessageToSend(msg);
    }
}
