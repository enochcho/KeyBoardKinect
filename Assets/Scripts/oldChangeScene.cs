using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class oldChangeScene : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // Start is called before the first frame update

    public Text gestureInfo;
    //public GameObject nextbutton;
    public GameObject startbutton;
    public GameObject optionbutton;
    public GameObject freeplaybutton;
    public GameObject spaceship;
    public GameObject tutorialbutton;

    private bool progressDisplayed;

    void Start()
    {
        // DontDestroyOnLoad(this);
       // nextbutton = GameObject.Find("Next");
        startbutton = GameObject.Find("Start");
        optionbutton = GameObject.Find("Options");
        freeplaybutton = GameObject.Find("Free play");
        tutorialbutton = GameObject.Find("Tutorial");

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
                if (hit.transform.gameObject.name.Contains("Start"))
                {
                    SceneManager.LoadScene("PickSong");
                } else if (hit.transform.gameObject.name.Contains("Options"))
                {
                    SceneManager.LoadScene("OptionMenu");
                } else if (hit.transform.gameObject.name.Contains("Free Play"))
                {
                    SceneManager.LoadScene("FreePlay");
                } else if(hit.transform.gameObject.name.Contains("Tutorial"))
                {
                    SceneManager.LoadScene("PickTutorial");
                }
            }
        }
    }
}


