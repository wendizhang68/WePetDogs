using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionManager: MonoBehaviour {

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
    public GameObject Player;
    public GameObject Dog;
    public Animator playerAnim;
    public Animator dogAnim;
    public Player_Underworld playerScript;

    //anxiety bar
    GameManager gameManager;
    GameObject GM;

    //dog preferences
    public static string curDog;
    #endregion

    void Start()
    {
        GM = GameObject.Find("GameManager(Clone)");            //Get the game manager
        gameManager = GM.GetComponent<GameManager>();          // Get game manager script from gameobject
        playerAnim = Player.GetComponent<Animator>();          // Animator for Player
        dogAnim = Dog.GetComponent<Animator>();                // Animator for Dog
        playerScript = Player.GetComponent<Player_Underworld>();
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
    }

    void Update()
    {
        MenuSelection();
    }

    public void PetTheDog()
    {
        playerScript.Pet();
    }

    public void ThrowToy()
    {
        playerScript.Throw();
    }

    public void GiveTreat()
    {
        Debug.Log("Gave Treat");
        playerScript.Treat();
        if (curDog.Equals("poodle"))
        {
            Debug.Log("dog is happier, happiness +2");
        }
        else
        {
            Debug.Log("dog is happier, happiness +1");

        }
    }

    public void BackOff()
    {
        Debug.Log("Backed Off");
        gameManager.returnToOverworld();
    }

    public void MenuSelection() {
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
}
