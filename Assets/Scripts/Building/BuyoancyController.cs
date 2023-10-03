using Bitgem.VFX.StylisedWater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TH.Core;
using System;

public class BuyoancyController : MonoBehaviour
{
    public bool isFloating = false;
   //public ComponentGetter<WaterVolumeHelper> WaterVolumeHelper = new ComponentGetter<WaterVolumeHelper>(TypeOfGetter.Global);
    //========moving======
    //=======buyonacy==========
    public float maxBuyoncyPower = 3f;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public ComponentGetter<Rigidbody> _rigidbody = new ComponentGetter<Rigidbody>(TypeOfGetter.This);
    //=======================

    void FixedUpdate()
    {
        buyoancy();
        SwitchState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isFloating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isFloating = false;
        }
    }

    private void buyoancy()
    {
      if(isFloating == true){
            float difference = transform.position.y - GameObject.FindGameObjectWithTag("Water").transform.parent.position.y + 0.01f * Mathf.Sin(Time.time);
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


