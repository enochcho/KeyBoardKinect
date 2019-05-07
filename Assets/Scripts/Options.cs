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
    public GameObject Hand, HandChoice;
    private bool progressDisplayed;
    private bool redgreen;
    private ulong playerID=999;

    private bool righthand;
    private List<Kinect.JointType> _joints;
    

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
        Hand = GameObject.Find("Hand");
        HandChoice = GameObject.Find("HandChoice");
        if(PlayerPrefs.GetString("noteColors","rg") == "rg"){
            redgreen = true;
            ColorA.GetComponent<Text>().color = Color.red;
            ColorA.GetComponent<Text>().text = "RED";
            ColorB.GetComponent<Text>().color = Color.green;
            ColorB.GetComponent<Text>().text = "GREEN";
            
        } else {
            redgreen = false;
            ColorA.GetComponent<Text>().color = Color.gray;
            ColorA.GetComponent<Text>().text = "Gray";
            ColorB.GetComponent<Text>().color = Color.magenta;
            ColorB.GetComponent<Text>().text = "MAGENTA";
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

        if(PlayerPrefs.GetString("hand", "right") == "right"){
            Hand.GetComponent<Text>().text = "Right";
           _joints = new List<Kinect.JointType>{Kinect.JointType.HandRight,};
           righthand= true;
        } else{
            Hand.GetComponent<Text>().text = "Left";
            _joints = new List<Kinect.JointType>{Kinect.JointType.HandLeft,};
            righthand= false;

        }
    }

    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
  

    
    void Update () 
    {
        #region Process Clicks
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
                        ColorA.GetComponent<Text>().color = Color.gray;
                        ColorA.GetComponent<Text>().text = "GRAY";
                        ColorB.GetComponent<Text>().color = Color.magenta;
                        ColorB.GetComponent<Text>().text = "MAGENTA";
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
                    //pprefs lets you set properties such as this note speed, so it's saved in the other scenes
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
                } else if (hit.transform.gameObject.name.Contains("HandChoice")){
                   
                    if(righthand){
                        Hand.GetComponent<Text>().text = "Left";
                        //_joints = new List<Kinect.JointType>{Kinect.JointType.HandLeft,};
                        righthand= false;
                        PlayerPrefs.SetString("hand", "left");
                    } else{
                        Hand.GetComponent<Text>().text = "Right";
                        //_joints = new List<Kinect.JointType>{Kinect.JointType.HandRight,};
                        righthand= true;
                        PlayerPrefs.SetString("hand", "right");
                    }
                }
            }
        }
        #endregion

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
                //if the main player disappears
                if(trackingID == playerID){
                    //set spaceship parent to null so it doesn't get destroyed
                    spaceship.transform.parent = null;
                    playerID = 999;
                }
                //Destroy body object
                Destroy(mBodies[trackingID]);

                //Remove from list
                mBodies.Remove(trackingID);
            }
        }
        #endregion

        #region Create and update Kinect bodies
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
                
                if(playerID == 999){
                    playerID = body.TrackingId;
                    AssignSpaceship(body.TrackingId, mBodies[body.TrackingId]);
                }
                // Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);

            }
        }
        #endregion
       
    }

    private GameObject CreateBodyObject(ulong id)
    {
        //Create body parent
        GameObject body = new GameObject("Body:" + id);

         //Creates joints if a player does not exist
        if(playerID == 999){
            playerID = id;
            //Create joints
            foreach (Kinect.JointType joint in _joints)
            {
                //Create object
                spaceship.name = joint.ToString();

                //Parent to body
                spaceship.transform.parent = body.transform;

            }
        } 
        return body;
    }

    private void AssignSpaceship(ulong id, GameObject body){
        playerID = id;
        //Create joints
        foreach (Kinect.JointType joint in _joints)
            {
                //Create object
                spaceship.name = joint.ToString();

                //Parent to body
                spaceship.transform.parent = body.transform;

            }
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
