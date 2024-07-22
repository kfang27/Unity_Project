using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    // The index of 1 refers to the scene index in the build settings (starting from 0, aka main menu)
    [SerializeField] int worldSceneIndex = 1;

    private void Awake()
    {
        // There can only be one instance of this SaveGame manager
        if (instance == null)
        {
            instance = this;
        }
        // If there is another manager script, destroy it
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadNewGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
