using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
public class Branch : MonoBehaviour
{
    SpriteRenderer sr;
    private Vector3 dir=Vector3.up;
    public uint id = 0;
    public uint branch_order = 0;

     float prefered_energy_density ; 
     float energy_consumption_factor ; // amount of consumed energy per volume
     float energy_growing_factor ; 
     float energy_growing_effort; // energy needed for each added volume
     float width_to_length_ratio ;
     float max_length_increase_ratio ;
     float max_branch_length ;
     float end_to_start_width_ratio ;
     float child_to_parent_branch_width_ratio ;
     float energy_transfer_constant;
     public Branch prev_branch;
     public List<Branch> next_branches;
      float original_scale;
     static Tree tree;
    
    float my_delta_time;
    public static GameObject branchobjPrefab ;
    public static GameObject leafobjPrefab ;
    // Start is called before the first frame update
    void Start()
    {
        sr=GetComponent<SpriteRenderer>();
        if(branch_order==0){
        tree=gameObject.GetComponentInParent<Tree>();
        dir=Quaternion.Euler(0,0,tree.transform.eulerAngles.z)*Vector3.up;
        Energy =  tree.start_energy;
        branchobjPrefab = Resources.Load("Strait branch") as GameObject;
        leafobjPrefab = Resources.Load("Leaf go") as GameObject;

        }
    

    prefered_energy_density = tree.prefered_energy_density* Random.Range(0.9F,1.1F);
    energy_consumption_factor = tree.energy_consumption_factor * Random.Range(0.9F,1.1F);
    energy_growing_factor = tree.energy_growing_factor * Random.Range(0.9F,1.1F);
    energy_growing_effort = tree.energy_growing_effort * Random.Range(0.9F,1.1F);
    width_to_length_ratio = tree.width_to_length_ratio * Random.Range(0.9F,1.1F);
    max_length_increase_ratio = tree.max_length_increase_ratio * Random.Range(0.9F,1.1F);
    max_branch_length = tree.max_branch_length * Random.Range(0.7F,1.3F);
    end_to_start_width_ratio  = tree.end_to_start_width_ratio * Random.Range(0.9F,1.1F);
    child_to_parent_branch_width_ratio  = tree.child_to_parent_branch_width_ratio * Random.Range(0.9F,1.1F);
    my_delta_time = tree.my_delta_time;
    energy_transfer_constant = tree.energy_transfer_constant;
    id = tree.no_branches;
    tree.no_branches +=1 ;
    InvokeRepeating("Main_fun", Random.Range(0.0f,my_delta_time) , my_delta_time);
    original_scale=transform.localScale.y/(sr.sprite.rect.height / sr.sprite.pixelsPerUnit);
    length=1;
        Update_Shape_Drawing();
        sr.enabled=true;
    }

    public Vector3 end_point;
    public Vector3 start_point;
    
