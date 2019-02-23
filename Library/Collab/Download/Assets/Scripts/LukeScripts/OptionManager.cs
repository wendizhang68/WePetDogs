using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour {

    public static OptionManager instance;

    #region instance variables
    //arrow key options
    public Text petDog;
    private string norPetDog;
    public Text giveTreat;
    private string norGiveTreat;
    public Text throwToy;
    private string norThrowToy;
    public Text backOff;
    private string norBackOff;
    private int lastOption;
    private int selectedOption;
    private Text[] optionsList;
    private string[] stringList;

    //to play animation
    public GameObject playerAndDog;
    public GameObject Player;
    public GameObject Dog;
    public Animator playerAnim;
    public Animator dogAnim;

    //anxiety bar
    public float maxAnxiety = 100;
    public float anxietyLevel;
    public Slider anxietyBar;
    private float petPoints;
    private float wrongMove;

    //game over
    public Text youLose;

    //dog preferences
    public string curDog;
    #endregion

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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        #endregion

        //Assign enemies to a new List of Enemy objects.
        //enemies = new List<Enemy>();

        //Call the InitGame function to initialize the first level 
        //InitGame();
    }

    void Start()
    {
        playerAnim = Player.GetComponent<Animator>();
        dogAnim = Dog.GetComponent<Animator>();
        optionsList = new Text[4];
        optionsList[0] = petDog;
        optionsList[1] = giveTreat;
        optionsList[2] = throwToy;
        optionsList[3] = backOff;
        selectedOption = 0;
        norPetDog = petDog.text;
        norGiveTreat = giveTreat.text;
        norThrowToy = throwToy.text;
        norBackOff = backOff.text;
        stringList = new string[4];
        stringList[0] = norPetDog;
        stringList[1] = norGiveTreat;
        stringList[2] = norThrowToy;
        stringList[3] = norBackOff;
        petDog.text = "> " + petDog.text;

        CalculateAnxiety(0);
        petPoints = -20;
        wrongMove = 10;
        youLose.text = "";
    }

    void Update()
    {
        anxietyLevel += 0.01f;
        anxietyBar.value = anxietyLevel / maxAnxiety;
        if (anxietyLevel == maxAnxiety)
        {
            youLose.text = "YOU LOSE!";
        }
        bool wasChanged = false;
        lastOption = selectedOption;
        //Uses the arrow keys to highlight the desired option in the menu.
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedOption == 0 || selectedOption == 2)
            {
                selectedOption++;
                wasChanged = true;
            }
            else if (selectedOption == 1 || selectedOption == 3)
            {
                selectedOption--;
                wasChanged = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedOption == 0 || selectedOption == 1)
            {
                selectedOption += 2;
                wasChanged = true;
            }
            else if (selectedOption == 2 || selectedOption == 3)
            {
                selectedOption -= 2;
                wasChanged = true;
            }
        }

        //If an arrow key was pressed and the option changed, indicates which option is
        //highlighted now.
        if (wasChanged)
        {
            switch (selectedOption)
            {
                case 0:
                    petDog.text = "> " + petDog.text;
                    break;
                case 1:
                    giveTreat.text = "> " + giveTreat.text;
                    break;
                case 2:
                    throwToy.text = "> " + throwToy.text;
                    break;
                case 3:
                    backOff.text = "> " + backOff.text;
                    break;
            }
            optionsList[lastOption].text = stringList[lastOption];
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectedOption)
            {
                case 0:
                    PetTheDog();
                    break;
                case 1:
                    GiveTreat();
                    break;
                case 2:
                    ThrowToy();
                    break;
                case 3:
                    BackOff();
                    break;
            }
        }
    }

    public void CalculateAnxiety(float amount)
    {
        anxietyLevel += amount;
        anxietyLevel = Mathf.Clamp(anxietyLevel, 0, maxAnxiety);
        Debug.Log(anxietyLevel / maxAnxiety);
        anxietyBar.value = anxietyLevel;
        if (anxietyLevel >= maxAnxiety)
        {
            youLose.text = "YOU LOSE!";
        }
    }

    public void PetTheDog()
    {
        Debug.Log("Pet Dog");
        playerAnim.SetTrigger("pet");
        //dogAnim.SetTrigger("pet");
        //StartCoroutine(Wait(2f));
        //Dog.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0);
        //Debug.Log("Walking!");
        //StartCoroutine(Wait(2f));
        //Dog.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //dogAnim.SetTrigger("pet");
        CalculateAnxiety(petPoints);
    }

    public void ThrowToy()
    {
        Debug.Log("Threw Toy");
        playerAnim.SetTrigger("throw");
        CalculateAnxiety(wrongMove);
        ShopScript.items[2]--;
    }

    public void GiveTreat()
    {
        Debug.Log("Gave Treat");
        /**if (curDog.Equals("poodle"))
        {
            CalculateAnxiety(petPoints);
        }
        else
        {
            CalculateAnxiety(wrongMove);
        } */
        ShopScript.items[0]--;
    }

    public void BackOff()
    {
        Debug.Log("Backed Off");
        SceneManager.LoadScene(0);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
