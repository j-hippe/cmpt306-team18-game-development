using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour

{

    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float maxShield = 100.0f;
    [SerializeField] private float shield = 0.0f;
    public GameObject deathEffect;
    public Image healthbar;
    public Image shieldbar;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(float damage)
    {
        
        if (shield >0.0f){
            if(damage > shield){
                shield = 0.0f;
                shieldbar.fillAmount = 0.0f;
            }else{
                shield -= damage;
                shieldbar.fillAmount = Mathf.Clamp(shield/maxShield, 0.0f,1.0f);
            }
        }else{
            health -= damage;
            healthbar.fillAmount = Mathf.Clamp(health/maxHealth,0.0f,1.0f);
            if (health <= 0.0f)
            {
                //GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
                //Destroy(effect, 1.0f);
                
                Destroy(this.gameObject);
            }
        }
        
    }
    public void shieldFill(){
        shield = maxShield;
        shieldbar.fillAmount = Mathf.Clamp(shield/maxShield, 0.0f,1.0f);
    }

    public void RefillHealth(float hp){
        this.health += hp;
        healthbar.fillAmount = Mathf.Clamp(health/maxHealth,0.0f,1.0f);
    }
}


