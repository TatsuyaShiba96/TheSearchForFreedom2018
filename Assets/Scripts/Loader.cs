using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
    public GameObject gameManager;

    void Awake() {
        if (GameManager.instancia == null)
            Instantiate(gameManager);
    }
}
