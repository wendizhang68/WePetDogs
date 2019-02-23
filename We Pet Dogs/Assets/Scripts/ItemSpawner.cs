using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    #region Public Class Variables (and prefab holders)
    public GameObject[] itemsToSpawn;                        // GameObject prefab holder for items to spawn
    public GameObject[] dogsToSpawn;                         // GameObject prefab holder for dogs to spawn
    public static ItemSpawner instance = null;               //Static instance of ItemSpawner which allows it to be accessed by any other script.
    #endregion

    #region Private Class Variables
    private Vector3 bottomLeftPosition = new Vector3(-42.64f, -13.92f, 663.232f); // Hardcoded bottom left position
    private Vector3 topRightPosition = new Vector3(42.93f, 37.22f, 1156.059f);    // Hardcoded top right position
    private int minItems = 10;      // Minimum number of items to spawn in the world
    private int maxItems = 20;      // Maximum number of items to spawn in the world
    private int minDogs = 15;      // Minimum number of dogs to spawn in the world
    private int maxDogs = 25;      // Maximum number of dogs to spawn in the world
    #endregion

    // Use this for initialization
    void Start () {
        #region Singleton enforced on ItemSpawner instance
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a ItemSpawner.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        #endregion

        #region Spawn Dogs and Items In The World Based Off Private Preset Values (Top Right and Bottom Left corner coordinates)
        int numItemsToSpawn = Random.Range(minItems, maxItems + 1);
        int numDogsToSpawn = Random.Range(minDogs, maxDogs + 1);
        while (numItemsToSpawn > 0)
        {
            //Generate random position in board for item to be placed
            Vector3 positionToPlace = new Vector3(Random.Range(bottomLeftPosition.x, topRightPosition.x), Random.Range(bottomLeftPosition.y, topRightPosition.y), 0f);
            //Pick random item to instantiate
            GameObject toInstantiate = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];
            //Finally intantiate item and reduce num items to spawn
            GameObject instance =
                        Instantiate(toInstantiate, positionToPlace, Quaternion.identity) as GameObject;
            numItemsToSpawn -= 1;
        }
        while (numDogsToSpawn > 0)
        {
            //Generate random position in board for dog to be placed
            Vector3 positionToPlace = new Vector3(Random.Range(bottomLeftPosition.x, topRightPosition.x), Random.Range(bottomLeftPosition.y, topRightPosition.y), 0f);
            //Pick random dog to instantiate
            GameObject toInstantiate = dogsToSpawn[Random.Range(0, dogsToSpawn.Length)];
            //Finally intantiate item and reduce num items to spawn
            GameObject instance =
                        Instantiate(toInstantiate, positionToPlace, Quaternion.identity) as GameObject;
            numDogsToSpawn -= 1;
        }
        #endregion
    }

    // Update is called once per frame
    void Update () {
		
	}
}
