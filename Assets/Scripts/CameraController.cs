using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;                               //Player prefab to follow with camera.
    private SpriteRenderer playerSR;
    public float yOffset;
    private Vector3 offset;

    void Start()
    {
        playerSR = player.GetComponent<SpriteRenderer>();
        offset = new Vector3(0, yOffset, -2);         //Offset distance camera will be from player.
    }

    void Update()
    {
        offset = new Vector3(0, yOffset, -2);         //Offset distance camera will be from player.
        transform.position = player.transform.position + offset;
    }
}
