using System;
using System.Collections;
using System.Collections.Generic;
using TH.Core;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    #region PublicVariables
    public BlockDataSO Data => _blockData;
    #endregion

    #region PrivateVariables
    [SerializeField] private BlockDataSO _blockData;
    private Action<FloatingBlock> _onFloatingBlockClicked;
    private bool _hasInit = false;
    public Rigidbody _rb;
    private bool _isMoving;
    private bool _isFlying = false;
    private bool _isDestroying = false;
    private float _yPos;
    private float _destroyYpos = -10f;

    private float _flyingSpeed = 5;
    private float _randomSpeed;
    public Vector3 myDirection = Vector3.down;

    public ObjectGetter _bigUI = new ObjectGetter(TypeOfGetter.ChildByName, "Canvas/Big");
    public ObjectGetter _smallUI = new ObjectGetter(TypeOfGetter.ChildByName, "Canvas/Small");
    #endregion

    #region PublicMethod
    public void Init(BlockDataSO blockData, Action<FloatingBlock> onFloatingBlockClicked)
    {
        _blockData = blockData;
        _onFloatingBlockClicked = onFloatingBlockClicked;
        _yPos = GridManager.Instance.CurrentCenterLevel - 2f;
        _hasInit = true;

        _rb = GetComponent<Rigidbody>();
    }
    #endregion

    #region PrivateMethod


    private void OnMouseOver()
    {
        _smallUI.Get(gameObject).SetActive(false);
        _bigUI.Get(gameObject).SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
            
                StartCoroutine(destroyObject());
            }
        
    }

    private void OnMouseExit()
    {
        _smallUI.Get(gameObject).SetActive(true);
        _bigUI.Get(gameObject).SetActive(false);
    }

    private void Awake()
    {
        _randomSpeed = UnityEngine.Random.Range(5f, 12f);
    }
    private void moveDirection()
    {
        _rb.AddForce(myDirection * _randomSpeed);
    }
    private void FixedUpdate()
    {
        if (transform.position.y < _destroyYpos)
        {
            Destroy(gameObject);
        }
        if (!_isFlying)
        {
            moveDirection();
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }
    private IEnumerator destroyObject()
    {
        _rb.useGravity = true;
        _isDestroying = true;
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }
    #endregion

}