using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Default Enemy stats
    [SerializeField] private float moveSpeed = 15.0f;
    [SerializeField] private float health = 100.0f;

    [SerializeField] private bool ranged = false;
    [SerializeField] private GameObject enemyProjectile;
    [SerializeField] private float projectileFireRate = 0.5f;
    [SerializeField] private float projectileRange = 20f;

    private float rangedFireTime;


    [SerializeField] private float contactDamageToPlayer = 20.0f;
    [SerializeField] private float contactDamageRate = 0.2f;
    [SerializeField] private float contactDamageTime;
    [SerializeField] private float seekDistance = 20f;

    [SerializeField] Rigidbody rb;

    public GameObject deathEffect;

    public GameObject[] itemDrop = new GameObject[4];



    // Update is called once per frame
    void Update()
    {

        if (ranged) {
            if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < projectileRange) {
                RangedAttack();
            }
        }

         Movement();
     
    }

    private void Movement()
    {

        if (GameManager.instance.player)
        { //null reference check
            transform.LookAt(GameManager.instance.player.transform.position); //Look at the player


            if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < seekDistance) {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

            //var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //rb.velocity = dir * moveSpeed;
        }

    }



    private void RangedAttack()
    {
        if (Time.time > rangedFireTime) {
            GameObject projectile = Instantiate(enemyProjectile, transform.position, transform.rotation);
            rangedFireTime = Time.time + projectileFireRate;
            }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            float rand = Random.Range(0.0f,10.0f);
            //GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
            //Destroy(effect, 1.0f);
            Destroy(this.gameObject);

            //GameObject drop = Instantiate(itemDrop, transform.position, Quaternion.identity);
            if(rand >= 2.0f && rand<=4.0f){
                Instantiate(itemDrop[0],transform.position, Quaternion.identity);
            }else if(rand >=5.0f && rand <= 7.0f){
                Instantiate(itemDrop[1],transform.position, Quaternion.identity);
            }else if(rand >= 9.0f){
                Instantiate(itemDrop[2],transform.position, Quaternion.identity);
            }


        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && Time.time > contactDamageTime && other is CapsuleCollider)
        {
            other.transform.GetComponent<PlayerDamage>().TakeDamage(contactDamageToPlayer);
            contactDamageTime = Time.time + contactDamageRate;
        }

    }
}