using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    ProjectileLauncher launcher;

    // Details for launcher
    // with default values
    float idleSpeed = 6;
    float idleTime = 5;
    float idleDelay = 0.2f;

    float explosionSpeed = 6;
    float explosionCircles = 5;
    float explosionBreak = 30;
    float explosionDelay = 0.3f;
    //        //          //

    float boilTime = 2f;

    private void Start()
    {
        launcher = GetComponent<ProjectileLauncher>();
        Spawn();
    }

    private void Update()
    {
        
    }

    void Spawn()
    {
        AudioManager.instance.Play("SFXBurst");
        //camera shake
        StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.75f, 0.1f));
    }

    IEnumerator Idle()
    {
        GetComponent<Collider2D>().enabled = true;
        launcher.delay = idleDelay;
        launcher.speed = idleSpeed;
        launcher.time = idleTime;

        StartCoroutine(launcher.LaunchRandom());
        yield return new WaitForSeconds(idleTime);
        GetComponent<Animator>().SetBool("Boil", true);
        yield return new WaitForSeconds(boilTime);
        GetComponent<Animator>().SetBool("Boil", false);
    }

    IEnumerator Explode()
    {
        launcher.delay = explosionDelay;
        launcher.angleBreak = explosionBreak;
        launcher.time = explosionCircles;
        launcher.speed = explosionSpeed;

        StartCoroutine(launcher.LaunchCircles());

        //SFX
        AudioManager.instance.Play("SFXBlast");

        //camera shake
        StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.75f, 0.1f));

        // wait for explosion finish
        yield return new WaitForSeconds(explosionCircles * explosionDelay);
        Destroy(gameObject);
    }
}
