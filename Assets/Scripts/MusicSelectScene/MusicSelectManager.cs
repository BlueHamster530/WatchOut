using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicSelectManager : MonoBehaviour
{
    public void ChangeScene(string _Name)
    {
        SceneManager.LoadScene(_Name);
    }
}
