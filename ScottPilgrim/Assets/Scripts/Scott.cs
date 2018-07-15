using UnityEngine;
using System.Collections;

public class Scott : MonoBehaviour {

    //Character Components
    Animator anim;
    Rigidbody scott;

    //Variables for movement
    [Range(1,20)]
    public float speed;
    KeyCode walkRight = KeyCode.RightArrow;

    //Variables for jumping
    [Range(1, 20)]
    public float jumpVelocity;
    public bool run = false;
    public float runSpeed;
    float lastTime = -1.0f;
    public bool grounded = true;
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
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        scott.AddForce(movement * speed);

    }
    // Update is called once per frame
    void Update () {

        Attack();
        Movement();
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
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
                //scott.AddForce(movement * speed * 2);
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

        if (Input.GetKeyDown(jump) && grounded) 
        {
            anim.SetTrigger("Jump");
        }

    }
    void Attack()
    {
        bool punch = Input.GetKey(KeyCode.Z);
        bool kick = Input.GetKey(KeyCode.X);
        bool heavy = Input.GetKey(KeyCode.C);

        if (punch)
            LaunchAttack(attackHitBoxes[0]);
        if (kick)
            LaunchAttack(attackHitBoxes[1]);
        if (heavy)
            LaunchAttack(attackHitBoxes[2]);

        anim.SetBool("Punch", punch);
        anim.SetBool("Kick", kick);
        anim.SetBool("Heavy", heavy);


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

}
