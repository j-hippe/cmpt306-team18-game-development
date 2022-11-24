using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAbilities : MonoBehaviour
{

    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private float basicFireRate = 0.5f;
    [SerializeField] private float fireTime;
    [SerializeField] private bool allowFire;
    [SerializeField] public float damage = 30.0f;
    private float remainingBuffTime = 0.0f;
    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
    [SerializeField] GameObject rotationTarget;


    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        BasicAttack();
        LookAtCamera();
        if(remainingBuffTime > 0){
            remainingBuffTime -= Time.deltaTime;
        }else{
            this.damage = 30.0f;
        }
    }
    public void damageBuff(){
        this.damage = 60.0f;
        this.remainingBuffTime = 10.0f;
    }

    private void BasicAttack()
    {


        if (Input.GetMouseButton(0) && Time.time > fireTime)
        {
            Instantiate(basicProjectile, transform.position, transform.rotation);
            fireTime = Time.time + basicFireRate;
        }
    }
    public float GetDamage(){
        return this.damage;
    }

    private void LookAtCamera()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        object_pos = Camera.main.WorldToScreenPoint(rotationTarget.transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        rotationTarget.transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
    }

}