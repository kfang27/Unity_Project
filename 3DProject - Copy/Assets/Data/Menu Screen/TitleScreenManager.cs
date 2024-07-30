using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    //This function is called in the start button when clicked
    public void StartNetworkAsHost()
    {
        // Allows us to access the NetworkManager (from the gameobject called Network Manager)
        // in the scene since there's only one at a time
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        StartCoroutine(WorldSaveGameManager.instance.LoadNewGame());
    }
}
