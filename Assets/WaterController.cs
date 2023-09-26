using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region PublicVariables
    
    public float _initialSpeed = 0.01f;
    public float _acceleration = 0.01f;
    public float _maxSpeed = 0.1f; // Maximum speed
    public float _stageDuration = 10f;

    #endregion

    #region PrivateVariables
    #endregion

    #region PublicMethod

    private GameObject _waterGroup;
    private float _startTime;
    private float _stageEndTime;
    private float _currentSpeed;

    #endregion

    #region PrivateMethod

    private void Start()
    {
        _waterGroup = GameObject.FindGameObjectWithTag("Water");
        _startTime = Time.time;
        _stageEndTime = _startTime + _stageDuration;
        _currentSpeed = _initialSpeed;
    }

    private void Update()
    {
        float _elapsedTime = Time.time - _startTime;

        if (Time.time > _stageEndTime)
        {
            _startTime = Time.time;
            _stageEndTime = _startTime + _stageDuration;
            _currentSpeed = Mathf.Min(_currentSpeed + _acceleration, _maxSpeed);
        }

        float _newYPos = _waterGroup.transform.position.y + _currentSpeed * Time.deltaTime;

        _waterGroup.transform.position = new Vector3(_waterGroup.transform.position.x, _newYPos, _waterGroup.transform.position.z);
    }
    #endregion
}