using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    // delay means something different depending on the pattern.
    public float delay;
    // time that enemy shoots for
    public float time;
    // speed projectiles move at
    public float speed;
    // angle break between each launched projectile, where applicable
    public float angleBreak;

    // prefab for all projectiles
    public GameObject projectile;

    public GameObject target;

    // LAUNCH PATTERNS

    // CONCENTRIC CIRCLES
    // spawns in a circle based on angle break
    // launches at speed
    // waits for delay, then repeats 
    // here, time indicates number of circles to launch
    public IEnumerator LaunchCircles()
    {
        Vector2 direction = Vector2.right;
        float angle = 0;

        int i = 0;

        while (i < time)
        {
            // repeat until done full circle
            while (angle + angleBreak < 360)
            {
                GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
                p.SetActive(true);
                p.GetComponent<Rigidbody2D>().velocity = direction * speed;

                //SFX
                AudioManager.instance.Play("SFXCircle");

                angle += angleBreak;
                direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            }
            angle = 0;
            i++;
            yield return new WaitForSeconds(delay);
        }
    }

    // SPIRAL
    // spawns one in front of enemy, the one to the right of that, so on
    // in a circle forming spiraling pattern
    // delay: indicates space between each circle of spiral
    // angle break: indicates space between each projecticle in a spiral
    public IEnumerator LaunchSpiral()
    {
        Vector2 direction = Vector2.right;
        float angle = 0;

        float i = 0;

        while (i < time)
        {
            GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
            p.SetActive(true);
            p.GetComponent<Rigidbody2D>().velocity = direction * speed;

            //SFX
            AudioManager.instance.Play("SFXSpiral");

            angle -= angleBreak;
            //Quaternion rotate = Quaternion.AngleAxis(angle, Vector2.right);
            direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            i += Time.deltaTime;
            yield return new WaitForSeconds(delay);
        }
    }

    // RANDOM
    // fires in random directions at variable speeds
    public IEnumerator LaunchRandom()
    {
        Vector2 direction = Vector2.right;
        float angle = 0;

        float i = 0;

        while (i < time)
        {
            angle = Random.Range(0, 360);
            //Quaternion rotate = Quaternion.AngleAxis(angle, Vector2.right);
            direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

            //SFX
            AudioManager.instance.Play("SFXShoot");

            GameObject p = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
            p.SetActive(true);
            p.GetComponent<Rigidbody2D>().velocity = direction * Random.Range(speed - 2, speed + 2);

            i += Time.deltaTime;
            yield return new WaitForSeconds(delay);
        }
    }

    // TARGET PLAYER
    // fires at player
    public IEnumerator LaunchTarget()
    {
        Vector2 direction;

        float i = 0;

        while (i < time)
        {
            Vector2 currPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 playerPos = new Vector2(target.transform.position.x, target.transform.position.y);

            Vector2 unit = playerPos - currPos;
            unit.Normalize();

            direction = unit;

            //SFX
            AudioManager.instance.Play("SFXShoot");

            GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
            p.SetActive(true);
            p.GetComponent<Rigidbody2D>().velocity = direction * speed;

            i += Time.deltaTime;
            yield return new WaitForSeconds(delay);
        }
    }
}
