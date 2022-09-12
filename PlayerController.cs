using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement
    Rigidbody2D rb; //Rigidbody of player
    Vector2 dash_vector;
    Vector2 movement;
    public float moveSpeed = 5f;
    float last_dash_time = 0;
    bool is_sprinting = false;
    bool is_dashing = false;

    //input vars
    bool a_check;
    bool d_check;
    bool w_check;
    bool s_check;
    bool space_check;
    bool mouse_check;

    //Player vars
    float health = 100000000;

    //Shooting
    public GameObject bulletPrefab; //Attatch a projectile prefab to this
    float bulletForce = 2f;
    float last_shot = 0f;
    float fire_rate = .2f;

    //Rotation
    public Camera cam; //Attatch player cam to this for mouse pos //Getting rigidbody of player for rotation
    Vector2 look_dir;
    Vector2 mousePos; //For mouse position
    float angle;

    //Animations & states
    Animator animator;
    string currentState;
    const string PLAYER_UP = "up";
    const string PLAYER_LEFT = "left";
    const string PLAYER_RIGHT = "right";
    const string PLAYER_DOWN = "down";
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject incoming_proj = collision.gameObject;
        if (incoming_proj.tag == "projectile")
        {
            enemy_projectile script = incoming_proj.GetComponent<enemy_projectile>();
            float inc_damage = script.damage; //Getting damage from script
            health -= inc_damage;
        }
    }


    void Update()
    {
        //Check If shooting
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) { mouse_check = true; }
        else { mouse_check = false; }

        //Checking if dashing
        space_check = Input.GetKey("space");
        a_check = Input.GetKey("a");
        d_check = Input.GetKey("d");
        w_check = Input.GetKey("w");
        s_check = Input.GetKey("s");

        if (Time.time - last_dash_time > 4f)
        {
            if (a_check == true && space_check == true)
            {
                is_dashing = true;
                dash_vector = new Vector2(-5000f, 0f);
                last_dash_time = Time.time;
            }
            else if (d_check == true && space_check == true)
            {
                is_dashing = true;
                dash_vector = new Vector2(5000f, 0f);
                last_dash_time = Time.time;
            }
            else if (w_check == true && space_check == true)
            {
                is_dashing = true;
                dash_vector = new Vector2(0f, 5000f);
                last_dash_time = Time.time;
            }
            else if (s_check == true && space_check == true)
            {
                is_dashing = true;
                dash_vector = new Vector2(0f, -5000f);
                last_dash_time = Time.time;
            }
        }

        //Check if sprinting
        if (Input.GetKey("left shift") == true) { is_sprinting = true; }
        else { is_sprinting = false; }

        //Checking movement direction
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Check player's health
        if (health <= 0) { Destroy(gameObject); }
    }


    void FixedUpdate()
    {
        //Dashing
        if (is_dashing == true)
        {
            rb.AddForce(dash_vector, ForceMode2D.Force);
            is_dashing = false;
        }

        //Sprinting
        if (is_sprinting == true) { moveSpeed = 8f; }
        else { moveSpeed = 4f; }

        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //Rotation and Shooting
        if (mouse_check == true && Time.time > fire_rate + last_shot)
        {
            last_shot = Time.time;

            //Get angle of mouse pos from character
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //Mouse pos
            look_dir = mousePos - new Vector2(transform.position.x, transform.position.y); //Vector of look direction
            angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg - 90f; //Gets angle of direction based on x-axis

            //Based on angle change player animation
            //if (angle >= -120f && angle < -60f) { ChangeAnimationState(PLAYER_RIGHT); }
            //else if (angle >= -60f && angle < 60f) { ChangeAnimationState(PLAYER_UP); }
            //else if (angle >= 60f || angle < -240f) { ChangeAnimationState(PLAYER_LEFT); }
            //else if (angle >= -240f && angle < -120f) { ChangeAnimationState(PLAYER_DOWN); }
            Shoot();
        }
        else if (mouse_check == false)
        {
            //Based on movement direcion change player animation
            //if (a_check == true) { ChangeAnimationState(PLAYER_LEFT); }
            //else if (d_check == true) { ChangeAnimationState(PLAYER_RIGHT); }
            //else if (w_check == true) { ChangeAnimationState(PLAYER_UP); }
            //else if (s_check == true) { ChangeAnimationState(PLAYER_DOWN); }
        }
    }

    
    //Shooting Function
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, angle));
        Rigidbody2D rb2 = bullet.GetComponent<Rigidbody2D>();
        rb2.AddForce(look_dir* bulletForce, ForceMode2D.Impulse);
    }
}
