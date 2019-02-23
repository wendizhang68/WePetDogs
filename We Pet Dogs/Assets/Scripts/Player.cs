using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

public class Player : MonoBehaviour
{
    
    #region global variables
    public GameObject GM;
    public GameManager gameManager;
    #endregion

    #region Player components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator anim;
    private Color playerColor;
    #endregion

    #region Player movement variables
    private Vector3 current;
    private int speed = 3;
    private int runspeed = 5;
    private Vector3 zeroVec = new Vector3(0, 0, 0);
    #endregion

    #region Player pickups/mechanics
    private bool statsUpdate = false;
    public Text statsTextObject;
    public Slider anxietyBar;
    public Text youLose;
    #endregion

    void Start()
    {
        #region Reset player stats if transitioning from underworld to those saved in GameManager
        GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        gameManager = GM.GetComponent<GameManager>();           // Get game manager script from gameobject
        //OM = GameObject.Find("OptionManager(Clone)");
        //optionManager = OM.GetComponent<OptionManager>();
        if (gameManager.toOverworld)
        {
            //Find player and set position via saved position in game manager
            GameObject player = this.gameObject;
            player.GetComponent<Transform>().position = gameManager.savedPlayerPosition;
        }
        #endregion
        current = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerColor = spriteRenderer.color;
    }

    void Update()
    {
        Move();
    }

    #region Player movement scripts
    public void Move()
    {
        Vector3 movement = Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0));
        
        if (movement != zeroVec)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                movement = movement * runspeed;
            } else {
                movement = movement * speed;
            }
            rb.velocity = movement;
            Walk(movement);
        }
        else
        {
            rb.velocity = zeroVec;
            StopWalk();
        }
    }

    public void Walk(Vector3 movement)
    {
        movement = Vector3.Normalize(movement);
        float xDir = movement.x;
        float yDir = movement.y;
        if (movement == zeroVec)
        {
            StopWalk();
        }
        anim.SetBool("walking", true);
        anim.SetFloat("xVel", xDir);
        anim.SetFloat("yVel", yDir);
    }

    public void StopWalk()
    {
        anim.SetBool("walking", false);
    }
    #endregion

    #region Collision handling
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // If collision is a dog, call GameManager script for transitioning to underworld with dog's position input
        if (collision.gameObject.tag == "Dog")
        {
            Debug.Log("ENTER");
            Vector3 dogPosition = collision.transform.position; // Get the collision's (dog's) position
            Dog dogScript = collision.gameObject.GetComponent<Dog>();
            if (!dogScript.hasBeenInteractedWith)
            {
                dogScript.hasBeenInteractedWith = true;
                gameManager.playerDogCounter -= 1;
            }
            if (GM != null) {
                gameManager.goToUnderworld(dogPosition);
            }
        }
        else if (collision.gameObject.tag == "Shop")
        {
            Debug.Log("Found shop");
            SceneManager.LoadScene(3);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Toy")
        {
             gameManager.playerToys += 1;
            collision.gameObject.SetActive(false);
            Item itemScript = collision.gameObject.GetComponent<Item>();
            itemScript.pickedUp = true;
        }
        if (collision.gameObject.tag == "Treat")
        {
            gameManager.playerTreats += 1;
            collision.gameObject.SetActive(false);
            Item itemScript = collision.gameObject.GetComponent<Item>();
            itemScript.pickedUp = true;
        }
        //if (collision.gameObject.name == "Shadow")
        //{
        //    //Debug.Log("SHADOW");
        //    Color c = spriteRenderer.color;
        //    spriteRenderer.color = new Color(c.r/2, c.g/2, c.b/2);
        //}
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        //if (collision.gameObject.name == "Shadow")
        //{
        //    spriteRenderer.color = playerColor;
        //}
    }
    #endregion

}