using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float ShakeAmount = 0.4f;
    [SerializeField]
    private float ShakeAngleAmount = 45.0f;
    private float ShakeTime;
    [SerializeField]
    private float testanglespeedgo = 2.0f;
    [SerializeField]
    private float testanglespeedback = 1.0f;
    private bool istest = false;
    Vector3 initalPosition;

    float InitalProjectionSize;

    [SerializeField]
    private float StartorthographicSize = 5.0f;
    [SerializeField]
    private float TargetorthographicSize = 5.0f;
    [SerializeField]
    private float TargetOrtSizeTime = 1.0f;
    [SerializeField]
    private float ReturnOrtSizeTime = 1.0f;
    private float CurrentOrtTime = 0.0f;
    private float OrtreturnTime = 0;
    private bool IsOrtSizeChanged = false;
    Camera Cam;

    MultipleCamera multiCam;

    int _IsLeft;
    float _TargetAngle;
    Vector3 _OriginCamAngle;
    Vector3 _TargetCamAngle;
    float _CurrnetTime = 0;
    int _type = 0;

    public void VivrateForTime(float _time)
    {
        ShakeTime = _time;
        if (istest == false)
        {
            _IsLeft = Random.Range(0, 2);
            _TargetAngle = ((float)(_IsLeft) - 0.5f) * 2.0f * ShakeAngleAmount;
            _OriginCamAngle = Cam.transform.eulerAngles;
            _TargetCamAngle = new Vector3(0, 0, _TargetAngle);
            _CurrnetTime = 0;
            _type = 0;
            istest = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cam = GetComponent<Camera>();
        multiCam = GetComponentInChildren<MultipleCamera>();
        initalPosition = this.transform.position;
        InitalProjectionSize = Cam.orthographicSize;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            VivrateForTime(0.1f);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CloseUpForTime(StartorthographicSize, TargetorthographicSize, TargetOrtSizeTime, ReturnOrtSizeTime);
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    multiCam.CamShowFour();
        //}
        if (istest == true)
            ChangeCamShakeAngle();
    }
    public void CloseUpForTime(float StartortSize, float TartgetortSize, float _time=1.0f, float returnTime = 0.0f)
    {
        CancelInvoke("ResetCamOrtSize");
        StartorthographicSize = StartortSize;
        TargetorthographicSize = TartgetortSize;
        Cam.orthographicSize = StartorthographicSize;
        TargetOrtSizeTime = _time;
        OrtreturnTime = returnTime;
        IsOrtSizeChanged = true;
        CurrentOrtTime = 0.0f;
    }
    private void ChangeCamShakeAngle()
    {
        if (_type == 0)
        {
            _CurrnetTime += Time.deltaTime * testanglespeedgo;
            Cam.transform.eulerAngles = Vector3.Lerp(_OriginCamAngle, _TargetCamAngle, _CurrnetTime);
            if (_CurrnetTime >= 1.0f)
                _type = 1;
        }
        if (_type == 1)
        {
            _CurrnetTime -= Time.deltaTime * testanglespeedback;
            Cam.transform.eulerAngles = Vector3.Lerp(_OriginCamAngle, _TargetCamAngle, _CurrnetTime);
            if (_CurrnetTime <= 0.0f)
            {
                _type = 0;
                istest = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initalPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initalPosition;
        }
        if (IsOrtSizeChanged == true)
        {
            CurrentOrtTime += Time.deltaTime / TargetOrtSizeTime;
            Cam.orthographicSize = Mathf.Lerp(StartorthographicSize, TargetorthographicSize, CurrentOrtTime);
            if (CurrentOrtTime >= 1)
            {
                Invoke("ResetCamOrtSize", OrtreturnTime);
                CurrentOrtTime = 0;
                IsOrtSizeChanged = false;
            }
        }
    }
    private void ResetCamOrtSize()
    {
        Cam.orthographicSize = InitalProjectionSize;
    }
}
