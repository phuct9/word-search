using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void muteToggleBackgroundMusic()
    {
        SoundManager.instance.toggleBackgroundMusic();
    }

    public void muteToggleSoundFX()
    {
        SoundManager.instance.toggleSoundFx();
    }


}
