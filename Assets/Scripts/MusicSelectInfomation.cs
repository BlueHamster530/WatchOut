using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicSelectInfomation : MonoBehaviour
{
    [SerializeField]
    AudioClip Music;

    public void ButtonClicked()
    {
        cGameDataManager.instance.SetMusic(Music);
        SceneManager.LoadScene("GameStage");
    }
   //private void OnMouseEnter()
   //{
   //    if (Input.GetMouseButtonDown(0))
   //    {
   //        ButtonClicked();
   //    }
   //}
}
