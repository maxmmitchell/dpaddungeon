using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    Plug parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = GetComponentInParent<Plug>();
    }

    public void Boil()
    {
        StartCoroutine(parent.Shockwave());
    }

    public void Retreat()
    {
        parent.Retreat();
    }
}
