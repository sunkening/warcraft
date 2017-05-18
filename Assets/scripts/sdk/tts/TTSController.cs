using UnityEngine;
using System.Collections;

public class TTSController : MonoBehaviour {
    private ITTS tts;
	// Use this for initialization
	void Start () {
        
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    tts = new BaiDuTTS();
                }
                break;
            case RuntimePlatform.WindowsEditor:
                {
                    tts = new PrintTTS();
                }
                break;
        }
    }
    public void speak(string text)
    {
        if (text==null||text.Equals(""))
        {
            return;
        }
        tts.speak(text);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
