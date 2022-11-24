using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.0f;
    [SerializeField] private float moveSpeed = 100.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBasicProjectile();
    }

    private void MoveBasicProjectile()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            //Debug.Log(GameManager.instance.player.transform.GetChild(2).gameObject.GetComponent<PlayerAbilities>().GetDamage());
            other.GetComponent<EnemyController>().TakeDamage(GameManager.instance.player.transform.GetChild(2).gameObject.GetComponent<PlayerAbilities>().GetDamage());
            Destroy(this.gameObject);
        }
    }

    
}
