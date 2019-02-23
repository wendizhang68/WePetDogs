using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogWalker : MonoBehaviour {

    //public GameObject corgi;
    public float speed = 1.0f;
    public GameObject[] dogArray;
    Vector3 zeroVec = new Vector3(0, 0, 0);
    private float randDist = 5;

	// Use this for initialization
	void Start () {
        //dogArray = GameObject.FindGameObjectsWithTag("Dog");
        //foreach (GameObject dog in dogArray)
        //{
        //    //StartCoroutine(Walk_Stop(dog));
        //    Dog dogScript = dog.GetComponent<Dog>();
        //    StartCoroutine(dogScript.Walk_Stop());
        //}
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    IEnumerator Walk (GameObject dog) {
        Vector3 dogPosition = dog.transform.position;
        Vector3 circle = getRandomCircle();
        Vector3 walkTo = dogPosition + circle;
        Vector3 direction = Vector3.Normalize(circle);
        Animate(dog, direction, true);
        //yield return MoveToPosition(dogPosition, walkTo, dog);
        yield return MoveToPosition1(direction, walkTo, dog);
    }

    void Animate(GameObject dog, Vector3 direction, bool walking)
    {
        Animator dogAnim = dog.GetComponent<Animator>();
        if (walking)
        {
            dogAnim.SetBool("walking", false);
            //Debug.Log("Animate: " + dogAnim.GetBool("walking").ToString());
            dogAnim.SetBool("walking", true);
            //Debug.Log("Animate: " + dogAnim.GetBool("walking").ToString());
            dogAnim.SetFloat("xVel", direction.x);
            dogAnim.SetFloat("yVel", direction.y);
        } else{
            dogAnim.SetBool("walking", walking);
        }
    }

    IEnumerator Walk_Stop(GameObject dog) {
        while (true)
        {
            yield return Walk(dog);
            Animate(dog, zeroVec, false);
            yield return Wait(Random.Range(2, 5));
        }
    }

    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
    }

    IEnumerator MoveToPosition1(Vector3 direction, Vector3 newPos, GameObject dog) {
        //while (dog.transform.position != newPos)
        //{
        Rigidbody2D dogBody = dog.GetComponent<Rigidbody2D>();
        dogBody.velocity = Vector3.Normalize(direction);
        yield return Wait(Random.Range(2, 5));
        dogBody.velocity = zeroVec;
        //}

    }

    IEnumerator MoveToPosition(Vector3 oldPos, Vector3 newPos, GameObject dog) {
        //Debug.Log("Moving");
        float journeyLength = Vector3.Distance(oldPos, newPos);
        float startTime = Time.time;
        Vector3 currPos = oldPos;
        float distCovered;
        float fracJourney;
        while (currPos != newPos) {
            distCovered = (Time.time - startTime)*speed;
            fracJourney = distCovered / journeyLength;
            currPos = Vector3.Lerp(oldPos, newPos, fracJourney);
            dog.transform.position = currPos;
            yield return null;
        }
    }

    Vector3 getRandomCircle() {
        float t = 2.0f * Mathf.PI * Random.Range(0.0f, 1.0f);
        //Debug.Log("t = " + t.ToString());
        float r = Random.Range(1, randDist);
        Vector3 circle = new Vector3(r * Mathf.Cos(t), r * Mathf.Sin(t), 0);
        return circle;
    }
}
