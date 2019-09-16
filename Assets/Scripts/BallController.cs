using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour {

    private Rigidbody2D rb;
    private GameObject winnerText;
    private bool gameFinished;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        winnerText = GameObject.FindGameObjectWithTag("Finish");
        winnerText.GetComponent<TextMesh>().text = "";
        gameFinished = false;
    }

    void Update()
    {
        if (gameFinished)
        {
            if (Input.GetButtonDown("Restart"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.bodyType = RigidbodyType2D.Static;
        gameFinished = true;

        if (collision.gameObject.CompareTag("Goal_1"))
        {
            winnerText.GetComponent<TextMesh>().text = "Team_2 wins!";
        }
        else if (collision.gameObject.CompareTag("Goal_2"))
        {
            winnerText.GetComponent<TextMesh>().text = "Team_1 wins!";
        }


    }
}
