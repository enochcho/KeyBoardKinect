using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
public class GamePlay : MonoBehaviour 
{
    //Collects data from Kinect
    public BodySourceManager mBodySourceManager;
    public Text gestureInfo;

    
    // GUI Text to display the score.
    public Text scoreInfo;

    // Song to play
    Song currentSong;

    // GameObjects
    public GameObject nextbutton;
    public GameObject spaceship;
    public GameObject selectedKey;
    public GameObject homebutton;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;

    private bool keyClicked = false;
    // Variables for tracking the user's progress
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

    //will track the uLong of the main player. This allows for the spaceship cursor to lock onto one player
    //regardless of whether there are other people in the frame. 
    ulong playerID = 999;

    //A list of the joints the kinect should track for each body.
    private List<Kinect.JointType> _joints;
    //This method will run everytime the script is loaded before anything else. 
    void Start()
    {
        notecolors = PlayerPrefs.GetString("noteColors", "rg");
        //Debug.Log(notecolors);

        nextbutton = GameObject.Find("Next");
        homebutton = GameObject.Find("Home");

        currentSong= new Song(PlayerPrefs.GetInt("songNum"),PlayerPrefs.GetFloat("noteSpeed", 1f));
        

        // set all of the arrays here
        player = new Key[currentSong.getNumNotes()];
        correct = currentSong.getSongNotes();
        duration = currentSong.getKeyDurations();
        notesPerLevel = currentSong.getNotesPerLevel();
        notesPerLevelSum = currentSong.getNotesPerLevelSum();

        // set score text
        scoreInfo.text = "Score:";

        //set left or right hand
        //Look at the options script for more information on PlayerPrefs
        if(PlayerPrefs.GetString("hand", "right") == "right"){
           _joints = new List<Kinect.JointType>{Kinect.JointType.HandRight,};
        } else{ 
            _joints = new List<Kinect.JointType>{Kinect.JointType.HandLeft,};
        }

        //This starts the Simon Says aspect of the game.
        StartCoroutine(PlayForTime(correct, duration, 0, notesPerLevel[0]));
    }

    //Holds the bodies that the Kinect is tracking
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    

