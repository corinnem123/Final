using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCloudLevel : MonoBehaviour
{
    private int speed;
    private Rigidbody2D rb;
    private int jumpPower;

    // Start is called before the first frame update
    void Start()
    {
        //set initial speed
        speed = 10;

        //putting rigidBody in the angle brackets gives you access to it thru rb
        rb = gameObject.GetComponent<Rigidbody2D>();

        //set initial jump power
        jumpPower = 20000;

        //do not let the player character rotate
        //if you let him rotate his axis get all messed up
        //whenever he jumps so Do Not
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    // Update is called once per frame
    void Update()
    {
        /*
        * check if you're far enough along to laod another part of the level
        */
        Level.LoadSection();

        /*
         * if you exit stage right,
         * return to menu screen
         * and be prepared to run the game again
         */
        if (GameObject.Find("Body").transform.position.x > 25)
        {
            Debug.Log(Level.level);
            Level.level = 1;
            LoadNext.Restart();
        }

        MovePlayer();

    }

    /*
    * makes the player move with arrow keys and space bar!
    */

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //move to the right
            transform.Translate(Vector2.right * speed * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //move to the left
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump
            rb.AddForce(Vector2.up * jumpPower * Time.deltaTime);
        }
    }

    //handle collisions
    private void OnCollisionEnter2D(Collision2D collider)
    {
        //encountering an obstacle
        if (collider.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Collision with obstacle!");
            //SetActive takes a boolean
            //make the obstacle you touched dissapear
            collider.gameObject.SetActive(false);
        }
    }

    //handle encounters with triggers
    private void OnTriggerStay2D(Collider2D collision)
    {
        //collision with first raincloud
        if (collision.gameObject.CompareTag("waterEnd"))
        {
            //jump back to level 4
            Level.level = 4;
            LoadNext.Reload();

        }

        //collision with second cloud
        if (collision.gameObject.CompareTag("GameController"))
        {
            //jump to level 5
            Level.level = 5;
            LoadNext.Reload();
        }


    }
}
