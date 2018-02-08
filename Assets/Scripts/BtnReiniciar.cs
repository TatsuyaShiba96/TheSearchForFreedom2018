using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BtnReiniciar : MonoBehaviour {
    private GameObject btn;

    void Start()
    {
        btn = GameObject.Find("BtnReiniciar");
        btn.SetActive(false);
    }

    public void Reiniciar()
    {
        GameManager.instancia.LevelReset();
    }
}