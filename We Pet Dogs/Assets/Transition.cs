using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

    public Animator animator;
    public int scene;
    public bool collision;

    void Awake()
    {
        collision = false;
    }
    // Update is called once per frame
    void Update () {
		if (collision)
        {
            StartCoroutine(LoadNext());
        }
	}

    IEnumerator LoadNext()
    {
        animator.SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
        collision = false;
    }
}
