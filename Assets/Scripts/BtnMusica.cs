using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnMusica : MonoBehaviour {
    private Text onofftext;
    
    public void MusicOnOff()
    {
        onofftext = GameObject.Find("Text").GetComponent<Text>();
        if (onofftext.text == "Music On") {
            onofftext.text = "Music Off";
            SoundManager.instancia.musicSource.Stop();
        }
        else if (onofftext.text == "Music Off")
        {
            onofftext.text = "Music On";
            SoundManager.instancia.musicSource.Play();
        }
    }
}
