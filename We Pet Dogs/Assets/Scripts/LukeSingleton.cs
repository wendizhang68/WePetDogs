using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LukeSingleton : MonoBehaviour {

    public static LukeSingleton instance = null;

    private void Awake()
    {
        #region Singleton enforced on LukeSingleton instance
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
