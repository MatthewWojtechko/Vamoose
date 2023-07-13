using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCol : MonoBehaviour
{
    private GameObject LevelControllerObject;
    private LevelController levelController;

    // Use this for initialization
    void Start ()
    {
        LevelControllerObject = GameObject.FindGameObjectWithTag("LevelController");
        levelController = LevelControllerObject.GetComponent<LevelController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObj = collision.gameObject;

        if (collisionObj.tag == "Lava")
        {
            //play sound exec lavacontact
            levelController.LavaContact();
        }
        else if (collisionObj.tag == "Diamond")
        {
            //call diamondGot [increment score and play sound]
            Destroy(collisionObj);
            levelController.DiamondGet();
        }
        else if (collisionObj.tag == "CheckPoint")
        {
            levelController.CheckPoint();
        }
    }
}