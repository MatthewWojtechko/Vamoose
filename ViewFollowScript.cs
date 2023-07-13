using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFollowScript : MonoBehaviour
{
    public GameObject Player;

    private Vector3 offset;
    // Use this for initialization
    void Start()
    {
        offset = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + offset;
    }
}
