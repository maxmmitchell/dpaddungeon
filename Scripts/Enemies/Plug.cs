using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public GameObject plug;
    public GameObject shock;

    public Animator lazerAnim;

    void Start()
    {
        // on spawn start spawn animation
        // transition direct to telegraph animation
        // on finish, call SHOCKWAVE
        // transition to boil animation, go for as long as we
        // declare in SHOCKWAVE
        // transition to powerdown animation/disappear
        // on disappear ending, call function to destroy self
    }

    public void Warning()
    {
        //SFX
        AudioManager.instance.Play("SFXSiren");
    }

    public void Telegraph()
    {
        plug.GetComponent<Collider2D>().enabled = true;
        //SFX
        AudioManager.instance.Play("SFXCharge");
        lazerAnim.SetBool("Start", true);
    }

    public IEnumerator Shockwave()
    {
        //SFX
        AudioManager.instance.Play("SFXLazer");
        lazerAnim.SetBool("Start", false);
        // called by animator after telegraphing attack
        // start boil
        lazerAnim.SetBool("Boil", true);

        StartCoroutine(FindObjectOfType<CameraShake>().Shake(0.5f, 0.05f));

        shock.GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(2f);

        shock.GetComponent<Collider2D>().enabled = false;

        //transition
        lazerAnim.SetBool("Boil", false);
    }

    public void Retreat()
    {
        AudioManager.instance.Stop("SFXLazer");
        plug.GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetBool("Retreat", true);
    }

    // called at end of all animation
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
