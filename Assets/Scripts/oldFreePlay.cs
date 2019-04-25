using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class oldFreePlay : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
    public Text gestureInfo;

    public GameObject spaceship;

    // private bool to track if progress message has been displayed
    public GameObject homebutton;

    void Start()
    {
        homebutton = GameObject.Find("Home");
    }

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
       
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
       
        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
        KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        return true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            //currentSong.tempo = 
            Debug.Log("Pressed primary button.");
            Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                Debug.Log(hit);
                if (hit.transform.gameObject.name.Contains("Home"))
                {
                    SceneManager.LoadScene("MainMenu");
                } else if (hit.transform.gameObject.name.Contains("Key"))
                {
                  hit.transform.gameObject.GetComponent<AudioSource>().Play();
                    
                } 
            }
        }
    }
}
