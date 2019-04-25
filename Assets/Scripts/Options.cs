using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
public class Options : MonoBehaviour 
{
    
    public BodySourceManager mBodySourceManager;   
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

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<Kinect.JointType> _joints = new List<Kinect.JointType>
    {
        //Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight,
    };

    
    void Update () 
    {
        #region Get Kinect data
        Kinect.Body[] data = mBodySourceManager.GetData();

        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }

        #endregion

        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingID in knownIds)
        {
            if (!trackedIds.Contains(trackingID))
            {
                //set spaceship parnet to null so it doesn't get destroyed
                spaceship.transform.parent = null;
                //Destroy body object
                Destroy(mBodies[trackingID]);

                //Remove from list
                mBodies.Remove(trackingID);
            }
        }
        #endregion

        #region Create Kinect bodies
        foreach (var body in data)
        {
            //if no body, skip
            if(body == null)
                continue;

            if (body.IsTracked)
            {
                //IF body isn't tracked, create body
                if(!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                // Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);

            }
        }
        #endregion

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
    
    private GameObject CreateBodyObject(ulong id)
    {
        //Create body parent
        GameObject body = new GameObject("Body:" + id);

        //Create joints
        foreach (Kinect.JointType joint in _joints)
        {
            //Create object
            spaceship.name = joint.ToString();

            //Parent to body
            spaceship.transform.parent = body.transform;

        }
        return body;
    }

    private void UpdateBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        //Update joints
        foreach (Kinect.JointType _joint in _joints)
        {
            //Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z=-2;

            //Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            
            
            /* Debug.Log("new hand movement");
            Debug.Log(jointObject.transform.position);
            Debug.Log(Vector3.Scale(jointObject.transform.position, new Vector3(2,2,1)));
            Vector3 newp = Vector3.Scale(jointObject.transform.position, new Vector3(2,2,1));
            */
            jointObject.transform.position = Vector3.Lerp(jointObject.transform.position, targetPosition*2, 3*Time.deltaTime);
        }
    }
    
    private Vector3 GetVector3FromJoint (Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

}
