using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    void Awake()
    {
        Time.timeScale = 0.0f;
    }

     public void Go () {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
