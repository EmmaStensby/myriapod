import pickle
import matplotlib.pyplot as plt
import numpy as np
from collections import Counter 

if __name__ == "__main__":

    exp = 1

    # Experiment 1
    if exp == 1:
        paths_A = ['results1295785_A.pkl', 'results2854521_A.pkl', 'results3253966_A.pkl', 'results3754198_A.pkl', 'results502995_A.pkl', 'results5145333_A.pkl', 'results6049732_A.pkl', 'results6453636_A.pkl', 'results8751954_A.pkl', 'results926387_A.pkl']
        paths_T = ['results5622638_T.pkl', 'results141263_T.pkl', 'results1764647_T.pkl', 'results184900_T.pkl', 'results3648147_T.pkl', 'results4561264_T.pkl', 'results502116_T.pkl', 'results6516500_T.pkl', 'results7287583_T.pkl', 'results8197323_T.pkl']
        paths_AT = ['results7254560_A_T.pkl', 'results1083005_A_T.pkl', 'results1358431_A_T.pkl', 'results1513989_A_T.pkl','results4048447_A_T.pkl', 'results4976910_A_T.pkl', 'results6770910_A_T.pkl', 'results7307255_A_T.pkl', 'results7309227_A_T.pkl', 'results7615467_A_T.pkl']

    # Experiment 2
    if exp == 2:
        paths_T = ['results1893976_T.pkl', 'results2004318_T.pkl', 'results2008844_T.pkl', 'results2619261_T.pkl', 'results488616_T.pkl', 'results553575_T.pkl', 'results5994942_T.pkl', 'results6867000_T.pkl', 'results7672286_T.pkl', 'results9611687_T.pkl']
        paths_A = ['results2341376_A.pkl', 'results2408114_A.pkl', 'results3321228_A.pkl', 'results4696507_A.pkl', 'results5090049_A.pkl', 'results615608_A.pkl', 'results7062533_A.pkl', 'results7233890_A.pkl', 'results7462828_A.pkl', 'results7757872_A.pkl']
        paths_AT = ['results1500367_A_T.pkl', 'results2208284_A_T.pkl', 'results3509883_A_T.pkl', 'results4608487_A_T.pkl', 'results5983125_A_T.pkl', 'results8115216_A_T.pkl', 'results8709969_A_T.pkl', 'results8819273_A_T.pkl', 'results9724141_A_T.pkl']

    # Experiment 3
    if exp == 3:
        paths_A = ['results1119111_A.pkl','results1217112_A.pkl','results1871755_A.pkl','results5746335_A.pkl','results5953090_A.pkl','results6630077_A.pkl','results7590176_A.pkl','results8146077_A.pkl','results9759450_A.pkl','results9983834_A.pkl']
        paths_T = ['results1064878_T.pkl','results2211124_T.pkl','results237488_T.pkl','results3570538_T.pkl','results4757059_T.pkl','results5288890_T.pkl','results5933963_T.pkl','results7349972_T.pkl','results7746603_T.pkl','results7917454_T.pkl']
        paths_AT = ['results325850_A_T.pkl','results5516136_A_T.pkl','results560936_A_T.pkl','results5707992_A_T.pkl','results5764285_A_T.pkl','results5928874_A_T.pkl','results6022481_A_T.pkl','results6525546_A_T.pkl','results7538470_A_T.pkl','results8647462_A_T.pkl']
    
    paths_all = [paths_A, paths_T, paths_AT]
    plot_index = 0
    fig, axs = plt.subplots(2, 3, figsize=(7.16, 2*2*1.5))
    for i in range(3):

        result_sets = []

        paths = paths_all[i]
        for path in paths:
            dir_path = 'results/'
            with open(dir_path + path, 'rb') as f: 
                results = pickle.load(f)
                result_sets.append(results)

        alpha_legss = [20]
        alpha_feets = [20]
        betas = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 3, 3.1]
        contractions_buffer_lengths = [20]
        gammas = [0.005]
        sigma_as = [4.5]
        sigma_ts = [4]
        omegas = [3.14]
        middle_sectionss = [10]
        leg_lengths = [0.5]
        leg_heights = [-0.5]
        foot_leg_indexs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
        foot_lengths = [0.25*0.7*(foot_leg_index+4) for foot_leg_index in foot_leg_indexs]
        leg_lengths = [0.25*0.3*(foot_leg_index+4) for foot_leg_index in foot_leg_indexs]
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

        parameters = [[alpha_legs, alpha_feet, beta, contractions_buffer_length, gamma, sigma_a, sigma_t, omega, middle_sections, leg_lengths[foot_leg_index], leg_height, foot_lengths[foot_leg_index], point_mass, body_mass_modifier, climb_mode, clamp_amount, offset_variable, ant_mode, foot_friction, gravity] for alpha_legs in alpha_legss for alpha_feet in alpha_feets for beta in betas for contractions_buffer_length in contractions_buffer_lengths for gamma in gammas for sigma_a in sigma_as for sigma_t in sigma_ts for omega in omegas for middle_sections in middle_sectionss for leg_height in leg_heights for foot_leg_index in foot_leg_indexs for point_mass in point_masss for body_mass_modifier in body_mass_modifiers for climb_mode in climb_modes for clamp_amount in clamp_amounts for offset_variable in offset_variables for ant_mode in ant_modes for foot_friction in foot_frictions for gravity in gravitys for _ in range(iterations)]

        for offset_variable in offset_variables:
            for inner_plot_index in range(2):
                mat_sigma_0 = []
                mat_sigma_0_alpha = []
                mat_sigma_0_titles = ["Distance walked\nOffset: {}".format(offset_variable), "Undulation\nOffset: {}".format(offset_variable), "Std. segment length\nOffset: {}".format(offset_variable), "Trans. phase diff.\nOffset: {}".format(offset_variable), "Lat. phase diff.\nOffset: {}".format(offset_variable)]
                vmins = [0, 0, 0, -2, -2]
                vmaxs = [400, 6, 0.08, 1, 2]
                if exp == 3:
                    vmins = [0, 0, 0, -2, -2]
                    vmaxs = [10, 6, 0.08, 1, 2]
                index = 0
                for beta in betas: 
                    row_sigma_0 = []
                    row_sigma_0_alpha = []
                    for leg_length in foot_leg_indexs:
                        vals = []
                        if inner_plot_index == 0:
                            val = 0
                            for result_set in result_sets:
                                val += result_set[index][0]/len(result_sets) # Distance walked
                            most_frequent = val
                            the_count = 0
                        elif result_sets[0][index][0] < 0:
                            val = np.nan
                            most_frequent = val
                            the_count = 0
                        elif inner_plot_index == 1:
                            convergence_threshold = 0.7
                            in_phase_threshold = 0.6
                            contraction_threshold = 0.02
                            undulation_threshold = 2
                            vals = []
                            for result_set in result_sets:
                                val_u = np.mean([row[2] for row in result_set[index][2][-200:]])  # Undulation
                                val_c = np.mean([row[1] for row in result_set[index][2][-200:]])  # Contraction variation
                                val_p = np.mean([row[4] for row in result_set[index][2][-200:]])  # Phase differences
                                val_pv = np.mean([row[5] for row in result_set[index][2][-200:]])  # Variance phase differences
                                val_l = np.mean([row[6] for row in result_set[index][2][-200:]])  # Lateral phase differences
                                val_lv = np.mean([row[7] for row in result_set[index][2][-200:]])  # Variance lateral phase differences
                                val = np.nan
                                if val_lv < convergence_threshold and abs(val_l) < in_phase_threshold:
                                    val = 1 # in phase pattern
                                    if val_c > contraction_threshold:
                                        val = 2 # peristaltic pattern
                                elif val_pv < convergence_threshold:
                                    if val_p > 0:
                                        val = 3 # direct pattern
                                        if val_u > undulation_threshold:
                                            val = 4 # undulatory direct pattern
                                    elif val_p < 0:
                                        val = 5 # retrograde pattern
                                        if val_u > undulation_threshold:
                                            val = 6 # undulatory retrograde pattern
                                if not np.isnan(val):
                                    vals.append(val)
                            
                            if len(vals) > 0:
                                #print(vals)
                                counts = Counter(vals) 
                                most_frequent, the_count = counts.most_common(1)[0] 
                            else:
                                most_frequent = np.nan
                                the_count = 0.001

                        row_sigma_0.append(most_frequent)
                        row_sigma_0_alpha.append(0.999)
                        index += 1

                    final_row = row_sigma_0

                    mat_sigma_0.append(final_row)
                    mat_sigma_0_alpha.append(row_sigma_0_alpha)

                if inner_plot_index == 1:
                    im = axs[inner_plot_index][plot_index].matshow(mat_sigma_0, vmin=vmins[inner_plot_index], vmax=vmaxs[inner_plot_index], cmap="magma", origin='lower', alpha=mat_sigma_0_alpha)
                else:
                    ims = axs[inner_plot_index][plot_index].matshow(mat_sigma_0, vmin=vmins[inner_plot_index], vmax=vmaxs[inner_plot_index], cmap="magma", origin='lower')
                titler = ['A', 'C', 'A+C', 'A', 'C', 'A+C']
                axs[inner_plot_index][plot_index].set_title(titler[3*inner_plot_index+plot_index])
                if exp == 1:
                    axs[inner_plot_index][plot_index].set_xlabel("Leg length") 
                if exp == 2:
                    axs[inner_plot_index][plot_index].set_xlabel("Spine segment mass")
                if exp == 3:
                    axs[inner_plot_index][plot_index].set_xlabel("Climb angle")
                axs[inner_plot_index][plot_index].set_ylabel("β", rotation=0)
                axs[inner_plot_index][plot_index].xaxis.tick_bottom()

                # Walk labels
                if exp == 1:
                    axs[inner_plot_index][plot_index].set_xticks([0, 6, 12, 18, 24, 30], labels=[0.25*(x+4) for x in [0, 6, 12, 18, 24, 30]])
                # Mass lablels
                if exp == 2:
                    axs[inner_plot_index][plot_index].set_xticks([0, 10, 20, 30], labels=[str(round(body_mass_modifiers[x]*6*point_masss[0], 2)) for x in [0, 10, 20, 30]])

                # Climb labels
                if exp == 3:
                    axs[inner_plot_index][plot_index].set_xticks([0, 6, 12, 18, 24, 30], labels=[climb_angles[x] for x in [0, 6, 12, 18, 24, 30]])
                axs[inner_plot_index][plot_index].set_yticks([0, 5, 10, 15, 20, 25, 30], labels=[betas[x] for x in [0, 5, 10, 15, 20, 25, 30]])
                if inner_plot_index == 1 and plot_index == 2:
                    values = [1, 2, 3, 4, 5, 6]
                    colors = [ im.cmap(im.norm(value)) for value in values]
                    import matplotlib.patches as mpatches
                    labels = ["In phase", "Peristalsis", "Direct", "Undulatory direct", "Retrograde", "Undulatory retrograde"]
                    patches = [ mpatches.Patch(color=colors[j], label=labels[j]) for j in range(len(values)) ]
                if inner_plot_index == 1:
                    box = axs[inner_plot_index][plot_index].get_position()
                    axs[inner_plot_index][plot_index].set_position([box.x0, box.y0 - box.height * 0.3, box.width, box.height])
                box = axs[inner_plot_index][plot_index].get_position() 
                axs[inner_plot_index][plot_index].set_position([box.x0+box.x0*0.11, box.y0, box.width*1.1, box.height*1.1])
        plot_index += 1
    fig.legend(handles=patches, loc='lower center', bbox_to_anchor=(0.46, 0.03), ncol=3)
    
    pos = axs[0][2].get_position()
    cbar_ax = fig.add_axes([.798 +0.1, pos.y0 + 0.055, 0.02, pos.height*0.85])
    cbar = fig.colorbar(ims, cax=cbar_ax)

    if exp == 3:
        cbar.set_label("Distance climbed") # walked
    else:
        cbar.set_label("Distance walked")

    fig.set_tight_layout(True)
    plt.savefig("fig3.pdf")
