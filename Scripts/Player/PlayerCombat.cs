using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    ProjectileLauncher launcher;
    PlayerHealth health;

    public GameObject shield;

    bool pressA = false;
    bool pressB = false;

    public bool shooting = false;
    public bool shotReset = false;
    public bool shielding = false;
    public bool shieldReset = false;

    void Start()
    {
        launcher = GetComponent<ProjectileLauncher>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        TakeInput();

        if (pressA)
        {
            pressA = false;
            StartCoroutine(Shoot());
        }

        if (pressB)
        {
            pressB = false;
            StartCoroutine(Shield());
        }
    }

    void TakeInput()
    {
        // A BUTTON
        if (Input.GetKeyDown(KeyCode.Mouse0) && health.A.GetComponent<Key>().alive && !shooting)
        {
            pressA = true;
        }

        // B BUTTON
        if (Input.GetKeyDown(KeyCode.Mouse1) && health.B.GetComponent<Key>().alive && !shielding)
        {
            pressB = true;
        }
    }

    IEnumerator Shoot()
    {
        shooting = true;
        health.A.GetComponent<Key>().Press();
        StartCoroutine(health.SwapFace(health.hehFace, 3f));
        StartCoroutine(launcher.LaunchSpiral());

        //cooldown
        yield return new WaitForSeconds(32f);
        // TODO SFX TO INDICATE COOLDOWN OVER
        if (shooting && !shotReset)
        {
            shooting = false;
            health.A.GetComponent<Key>().Release();
        }
        shotReset = false;
    }

    IEnumerator Shield()
    {
        shielding = true;
        health.B.GetComponent<Key>().Press();
        shield.SetActive(true);
        health.invincible = true;
        //SFX
        AudioManager.instance.Play("SFXShield");
        //time active
        yield return new WaitForSeconds(2f);

        //SFX
        AudioManager.instance.Play("SFXShield", 1.2f);
        shield.SetActive(false);
        health.invincible = false;

        //cooldown
        yield return new WaitForSeconds(30f);
        // TODO SFX TO INDICATE COOLDOWN OVER
        if (shielding && !shieldReset)
        {
            shielding = false;
            health.B.GetComponent<Key>().Release();
        }
        shieldReset = false;
    }
}
