using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class cGameMainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] Pannels;
    private void Start()
    {
        for(int i = 0; i < Pannels.Length; i ++)
            Pannels[i].SetActive(false);

        Pannels[0].SetActive(true);
    }
    public void ChangeScene(string _Name)
    {
        SceneManager.LoadScene(_Name);
    }
    public void OpenPannel(int _PannelInDex)
    {
        for (int i = 0; i < Pannels.Length; i++)
            Pannels[i].SetActive(false);

        Pannels[_PannelInDex].SetActive(true);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
