using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dogu : MonoBehaviour
{

    //public GameObject Player;
    //public Player_Underworld playerScript;
    private Rigidbody2D dogBody;
    private SpriteRenderer spriteRenderer;
    private Animator dogAnim;
    private GameObject dog;

    #region movement
    public float speed = 1.0f;
    Vector3 zeroVec = new Vector3(0, 0, 0);
    private float randDist = 5;
    #endregion

    #region AI... kinda
    private float affection;
    private bool petting;
    private bool idling;
    private float idleTime;
    #endregion

    // Use this for initialization
    void Start()
    {
        dog = this.gameObject;
        dogBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        dogAnim = gameObject.GetComponent<Animator>();
        idling = true;
        affection = 100;
    }

    // Update is called once per frame
    void Update()
    {
        idleTime += Time.deltaTime;
        //Debug.Log(idleTime);
        if (idling)
        {
            StartCoroutine(Walk_Stop());
        }
        if (petting) {
            petting = false;
        }
        //Debug.Log(dogAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
    }

    #region Walking
    IEnumerator Walk_Stop()
    {
        idling = false;
        while (true)
        {
            yield return Walk();
            Animate(zeroVec, false);
            yield return Wait(Random.Range(2, 5));
        }
    }

    void Stop() {
        StopAllCoroutines();
    }

    IEnumerator Walk()
    {
        idling = false;
        Vector3 dogPosition = dog.transform.position;
        Vector3 circle = getRandomCircle();
        Vector3 walkTo = dogPosition + circle;
        Vector3 direction = Vector3.Normalize(circle);
        Animate(direction, true);
        yield return MoveToPosition(dogPosition, walkTo);
    }

    void Animate(Vector3 direction, bool walking)
    {
        if (walking)
        {
            dogAnim.SetBool("pet", false);
            dogAnim.SetBool("walking", walking);
            dogAnim.SetFloat("xVel", direction.x);
            dogAnim.SetFloat("yVel", direction.y);
        }
        else
        {
            dogAnim.SetBool("pet", false);
            dogAnim.SetBool("walking", walking);
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator MoveToPosition(Vector3 oldPos, Vector3 newPos)
    {
        //Debug.Log("Moving");
        float journeyLength = Vector3.Distance(oldPos, newPos);
        float startTime = Time.time;
        Vector3 currPos = oldPos;
        float distCovered;
        float fracJourney;
        while (currPos != newPos)
        {
            distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            currPos = Vector3.Lerp(oldPos, newPos, fracJourney);
            dog.transform.position = currPos;
            yield return null;
        }
    }

    Vector3 getRandomCircle()
    {
        float t = 2.0f * Mathf.PI * Random.Range(0.0f, 1.0f);
        //Debug.Log("t = " + t.ToString());
        float r = Random.Range(1, randDist);
        Vector3 circle = new Vector3(r * Mathf.Cos(t), r * Mathf.Sin(t), 0);
        return circle;
    }
    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        string collname = collision.gameObject.name;
        if (collname.Contains("Petting Collider")) {
            petting = true;
            dogAnim.SetBool("pet", true);
            Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string collname = collision.gameObject.name;
        if (collname.Contains("Petting Collider"))
        {
            dogAnim.SetBool("pet", false);
            StartCoroutine(EndPet(2f));
        }
    }

    IEnumerator EndPet(float time)
    {
        yield return Wait(time);
        idling = true;
        yield return null;
    }
}
