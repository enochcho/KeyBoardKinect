using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently not in use, but good for modularity when we get the chance
public class SetOfKeys : MonoBehaviour
{
    // Keys
    public Key b3, c4, cs4, d4, ds4, e4, f4, fs4, g4, gs4, a4, as4, b4, c5, cs5, d5;
    // Start is called before the first frame update
    void Start()
    {
        b3 = (Key)GameObject.Find("Key B3").GetComponent<Key>();
        c4 = (Key)GameObject.Find("Key C4").GetComponent<Key>();
        cs4 = (Key)GameObject.Find("Key C#4").GetComponent<Key>();
        d4 = (Key)GameObject.Find("Key D4").GetComponent<Key>();
        ds4 = (Key)GameObject.Find("Key D#4").GetComponent<Key>();
        e4 = (Key)GameObject.Find("Key E4").GetComponent<Key>();
        f4 = (Key)GameObject.Find("Key F4").GetComponent<Key>();
        fs4 = (Key)GameObject.Find("Key F#4").GetComponent<Key>();
        g4 = (Key)GameObject.Find("Key G4").GetComponent<Key>();
        gs4 = (Key)GameObject.Find("Key G#4").GetComponent<Key>();
        a4 = (Key)GameObject.Find("Key A4").GetComponent<Key>();
        as4 = (Key)GameObject.Find("Key A#4").GetComponent<Key>();
        b4 = (Key)GameObject.Find("Key B4").GetComponent<Key>();
        c5 = (Key)GameObject.Find("Key C5").GetComponent<Key>();
        cs5 = (Key)GameObject.Find("Key C#5").GetComponent<Key>();
        d5 = (Key)GameObject.Find("Key D5").GetComponent<Key>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
