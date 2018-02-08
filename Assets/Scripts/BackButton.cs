using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
    public GameObject image;

    public void onClick()
    {
        image = GameObject.Find("InfoImage");
        image.SetActive(false);
    }
}
