import uuid
from mlagents_envs.side_channel.side_channel import (
    SideChannel,
    IncomingMessage,
    OutgoingMessage,
)

class RobotConfigChannel(SideChannel):

    def __init__(self) -> None:
        super().__init__(uuid.UUID("621f0a70-4f87-11ea-a6bf-784f4387d1f7"))
        self.message = None 

    def reset(self):
        self.message = None 

    def send_config(self, seed, parameters, pd_parameters):
        alpha_legs = parameters[0]
        alpha_feet = parameters[1]
        beta = parameters[2]
        contractions_buffer_length = parameters[3]
        gamma = parameters[4]
        sigma_a = parameters[5]
        sigma_t = parameters[6]
        omega = parameters[7]
        middle_sections = parameters[8]
        leg_length = parameters[9]
        leg_height = parameters[10]
        foot_length = parameters[11] 
        point_mass = parameters[12]
        body_mass_modifier = parameters[13]
        climb_mode = parameters[14]
        clamp_amount = parameters[15]
        offset_variable = parameters[16]
        ant_mode = parameters[17]
        foot_friction = parameters[18]
        gravity = parameters[19]
        climb_angle = parameters[20]

        linear_spring_active_legs = pd_parameters[0]
        linear_damper_active_legs = pd_parameters[1]
        rotation_spring_active_legs = pd_parameters[2]
        rotation_damper_active_legs = pd_parameters[3]
        linear_spring_active_feet = pd_parameters[4]
        linear_damper_active_feet = pd_parameters[5]
        rotation_spring_active_feet = pd_parameters[6]
        rotation_damper_active_feet = pd_parameters[7]
        linear_spring_active_body1 = pd_parameters[8]
        linear_damper_active_body1 = pd_parameters[9]
        rotation_spring_active_body1 = pd_parameters[10]
        rotation_damper_active_body1 = pd_parameters[11]
        linear_spring_active_body2 = pd_parameters[12]
        linear_damper_active_body2 = pd_parameters[13]
        rotation_spring_active_body2 = pd_parameters[14]
        rotation_damper_active_body2 = pd_parameters[15]
        linear_spring_active_body3 = pd_parameters[16]
        linear_damper_active_body3 = pd_parameters[17]
        rotation_spring_active_body3 = pd_parameters[18]
        rotation_damper_active_body3 = pd_parameters[19]

        msg = OutgoingMessage()
        msg.write_int32(seed)
        msg.write_float32(alpha_legs)
        msg.write_float32(alpha_feet)
        msg.write_float32(beta)
        msg.write_int32(contractions_buffer_length)
        msg.write_float32(gamma)
        msg.write_float32(sigma_a)
        msg.write_float32(sigma_t)
        msg.write_float32(omega)
        msg.write_int32(middle_sections)
        msg.write_float32(leg_length)
        msg.write_float32(leg_height)
        msg.write_float32(foot_length)
        msg.write_float32(point_mass)
        msg.write_float32(body_mass_modifier)
        msg.write_int32(climb_mode)
        msg.write_float32(clamp_amount)
        msg.write_float32(offset_variable)
        msg.write_int32(ant_mode)
        msg.write_float32(foot_friction)
        msg.write_float32(gravity)
        msg.write_float32(climb_angle)

        msg.write_float32(linear_spring_active_legs)
        msg.write_float32(linear_damper_active_legs)
        msg.write_float32(rotation_spring_active_legs)
        msg.write_float32(rotation_damper_active_legs)
        msg.write_float32(linear_spring_active_feet)
        msg.write_float32(linear_damper_active_feet)
        msg.write_float32(rotation_spring_active_feet)
        msg.write_float32(rotation_damper_active_feet)
        msg.write_float32(linear_spring_active_body1)
        msg.write_float32(linear_damper_active_body1)
        msg.write_float32(rotation_spring_active_body1)
        msg.write_float32(rotation_damper_active_body1)
        msg.write_float32(linear_spring_active_body2)
        msg.write_float32(linear_damper_active_body2)
        msg.write_float32(rotation_spring_active_body2)
        msg.write_float32(rotation_damper_active_body2)
        msg.write_float32(linear_spring_active_body3)
        msg.write_float32(linear_damper_active_body3)
        msg.write_float32(rotation_spring_active_body3)
        msg.write_float32(rotation_damper_active_body3)

        super().queue_message_to_send(msg)

    def on_message_received(self, msg: IncomingMessage) -> None:
        self.message = 1