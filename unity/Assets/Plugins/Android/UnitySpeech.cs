using UnityEngine;
using System.Collections;

public abstract class UnitySpeech : MonoBehaviour {
    private AndroidJavaClass speech;
    private static bool single = true;
	/// <summary>
    /// Initializes the class (must call from child class's Start() )
    /// </summary>
	public virtual void Start () {
        if(!single) {
            throw new System.Exception("you can only have 1 derived class of UnitySpeech");
            return;
        }
        speech = new AndroidJavaClass("io.github.saltyjeff.unityspeech.UnitySpeech");
        speech.CallStatic("init", gameObject.name);
        single = true;
	}
    /// <summary>
    /// starts listening to user input
    /// </summary>
    public void beginListening () {
        speech.CallStatic("beginListening");
    }
    /// <summary>
    /// will be called when listening begins
    /// </summary>
    public abstract void onSpeechStart();
    private void hasRmsChange(string f) {
        onRmsChange(float.Parse(f));
    }
    /// <summary>
    /// will be called when the RMS changes (still don't know what that is)
    /// </summary>
    /// <param name="value">the new RMS</param>
    public abstract void onRmsChange(float value);
    /// <summary>
    /// will be called when there are partial results
    /// </summary>
    /// <param name="words">array of partials</param>
    public abstract void onPartialResults(string[] words);
    private void hasPartialResults () {
        using (AndroidJavaObject jo = speech.CallStatic<AndroidJavaObject>("getPartials")) {
            onPartialResults(AndroidJNIHelper.ConvertFromJNIArray<string[]>(jo.GetRawObject()));
        }
    }
    /// <summary>
    /// will be called when listening is over and results are in
    /// </summary>
    /// <param name="result">the result</param>
    public abstract void onResult(string result);
    /// <summary>
    /// will be called when the user doesn't have google voice typing installed
    /// </summary>
    public abstract void errNoSpeechRecognition();
    /// <summary>
    /// will be called when the user disabled google voice typing
    /// </summary>
    public abstract void errVoiceTypingDisabled();
    /// <summary>
    /// stops the listening
    /// </summary>
    public void stopListening () {
        speech.CallStatic("stopListening");
    }
    /// <summary>
    /// it's supposed to clear memory leaks, but I don't see much of a problem without it
    /// </summary>
    public void cleanup () {
        speech.CallStatic("cleanup");
    }
    /// <summary>
    /// speech synthesis
    /// </summary>
    /// <param name="words">the words to say</param>
    public void talk(string words) {
        speech.CallStatic("talk", words);
    }
}
