package io.github.saltyjeff.unityspeech;

import android.app.Activity;

import com.unity3d.player.UnityPlayer;

import net.gotev.speech.*;

import java.util.List;

public final class UnitySpeech {
    private static String gameObject = "UnitySpeech";
    private static Activity activity;
    private static String[] partials;
    public static final String TAG = "UnitySpeech";
    private UnitySpeech () {
        //its static
    }
    public static void init () {
        activity = UnityPlayer.currentActivity;
        activity.runOnUiThread(new Runnable () {
            @Override
            public void run() {
                Speech.init(activity.getApplication());
                Speech.getInstance().setPreferOffline(true);
            }
        });
    }
    public static void init (String go) {
        gameObject = go;
        init();
    }
    //when there are partials, call this guy to get the array
    public static String[] getPartials() {
        return partials;
    }
    public static void beginListening () {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if(Speech.getInstance().isListening()) {
                    return;
                }
                try {
                    // you must have android.permission.RECORD_AUDIO granted at this point
                    Speech.getInstance().startListening(new SpeechDelegate() {
                        //fires when speech recognition starts
                        @Override
                        public void onStartOfSpeech() {
                            UnityPlayer.UnitySendMessage(gameObject, "onSpeechStart", "");
                        }
                        //fires on whatever RMS is
                        @Override
                        public void onSpeechRmsChanged(float value) {
                            //can someone please expalin what RMS is????
                            UnityPlayer.UnitySendMessage(gameObject, "hasRmsChange", Float.toString(value));
                        }
                        //fires on partial results
                        @Override
                        public void onSpeechPartialResults(List<String> results) {
                            partials = (String[]) results.toArray();
                            UnityPlayer.UnitySendMessage(gameObject, "hasPartialResults", "");
                        }
                        //fires when results are final
                        @Override
                        public void onSpeechResult(String result) {
                            UnityPlayer.UnitySendMessage(gameObject, "onResult", result);
                        }
                    });
                }
                //fires when there's no speech recognition software
                catch (SpeechRecognitionNotAvailable e) {
                    UnityPlayer.UnitySendMessage(gameObject, "errNoSpeechRecognition", "");
                }
                //fires when google voice typing is disabled
                catch (GoogleVoiceTypingDisabledException e) {
                    UnityPlayer.UnitySendMessage(gameObject, "errVoiceTypingDisabled", "");
                }
            }
        });
    }
    //stops listening
    public static void stopListening () {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if(!Speech.getInstance().isListening()) {
                    return;
                }
                Speech.getInstance().stopListening();
            }
        });
    }
    //cleans up
    public static void cleanup () {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Speech.getInstance().unregisterDelegate();
            }
        });
    }
    public static void talk (String words) {
        final String scopeShift = words;
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Speech.getInstance().say(scopeShift);
            }
        });
    }
}
