using Bitgem.VFX.StylisedWater;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyoancyController : MonoBehaviour
{
    public bool isFloating = false;
    public WaterVolumeHelper WaterVolumeHelper = null;
    //====================
    public float maxBuyoncyPower = 3f;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;
    public Rigidbody rigidbody;
    //=======================

    private void Awake()
    {
        WaterVolumeHelper = FindObjectOfType<WaterVolumeHelper>();
    }

    void FixedUpdate()
    {

        buyoancy();
        SwitchState();

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
        if (!WaterVolumeHelper)
        {
            return;
        }

        if (WaterVolumeHelper.GetHeight(transform.position) != null)
        {
            float difference = transform.position.y - (float)WaterVolumeHelper.GetHeight(transform.position);
            if (difference < 0)
            {
                rigidbody.AddForce(Vector3.up * floatingPower * (Mathf.Clamp(Mathf.Abs(difference), 0, maxBuyoncyPower)), ForceMode.Force);
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
            rigidbody.drag = underWaterDrag;
            rigidbody.angularDrag = underWaterAngularDrag;
        }
        else
        {
            rigidbody.drag = airDrag;
            rigidbody.angularDrag = airAngularDrag;
        }
    }

}


