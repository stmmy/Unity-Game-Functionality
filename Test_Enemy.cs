using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour
{
    //This script needs to be attatched to a testing enemy


    //Target
    public GameObject target; //***Attatch*** who the target of this enemy is (player)
    Transform target_transform;

    //Shooting vars
    public GameObject projectile; //Projectile prefab
    float projectile_force = 2f;
    float last_shot = 0f;
    float fire_rate = .2f;

    //Wandering vars
    Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.zero};
    Vector2 decision_time = new Vector2(1, 4);
    Vector3 starting_pos;
    float decision_time_count = 0;
    int currentMoveDirection;
    bool is_wandering = false;
    bool is_returning = false;
    float upper_bound;
    float lower_bound;
    float right_bound;
    float left_bound;

    //Enemy vars
    public float health = 100;
    float move_speed = 1f;
    float inc_damage;

    //Rotation & Movement vars
    float angle;
    float step;

    //Animation
    Animator animator;
    string currentState;
    const string RUN_UP = "demon_running_up";
    const string RUN = "demon_running";
    const string IDLE = "demon_idle";

    //Rendering
    SpriteRenderer spriteRenderer;

    void Start() 
    {
        target_transform = target.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //Get the start pos and calc the bounds for wandering
        starting_pos = transform.position;
        upper_bound = starting_pos.y + 3;
        lower_bound = starting_pos.y - 3;
        right_bound = starting_pos.x + 3;
        left_bound = starting_pos.x - 3;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject incoming_proj = collision.gameObject;
        if(incoming_proj.tag == "projectile")
        {
            player_projectile script = incoming_proj.GetComponent<player_projectile>();
            inc_damage = script.damage; //Getting damage from script
            health -= inc_damage;
            StartCoroutine(FlashRed());
        }
    }

    void Update()
    {
        
        //Regulate how fast the enemy shoots
        if (Vector3.Distance(transform.position, target_transform.position) <= 10f && Time.time > fire_rate + last_shot) //If player is within 10 and they can shoot, shoot at player
        {
            last_shot = Time.time;
            Shoot();
        }

        if (health <= 0) { Destroy(gameObject); }
    }

    void FixedUpdate()
    {
        //Decide on movement
        step = move_speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target_transform.position) <= 10f) //Highest prio movement. Move towards target if they are in range
        {
            transform.position = Vector3.MoveTowards(transform.position, target_transform.position, step);
            is_returning = true;
            is_wandering = false;
        }
        else if (transform.position != starting_pos && is_returning == true) //If out of range of bounds or target has gotten away move back to start pos
        {
            angle = Mathf.Atan2(starting_pos.y - transform.position.y, starting_pos.x - transform.position.x) * Mathf.Rad2Deg;

            //Change animation based on angle of enemy to starting pos
            if (angle >= -90f && angle <= 90f) { ChangeAnimationState(RUN_UP); }
            else { ChangeAnimationState(RUN); }

            transform.position = Vector3.MoveTowards(transform.position, starting_pos, step);
            is_wandering = false;
        }
        else //If not attacking someone or retuning, then set wander state to true
        {
            is_wandering = true;
            is_returning = false;
        }

        //Wandering 
        if (is_wandering == true)
        {
            //If object wanders too far, have go back to starting pos
            if (transform.position.x >= right_bound || transform.position.x <= left_bound ||
                transform.position.y >= upper_bound || transform.position.y <= lower_bound) 
            {
                is_returning = true;
                is_wandering = false;
            }

            //Move in direction chosen & rotate
            transform.position += moveDirections[currentMoveDirection] * Time.deltaTime * move_speed;
            
            //Change animation based on current move direction
            if (currentMoveDirection == 2) { ChangeAnimationState(RUN_UP); }
            else if (currentMoveDirection == 4) { ChangeAnimationState(IDLE); }
            else { ChangeAnimationState(RUN); }

            //This part honestly confuses the fuck out of me - got it online but it works 
            if (decision_time_count > 0) decision_time_count -= Time.deltaTime;
            else
            {
                decision_time_count = Random.Range(decision_time.x, decision_time.y);
                currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length)); //Get Direction of wander 
            }
        }
    }

    //Rotate towards enemy and shoot
    void Shoot()
    {
        //Get Angle
        Vector2 look_dir = target_transform.position - transform.position;
        angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;

        //Rotate based on angle
        //Debug.Log(angle);
        if (angle >= 0f && angle <= 180f) { ChangeAnimationState(RUN_UP); }
        else { ChangeAnimationState(RUN); }

        //Shoot out projectile
        GameObject bullet = Instantiate(projectile, gameObject.transform.position, Quaternion.Euler(0, 0, angle+90));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(look_dir * projectile_force, ForceMode2D.Impulse);
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return; //Stop animation from interrupting
        animator.Play(newState);
        currentState = newState;
    }

    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}