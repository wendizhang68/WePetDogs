using UnityEngine;

using System.Collections.Generic;       // Allows us to use Lists. 

public class OverworldManager : MonoBehaviour
{
    private List<GameObject> movingObjects;
    private List<GameObject> dogs;
    private GameObject player;

    // Use this for initialization
    void Awake()
    {
        InitializeObjects();
    }

    // Update is called once per frame
    void Update()
    {
        SetSortingOrder(movingObjects);
    }

    private void SetSortingOrder(List<GameObject> objects)
    {
        foreach (GameObject g in objects)
        {
            try
            {
                SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
                Vector2 pos = g.transform.position;
                sr.sortingOrder = (int)(pos.y * -100);
            }
            catch (MissingComponentException m)
            {
                continue;
            }
        }
    }

    public void InitializeObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dogs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Dog"));
        //movingObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        //movingObjects.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Dog")));
        movingObjects = new List<GameObject>(dogs);
        movingObjects.Add(player);
        SetSortingOrder(new List<GameObject>((GameObject[])FindObjectsOfType(typeof(GameObject))));
    }
}
