using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    bool isLoading = false;

    public void Load(int id)
    {
        if (isLoading)
            return;
        isLoading = true;

        SceneManager.LoadScene(id);
    }
}
