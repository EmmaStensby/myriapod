import pickle
import matplotlib.pyplot as plt
import numpy as np
from matplotlib import cm


if __name__ == "__main__":
    paths_AT = ['results1169605_A_T.pkl','results2758288_A_T.pkl','results2845613_A_T.pkl','results4207622_A_T.pkl','results498274_A_T.pkl','results5547994_A_T.pkl','results5639711_A_T.pkl','results6230966_A_T.pkl','results6576234_A_T.pkl','results8959124_A_T.pkl']

    betas = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2, 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 3, 3.1]
    leg_indexs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
    leg_lengths = [0.25*(x+4) for x in leg_indexs]

    first = True
    first_2 = True
    
    title = ""

    for path in paths_AT:

        with open('results/' + path, 'rb') as f: 
            results = pickle.load(f)

        if first:
            val_u = [row[2] for row in results[0][2]] # Undulation
            val_c = [row[1] for row in results[0][2]] # Contraction variation
            val_p = [row[4] for row in results[0][2]] # Phase differences
            val_pv = [row[5] for row in results[0][2]] # Variance phase differences
            val_l = [row[6] for row in results[0][2]] # Lateral phase differences
            val_lv = [row[7] for row in results[0][2]]
            first = False
        else:
            for i in range(len(val_u)):
                val_u[i] += [row[2] for row in results[0][2]][i] # Undulation
                val_c[i] += [row[1] for row in results[0][2]][i] # Contraction variation
                val_p[i] += [row[4] for row in results[0][2]][i] # Phase differences
                val_pv[i] += [row[5] for row in results[0][2]][i] # Variance phase differences
                val_l[i] += [row[6] for row in results[0][2]][i] # Lateral phase differences
                val_lv[i] += [row[7] for row in results[0][2]][i]
            
        if first_2:
            val_u_2 = [row[2+8] for row in results[0][2]] # Undulation
            val_c_2 = [row[1+8] for row in results[0][2]] # Contraction variation
            val_p_2 = [row[4+8] for row in results[0][2]] # Phase differences
            val_pv_2 = [row[5+8] for row in results[0][2]] # Variance phase differences
            val_l_2 = [row[6+8] for row in results[0][2]] # Lateral phase differences
            val_lv_2 = [row[7+8] for row in results[0][2]]
            first_2 = False
        else:
            for i in range(len(val_u_2)):
                val_u_2[i] += [row[2+8] for row in results[0][2]][i] # Undulation
                val_c_2[i] += [row[1+8] for row in results[0][2]][i] # Contraction variation
                val_p_2[i] += [row[4+8] for row in results[0][2]][i] # Phase differences
                val_pv_2[i] += [row[5+8] for row in results[0][2]][i] # Variance phase differences
                val_l_2[i] += [row[6+8] for row in results[0][2]][i] # Lateral phase differences
                val_lv_2[i] += [row[7+8] for row in results[0][2]][i]

    for i in range(len(val_u)):
        val_u[i] = val_u[i]/len(paths_AT)
        val_c[i] = val_c[i]/len(paths_AT)
        val_p[i] = val_p[i]/len(paths_AT)
        val_pv[i] = val_pv[i]/len(paths_AT)
        val_l[i] = val_l[i]/len(paths_AT)
        val_lv[i] = val_lv[i]/len(paths_AT)

    for i in range(len(val_u_2)):
        val_u_2[i] = val_u_2[i]/len(paths_AT)
        val_c_2[i] = val_c_2[i]/len(paths_AT)
        val_p_2[i] = val_p_2[i]/len(paths_AT)
        val_pv_2[i] = val_pv_2[i]/len(paths_AT)
        val_l_2[i] = val_l_2[i]/len(paths_AT)
        val_lv_2[i] = val_lv_2[i]/len(paths_AT)

    values = [1, 2, 3, 4, 5, 6]
    colors = [cm.magma(x/6) for x in values]

    fig, axs = plt.subplots(1, 2, figsize=(6.4*1.1, 2.4*1.1))
    val_pv_1 = []
    val_pv_22 = []
    for i in range(len(val_p)):
        val_pv_1.append(val_p[i] + val_pv[i])
        val_pv_22.append(val_p[i] - val_pv[i])
    val_lv_1 = []
    val_lv_2 = []
    for i in range(len(val_l)):
        val_lv_1.append(val_l[i] + val_lv[i])
        val_lv_2.append(val_l[i] - val_lv[i])
    val_pv_1_2 = []
    val_pv_2_2 = []
    for i in range(len(val_p_2)):
        val_pv_1_2.append(val_p_2[i] + val_pv_2[i])
        val_pv_2_2.append(val_p_2[i] - val_pv_2[i])
    val_lv_1_2 = []
    val_lv_2_2 = []
    for i in range(len(val_l_2)):
        val_lv_1_2.append(val_l_2[i] + val_lv_2[i])
        val_lv_2_2.append(val_l_2[i] - val_lv_2[i])
    axs[0].plot(val_p, color=colors[3])
    axs[0].plot(val_l, color=colors[0])
    axs[0].fill_between(range(len(val_pv_22)),val_lv_1, val_lv_2, color=colors[1], alpha=0.2)
    axs[0].fill_between(range(len(val_pv_22)), val_pv_1, val_pv_22, color=colors[3], alpha=0.2)
    axs[1].plot(val_p_2, label="Ipsilateral phase difference", color=colors[3])
    axs[1].plot(val_l_2, label="Contralateral phase difference", color=colors[0])
    axs[1].fill_between(range(len(val_pv_2_2)),val_lv_1_2, val_lv_2_2, color=colors[1], alpha=0.2)
    axs[1].fill_between(range(len(val_pv_2_2)), val_pv_1_2, val_pv_2_2, color=colors[3], alpha=0.2)
    fig.subplots_adjust(bottom=0.35)
    fig.subplots_adjust(wspace=0.3)
    fig.legend(loc='lower center', bbox_to_anchor=(0.5, 0.03), ncol=2)
    axs[0].set_title("Front half (Long legs)")
    axs[0].set_xlabel("Time (phyics steps)")
    axs[0].set_ylabel("Phase (radians)")
    axs[0].set_xlim(0, 10000)
    axs[0].set_ylim(-4, 4)
    axs[1].set_title("Back half (Short legs)")
    axs[1].set_xlabel("Time (phyics steps)")
    axs[1].set_ylabel("Phase (radians)")
    axs[1].set_xlim(0, 10000)
    axs[1].set_ylim(-4, 4)
    plt.savefig("fig4.pdf")

