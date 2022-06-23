using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCamera : MonoBehaviour
{
    [SerializeField]
    GameObject[] Cams;
    [SerializeField]
    float ShowNHideTime = 0.5f;

    bool IsCamShow = false;
    private void Start()
    {
        for (int i = 0; i < Cams.Length; i++)
        {
            Cams[i].SetActive(false);
        }
    }
    public void CamShowFour()
    {
        if (IsCamShow == true) return;
        IsCamShow = true;
        for (int i = 0; i < Cams.Length; i++)
        {
            Cams[i].SetActive(false);
        }
        StartCoroutine("CamShowTime");
    }
    IEnumerator CamShowTime()
    {
        for (int i = 0; i < Cams.Length; i++)
        {
            Cams[i].SetActive(true);
            yield return new WaitForSeconds(ShowNHideTime);
        }
        //for (int i = Cams.Length - 1; i >= 0; i--)
        for (int i = 0; i < Cams.Length; i++)
        {
            Cams[i].SetActive(false);
            yield return new WaitForSeconds(ShowNHideTime);
        }
        IsCamShow = false;
        yield return null;
    }
    // Update is called once per frame
    
}
