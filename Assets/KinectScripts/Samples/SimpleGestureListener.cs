using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
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
         /*if (gesture == KinectGestures.Gestures.Click && progress > 0.3f)
        {
            string sGestureText = string.Format("capture {0:F1}% complete", progress * 100);

            if (gestureInfo != null)
                //   gestureInfo.text = sGestureText;
                progressDisplayed = true;
        }*/
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

    void Update()
    {
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
                       }
                }
            }
        }
    }
}
