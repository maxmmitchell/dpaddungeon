using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour
{
    // wraps non projectile, non player gameobjects around screen

        // TODO: FIX! this doesnt work!

    Camera cam;
    Vector2 screenBottomLeft;
    Vector2 screenTopRight;

    float screenWidth;
    float screenHeight;
    private void Start()
    {
        cam = Camera.main;

        screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // not player, not projectile
        if (other.gameObject.layer != 14 && other.gameObject.layer != 10)
        {
            Vector2 pos = other.gameObject.transform.position;
            if (OffScreenX())
            {
                other.gameObject.transform.position = new Vector2(-pos.x, pos.y);
            }
            if (OffScreenX())
            {
                other.gameObject.transform.position = new Vector2(pos.x, -pos.y);
            }
        }
    }

    bool OffScreenX()
    {
        float posX = transform.position.x;
        return posX < -screenWidth / 2 || posX > screenWidth / 2;
    }

    bool OffScreenY()
    {
        float posY = transform.position.y;
        return posY < -screenHeight / 2 || posY > screenHeight / 2;
    }
}
