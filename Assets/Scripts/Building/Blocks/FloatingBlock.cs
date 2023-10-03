using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core
{

    public class FloatingBlock : MonoBehaviour
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
        public Vector3 myDirection;

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
            if (_hasInit == false)
            {
                return;
            }

			if (GridManager.Instance.State == GridManager.BuildingState.Normal) {
				if (Input.GetMouseButtonDown(0)) {
					GridManager.Instance.ChangeState(GridManager.BuildingState.Building);
					_onFloatingBlockClicked?.Invoke(this);
				}
			}

            if (GridManager.Instance.State == GridManager.BuildingState.Building && _isDestroying == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _onFloatingBlockClicked?.Invoke(this);
                }
                else if (Input.GetMouseButtonDown(1)) // Right mouse button is button 1
                {
                    _rb.mass = 100;
                    StartCoroutine(nameof(destroyObject));
                }
                else if (Input.GetMouseButtonDown(2)) // Middle mouse button is button 2
                {
                    _isMoving = false;
                    StartFlying();
                }
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
            } else {
				_rb.velocity = Vector3.zero;
			}
        }
        private void StartFlying()
        {
            if (_isFlying) return; 

            _isFlying = true;
            _rb.useGravity = false; // Disable gravity

            StartCoroutine(FlyToHeight());
        }

        private IEnumerator FlyToHeight()
        {
            Vector3 targetPosition = new Vector3(transform.position.x, _yPos, transform.position.z);

            while (transform.position.y < _yPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _flyingSpeed * Time.deltaTime);
                yield return null;
            }

            _isFlying = true;
            _rb.useGravity = false; 
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

    

}