using UnityEngine;
using System.Collections;

public class Scott : MonoBehaviour {

    //Character Components
    Animator anim;
    Rigidbody scott;
    bool isKeyDisabled = false;

    //Variables for movement
    [Range(1,20)]
    public float speed;
    public bool run = false;
    public float runSpeed;
    float lastTime = -1.0f;
    KeyCode walkRight = KeyCode.RightArrow;

    //Variables for jumping
    [Range(1, 20)]
    public float jumpVelocity;
    public bool grounded = true;

    //Variables for Attacking
    KeyCode neutralAttack = KeyCode.Z;
    float lastAttack = -1.0f;
    float attackTime = 0.5f;
    int numOfClicks = 0;
    bool attacking = false;

    KeyCode jump = KeyCode.UpArrow;

    //Character Colliders
    public Collider[] attackHitBoxes;

    //Other Colliders and Elements
    public Collider floor;

    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
        scott = GetComponent<Rigidbody>();
	}
	void FixedUpdate()
    {
        if (!isKeyDisabled)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
            scott.AddForce(movement * speed);
        }

    }
    // Update is called once per frame
    void Update () {
        if (!isKeyDisabled)
        {
            Attack();
            Movement();
            Jump();
        }
        Block();
    }

    void Jump()
    {
        if (Input.GetKeyDown(jump) && grounded || Input.GetKeyDown(KeyCode.A) && grounded) 
        {
            anim.SetTrigger("Jump");
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
        }

    }
    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (run && grounded)
        {
            scott.AddForce(movement * speed * runSpeed);
        }
        //If statement determines movement speed
        if (Input.GetKeyDown(walkRight) && grounded)
        {
            //If the arrow key is pressed twice, Run
            if (Time.time - lastTime < 0.2f)
            {
                lastTime = Time.time;
                anim.SetBool("Run", true);
                run = true;
            }
            //If the arrow key is pressed once, Walk
            else
            {
                lastTime = Time.time;
                anim.SetBool("Walk", true);
                scott.AddForce(movement * speed);
            }
        }
        if (Input.GetKeyUp(walkRight))
        {
            run = false;
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }
    void Attack()
    {
        //bool punch = Input.GetKey(KeyCode.Z);
        //bool kick = Input.GetKey(KeyCode.X);
        //bool heavy = Input.GetKey(KeyCode.C);

        // TODO: "X" button will be reserved for special attacks
        // HitBoxes
        /*if (punch)
            LaunchAttack(attackHitBoxes[0]);
        if (kick)
            LaunchAttack(attackHitBoxes[1]);
        if (heavy)
            LaunchAttack(attackHitBoxes[2]);*/

        bool firstAttack = false, secondAttack = false, thirdAttack = false;

        // Second Attack is triggered
        if (Input.GetKeyDown(neutralAttack) && numOfClicks == 1) 
        {

            //secondAttack = true;
            //Debug.Log("Test");

            //firstAttack = true;
            //If the Z key is pressed twice, secondAttack
            if (Time.time - lastAttack < attackTime) 
            {
                lastAttack = Time.time;
                secondAttack = true;
                numOfClicks = 2;
                attacking = true;
            }
            else
            {
                lastAttack = Time.time;
                numOfClicks = 0;
                attacking = false;
            }

        }

        // Third Attack is triggered
        if (Input.GetKeyDown(neutralAttack) && numOfClicks == 2) 
        {
            if (Time.time - lastAttack < attackTime)
            {
                lastAttack = Time.time;
                thirdAttack = true;
                numOfClicks = 0;
                attacking = true;
            }
            else
            {
                lastAttack = Time.time;
                numOfClicks = 0;
                attacking = false;
            }
        }

        // First Attack is triggered
        if(Input.GetKeyDown(neutralAttack) && numOfClicks == 0)
        {
            lastAttack = Time.time;
            firstAttack = true;
            numOfClicks = 1;
            attacking = true;
        }

        if (Time.time - lastAttack > attackTime)
        {
            lastAttack = Time.time;
            attacking = false;
        }

        // Attack Button is released
        if (Input.GetKeyUp(neutralAttack)) 
        {
            firstAttack = false;
            secondAttack = false;
            thirdAttack = false;
        }

        // Attack Animations
        anim.SetBool("Punch", firstAttack);
        anim.SetBool("Kick", secondAttack);
        anim.SetBool("Heavy", thirdAttack);

    }
    void AnimationManager(string animation)
    {
        // TODO: Make serparate function for Animations and Physics calculations (Look into Finite State Machines)
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            grounded = false;
        }
    }

    private void LaunchAttack(Collider col) {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (c.transform.parent.parent == transform)
                continue;
            else
            {
                anim.SetBool("LightHit", true);
            }
            Debug.Log(c.name);
        }
    }

    // Disable other attacks on block
    void Block()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && !run && !attacking) 
        {
            anim.SetBool("Block", true);
            anim.speed = 0.0f;
            isKeyDisabled = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("Block", false);
            anim.speed = 1f;
            isKeyDisabled = false;
        }
    }

}
