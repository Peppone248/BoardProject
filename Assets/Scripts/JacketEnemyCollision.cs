using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacketEnemyCollision : MonoBehaviour
{

    public Material enemyPatrolMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hitman")
        {
            Destroy(gameObject);
            GameObject.Find("Hitman").GetComponent<Renderer>().material = enemyPatrolMat;
        }
    }
}
