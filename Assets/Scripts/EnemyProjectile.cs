using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float lifeTime = 2.0f;
    [SerializeField] private float moveSpeed = 100.0f;
    [SerializeField] private float projectileDamage = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemyProjectile();
    }

    private void MoveEnemyProjectile()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && other is CapsuleCollider)
        {
            other.transform.GetComponent<PlayerDamage>().TakeDamage(projectileDamage);
            Destroy(this.gameObject);

        }

        


}
}
