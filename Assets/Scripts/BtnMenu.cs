using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BtnMenu : MonoBehaviour {
    private GameObject btn;
    private GameObject image;

    void Start()
    {
        btn = GameObject.Find("BtnMenu");
        btn.SetActive(false);
    }

    public void onClick()
    {
        //image.SetActive(true);
        GameManager.instancia.LevelReset();
        SceneManager.LoadScene("MenuJuego");
    }
}