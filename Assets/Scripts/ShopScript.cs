using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopScript : MonoBehaviour {

    #region Instance variables
    //text options
    public GameObject GM;
    public GameManager gameManager;
    public Text pineCount;
    public Text menuText;
    public Text dogBiscuit;
    private string norDogBiscuit;
    public Text boneToy;
    private string norBoneToy;
    private int lastOption;
    private int selectedOption;
    private Text[] optionsList;
    private string[] stringList;
    public Text dBNum;
    public Text bToNum;
    //editable variables
    private static int acorns;
    public static int[] items;
    //noneditable variables
    private int[] costs;
    #endregion

    // Use this for initialization
    void Start () {
        GM = GameObject.Find("GameManager");            //Get the game manager
        gameManager = GM.GetComponent<GameManager>();
        //test
        acorns = 20;
        //vars
        items = new int[2];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = 0;
        }
        //text
        optionsList = new Text[2];
        optionsList[0] = dogBiscuit;
        optionsList[1] = boneToy;
        selectedOption = 0;
        norDogBiscuit = dogBiscuit.text;
        norBoneToy = boneToy.text;
        stringList = new string[2];
        stringList[0] = norDogBiscuit;
        stringList[1] = norBoneToy;
        dogBiscuit.text = "> " + dogBiscuit.text;
        costs = new int[2];
        costs[0] = 3;
        costs[1] = 8;
        dBNum.text = "" + gameManager.playerTreats;
        bToNum.text = "" + gameManager.playerToys;
        pineCount.text = "" + acorns;
    }

    // Update is called once per frame
    void Update () {
        ShopArrowKeys();
        UpdateItems();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            menuText.text = "Thank you! Come again!";
            SceneManager.LoadScene(0);
        }
    }

    #region Helper Methods
    public void UpdateItems()
    {
        dBNum.text = "" + gameManager.playerTreats;
        bToNum.text = "" + gameManager.playerToys;
        pineCount.text = "" + acorns;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            menuText.text = "Thank you! Come again!";
            SceneManager.LoadScene(0);
        }
    }

    public void ShopArrowKeys()
    {
        bool wasChanged = false;
        lastOption = selectedOption;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (selectedOption)
            {
                case 0:
                    selectedOption++;
                    wasChanged = true;
                    break;
                case 1:
                    selectedOption--;
                    wasChanged = true;
                    break;
            }
        }

        if (wasChanged)
        {
            switch (selectedOption)
            {
                case 0:
                    dogBiscuit.text = "> " + dogBiscuit.text;
                    break;
                case 1:
                    boneToy.text = "> " + boneToy.text;
                    break;
            }
            optionsList[lastOption].text = stringList[lastOption];
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Purchase();
        }
    }

    public void Purchase()
    {
        switch (selectedOption)
        {
            case 0:
                Debug.Log("Bought dog biscuit");
                gameManager.playerTreats++;
                menuText.text = "Thank you for purchasing a dog biscuit!";
                break;
            case 1:
                Debug.Log("Bought bone toy");
                gameManager.playerToys++;
                menuText.text = "Thank you for purchasing a dog toy!";
                break;
        }

        if (acorns < costs[selectedOption])
        {
            Debug.Log("Don't have enough pinecones");
            menuText.text = "Sorry! You don't have enough pinecones to buy this!";
        }
        else
        {
            acorns -= costs[selectedOption];
            items[selectedOption]++;
        }
    }
    #endregion
}
