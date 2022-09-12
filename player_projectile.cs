using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_projectile : MonoBehaviour
{
    //This script needs to be attatched to a projectile prefab


    Vector2 init_pos;
    public float damage = 10f;

    void Start()
    {
        init_pos = transform.position;
        //Physics2D.IgnoreLayerCollision(7, 7, true);
        //Physics2D.IgnoreLayerCollision(7, 6, true);
        //Physics2D.IgnoreLayerCollision(7, 9, true);
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
