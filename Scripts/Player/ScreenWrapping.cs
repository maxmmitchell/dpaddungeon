using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenWrapping : MonoBehaviour
{
    Camera cam;
    Vector2 screenBottomLeft;
    Vector2 screenTopRight;

    float screenWidth;
    float screenHeight;

    public Transform[] ghosts = new Transform[8];

    private void Start()
    {
        cam = Camera.main;

        screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        CreateGhostShips();
        PositionGhostShips();
    }

    private void Update()
    {
        if (OffScreen())
        {
            SwapShips();
        }
    }
    void CreateGhostShips()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
            ghosts[i].gameObject.AddComponent<Ghost>();

            DestroyImmediate(ghosts[i].GetComponent<PlayerCombat>());
            DestroyImmediate(ghosts[i].GetComponent<ScreenWrapping>());
        }
    }

    void PositionGhostShips()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        var ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }

    void SwapShips()
    {
        foreach (var ghost in ghosts)
        {
            if (ghost.position.x < screenWidth / 2 && ghost.position.x > -screenWidth / 2 &&
                ghost.position.y < screenHeight / 2 && ghost.position.y > -screenHeight / 2)
            {
                transform.position = ghost.position;

                break;
            }
        }

        PositionGhostShips();
    }

    bool OffScreen()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        return posX < -screenWidth / 2 || posX > screenWidth / 2 || posY < -screenHeight / 2 || posY > screenHeight / 2;
    }
}
