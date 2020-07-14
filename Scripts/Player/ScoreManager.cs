using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshPro textModel;

    public TextMeshProUGUI scoreText;

    public void AddScore(int s, Vector2 position)
    {
        AudioManager.instance.Play("SFXPoints");
        score += s;
        scoreText.text = "Score: " + score.ToString();
        StartCoroutine(Points(s, position));
    }

    IEnumerator Points(int s, Vector2 pos)
    {
        TextMeshPro t = Instantiate(textModel, pos, Quaternion.identity);
        t.text = "+" + s.ToString();
        t.gameObject.SetActive(true);

        //fade and float
        while (t.color.a > 0)
        {
            t.transform.position = new Vector2(t.transform.position.x, t.transform.position.y + 0.5f);

            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - 0.1f);

            yield return new WaitForSeconds(0.1f);
        }
        Destroy(t.gameObject);
    }
}
