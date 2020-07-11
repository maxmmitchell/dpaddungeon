using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // not sure if this code is needed
    // key can reference parent PlayerHealth script
    // only will be one of those, they will handle
    // contingency between sprites
    Camera cam;
    Vector2 screenBottomLeft;
    Vector2 screenTopRight;

    float screenWidth;
    float screenHeight;

    Collider2D[] colliders;
    public PlayerHealth health;

    void Start()
    {
        // gather all colliders. we will turn them on when the ghost is
        // on screen and can interact with the world
        colliders = GetComponentsInChildren<Collider2D>();

        // but first, turn all off
        foreach (Collider2D c in colliders)
        {
            c.enabled = false;
        }

        // reference to main character
        health = GetComponentInParent<PlayerHealth>();

        cam = Camera.main;

        screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;
        print("width: ")
    }

    void Update()
    {
        // size of player is 4x4 world units at largest points
        // thus, we can calculate sprites being on screen as follows:
        if (OnScreen())
        {
            print("on screen at: (" + transform.position.x + ", " + transform.position.y + ")");
            foreach (Collider2D c in colliders)
            {
                c.enabled = true;
            }
        }
    }

    bool OnScreen()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        return (posX - 2 < screenWidth / 2 || posX + 2 > -screenWidth / 2) && (posY - 2 < screenHeight / 2 || posY + 2 > -screenHeight / 2);
    }
}
