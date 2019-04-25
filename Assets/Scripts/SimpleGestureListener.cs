using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
public class SimpleGestureListener : MonoBehaviour 
{
    
    public BodySourceManager mBodySourceManager;
    public Text gestureInfo;

    // GUI Text to display the score.
    public Text scoreInfo;

    // Song to play
    Song currentSong;

    // GameObjects
    public GameObject nextbutton;
    public GameObject spaceship;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    private bool keyClicked = false;
    public GameObject selectedKey;
    public GameObject homebutton;
    public GameObject skiptutorialbutton;
    public GameObject instructions;

    int numNotesPlayed = 0;
    int level = 0;
    bool endGame = false;
    int score = 0;
    // Arrays for songs 
    // Need a method to assign songs below to these arrays based on button clicked
    Key[] player;
    Key[] correct;
    float[] duration;
    int[] notesPerLevel;
    int[] notesPerLevelSum;

    String notecolors;

    void Start()
    {
        notecolors = PlayerPrefs.GetString("noteColors", "rg");
        //Debug.Log(notecolors);

        nextbutton = GameObject.Find("Next");
        homebutton = GameObject.Find("Home");
        skiptutorialbutton = GameObject.Find("Skip Tutorial");
        instructions = GameObject.Find("Instructions");
        

        // Set currentSong based on what the player chooses
        // Now defaults to Ballgame
        //currentSong = new Song(songNum);
        //currentSong = new Song(ChooseSong.songNum);
        currentSong= new Song(PlayerPrefs.GetInt("songNum"),PlayerPrefs.GetFloat("noteSpeed", 1f));
        //Debug.Log(PlayerPrefs.GetInt("songNum"));
        // set all of the arrays here
        player = new Key[currentSong.getNumNotes()];
        correct = currentSong.getSongNotes();
        duration = currentSong.getKeyDurations();
        notesPerLevel = currentSong.getNotesPerLevel();
        notesPerLevelSum = currentSong.getNotesPerLevelSum();

        // set score text
        scoreInfo.text = "Score:";

        // Maybe put spaceship elsewhere? Start on play?
        StartCoroutine(PlayForTime(correct, duration, 0, notesPerLevel[0]));
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
         bool canMoveOn = false;
        bool playMore = true;
        nextbutton.GetComponent<SpriteRenderer>().color = Color.grey;

        //Test
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            spaceship.transform.position += move * (float)10.0 * Time.deltaTime;
        //endTest
        if (numNotesPlayed == notesPerLevel[level])
        {
            //Debug.Log("can move on");
            nextbutton.GetComponent<SpriteRenderer>().color = Color.white;
            canMoveOn = true;
            playMore = false;
            if (level == (notesPerLevel.Length - 1))
            {
                endGame = true;
            }
        }
        if (keyClicked == true && selectedKey != null)// && correctlyPlaced.correctlyPlaced != true)
        {
            //selectedKey.transform.position = spaceship.transform.position;
            //gestureInfo.text = "don't move";
            //Debug.Log("clicked");
        }
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
                } else if (hit.transform.gameObject.name.Contains("Key") && playMore)
                {
                    keyClicked = true;
                    selectedKey = hit.transform.gameObject;
                    selectedKey.GetComponent<AudioSource>().Play();
                    numNotesPlayed++;
                    player[notesPerLevelSum[level] + numNotesPlayed - 1] = selectedKey.GetComponent<Key>();
                } else if (hit.transform.gameObject.name.Contains("Next") && canMoveOn)
                {
                    if (!endGame)
                    {
                        nextbutton.GetComponent<SpriteRenderer>().color = Color.grey;
                        level++;
                        numNotesPlayed = 0;
                        StartCoroutine(PlayForTime(correct, duration, notesPerLevelSum[level], notesPerLevelSum[level + 1]));
                    }
                    else
                    {
                        Debug.Log("end game");
                        // For Practice => just move on
                        nextbutton.GetComponent<SpriteRenderer>().color = Color.grey;
                        canMoveOn = false;
//think spaceship is the right number of notes-was just 60 before 
                        StartCoroutine(PlayBack(player, duration, 0, currentSong.getNumNotes()));
                        //StartCoroutine(PlayForTime(correct, duration, 0, notesPerLevel[0]));
//spaceship will just have the same skip tutorial for now...hopefully it will put the button back on screen 
                       // skiptutorialbutton.enabled = true;                     
                       }
                }
            }
        }
       
    }

        public IEnumerator PlayForTime(Key[] notes, float[] durations, int start, int stop)
    {
        AudioSource audio1;
        Key key;
        Color32 startColor;
        Color32 flashColor = Color.blue;
        for (int i = start; i < stop; i++)
        {
            key = notes[i];
            audio1 = notes[i].GetComponent<AudioSource>();
            audio1.Play();
            startColor = key.GetComponent<SpriteRenderer>().color;
            key.GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSeconds(durations[i]);
            key.GetComponent<SpriteRenderer>().color = startColor;
        }
    }


    public IEnumerator PlayBack(Key[] notes, float[] durations, int start, int stop)
    {
        AudioSource audio1;
        Key key;
        Color32 startColor;
        Color32 flashColor; 
        if(notecolors == "rg"){
            flashColor = Color.green;
        } else{
            flashColor = Color.gray;
        }

        for (int i = start; i < stop; i++)
        {
            key = notes[i];
            Debug.Log(key.name);
            Debug.Log(correct[i].name);
            if (String.Compare(key.name, correct[i].name) != 0)
            {
                
                if(notecolors == "rg"){
                    flashColor = Color.red;
                } else{
                    flashColor = Color.magenta;
                }
            }
            if (String.Compare(key.name, correct[i].name) == 0)
            {
                if(notecolors == "rg"){
                    flashColor = Color.green;
                } else{
                    flashColor = Color.gray;
                }
                score += 100;
            }

            String scoreText = "Score: " + score.ToString();
            scoreInfo.text = scoreText;
            audio1 = notes[i].GetComponent<AudioSource>();
            audio1.Play();
            startColor = key.GetComponent<SpriteRenderer>().color;
            key.GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSeconds(durations[i]);
            key.GetComponent<SpriteRenderer>().color = startColor;
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
