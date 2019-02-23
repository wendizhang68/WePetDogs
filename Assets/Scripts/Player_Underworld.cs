using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Underworld : MonoBehaviour {

    #region global variables
    public GameObject GM;
    public GameManager gameManager;
    #endregion

    #region Player movement variables
    private Vector2 current;
    private int speed = 3;
    private Vector2 zeroVec = new Vector2(0, 0);
    private bool immovable = false;
    #endregion

    #region Player components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;
    #endregion

    #region Petting variables
    private float petPoints;
    private float wrongMove;
    private float timePetting;
    private bool petting;
    private bool crouching;
    public bool dog;
    #endregion

    // Use this for initialization
    void Start ()
    {
        GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        gameManager = GM.GetComponent<GameManager>();           // Get game manager script from gameobject
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        petPoints = -1;
        wrongMove = 10;
	}
	
	// Update is called once per frame
	void Update () {
        StopWalk();
        if (!immovable)
            Move();
        checkPetting();
        checkCrouching();
        checkIdle();
	}

    #region Player movement scripts
    public void Move()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        Vector2 movement = (new Vector2(xDir, yDir)).normalized;
        current = movement;

        if (movement != zeroVec)
        {
            rb.velocity = movement * speed;
            Walk(movement);
        }
        else
        {
            StopWalk();
        }
    }

    public void Walk(Vector2 movement)
    {
        float xDir = movement.x;
        float yDir = movement.y;
        if (movement == zeroVec)
        {
            StopWalk();
        }
        anim.SetBool("walking", true);
        if (Mathf.Abs(xDir) > 0)
        {
            anim.SetFloat("xVel", xDir);
        }
        anim.SetFloat("yVel", yDir);
    }

    public void StopWalk()
    {
        //Debug.Log("Stop!");
        //foreach (string d in directions) {
        //    anim.SetBool(d + "_walk", false);
        //}
        rb.velocity = zeroVec;
        anim.SetBool("walking", false);
    }

    void checkIdle() {
        bool isIdle = anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        if (isIdle)
        {
            immovable = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    #region Petting handling
    public void Pet()
    {
        petting = true;
    }

    public void checkPetting() {
        if (petting && Input.GetKey(KeyCode.Return))
        {
            timePetting += Time.deltaTime;
            immovable = true;
            transform.GetChild(0).gameObject.SetActive(true);
            anim.SetBool("pet", true);
            if (dog && timePetting > 2f) {
                gameManager.CalculateAnxiety(petPoints);
            }

        } else if (Input.GetKeyUp(KeyCode.Return)) {
            anim.SetBool("pet", false);
            petting = false;
            dog = false;
            timePetting = 0;
        }
    }
    #endregion

    public void Throw() {
        anim.SetTrigger("throw");
        gameManager.CalculateAnxiety(wrongMove);
    }

    public void Treat() {
        Debug.Log("Crouching!");
        crouching = true;
    }

    public void checkCrouching() {
        if (crouching && Input.GetKey(KeyCode.Return)) {
            anim.SetBool("crouching", true);
            immovable = true;
        } else if (Input.GetKeyUp(KeyCode.Return)) {
            anim.SetBool("crouching", false);
            petting = false;
            dog = false;
            timePetting = 0;
        }
    }

    public void BackOff() {

    }

    #endregion
}
