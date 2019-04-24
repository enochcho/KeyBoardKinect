using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song
{
    Key[] songNotes;
    float[] keyDurations;
    int[] notesPerLevel;
    int[] notesPerLevelSum;
    enum possibleSongs {BallGame=0, BallPractice=1, BirthdaySong=2, BirthdayPractice=3, LondonBridge=4, LondonPractice=5}
    possibleSongs currentSong = 0;

    public static float tempo = PlayerPrefs.GetFloat("noteSpeed", 1f);

    //SetOfKeys allKeys;
    public Key b3, c4, cs4, d4, ds4, e4, f4, fs4, g4, gs4, a4, as4, b4, c5, cs5, d5;
    float dottedhalf = 1f * tempo;
    float quarter = .33f * tempo;
    float half = .66f * tempo;
    float tied = 2f * tempo;
    float whole = 1.33f * tempo;

    public Song(int songToPlay) {
        currentSong = (possibleSongs)songToPlay;
        Start();
    }

    public int getNumNotes()
    {
        return notesPerLevelSum[notesPerLevelSum.Length - 1];
    }

    public Key[] getSongNotes()
    {
        return songNotes;
    }

    public float[] getKeyDurations()
    {
        return keyDurations;
    }

    public int[] getNotesPerLevel()
    {
        return notesPerLevel;
    }

    public int[] getNotesPerLevelSum()
    {
        return notesPerLevelSum;
    }

    // Start is called before the first frame update
    void Start()
    {
        //allKeys = GameObject.Find("SetOfKeys").GetComponent<SetOfKeys>();
        b3 = GameObject.Find("Key B3").GetComponent<Key>();
        c4 = GameObject.Find("Key C4").GetComponent<Key>();
        cs4 = GameObject.Find("Key C#4").GetComponent<Key>();
        d4 = GameObject.Find("Key D4").GetComponent<Key>();
        ds4 = GameObject.Find("Key D#4").GetComponent<Key>();
        e4 = GameObject.Find("Key E4").GetComponent<Key>();
        f4 = GameObject.Find("Key F4").GetComponent<Key>();
        fs4 = GameObject.Find("Key F#4").GetComponent<Key>();
        g4 = GameObject.Find("Key G4").GetComponent<Key>();
        gs4 = GameObject.Find("Key G#4").GetComponent<Key>();
        a4 = GameObject.Find("Key A4").GetComponent<Key>();
        as4 = GameObject.Find("Key A#4").GetComponent<Key>();
        b4 = GameObject.Find("Key B4").GetComponent<Key>();
        c5 = GameObject.Find("Key C5").GetComponent<Key>();
        cs5 = GameObject.Find("Key C#5").GetComponent<Key>();
        d5 = GameObject.Find("Key D5").GetComponent<Key>();
        //startbutton = GameObject.Find("Start");
        switch (currentSong) {
            case possibleSongs.BallGame:
                songNotes = new Key[] {
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
                keyDurations = new float[] {
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
                notesPerLevel = new int[]
                {
                    7, 6, 6, 3, 5, 6, 8, 6, 8, 5
                };
                notesPerLevelSum = new int[]
                {
                    0, 7, 13, 19, 22, 27, 33, 41, 47, 55, 60
                };
                break;
            case possibleSongs.BallPractice:
                songNotes = new Key[]
                {
                    c4,
                    c4, c5,
                    c4, c5, a4,
                    c4, c5, a4, g4,
                    c4, c5, a4, g4, e4,
                    c4, c5, a4, g4, e4, g4,
                    c4, c5, a4, g4, e4, g4, d4
                };
                keyDurations = new float[] {
                    half,
                    half, quarter,
                    half, quarter, quarter,
                    half, quarter, quarter, quarter,
                    half, quarter, quarter, quarter, quarter,
                    half, quarter, quarter, quarter, quarter, dottedhalf,
                    half, quarter, quarter, quarter, quarter, dottedhalf, dottedhalf
                };
                notesPerLevel = new int[]
                {
                    1, 2, 3, 4, 5, 6, 7
                };
                notesPerLevelSum = new int[]
                {
                    0, 1, 3, 6, 10, 15, 21, 28
                };
                break;
            case possibleSongs.BirthdaySong:
                songNotes = new Key[]
                {
                    d4, d4, e4, d4, g4, fs4,
                    d4, d4, e4, d4, a4, g4,
                    d4, d4, d5, b4, g4, fs4, e4,
                    c5, c5, b4, g4, a4, g4
                };
                keyDurations = new float[] {
                    quarter, quarter, half, half, half, whole,
                    quarter, quarter, half, half, half, whole,
                    quarter, quarter, half, half, half, half, half,
                    quarter, quarter, half, half, half, whole
                };
                notesPerLevel = new int[]
                {
                    6, 6, 7, 6
                };
                notesPerLevelSum = new int[]
                {
                    0, 6, 12, 19, 25
                };
                break;
            case possibleSongs.BirthdayPractice:
                songNotes = new Key[]
                {
                    d4,
                    d4, d4,
                    d4, d4, e4,
                    d4, d4, e4, d4,
                    d4, d4, e4, d4, g4,
                    d4, d4, e4, d4, g4, fs4
                };
                keyDurations = new float[] {
                    quarter,
                    quarter, quarter,
                    quarter, quarter, half,
                    quarter, quarter, half, half,
                    quarter, quarter, half, half, half,
                    quarter, quarter, half, half, half, whole
                };
                notesPerLevel = new int[]
                {
                    1, 2, 3, 4, 5, 6
                };
                notesPerLevelSum = new int[]
                {
                    0, 1, 3, 6, 10, 15, 21
                };
                break;
            case possibleSongs.LondonBridge:
                songNotes = new Key[]
                {
                    g4, a4, g4, f4,
                    e4, f4, g4,
                    d4, e4, f4,
                    e4, f4, g4,
                    g4, a4, g4, f4,
                    e4, f4, g4,
                    d4, g4, e4, c4
                };
                keyDurations = new float[] {
                    quarter, quarter, quarter, quarter,
                    quarter, quarter, half,
                    quarter, quarter, half,
                    quarter, quarter, half,
                    quarter, quarter, quarter, quarter,
                    quarter, quarter, half,
                    half, half, quarter, dottedhalf
                };
                notesPerLevel = new int[]
                {
                    4, 3, 3, 3, 4, 3, 4
                };
                notesPerLevelSum = new int[]
                {
                    0, 4, 7, 10, 13, 17, 20, 24
                };
                break;
            case possibleSongs.LondonPractice:
                songNotes = new Key[]
                {
                    g4,
                    g4, a4,
                    g4, a4, g4,
                    g4, a4, g4, f4
                };
                keyDurations = new float[] {
                    quarter,
                    quarter, quarter,
                    quarter, quarter, quarter,
                    quarter, quarter, quarter, quarter
                };
                notesPerLevel = new int[]
                {
                    1, 2, 3, 4
                };
                notesPerLevelSum = new int[]
                {
                    0, 1, 3, 6, 10
                };
                break;
        }
        
    }

}
