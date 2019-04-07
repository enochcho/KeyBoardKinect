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
    static float dottedhalf = 1f;
    static float quarter = .33f;
    static float half = .66f;
    static float tied = 2f;
    static float whole = 1.33f;



    float[] duration = new float[] {
            half, quarter, quarter, quarter, quarter, dottedhalf, dottedhalf,
            half, quarter, quarter, quarter, quarter, tied,
            quarter, quarter, quarter, quarter, quarter, quarter,
            half, quarter, dottedhalf,
            half, quarter, quarter, quarter, quarter,
            quarter, quarter, quarter, quarter, quarter, quarter,
            half, quarter, quarter, quarter, quarter, dottedhalf, half, quarter,
            half, quarter, quarter, quarter, quarter, whole,
            quarter, quarter, dottedhalf, dottedhalf, quarter, quarter, quarter, quarter,
            quarter, quarter, dottedhalf, dottedhalf, tied
        };
    GameObject[] player;

    int[] notesPerLevel = new int[]
        {
            7, 6, 6, 3, 5, 6, 8, 6, 8, 5, 60
        };

   

    void Awake(){
        b3 = GameObject.Find("Key B3");
        c4 = GameObject.Find("Key C4");
        cs4 = GameObject.Find("Key C#4");
        d4 = GameObject.Find("Key D4");
        ds4 = GameObject.Find("Key D#4");
        e4 = GameObject.Find("Key E4");
        f4 = GameObject.Find("Key F4");
        fs4 = GameObject.Find("Key F#4");
        g4 = GameObject.Find("Key G4");
        gs4 = GameObject.Find("Key G#4");
        a4 = GameObject.Find("Key A4");
        as4 = GameObject.Find("KeyA#4");
        b4 = GameObject.Find("Key B4");
        c5 = GameObject.Find("Key C5");
        cs5 = GameObject.Find("Key C#5");
        d5 = GameObject.Find("Key D5");

        GameObject[] correct = new GameObject[] {
            c4, c5, a4, g4, e4, g4, d4,
            c4, c5, a4, g4, e4, g4,
            a4, gs4, a4, e4, f4, g4,
            a4, f4, d4,
            a4, a4, a4, b4, c5,
            d5, b4, a4, g4, e4, d4,
            c4, c5, a4, g4, e4, g4, d4, d4,
            c4, d4, e4, f4, g4, a4,
            a4, b4, c5, c5, c5, b4, a4, g4,
            fs4, g4, a4, b4, c5
        };

        StartCoroutine(PlayForTime(correct, duration, 0, 8));
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

    public IEnumerator PlayForTime(GameObject[] notes, float[] durations, int start, int stop)
    {   AudioSource audio1;
        for(int i = start; i<stop; i++){
            audio1 = notes[i].GetComponent<AudioSource>();
            //audio1.time = audio1.clip.length -
            audio1.Play();
            //audio1.SetScheduledEndTime(t0 + duration[i]);
            yield return new WaitForSeconds(durations[i]);

            //Invoke("StopAudio",durations[i]);
        }
    }

    private IEnumerator LightUpKey(GameObject key, float time)
    {
        Color32 startColor;
        Color32 flashColor = Color.cyan;

        startColor = key.GetComponent<MeshRenderer>().material.color;
        key.GetComponent<MeshRenderer>().material.color = flashColor;
        yield return new WaitForSeconds(time);
        key.GetComponent<MeshRenderer>().material.color = startColor;
    }


    // private void StopAudio()
    // {
    //     GetComponent<AudioSource>().Stop();
    // }

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