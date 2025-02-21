using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class LevelReloader : MonoBehaviour
{
    public bool isEnabled = true;
    private void Update()
    {
        //reset current scene
        if (Input.GetKeyDown(KeyCode.R) && isEnabled)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
