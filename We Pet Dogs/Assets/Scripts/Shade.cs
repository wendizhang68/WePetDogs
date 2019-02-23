using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour {

    private void Start()
    {
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        string coltag = collision.gameObject.tag;
        if (coltag == "Player" || coltag == "Dog") {
            Shadow(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string coltag = collision.gameObject.tag;
        if (coltag == "Player" || coltag == "Dog")
        {
            unShadow(collision.gameObject);
        }
    }

    private void Shadow(GameObject obj) {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r / 2, c.g / 2, c.b / 2);
    }

    private void unShadow(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r * 2, c.g * 2, c.b * 2);
    }
}
