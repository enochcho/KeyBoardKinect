using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
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
        

        // Set currentSong based on what the player chooses
       
        currentSong= new Song(PlayerPrefs.GetInt("songNum"),PlayerPrefs.GetFloat("noteSpeed", 1f));
       
        // set all of the arrays here
        player = new Key[currentSong.getNumNotes()];
        correct = currentSong.getSongNotes();
        duration = currentSong.getKeyDurations();
        notesPerLevel = currentSong.getNotesPerLevel();
        notesPerLevelSum = currentSong.getNotesPerLevelSum();

        // set score text
        scoreInfo.text = "Score:";

       //starts playing the selected song
        StartCoroutine(PlayForTime(correct, duration, 0, notesPerLevel[0]));
    }

    //these next 5 methods are from kinect manager-don't really do anything 
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
        if (progressDisplayed)
        {
            // clear the progress info
            if (gestureInfo != null)
                gestureInfo.text = String.Empty;

            progressDisplayed = false;
        }

        return true;
    }

    //plays the notes that the player is to copy 
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

    void Update()
    {
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
    }
}
