using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickFace : MonoBehaviour
{
    Joystick j;

    private void Start()
    {
        j = GetComponentInParent<Joystick>();
    }

    void Dash()
    {
        j.Dash();
    }

    void WakeUp()
    {
        j.WakeUp();
    }

    void Jump()
    {
        j.BodyJump();
    }
}
