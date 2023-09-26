using Bitgem.VFX.StylisedWater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TH.Core;
using System;

public class BuyoancyController : MonoBehaviour
{
    public Vector3 myDirection;
    public bool isFloating = false;
    public ComponentGetter<WaterVolumeHelper> WaterVolumeHelper = new ComponentGetter<WaterVolumeHelper>(TypeOfGetter.Global);
    //========moving======
    private float _randomSpeed;
    //=======buyonacy==========
    public float maxBuyoncyPower = 3f;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public ComponentGetter<Rigidbody> _rigidbody = new ComponentGetter<Rigidbody>(TypeOfGetter.This);
    //=======================

    private void Awake()
    {
        _randomSpeed = UnityEngine.Random.Range(1f, 1.5f);
    }

    void FixedUpdate()
    {
        moveDirection();
        buyoancy();
        SwitchState();
    }

    private void moveDirection()
    {
        _rigidbody.Get(gameObject).AddForce(myDirection * _randomSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isFloating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            isFloating = false;
        }
    }

    private void buyoancy()
    {
        if (!WaterVolumeHelper.Get(gameObject))
        {
            return;
        }
        if (WaterVolumeHelper.Get().GetHeight(transform.position) != null)
        {
            float difference = transform.position.y - (float)WaterVolumeHelper.Get().GetHeight(transform.position);
            if (difference < 0)
            {
                _rigidbody.Get(gameObject).AddForce(Vector3.up * floatingPower * (Mathf.Clamp(Mathf.Abs(difference), 0, maxBuyoncyPower)), ForceMode.Force);
                if (!isFloating)
                {
                    isFloating = true;
                }
            }
        }
    }

    void SwitchState()
    {
        if (isFloating)
        {
            _rigidbody.Get(gameObject).drag = underWaterDrag;
            _rigidbody.Get(gameObject).angularDrag = underWaterAngularDrag;
        }
        else
        {
            _rigidbody.Get(gameObject).drag = airDrag;
            _rigidbody.Get(gameObject).angularDrag = airAngularDrag;
        }
    }

}


