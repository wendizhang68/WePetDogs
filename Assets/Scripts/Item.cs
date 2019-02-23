using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    #region Public Variables (and prefab holders)
    public string nameInHierarchy;
    public string typeOfItem;
    public Vector3 position;
    #endregion

    #region Private Variables (empty right now)
    #endregion

    #region Item Class Constructor
    public Item (string name, string itemType, Vector3 xyz)
    {
        nameInHierarchy = name;
        typeOfItem = itemType;
        position = xyz;
    }
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
