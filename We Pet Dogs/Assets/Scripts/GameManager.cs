using UnityEngine;
using System.Collections;


using System.Collections.Generic;       // Allows us to use Lists. 
using UnityEngine.UI;                   // Allows us to use UI.
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Public class variables (and prefab placeholders).
    public static GameManager instance = null;               // Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject player;                                // Reference to player gameobject in game
    public Vector3 savedPlayerPosition = new Vector3(0f, 0f, 0f); // Placeholder for player position variable used when returning to overworld
    public bool toOverworld = false;                        // Boolean for checking if we are going to the overworld from underworld
    public int playerTreats;                                // Number of treats to save for player when returning to overworld
    public int playerToys;                                  // Number of toys to save for player when returning to overworld
    public int playerDogCounter;                            // Number of dogs the player hasn't pet in the zone
    public bool inShop = false;                             // Boolean to check if the player is in the shop scene
    public DogAndItemSpawner DIS;                               // Reference to DogAndItemSpawner
    public Dog[] savedDogScripts;
    public Item[] savedItemScripts;
    #endregion

    #region Anxiety handling
    public float anxietyLevel;                              // Player's current level of anxiety
    public float maxAnxiety = 100;                          // Max anxiety before a player loses the game
    #endregion

    #region UI handling
    public Text dogCounterTextUI;                            // Text UI reference to dogCounter
    public Text statsTextObject;                            // Text UI reference to stats
    public Slider anxietyBar;                               // Slider UI reference to anxiety bar
    public Text youLose;                                    // Text UI reference to "You Lose" text
    #endregion

    #region Private class variables
    // References to players and dogs, and sorts their layer order
    private List<GameObject> movingObjects;
    #endregion

    #region Awake function. Awake enforces a singleton GameManager and sets up variables for the scene.
    //Awake is always called before any Start functions
    void Awake()
    {
        #region Singleton enforced on GameManager instance
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion


        #region Set Up Scene Variables
        initializeSortingOrder();
        updateUI();
        // Get reference to DogAndItemSpawner to update/save dog positions/references
        DIS = GameObject.FindGameObjectWithTag("Spawner").GetComponent<DogAndItemSpawner>();
        #endregion
    }
    #endregion

    #region Layer Ordering Function
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

    private void initializeSortingOrder()
    {
        movingObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        movingObjects.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Dog")));
        SetSortingOrder(new List<GameObject>((GameObject[])FindObjectsOfType(typeof(GameObject))));
    }
    #endregion

    #region Update function.
    // Update is called once per frame
    private void Update()
    {
        UpdateAnxiety();
        dogCounterTextUI.text = "Dogs Left: " + playerDogCounter;
        if (playerDogCounter == 0)
        {
            youLose.text = "YOU PET ALL THE DOGS!";
        }
        statsTextObject.text = "Treats: " + playerTreats + " Toys: " + playerToys;
        SetSortingOrder(movingObjects);
        SceneManager.sceneLoaded += OnSceneLoaded;      // Necessary to check if the scene was loaded and, if so, to re-assign variables
    }
    #endregion

    #region Scripts for transitioning between scenes.
    // Things to do everytime the scene changes
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        updateUI();
        player = GameObject.FindGameObjectWithTag("Player");
        initializeSortingOrder();
    }

    // Have hit dog and enter underworld for interaction
    public void goToUnderworld(Vector3 dogPos)
    {
        DIS.deactivateAllDogs();
        /*
        for (int i = 0; i < DIS.dogList.Length; i++)
        {
            // For each dog, get their script reference, find them via their hierarchy name, and save position before transition
            GameObject currDog = DIS.dogList[i];
            currDog.GetComponent<Dog>().position = currDog.transform.position;
            currDog.SetActive(false);
        }
        */
        savedPlayerPosition = new Vector3(dogPos.x - 2f, dogPos.y, 0f); // Save player position to be displaced to the left of the dog for when returning from underworld
        toOverworld = true;                             // Set transition boolean to true (allows player position to be picked based off of what was stored in gamemanager)
        SceneManager.LoadScene(2);                      // Go to underworld scene (index 2 in build)
        SceneManager.sceneLoaded += OnSceneLoaded;      // Necessary to check if the scene was loaded and, if so, to re-assign variables
    }

    // Have hit back-off in the underworld.
    public void returnToOverworld()
    {
        SceneManager.LoadScene(1);
        updateUI();
        DIS.activateAllDogs();
    }

    // Update UI by searching for tag
    public void updateUI ()
    {
        dogCounterTextUI = GameObject.FindGameObjectWithTag("DogCounter").GetComponent<Text>();
        statsTextObject = GameObject.FindGameObjectWithTag("TreatsAndToys").GetComponent<Text>();
        youLose = GameObject.FindGameObjectWithTag("YouLose").GetComponent<Text>();
        youLose.text = "";
        anxietyBar = GameObject.FindGameObjectWithTag("Anxiety").GetComponent<Slider>();
    }
    #endregion

    #region Player anxiety handling
    public void UpdateAnxiety()
    {
        anxietyLevel += Time.deltaTime;
        anxietyBar.value = anxietyLevel;
        if (anxietyBar.value >= maxAnxiety)
        {
            youLose.text = "YOU HAVE BECOME TOO ANXIOUS. YOU LOSE!";
        }
    }

    public void CalculateAnxiety(float amount)
    {
        anxietyLevel += amount;
        anxietyLevel = Mathf.Clamp(anxietyLevel, 0, maxAnxiety);
        //Debug.Log(anxietyLevel / maxAnxiety);
        anxietyBar.value = anxietyLevel;
    }
    #endregion
}
