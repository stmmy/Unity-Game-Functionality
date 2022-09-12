using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_projectile : MonoBehaviour
{
    Vector2 init_pos;
    public float damage = 10f;

    void Start()
    {
        init_pos = transform.position;
        //Physics2D.IgnoreLayerCollision(6, 7, true);
        //Physics2D.IgnoreLayerCollision(6, 6, true);
        //Physics2D.IgnoreLayerCollision(6, 8, true);
    }

    //On collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }


    void Update()
    {
        //If the projectile goes to far, destroy it
        if (Vector2.Distance(transform.position, init_pos) > 9f)
        {
            Destroy(gameObject);
        }
    }
}