    //This constantly runs and checks for user input as well as processes data
    //from BodySourceManager. 
    void Update () 
    {
         #region Processing Clicks for Game
        bool canMoveOn = false;
        bool playMore = true;
        bool WorB = true;
        nextbutton.GetComponent<SpriteRenderer>().color = Color.grey;

        //Test--able to move the spaceship with the arrow keys 
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            spaceship.transform.position += move * (float)10.0 * Time.deltaTime;
        //endTest
        if (numNotesPlayed == notesPerLevel[level])
        {
            nextbutton.GetComponent<SpriteRenderer>().color = Color.white;
            canMoveOn = true;
            playMore = false;
            if (level == (notesPerLevel.Length - 1))
            {
                endGame = true;
            }
        }
        
        //when the clicker is clicked 
        if (Input.GetMouseButtonDown(0))
        {   
          //  Debug.Log("Pressed primary button.");
          //Raycasting is used to hit buttons.
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
                    //adding the notes the player presses to an aray to play back at the end 
                    player[notesPerLevelSum[level] + numNotesPlayed - 1] = selectedKey.GetComponent<Key>();
                    if (hit.transform.gameObject.name.Contains("#"))
                    {
                        WorB = false;
                    } else
                    {
                        WorB = true;
                    }
                    StartCoroutine(LightUpKey(selectedKey.GetComponent<Key>(), notesPerLevelSum[level] + numNotesPlayed - 1, WorB));
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
                       // Debug.Log("end game :P");
                        // For Practice => just move on
                        nextbutton.GetComponent<SpriteRenderer>().color = Color.grey;
                        canMoveOn = false;
                        Debug.Log(currentSong.getNumNotes());
                        Debug.Log(notesPerLevelSum[notesPerLevel.Length - 1]);
                        Debug.Log(currentSong.getNumNotesPractice());
                        StartCoroutine(PlayBack(player, duration, 0, currentSong.getNumNotes()));
                        nextbutton.SetActive(false); //get rid of next button so the player has to click home button
                       }
                }
            }
        }
        #endregion
        //Gets data from the Kinect 
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
                    //resets the "main player" to null value of 999
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
                
                //IF there is no main player, make the first body in data the main player
                if(playerID == 999){
                    playerID = body.TrackingId;
                    AssignSpaceship(body.TrackingId, mBodies[body.TrackingId]);
                }
                // Update position of spaceship cursor
                UpdateBodyObject(body, mBodies[body.TrackingId]);

            }
        }
        #endregion
       
    }

    //Creates a body for each body in the frame
    private GameObject CreateBodyObject(ulong id)
    {
        //Create body parent
        GameObject body = new GameObject("Body:" + id);

         //Creates joints if a main player does not exist. 
         //Basically does AssignSpaceship but for the very first player. 
        if(playerID == 999){
            //Sets the PlayerID as the trackingID the Kinect assigns to each body. 
            playerID = id;
            //Create joints
            foreach (Kinect.JointType joint in _joints)
            {
                //Assign object to joint. 
                spaceship.name = joint.ToString();

                //Parent to body
                spaceship.transform.parent = body.transform;

            }
        } 
        return body;
    }

    //Assigns Spaceship cursor to a certain body. 
    private void AssignSpaceship(ulong id, GameObject body){
        playerID = id;
        //Create joints
        foreach (Kinect.JointType joint in _joints)
            {
                //Assign object to joint.
                spaceship.name = joint.ToString();

                //Parent to body
                spaceship.transform.parent = body.transform;

            }
    }

    //Plays the note when clicked and flashes if it was right or not. 
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

    //at the end of the game, it plays back all the notes the player played
    //different colors for correct and incorrect 
    public IEnumerator PlayBack(Key[] notes, float[] durations, int start, int stop)
    {
        AudioSource audio1;
        Key key;
        Color32 startColor;
        Color32 flashColor; 
        if(notecolors == "rg"){
            flashColor = Color.green;
        } else{
            flashColor = Color.magenta;
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
                    flashColor = Color.grey;
                }
            }
            if (String.Compare(key.name, correct[i].name) == 0)
            {
                if(notecolors == "rg"){
                    flashColor = Color.green;
                } else{
                    flashColor = Color.magenta;
                }
                score += 100;
            }

            String scoreText = "Score: " + score.ToString();    //updating score
            scoreInfo.text = scoreText;
            audio1 = notes[i].GetComponent<AudioSource>();
            audio1.Play();
            startColor = key.GetComponent<SpriteRenderer>().color;
            key.GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSeconds(durations[i]);
            key.GetComponent<SpriteRenderer>().color = startColor;
        }
    }

    public IEnumerator LightUpKey(Key key, int location, bool WorB)
    {
        Color32 startColor;
        Color32 flashColor;
        if (notecolors == "rg")
        {
            flashColor = Color.green;
        }
        else
        {
            flashColor = Color.magenta;
        }
        if (String.Compare(key.name, correct[location].name) != 0)
            {

                if (notecolors == "rg")
                {
                    flashColor = Color.red;
                }
                else
                {
                    flashColor = Color.grey;
                }
            }
            if (String.Compare(key.name, correct[location].name) == 0)
            {
                if (notecolors == "rg")
                {
                    flashColor = Color.green;
                }
                else
                {
                    flashColor = Color.magenta;
                }
            }
        // Set key color based on boolean
        if (WorB)
        {
            startColor = Color.white;
        } else
        {
            startColor = Color.black;
        }
        const float secToIncrement = 1f; //When to time out (Every 1 second)
        float counter = 0;
        bool loop = true;
        while (loop)
        {
            key.GetComponent<SpriteRenderer>().color = flashColor;
            // Check if we have reached time limit
            if (counter > secToIncrement)
            {
                // Time out flash
                key.GetComponent<SpriteRenderer>().color = startColor;
                loop = false;
                yield break;
            }

            // Increment counter
            counter += Time.deltaTime;

            // Check if we want to exit coroutine early due to mouse click
            if (Input.GetMouseButtonDown(0) && counter > 0.02)
            {
                Debug.Log("Color flash is broken");
                key.GetComponent<SpriteRenderer>().color = startColor;
                loop = false;
                yield break;
            }

            //Yield in a while loop to prevent freezing 
            yield return null;
        }
        // Literally a just in case thing
        key.GetComponent<SpriteRenderer>().color = startColor;
    }
    
    //Sets the position of the spaceship object
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
            //Adjust the second paramter of Vector3.Lerp to adjust how much one needs to move their arm/body. 
            jointObject.transform.position = Vector3.Lerp(jointObject.transform.position, targetPosition*3, 3*Time.deltaTime);
        }
    }
    
    private Vector3 GetVector3FromJoint (Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

}
