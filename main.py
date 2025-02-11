from unity_interface import RobotConfigChannel
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.base_env import ActionTuple
from multiprocessing import Pool
import os
import numpy as np
import time
import random
import pickle

BUILD_PATH="Builds/TestBuild.x86_64"

import socket
HIGHEST_WORKER_ID = 65535 - UnityEnvironment.BASE_ENVIRONMENT_PORT
def is_port_in_use(port: int) -> bool:
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        return s.connect_ex(('localhost', port)) == 0

def is_worker_id_open(worker_id: int) -> bool:
    return not is_port_in_use(
        UnityEnvironment.BASE_ENVIRONMENT_PORT + worker_id
    )

def get_worker_id() -> int:
    pid = random.randrange(HIGHEST_WORKER_ID)
    while not is_worker_id_open(pid):
        print("Socket is occupied, trying a new worker_id")
        pid = random.randrange(HIGHEST_WORKER_ID)
    return pid


class Evaluator():
    def __init__(self, editor_mode=False, headless=True, worker_id=0, evaluation_steps=2000):
        self.channel = RobotConfigChannel()
        self.evaluation_steps = evaluation_steps
        self.env = None
        if editor_mode:
            self.env = UnityEnvironment(file_name=None, seed=0, side_channels=[self.channel])
        elif headless:
            self.env = UnityEnvironment(file_name=BUILD_PATH, seed=0, side_channels=[self.channel], no_graphics=True, worker_id=worker_id)
        else:
            self.env = UnityEnvironment(file_name=BUILD_PATH, seed=0, side_channels=[self.channel], no_graphics=False, worker_id=worker_id)
        
    def evaluate(self, seed, centipede_parameters, pd_parameters):
        self.channel.reset()
        self.channel.send_config(seed, centipede_parameters, pd_parameters)
        self.env.reset()

        while (self.channel.message is None):
            continue

        behavior_names = self.env.behavior_specs.keys()
        behavior_name = list(behavior_names)[0]

        observations = []

        for i in range(self.evaluation_steps):
            decisionSteps, _ = self.env.get_steps(behavior_name)
            if len(list(decisionSteps)) == 0:
                break
            if len(decisionSteps.obs) > 0:
                observations.append(list(decisionSteps.obs[0][0]))
            actions = np.ndarray(shape=(1,1),dtype=np.float32)
            self.env.set_actions(behavior_name, ActionTuple(actions))
            self.env.step()

        print(pd_parameters, decisionSteps[list(decisionSteps)[-1]].reward)

        return decisionSteps[list(decisionSteps)[-1]].reward, centipede_parameters, observations, pd_parameters, seed

    def close(self):
        self.env.close()

def evaluate_individual(parameter_set_all):
    parameter_set, pd_parameter_set, seed_set = parameter_set_all
    worker_id=get_worker_id()
    evaluator = Evaluator(worker_id=worker_id, evaluation_steps=5000)
    fitnesses = []
    for parameters, pd_parameters, seed in zip(parameter_set, pd_parameter_set, seed_set):
        fitnesses.append(evaluator.evaluate(seed, parameters, pd_parameters))
    return fitnesses

if __name__ == "__main__":
    threads = 2
    master_seed = np.random.randint(10000000)
    rng = np.random.default_rng(master_seed)
    path = 'results{}_A_T.pkl'.format(master_seed)

    import sys
    the_input = int(sys.argv[2])

    if the_input == 0:
        path = 'results{}_A.pkl'.format(master_seed)
    if the_input == 1:
        path = 'results{}_T.pkl'.format(master_seed)
        
    print(path)

    
    alpha_legss = [20]
    alpha_feets = [20]
    betas = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 3, 3.1]
    contractions_buffer_lengths = [20]
    gammas = [0.005]
    sigma_as = [4.5]
    sigma_ts = [3.8]
    if the_input == 0:
        sigma_ts = [0]
    if the_input == 1:
        sigma_as = [0]
    omegas = [3.14]
    middle_sectionss = [10]
    leg_heights = [-0.5]
    foot_leg_indexs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
    foot_lengths = [0.25*0.7*(foot_leg_index+4) for foot_leg_index in foot_leg_indexs]
    leg_lengths = [0.25*0.3*(foot_leg_index+4) for foot_leg_index in foot_leg_indexs]
    foot_leg_indexs = [foot_leg_indexs[8]]
    point_masss = [0.05]
    body_mass_modifiers = [0.2, 0.6, 1, 1.4, 1.8, 2.2, 2.6, 3, 3.4, 3.8, 4.2, 4.6, 5, 5.4, 5.8, 6.2, 6.6, 7, 7.4, 7.8, 8.2, 8.6, 9, 9.4, 9.8, 10.2, 10.6, 11, 11.4, 11.8, 12.2, 12.6]
    climb_modes = [0]
    clamp_amounts = [115]
    offset_variables = [0.5]
    ant_modes = [0]
    iterations = 1
    foot_frictions = [5]
    gravitys = [0.25]
    climb_angles = [0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48, 51, 54, 57, 60, 63, 66, 69, 72, 75, 78, 81, 84, 87, 90, 93]
    climb_angles =  [0]

    parameters_one = [[alpha_legs, alpha_feet, beta, contractions_buffer_length, gamma, sigma_a, sigma_t, omega, middle_sections, leg_lengths[foot_leg_index], leg_height, foot_lengths[foot_leg_index], point_mass, body_mass_modifier, climb_mode, clamp_amount, offset_variable, ant_mode, foot_friction, gravity, climb_angle] for alpha_legs in alpha_legss for alpha_feet in alpha_feets for beta in betas for contractions_buffer_length in contractions_buffer_lengths for gamma in gammas for sigma_a in sigma_as for sigma_t in sigma_ts for omega in omegas for middle_sections in middle_sectionss for leg_height in leg_heights for foot_leg_index in foot_leg_indexs for point_mass in point_masss for body_mass_modifier in body_mass_modifiers for climb_mode in climb_modes for clamp_amount in clamp_amounts for offset_variable in offset_variables for ant_mode in ant_modes for foot_friction in foot_frictions for gravity in gravitys for climb_angle in climb_angles for _ in range(iterations)]
  
    parameters = []
    for parameter in parameters_one:
        if(not (parameter[5] == 0 and parameter[6] == 0)):
            parameters.append(parameter)
    
    x = 1
    pd_parameters = [[1750*x, 20*x, 8500*x, 0*x, 4500*x, 5*x, 7000*x, 40*x, 5000*x, 300*x, 600*x, 0*x, 5500*x, 10*x, 5000*x, 20*x, 1500*x, 20*x, 6000*x, 30*x] for _ in range(len(parameters))]

    seeds = [rng.integers(0, 1000000000) for _ in range(len(parameters))]
    individuals = len(parameters)
    slice_size = individuals//threads 
    parameter_sets = [parameters[i*slice_size:(i+1)*slice_size] for i in range(threads)]
    pd_parameter_sets = [pd_parameters[i*slice_size:(i+1)*slice_size] for i in range(threads)]
    seed_sets = [seeds[i*slice_size:(i+1)*slice_size] for i in range(threads)]
    
    # Run
    t1 = time.time()
    with Pool(threads) as p:
        result_sets = p.map(evaluate_individual, zip(parameter_sets, pd_parameter_sets, seed_sets))
    t2 = time.time()
    print("{:.2f} seconds to evaulate {} individuals with {} threads".format(t2-t1, individuals, threads))

    # Flatten results
    results = []
    for result_set in result_sets:
        for result in result_set:
            results.append(list(result))
    
    with open('results/' + path, 'wb') as f:
        pickle.dump(results, f)