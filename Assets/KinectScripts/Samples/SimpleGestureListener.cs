using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public Text gestureInfo;

    // Keys
    public GameObject b3;
    public GameObject c4;
    public GameObject cs4;
    public GameObject d4;
    public GameObject ds4;
    public GameObject e4;
    public GameObject f4;
    public GameObject fs4;
    public GameObject g4;
    public GameObject gs4;
    public GameObject a4;
    public GameObject as4;
    public GameObject b4;
    public GameObject c5;
    public GameObject cs5;
    public GameObject d5;

    public GameObject spaceship;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    private bool keyClicked = false;
    public GameObject selectedKey;
    //private correctlyPlacedScript correctlyPlaced;
    static float whole = 1f;
    static float quarter = .33f;
    static float half = .66f;
    static float tied = 2f;

    /*float[] duration = new float[half, quarter, quarter, quarter, quarter, whole, whole,
        half, quarter, quarter, quarter, quarter, tied,
        quarter, quarter, quarter, quarter, quarter, quarter, half, quarter, whole,
        half, quarter, quarter, quarter, quarter, quarter, quarter, quarter, quarter, quarter, quarter,
        half, quarter, quarter, quarter, quarter, whole, half, quarter,
        half, quarter, quarter, quarter, quarter, whole, quarter/*rest*/, /* quarter, quarter,
        whole, whole, quarter, quarter, quarter, quarter, quarter, quarter,
        whole, whole, tied]; */
    GameObject[] player;

    public void UserDetected(uint userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        if (gestureInfo != null)
        {
           //gestureInfo.text = "Capture planets by clicking on them!";
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
        if (gesture == KinectGestures.Gestures.Click && progress > 0.3f)
        {
            string sGestureText = string.Format("capture {0:F1}% complete", progress * 100);

            if (gestureInfo != null)
             //   gestureInfo.text = sGestureText;

            progressDisplayed = true;
        }
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        /*string sGestureText = "";
        if (gesture == KinectGestures.Gestures.Click)
        {

            switch (keyClicked)
            {
                case true:
                    if (correctlyPlaced.correctlyPlaced == false)
                    {
                        sGestureText = selectedKey.tag + " deposited.";
                    }


                    keyClicked = false;
                    selectedKey = null;

                    break;
                case false:
                    Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
                    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                    if (hit)
                    {
                        Debug.Log(hit.transform.name);
                        Debug.Log(hit.transform.gameObject.tag);
                        keyClicked = true;
                        selectedKey = hit.transform.gameObject;
                        correctlyPlaced = selectedKey.GetComponent<correctlyPlacedScript>();
                        sGestureText = selectedKey.tag + " captured";
                    }
                    break;
            }
        }



        if (gestureInfo != null)
        {
            gestureInfo.text = sGestureText;
        }

        progressDisplayed = false;
        */
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

    public void PlayForTime(float time)
    {
        GetComponent<AudioSource>().Play();
        Invoke("StopAudio", time);
    }

    private void StopAudio()
    {
        GetComponent<AudioSource>().Stop();
    }

    void Update()
    {

        if (keyClicked == true && selectedKey != null)// && correctlyPlaced.correctlyPlaced != true)
        {
            //selectedKey.transform.position = spaceship.transform.position;
            //gestureInfo.text = "don't move";
            //Debug.Log("clicked");
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");


            /*switch (keyClicked)
            {
            // timer for not holding down button or audio clip stopped playing 
            // attach spaceship to hand modify in kinectmanager and this script
            // project settings > input > mess with axes
            // Unity Input.getaxis
                case true:

                    keyClicked = false;
                    selectedKey = null;

                    break;
                case false:
                    Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
                    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                    if (hit)
                    {
                        Debug.Log(hit.transform.name);
                        Debug.Log(hit.transform.gameObject.tag);
                        keyClicked = true;
                        selectedKey = hit.transform.gameObject;
                        correctlyPlaced = selectedKey.GetComponent<correctlyPlacedScript>();
                    }
                    break;
            }*/

            Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                if (hit.transform.gameObject.name.Contains("Key"))
                {
                    //gestureInfo.text = hit.transform.gameObject.name;
                    Debug.Log(hit.transform.name);
                    gestureInfo.text = hit.transform.name;
                    Debug.Log(hit.transform.gameObject.tag);
                    keyClicked = true;
                    selectedKey = hit.transform.gameObject;
                    selectedKey.GetComponent<AudioSource>().Play();
                    //correctlyPlaced = selectedKey.GetComponent<correctlyPlacedScript>();
                }
            }

        }
    }
}