using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        int amountMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        
        if (amountMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else 
        { 
            DontDestroyOnLoad(gameObject);
        }
    }

}
