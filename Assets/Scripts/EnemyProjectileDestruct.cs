using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileDestruct : MonoBehaviour
{

    [SerializeField] private float lifeTime = 2.0f;




    // Start is called before the first frame update
    void Start()
    {
        SelfDestruct();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);              // wait for lifeTime seconds, self Destruct
        Destroy(gameObject);
    }

}
