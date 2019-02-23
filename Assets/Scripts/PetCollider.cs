using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCollider : MonoBehaviour {

    public GameObject Player;
    public Player_Underworld playerScript;


	// Use this for initialization
	void Start () {
        Player = transform.parent.gameObject;
        playerScript = Player.GetComponent<Player_Underworld>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        string collName = collision.name;
        //Debug.Log("Petting: " + collName);
        if (collName == "Dog"){
            playerScript.dog = true;
        }
    }
}
