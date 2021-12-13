using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacketEnemyCollision : MonoBehaviour
{
    public Material enemyPatrolMat;
    MeshRenderer hitmanRend;
    Color orange;
    // Start is called before the first frame update
    void Start()
    {
        hitmanRend = GameObject.Find("Hitman").GetComponent<MeshRenderer>();
        ColorUtility.TryParseHtmlString("#FF7600", out orange);
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
            Material[] materials = hitmanRend.materials;
            materials[0] = enemyPatrolMat;
            materials[0].color = orange;
            hitmanRend.materials = materials;
            materials[4] = enemyPatrolMat;
            materials[4].color = orange;
            hitmanRend.materials = materials;
        }
    }
}
