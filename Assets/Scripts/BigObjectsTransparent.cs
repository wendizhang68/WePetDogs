using UnityEngine;
using System.Collections;

public class BigObjectsTransparent : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Color color;
    private bool faded;

	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        faded = false;

	}

    private void Update()
    {
        //if (gameObject.name == "Tree") {
        //    Debug.Log(spriteRenderer.color.a);
        //}
    }

    void OnTriggerEnter2D(Collider2D other) 
	{
        if ((other.tag == "Player" || other.tag == "Dog") && other.isTrigger == false && faded == false) 
			StartCoroutine ("FadeIn");
            

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Player" || other.tag == "Dog") && other.isTrigger == false)
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "Player" || other.tag == "Dog") && other.isTrigger == false && faded == true)
			StartCoroutine ("FadeOut");
	}

	
	IEnumerator FadeIn()
    {
        faded = true;
        for (float f = 1f; f >= 0.3f; f -= 0.1f) 
		{
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
			yield return null;
		}
	}

	IEnumerator FadeOut()
    {
        faded = false;
        for (float f = 0.3f; f <= 1.1f; f += 0.1f) 
		{
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
			yield return null;
		}
	}
}