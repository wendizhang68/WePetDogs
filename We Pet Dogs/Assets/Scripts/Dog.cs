using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{

    #region Public Variables (and prefab holders)
    public string nameInHierarchy;
    public string typeOfDog;
    public Vector3 position;
    public int prefabListIndex;
    public bool hasBeenInteractedWith;
    #endregion

    #region movement
    public float speed = 1.0f;
    Vector3 zeroVec = new Vector3(0, 0, 0);
    private float randDist = 5;
    #endregion

    #region Private Variables
    private Rigidbody2D dogBody;
    private SpriteRenderer spriteRenderer;
    private Animator dogAnim;
    private GameObject dog;
    #endregion

    #region Dog Class Constructor
    public Dog(string name, string dogType, Vector3 xyz, int index)
    {
        nameInHierarchy = name;
        typeOfDog = dogType;
        position = xyz;
        prefabListIndex = index;
        hasBeenInteractedWith = false;
    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        dog = this.gameObject;
        dogBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        dogAnim = gameObject.GetComponent<Animator>();
        StartCoroutine(Walk_Stop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Movement handling
    IEnumerator Walk()
    {
        dog = this.gameObject;
        Vector3 dogPosition = dog.transform.position;
        Vector3 circle = getRandomCircle();
        Vector3 walkTo = dogPosition + circle;
        Vector3 direction = Vector3.Normalize(circle);
        Animate(direction, true);
        //yield return MoveToPosition(dogPosition, walkTo, dog);
        yield return MoveToPosition1(direction, walkTo);
    }

    void Animate(Vector3 direction, bool walking)
    {
        if (walking)
        {
            dogAnim.SetBool("walking", false);
            //Debug.Log("Animate: " + dogAnim.GetBool("walking").ToString());
            dogAnim.SetBool("walking", true);
            //Debug.Log("Animate: " + dogAnim.GetBool("walking").ToString());
            dogAnim.SetFloat("xVel", direction.x);
            dogAnim.SetFloat("yVel", direction.y);
        }
        else
        {
            dogAnim.SetBool("walking", walking);
        }
    }

    public IEnumerator Walk_Stop()
    {
        while (true)
        {
            yield return Walk();
            Animate(zeroVec, false);
            yield return Wait(Random.Range(2, 5));
        }
    }

    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    IEnumerator MoveToPosition1(Vector3 direction, Vector3 newPos)
    {
        //while (dog.transform.position != newPos)
        //{
        dogBody.velocity = Vector3.Normalize(direction);
        yield return Wait(Random.Range(2, 5));
        dogBody.velocity = zeroVec;
        //}

    }

    Vector3 getRandomCircle()
    {
        float t = 2.0f * Mathf.PI * Random.Range(0.0f, 1.0f);
        //Debug.Log("t = " + t.ToString());
        float r = Random.Range(1, randDist);
        Vector3 circle = new Vector3(r * Mathf.Cos(t), r * Mathf.Sin(t), 0);
        return circle;
    }

    // commented bc it uses transform instead of rigidbody to move the dogs
    //IEnumerator MoveToPosition(Vector3 oldPos, Vector3 newPos, GameObject dog)
    //{
    //    //Debug.Log("Moving");
    //    float journeyLength = Vector3.Distance(oldPos, newPos);
    //    float startTime = Time.time;
    //    Vector3 currPos = oldPos;
    //    float distCovered;
    //    float fracJourney;
    //    while (currPos != newPos)
    //    {
    //        distCovered = (Time.time - startTime) * speed;
    //        fracJourney = distCovered / journeyLength;
    //        currPos = Vector3.Lerp(oldPos, newPos, fracJourney);
    //        dog.transform.position = currPos;
    //        yield return null;
    //    }
    //}
    #endregion

    public Dog copyOfThis()
    {
        return new Dog(nameInHierarchy, typeOfDog, position, prefabListIndex);
    }

    private void OnEnable()
    {
        StartCoroutine(Walk_Stop());
    }
}
