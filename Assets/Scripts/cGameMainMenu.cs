using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class cGameMainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject MusicChoosePannel;
    [SerializeField]
    GameObject MainButtons;
    private void Start()
    {
        MainButtons.SetActive(true);
        MusicChoosePannel.SetActive(false);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
    public void MusicSelect()
    {
        MainButtons.SetActive(false);
        MusicChoosePannel.SetActive(true);
    }
    public void ReturnToMain()
    {
        MainButtons.SetActive(true);
        MusicChoosePannel.SetActive(false);
    }
}
