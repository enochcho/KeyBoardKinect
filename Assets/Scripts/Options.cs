using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Options : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // Start is called before the first frame update

    public Text gestureInfo;
    public GameObject ColorofKeys;
    public GameObject spaceship;
    public GameObject ColorA;
    public GameObject ColorB;
    public GameObject Tempo;
    public GameObject Home;
    public GameObject Slow, Normal, Fast;
    private bool progressDisplayed;
    private bool redgreen;

    private String noteSpeed;

    void Start()
    {
        // nextbutton = GameObject.Find("Next");
        ColorofKeys = GameObject.Find("ColorofKeys");
        ColorA = GameObject.Find("ColorA");
        ColorB= GameObject.Find("ColorB");
        Home = GameObject.Find("Home");
        Slow = GameObject.Find("Slow");
        Normal = GameObject.Find("Normal");
        Fast = GameObject.Find("Fast");
        Tempo = GameObject.Find("Tempo");
        if(PlayerPrefs.GetString("noteColors","rg") == "rg"){
            redgreen = true;
            ColorA.GetComponent<Text>().color = Color.red;
            ColorA.GetComponent<Text>().text = "RED";
            ColorB.GetComponent<Text>().color = Color.green;
            ColorB.GetComponent<Text>().text = "GREEN";
            
        } else {
            redgreen = false;
            ColorA.GetComponent<Text>().color = Color.magenta;
            ColorA.GetComponent<Text>().text = "MAGENTA";
            ColorB.GetComponent<Text>().color = Color.grey;
            ColorB.GetComponent<Text>().text = "GREY";
        }

        if (PlayerPrefs.GetFloat("noteSpeed", 1f) == 1f) {
            noteSpeed = "normal";
            Tempo.GetComponent<Text>().text = "Normal";
        } else if (PlayerPrefs.GetFloat("noteSpeed", 1f) == 0.5f) {
            noteSpeed = "fast";
            Tempo.GetComponent<Text>().text = "Fast";
        } else if (PlayerPrefs.GetFloat("noteSpeed", 1f) == 2.0f) {
           noteSpeed = "slow";
           Tempo.GetComponent<Text>().text = "Slow";
        } else {
            noteSpeed = "normal";
        }
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
                if (hit.transform.gameObject.name.Contains("ColorofKeys"))
                { Debug.Log("Hit");
                    if (redgreen) {
                        redgreen = false;
                        ColorA.GetComponent<Text>().color = Color.magenta;
                        ColorA.GetComponent<Text>().text = "MAGENTA";
                        ColorB.GetComponent<Text>().color = Color.grey;
                        ColorB.GetComponent<Text>().text = "GREY";
                        PlayerPrefs.SetString("noteColors","mg");
                        
                    } else {
                        redgreen = true;
                        ColorA.GetComponent<Text>().color = Color.red;
                        ColorA.GetComponent<Text>().text = "RED";
                        ColorB.GetComponent<Text>().color = Color.green;
                        ColorB.GetComponent<Text>().text = "GREEN";
                        PlayerPrefs.SetString("noteColors","rg");
                    }
                    
                } else if (hit.transform.gameObject.name.Contains("Fast")) {
                    Debug.Log("Fast");
                    noteSpeed = "fast";
                    Tempo.GetComponent<Text>().text = "Fast";
                    PlayerPrefs.SetFloat("noteSpeed", 0.5f);
                } else if (hit.transform.gameObject.name.Contains("Normal")) {
                    Debug.Log("Normal");
                    noteSpeed = "normal";
                    Tempo.GetComponent<Text>().text = "Normal";
                    PlayerPrefs.SetFloat("noteSpeed", 1f);
                } else if (hit.transform.gameObject.name.Contains("Slow")) {
                    Debug.Log("Slow");
                    noteSpeed = "slow";
                    Tempo.GetComponent<Text>().text = "Slow";
                    PlayerPrefs.SetFloat("noteSpeed", 2f);
                } else if (hit.transform.gameObject.name.Contains("Home")){
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }
}
