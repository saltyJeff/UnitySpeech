using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Demo : UnitySpeech {
    public Text resultText;
    // Use this for initialization
    public override void Start () {
        base.Start(); //always call them parent constructors
	}
    public void pingOpen () {
        beginListening(); //begin listening starts the ping sound
    }
    public override void errNoSpeechRecognition() {
        //speech recognition software is not found (maybe send a link to download google voice typing)
    }

    public override void errVoiceTypingDisabled() {
        //voice typing is disabled
    }

    public override void onPartialResults(string[] words) {
        //the intermediate results
        string toPrint = "";
        foreach(string s in words) {
            toPrint += s + " ";
        }
        resultText.text = toPrint;
    }

    public override void onRmsChange(float value) {
        //idk what this is but it was in the lib
    }

    public override void onResult(string result) {
        //the final result
        resultText.text = result;
        talk(result);
    }

    public override void onSpeechStart() {
        //the ping sound played and its ready to take speech
    }
}
