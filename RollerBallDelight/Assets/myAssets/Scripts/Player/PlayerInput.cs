using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RockVR.Video;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private float verticalInput;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float _JumpCharge;

    [Header("Jump Variables")]
    [SerializeField] private float initialJumpMultiplier;
    [SerializeField] private float jumpChargeSpeed;
    private float _JumpMulitplier;
    


    // Start is called before the first frame update
    void Start()
    {
        _JumpCharge = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _JumpCharge = initialJumpMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _JumpMulitplier = _JumpCharge;
            _JumpCharge = 0f;
        }
        else if (Input.GetKey(KeyCode.Space) && _JumpCharge < 1f)
        {
            _JumpCharge += Time.deltaTime * jumpChargeSpeed;
            if (_JumpCharge > 1f)
                _JumpCharge = 1f;
        }
        HandleCameraInputs();
        HandleDemoInputs();
    }

    void HandleCameraInputs()
    {
        if (VideoCaptureCtrl.instance.status == VideoCaptureCtrlBase.StatusType.FINISH)
            Debug.Log("Done Recording!");

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (VideoCaptureCtrl.instance.status == VideoCaptureCtrlBase.StatusType.NOT_START)
            {
                VideoCaptureCtrl.instance.StartCapture();
                Debug.Log("Started Recording!");
            }

            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrlBase.StatusType.STARTED)
            {
                Debug.Log("Stopping Recording!");
                VideoCaptureCtrl.instance.StopCapture();
            }
        }
    }

    void HandleDemoInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyUp(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }


    public float jumpMultiplier()
    {
        float returnValue = _JumpMulitplier;
        _JumpMulitplier = 0f;
        return returnValue;
        
    }
    public float jumpCharge (){return _JumpCharge;}

    public float vertical (){return verticalInput;}

    public float horizontal (){return horizontalInput;}

}
