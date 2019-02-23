using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogAndItemSpawner : MonoBehaviour {

    #region Public Class Variables (and prefab holders)
    public GameObject[] itemsToSpawn;                        // GameObject prefab holder for items to spawn
    public GameObject[] dogsToSpawn;                         // GameObject prefab holder for dogs to spawn
    public static DogAndItemSpawner instance = null;         // Static instance of DogAndItemSpawner which allows it to be accessed by any other script.
    public GameObject[] dogList;                             // List of all the dogs that have spawned, manageable via their Dog Class
    public GameObject[] itemList;                           // List of all the items that have spawned, manageable via their Item Class
    public GameObject objectDogsList;                             // List to gameobject holding all the spawned dogs
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
    void Start()
    {
        #region Singleton enforced on DogAndItemSpawner instance
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a DogAndItemSpawner.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        #endregion

        objectDogsList = GameObject.FindGameObjectWithTag("All Dogs");            // Set this reference to dogslist parent gameobject in hierarchy
        GameObject GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        GameManager gameManager = GM.GetComponent<GameManager>();         // Get game manager script from gameobject
        // If coming from underworld, spawn via saved positions
        if (gameManager.toOverworld)
        {
            #region (CURRENTLY NOT IMPLEMENTED) Respawn dogs and items (if not picked up) in correct positions when returning to overworld
            #endregion
        }
        // Otherwise randomly spawn items and dogs since first time entering world
        else
        {
            #region Spawn Dogs and Items In The World Based Off Private Preset Values (Top Right and Bottom Left corner coordinates)
            // Generate the random number of items and dogs to spawn, as well as instantiate the lists for each to track/manage
            int numItemsToSpawn = Random.Range(minItems, maxItems + 1);
            itemList = new GameObject[numItemsToSpawn];
            int numDogsToSpawn = Random.Range(minDogs, maxDogs + 1);
            dogList = new GameObject[numDogsToSpawn];
            // List index variable for moving through dogList and itemList
            int currListIndex = 0;
            while (numItemsToSpawn > 0)
            {
                //Generate random position in board for item to be placed
                Vector3 positionToPlace = new Vector3(Random.Range(bottomLeftPosition.x, topRightPosition.x), Random.Range(bottomLeftPosition.y, topRightPosition.y), 0f);
                //Pick random item to instantiate
                Debug.Log(itemsToSpawn.Length);
                int indexToSpawn = Random.Range(0, itemsToSpawn.Length);
                GameObject toInstantiate = itemsToSpawn[indexToSpawn];
                //Finally intantiate item and reduce num items to spawn
                GameObject createdInstance =
                            Instantiate(toInstantiate, positionToPlace, Quaternion.identity) as GameObject;
                numItemsToSpawn -= 1;
                // Attach Item class script to newly created item gameobject and fill with relative values, then add to itemList
                createdInstance.AddComponent(typeof(Item));
                Item itemScript = createdInstance.GetComponent(typeof(Item)) as Item;
                itemScript.nameInHierarchy = createdInstance.name;
                itemScript.typeOfItem = toInstantiate.name;
                itemScript.position = positionToPlace;
                itemScript.prefabListIndex = indexToSpawn;
                itemList[currListIndex] = createdInstance;
                currListIndex += 1;
            }
            // Reset list index variable for next list to move through
            currListIndex = 0;
            while (numDogsToSpawn > 0)
            {
                //Generate random position in board for dog to be placed
                Vector3 positionToPlace = new Vector3(Random.Range(bottomLeftPosition.x, topRightPosition.x), Random.Range(bottomLeftPosition.y, topRightPosition.y), 0f);
                //Pick random dog to instantiate
                int indexToSpawn = Random.Range(0, dogsToSpawn.Length);
                GameObject toInstantiate = dogsToSpawn[indexToSpawn];
                //Finally intantiate item and reduce num dogs to spawn
                GameObject createdInstance =
                            Instantiate(toInstantiate, positionToPlace, Quaternion.identity) as GameObject;
                createdInstance.name = createdInstance.name + currListIndex;
                numDogsToSpawn -= 1;
                // Set parent to dogsList gameobject via reference
                createdInstance.transform.parent = objectDogsList.transform;
                // Attach Dog class script to newly created dog gameobject and fill with relative values, then add to dogList
                createdInstance.AddComponent(typeof(Dog));
                Dog dogScript = createdInstance.GetComponent(typeof(Dog)) as Dog;
                dogScript.nameInHierarchy = createdInstance.name;
                dogScript.typeOfDog = toInstantiate.name;
                dogScript.position = positionToPlace;
                dogScript.prefabListIndex = indexToSpawn;
                dogList[currListIndex] = createdInstance;
                currListIndex += 1;
                Debug.Log("Dog " + currListIndex + " reference: " + dogScript.nameInHierarchy + " name: " + dogScript.typeOfDog);
            }
            gameManager.playerDogCounter = dogList.Length;                    // Set dog counter once dogs have been instantiated
            #endregion
        }
    }

    public void deactivateAllDogs()
    {
        Debug.Log("Ran deactivateAllDogs().");
        GameObject GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        GameManager gameManager = GM.GetComponent<GameManager>();         // Get game manager script from gameobject
        gameManager.savedDogScripts = new Dog[dogList.Length];
        for (int i = 0; i < dogList.Length; i++)
        {
            GameObject currDog = dogList[i];
            currDog.SetActive(false);
        }
        for (int j = 0; j < itemList.Length; j++)
        {
            GameObject currItem = itemList[j];
            currItem.SetActive(false);
        }
    }

    public void activateAllDogs()
    {
        Debug.Log("Ran activateAllDogs().");
        GameObject GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        GameManager gameManager = GM.GetComponent<GameManager>();         // Get game manager script from gameobject
        for (int i = 0; i < dogList.Length; i++)
        {
            GameObject currDog = dogList[i];
            currDog.SetActive(true);
        }
        for (int j = 0; j < itemList.Length; j++)
        {
            GameObject currItem = itemList[j];
            if (!currItem.GetComponent<Item>().pickedUp)
            {
                currItem.SetActive(true);
            }
        }
    }
}
