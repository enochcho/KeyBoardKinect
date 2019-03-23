using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public Text gestureInfo;

    //planets
    public GameObject c3;
    public GameObject d3;
    public GameObject e3;
    public GameObject f3;
    public GameObject g3;
    public GameObject a3;
/*    public GameObject neptune;
    public GameObject uranus;
    public GameObject pluto; */

    public GameObject spaceship;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    private bool keyClicked = false;
    public GameObject selectedKey;
    private correctlyPlacedScript correctlyPlaced;

    public void UserDetected(uint userId, int userIndex)
    {
        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        if (gestureInfo != null)
        {
            gestureInfo.text = "Click a key for a message";
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
                gestureInfo.text = sGestureText;

            progressDisplayed = true;
        }
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        /*string sGestureText = "";
        if (gesture == KinectGestures.Gestures.Click)
        {

            switch (planetClicked)
            {
                case true:
                    if (correctlyPlaced.correctlyPlaced == false)
                    {
                        sGestureText = selectedPlanet.tag + " deposited.";
                    }


                    planetClicked = false;
                    selectedPlanet = null;

                    break;
                case false:
                    Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
                    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                    if (hit)
                    {
                        Debug.Log(hit.transform.name);
                        Debug.Log(hit.transform.gameObject.tag);
                        planetClicked = true;
                        selectedPlanet = hit.transform.gameObject;
                        correctlyPlaced = selectedPlanet.GetComponent<correctlyPlacedScript>();
                        sGestureText = selectedPlanet.tag + " captured";
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

    void Update()
    {

        if (keyClicked == true && selectedKey != null && correctlyPlaced.correctlyPlaced != true)
        {
            selectedKey.transform.position = spaceship.transform.position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");


            /*switch (planetClicked)
            {
            // timer for not holding down button or audio clip stopped playing 
            // attach spaceship to hand modify in kinectmanager and this script
            // project settings > input > mess with axes
            // Unity Input.getaxis
                case true:

                    planetClicked = false;
                    selectedPlanet = null;

                    break;
                case false:
                    Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
                    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
                    if (hit)
                    {
                        Debug.Log(hit.transform.name);
                        Debug.Log(hit.transform.gameObject.tag);
                        planetClicked = true;
                        selectedPlanet = hit.transform.gameObject;
                        correctlyPlaced = selectedPlanet.GetComponent<correctlyPlacedScript>();
                    }
                    break;
            }*/

            Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                if (hit.transform.gameObject.name.Contains("Key"))
                {
                    Debug.Log(hit.transform.name);
                    Debug.Log(hit.transform.gameObject.tag);
                    keyClicked = true;
                    selectedKey = hit.transform.gameObject;
                   // correctlyPlaced = selectedPlanet.GetComponent<correctlyPlacedScript>();
                }
            }

        }
    }
}