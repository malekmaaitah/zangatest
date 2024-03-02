using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // general define parameters
    public float start_energy=500;
    public float prefered_energy_density = 10;
    public float energy_consumption_factor = 0.1F;
    public float energy_growing_effort = 0.2f;
    public float energy_growing_factor = 0.5F;
    public float width_to_length_ratio = .15F;
    public float max_length_increase_ratio=0.1F;
    public float max_branch_length = 2.5F;
    public float end_to_start_width_ratio =0.8F;
    public float child_to_parent_branch_width_ratio = 0.9F;
    public float energy_transfer_constant = 0.3f;
    public float leaf_collected_energy_per_time = 10;
    public float max_leaf_length = 3;
    public float my_delta_time = 0.5f;
    public float falling_speed = 0.2f;
    public float transp_speed = 0.2f;
    public float intelligent = 0;
    public uint no_branches = 1;
    public uint no_leaves = 0;

}
