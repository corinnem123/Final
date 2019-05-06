
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*huge shout out to Faith Kim for running the smithies make games
 * workshop last semester she Saved My Life
 * this for adding music: https://www.youtube.com/watch?v=JKoBWBXVvKY
 * music credit to https://www.bensound.com/
 * also https://www.youtube.com/watch?v=a7rwCBDMyh4
 * I did not get through the whole tutorial but what i did
 * watch was Fantastic*/

public class PlayerControls : MonoBehaviour
{
    private int speed;
    private Rigidbody2D rb;
    private int jumpPower;
    private Boolean finish = false;

    // Start is called before the first frame update
    void Start()
    {

        //set initial speed
        speed = 10;

        //putting rigidBody in the angle brackets gives you access to it thru rb
        rb = gameObject.GetComponent<Rigidbody2D>();

        //set initial jump power
        jumpPower = 15000;

        //do not let the player character rotate
        //if you let him rotate his axis get all messed up
        //whenever he jumps so Do Not
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("starting...");
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * check if you're far enough along to laod another part of the level
         */
         if (Level.level >3)
        {
            Level.LoadSection();
        }

        /*
         * if you've encountered a level end marker,
         * load the next level
         */
        if (finish == true)
        {
            finish = false;
            Debug.Log("loading");
            LoadNext.Continue();
        }

        /*
         * if you fell offscreen,
         * restart the level
         */
        if(GameObject.Find("Body").transform.position.y < -5)
        {
            Debug.Log("you fell :(");
            if(Level.level <= 5 && Level.buffs > 0)
            {
                Level.buffs -= 1;
                LoadNext.Reload();
                Debug.Log("health subtracted");
               
            }
            else
            {
                Level.level = 1;
                LoadNext.Restart();
            }
        }

        /*
         * if you exit stage right on the last level,
         * return to menu screen
         * and be prepared to run the game again
         */
        if(GameObject.Find("Body").transform.position.x > 8 && Level.level >= 6)
        {
            Debug.Log(Level.level);
            Level.level = 1;
            LoadNext.Restart();
        }

        /*
         * load the secret cloud level if you find it
         */
         if (Level.level == 4)
        {
            if(GameObject.Find("Body").transform.position.y > 5)
            {
                SceneManager.LoadScene("SecretCloudLevel", LoadSceneMode.Single);
            }
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
            //time.deltatime just makes things smoother
            //by slowing it down apparently
            //move to the left
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //jump
            //gonna use RigidBody2d
            rb.AddForce(Vector2.up * jumpPower * Time.deltaTime);
        }


    }

    /*
     * handles player interacting with in-game objects
     */

    private void OnCollisionEnter2D(Collision2D collider)
 
    //the one with 2D on the end is for 2D, the unmarked one is for 3D objects
    //if we get an obstacle is tagged, and thats waht we get, we return true
    {
        //encountering an obstacle
        if (collider.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Collision with obstacle!");
            //SetActive takes a boolean
            //make the obstacle you touched dissapear
            collider.gameObject.SetActive(false);
        }

        //encountering a health boost
        if (collider.gameObject.CompareTag("buff"))
        {
            Debug.Log("health boosted!");
            //make the health increase item dissapear
            collider.gameObject.SetActive(false);

            //actually like boost your health
            Level.buffs += 1;
        }

        //encountering a health hazard
        if (collider.gameObject.CompareTag("nerf"))
        {
            Debug.Log("ouch");

            if(Level.buffs > 0)
            {
                //restart the level
                LoadNext.Reload();
            }
            else
            {
                //restart the game
                Level.level = 1;
                LoadNext.Restart();
            }
            
        }

        //encountering a level ending marker
        if(collider.gameObject.CompareTag("checkpoint"))
        {
            Debug.Log("load");
            //if finish is true, next frame will load
            //the next level
            finish = true;
        }

        }

    //handle encounters with triggers
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            //decrease speed and jump ability in water
            speed = 5;
            jumpPower = 5000;
            Debug.Log("trigger active");
        }

        if (collision.gameObject.CompareTag("nerf"))
        {
            //drop a rock }:)
            //sometimes the rock dissapears
            //or resets position onto the top of the hill
            //i have no idea why this happens
            collision.attachedRigidbody.gravityScale = 1;
            collision.attachedRigidbody.constraints = RigidbodyConstraints2D.None;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            //reset speed and jump power once you exit the water
            speed = 10;
            jumpPower = 20000;
        }
    }
}
