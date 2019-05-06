using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * Keeps global count of levels
 * Begins the game
 */


public class Level : MonoBehaviour
{

    //accessible int for tracking what level player is on
    public static int level = 1;

    //accessible int for tracking buffs
    public static int buffs = 0;

    //
    public static bool farOut = false;

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    /*
     * this script is only applied
     * to the menu screen
     * so this method is only called
     * on the menu screen
     */
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /*
     * method to handle bigger levels
     * so basically non-tutorial levels
     */

    public static void LoadSection()
    {
        if (GameObject.Find("Body").transform.position.x >= 8 )
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(18, 0, -10);
            farOut = true;
        }

        if (GameObject.Find("Body").transform.position.x <8 && farOut == true)
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -10);
            farOut = false;
        }
    }
}
