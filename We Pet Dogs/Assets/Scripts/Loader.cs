using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;          //GameManager prefab to instantiate.
    //public GameObject optionManager;
    //public GameObject itemSpawner;          //ItemSpawner prefab to instantiate.
    //public GameObject soundManager;         //SoundManager prefab to instantiate.


    void Awake()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.instance == null)
        {
            //Instantiate gameManager prefab
            Instantiate(gameManager);
        }

        //Check if a ItemSpawner has already been assigned to static variable ItemSpawner.instance or if it's still null
        //if (ItemSpawner.instance == null)
        //{
            //Instantiate itemSpawner prefab
            //Instantiate(itemSpawner);
        //}
        //if (OptionManager.instance == null)
        //{
        //    //Instantiate itemSpawner prefab
        //    Instantiate(optionManager);
        //}

        //Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
        //if (SoundManager.instance == null)

        //Instantiate SoundManager prefab
        //Instantiate(soundManager);
    }

    void Start()
    {
    }
}
