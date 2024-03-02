using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    SpriteRenderer sr;
    public Vector3 dir ;
    public float length;
    public uint id = 0;
    public uint branch_order = 0;
    public float original_scale;
    float leaf_collected_energy_per_time;
    float max_leaf_length;
    static Tree tree;
    static float my_delta_time;
    public Branch prev_branch;
    static bool first_time =true;
    static float intelligent;
    static float max_branch_length;
    static float falling_speed;
    static float transp_speed;
    // Start is called before the first frame update
    void Start()
    {

        if(first_time==true){
            first_time=false;
        tree=gameObject.GetComponentInParent<Tree>();
        my_delta_time=tree.my_delta_time; 
        intelligent=tree.intelligent;
        max_branch_length=tree.max_branch_length;
        falling_speed=tree.falling_speed;
        transp_speed=tree.transp_speed;
        }
        id = tree.no_leaves;
        tree.no_leaves +=1 ;
        sr=GetComponent<SpriteRenderer>();
        leaf_collected_energy_per_time=tree.leaf_collected_energy_per_time* Random.Range(0.7F,1.3F);
        
        max_leaf_length = tree.max_leaf_length* Random.Range(0.5F,1.5F);

    InvokeRepeating("Main_fun", Random.Range(0.0f,my_delta_time) , my_delta_time);
    original_scale=transform.localScale.y/(sr.sprite.rect.height / sr.sprite.pixelsPerUnit);
    length=0.5f;
        Update_Shape_Drawing();
        sr.enabled=true;
    }
    public Vector3 end_point;
    public Vector3 start_point;

    void Main_fun(){

        if(length<max_leaf_length){
            length += 0.2f;
        }else if(prev_branch.length>max_branch_length*2){
            start_point+=Vector3.down*falling_speed*my_delta_time;
            sr.color -= new Color(1f,1f,1f,0.8f)*transp_speed*my_delta_time;
        }else{
            start_point = prev_branch.end_point;
            prev_branch.Collected_Energy += leaf_collected_energy_per_time*my_delta_time*(1-intelligent) ;
            prev_branch.Energy += leaf_collected_energy_per_time*my_delta_time*intelligent ;
        }
        Update_Shape_Drawing(); 

    }
        public void Update_Shape_Drawing(){      
        end_point = start_point+dir*length;
        transform.position =  (start_point+end_point) /2;
        transform.localScale = new Vector3(1,1,1)*length*original_scale;
    }
}
