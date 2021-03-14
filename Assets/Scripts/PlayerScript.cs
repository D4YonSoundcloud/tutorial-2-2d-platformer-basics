using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text playerLives;
    public Text loseText;

    private int scoreValue = 0;
    private int lifeValue = 3;

    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        playerLives.text = "Lifes: " + lifeValue.ToString();
        winText.text = "";
        loseText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float vertMovement = Input.GetAxis("Vertical");
        float hozMovement = Input.GetAxis("Horizontal");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        Vector3 characterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0)
        {
            characterScale.x = -5;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterScale.x = 5;
        }

        transform.localScale = characterScale;

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            musicSource.clip = musicClipOne;
            musicSource.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Scene scene = SceneManager.GetActiveScene();

        if (collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);
        }

        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            //yes this isn't the best way to scale it, but a mans was lazy and didnt want to copy paste a new script
            if(scoreValue == 4 && scene.name == "SampleScene")
            {
                SceneManager.LoadScene("Level 2");
                lifeValue = 3;
                playerLives.text = "Lifes: " + lifeValue.ToString();
            } else if (scoreValue == 4 && scene.name == "Level 2")
            {
                SceneManager.LoadScene("WinScreen");
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            lifeValue -= 1;
            playerLives.text = "Lifes: " + lifeValue.ToString();
            Destroy(collision.collider.gameObject);
            if(lifeValue == 0)
            {
                SceneManager.LoadScene("LoseScreen");
            }
        }
        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (anim.GetInteger("State") == 2)
            {
                if(Input.GetKey(KeyCode.D) == false || Input.GetKey(KeyCode.A) == false)
                {
                    anim.SetInteger("State", 0);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetInteger("State", 2);
            }
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 2);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 1);
            }
        }
    }
}