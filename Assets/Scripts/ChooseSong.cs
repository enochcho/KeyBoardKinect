using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChooseSong : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // Start is called before the first frame update

    public Text gestureInfo;
    public GameObject happybdaybutton;
    public GameObject takemebutton;
    public GameObject londonbutton;
    public GameObject spaceship;

   static public int songNum;

    private bool progressDisplayed;

    void Start()
    {
        // nextbutton = GameObject.Find("Next");
        happybdaybutton = GameObject.Find("Happy Birthday");
        takemebutton = GameObject.Find("Take Me Out to The Ball Game");
        londonbutton = GameObject.Find("London Bridge");

    }


    public void UserDetected(uint userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        if (gestureInfo != null)
        {

        }
    }

    public void UserLost(uint userId, int userIndex)
    {
        if (gestureInfo != null)
        {
            gestureInfo.text = string.Empty;
        }
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
        float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        if (progressDisplayed)
        {
            // clear the progress info
            if (gestureInfo != null)
                gestureInfo.text = String.Empty;

            progressDisplayed = false;
        }

        return true;
    }
    // Update is called once per frame
    void Update()
    {
        //Test
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            spaceship.transform.position += move * (float)10.0 * Time.deltaTime;
        //endTest
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Pressed primary button.");
            Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                if (hit.transform.gameObject.name.Contains("Happy Birthday"))
                {
                    //songNum=PlayerPrefs.GetInt("songNum",1);
                    PlayerPrefs.SetInt("songNum",2);
                    SceneManager.LoadScene("Game");
                } else if (hit.transform.gameObject.name.Contains("Take Me Out to The Ball Game"))
                {
                   //songNum= PlayerPrefs.GetInt("songNum",3);
                   PlayerPrefs.SetInt("songNum",0);
                   SceneManager.LoadScene("Game");
                }else if (hit.transform.gameObject.name.Contains("London Bridge"))
                {
                   // songNum=PlayerPrefs.GetInt("songNum",5);
                   PlayerPrefs.SetInt("songNum",4);
                   SceneManager.LoadScene("Game");
                } else if (hit.transform.gameObject.name.Contains("Home"))
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

    void OnDestroy()
    {
       
    }
}
