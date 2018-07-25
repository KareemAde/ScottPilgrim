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
    public bool run = false;
    public float runSpeed;

    //Variables for jumping
    [Range(1, 20)]
    public float jumpVelocity;
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
    // Update is called once per physics update
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

    //Function handles Walking, Running, and Jumping
    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (run && grounded)
        {
            scott.AddForce(movement * speed * runSpeed);
            anim.SetBool("Run", true);
        }
        //If statement determines movement speed
        if (Input.GetKeyDown(walkRight) && grounded)
        {
            //If the arrow key is pressed twice, Run
            if (Time.time - lastTime < 0.2f)
            {
                lastTime = Time.time;
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

        if (Input.GetKeyDown(jump) && grounded) 
        {
            anim.SetTrigger("Jump");
        }

    }

    //Function handles Attack Animations
    void Attack()
    {
        bool punch = Input.GetKey(KeyCode.Z);

        anim.SetBool("Punch", punch);
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

}