    float collected_energy_per_time;
    public float collected_energy_density;
    public float collected_energy;
    public float Collected_Energy{
        get{
            return collected_energy;
        }
        set{
            collected_energy=value;
            collected_energy_density = collected_energy/length_sq;
        }
    }
    public float energy_density;
    public float energy = 0;
    public float Energy{
        get{
            return energy;
        }
        set{
            energy=value;
            energy_density = energy/length_qu;
        }
    }
    public bool has_children = false;
    public bool growth_enabled = true;
    float length_sq = 1;
    float length_qu = 1;
    public  float length = 1;
    public  float Length{
        get{
            return length;
        }
        set{
            length = value;
             length_sq = length*length;
             length_qu = length_sq*length;
             Update_Shape_Drawing();
        }
    }
    // Update is called once per frame
    void Main_fun()
    {
        if(branch_order == 0){
            start_point = tree.transform.position;
        }else{
            start_point = prev_branch.end_point;
        }

        // consumed energy is proportional to the volume
        Energy -= length_qu*energy_consumption_factor*my_delta_time;

        // energy transfer
        /*if (branch_order > 0){
            if(energy<prefered_stored_energy){
                Debug.Log(parent_plant.energy - energy);
                if( parent_plant.energy >energy){
                    float dE = (parent_plant.energy-prefered_stored_energy)*0.1F*my_delta_time ;
                    parent_plant.energy -= dE;
                    energy += dE;
                }
            }
        }*/

        // energy collected to the main branch
        if (branch_order > 0){
            float energy_bundle = Collected_Energy;
            Collected_Energy -= energy_bundle;
            prev_branch.Collected_Energy += energy_bundle;
            collected_energy_per_time = energy_bundle/my_delta_time;
        }else{
            float energy_bundle = Collected_Energy;
            Collected_Energy -= energy_bundle;
            Energy += energy_bundle;
            collected_energy_per_time = energy_bundle/my_delta_time;
        }


        // growing condition
        if(has_children==false){
            float delta_density=energy_density-prefered_energy_density;
            if(delta_density>0){
                // dE : delta energy is proportional to the increased volume
                // dE = l^2 *dl
                float dE = delta_density*energy_growing_factor*my_delta_time ;
                float dl = dE/length_sq/energy_growing_effort;
                if (dl/length>max_length_increase_ratio){
                    dl=length*max_length_increase_ratio;
                    dE=dl*length_sq*energy_growing_effort;
                }
                    Energy -= dE;
                    Length += dl;

            }
            if(Length>max_branch_length){
                has_children=true;
                int no_child = Random.Range(2,3+1);
                    uint[] unsorted_array;    
                    if(no_child==2){
                        Quaternion[] quat = new Quaternion[2];
                        quat[0] =  quaternion.RotateZ(transform.rotation.z+(-45)* Mathf.Deg2Rad*Random.Range(0.7F,1.3F)) ;
                        quat[1] =  quaternion.RotateZ(transform.rotation.z+(45)* Mathf.Deg2Rad*Random.Range(0.7F,1.3F)) ;
                        switch(Random.Range(0,2)){
                            case 0:
                            unsorted_array = new uint[2]{0,1};
                            break;
                            default:
                            unsorted_array = new uint[2]{1,0};
                            break;
                        }
                        if(branch_order>4){
                            Instantiate_Random(quat[unsorted_array[0]]);
                            Instantiate_Random(quat[unsorted_array[1]]);
                        }else{
                            Debug.Log(quat[unsorted_array[0]]);
                            Instantiate_branch(quat[unsorted_array[0]]);
                            Instantiate_Random(quat[unsorted_array[1]]);
                        }
                    }else if(no_child==3){
                        Quaternion[] quat = new Quaternion[3];
                        quat[0] =  quaternion.RotateZ(transform.rotation.z+(-65)* Mathf.Deg2Rad*Random.Range(0.7F,1.3F)) ;
                        quat[1] =  quaternion.RotateZ(transform.rotation.z+(0)* Mathf.Deg2Rad*Random.Range(0.7F,1.3F)) ;
                        quat[2] =  quaternion.RotateZ(transform.rotation.z+(65)* Mathf.Deg2Rad*Random.Range(0.7F,1.3F)) ;
                        switch(Random.Range(0,6)){
                            case 0:
                            unsorted_array = new uint[3]{0,1,2};
                            break;
                            case 1:
                            unsorted_array = new uint[3]{0,2,1};
                            break;
                            case 2:
                            unsorted_array = new uint[3]{1,0,2};
                            break;
                            case 3:
                            unsorted_array = new uint[3]{1,2,0};
                            break;
                            case 4:
                            unsorted_array = new uint[3]{2,0,1};
                            break;
                            default:
                            unsorted_array = new uint[3]{2,1,0};
                            break;
                        }
                        if(branch_order>4){
                            Instantiate_Random(quat[unsorted_array[0]]);
                            Instantiate_Random(quat[unsorted_array[1]]);
                            Instantiate_Random(quat[unsorted_array[2]]);
                        }else{
                            Debug.Log(quat[unsorted_array[0]]);
                            Instantiate_branch(quat[unsorted_array[0]]);
                            Instantiate_Random(quat[unsorted_array[1]]);
                            Instantiate_Random(quat[unsorted_array[2]]);
                        } 
                    }else{
                        Debug.Log("Error Number of children");
                    }

            }
        }else{
            float next_branches_area=0;
            foreach(Branch br in next_branches){
                next_branches_area += br.length_sq;
            }
            if(length_sq<next_branches_area){
                float dA = next_branches_area-length_sq;
                float dl = dA/2/Length;
                float dE = dl*length_sq*energy_growing_effort;
                    Energy -= dE;
                    Length += dl;
            }
            foreach(Branch br in next_branches){
                float delta_density = energy_density - br.energy_density;
                float dE = delta_density*energy_transfer_constant;
                Energy -= dE;
                br.Energy += dE;
            }
        }
        
    }
    void Instantiate_branch(Quaternion q){
        GameObject obj = Instantiate(branchobjPrefab,end_point,q,tree.transform ) as GameObject;
        Debug.Log(obj.name);
        Branch br = obj.GetComponent<Branch>();
        next_branches.Add(br);
        br.prev_branch = this;
        br.branch_order = branch_order + 1;
        br.dir=q*Vector3.up;
        br.start_point = end_point;

    }

    void Instantiate_leaf(Quaternion q){
        GameObject obj = Instantiate(leafobjPrefab,end_point,q,tree.transform ) as GameObject;
        Leaf lf = obj.GetComponent<Leaf>();
        lf.prev_branch = this;
        lf.branch_order = branch_order + 1;
        lf.dir=q*Vector3.up;
        lf.start_point=end_point;

    }

    void Instantiate_Random(quaternion q){
        float branch_probability;
        if (energy_density>prefered_energy_density *5){
            branch_probability=0.9f;
        }else if(energy_density>prefered_energy_density *4){
            branch_probability=0.7f;
        }else if(energy_density>prefered_energy_density *2){
            branch_probability=0.55f;
        }else if(energy_density>prefered_energy_density){
            branch_probability=0.5f;
        }else{
            branch_probability=0.0f;
        }
            float i = Random.Range(0.0f,1.0f);
            if(i<branch_probability)
            Instantiate_branch( q );
            else
            Instantiate_leaf( q );      
    }

    void Update_Shape_Drawing(){      
        end_point = start_point+dir*Length;
        transform.position =  (start_point+end_point) /2;
        transform.localScale = new Vector3(width_to_length_ratio,1,1)*Length*original_scale;
        foreach(Branch br in next_branches){
            br.Update_Shape_Drawing();
        }
    }

}
