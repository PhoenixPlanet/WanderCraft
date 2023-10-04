using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace TH.Core
{

    public class FloatingBlock : MonoBehaviour
    {
        #region PublicVariables
        public BlockDataSO Data => _blockData;
        
        #endregion

        public PropertyData myType;
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
        [SerializeField] int red;
        [SerializeField] int green;
        [SerializeField] int blue;
        [SerializeField] GameObject indicatorUI;
        #endregion

        #region PublicMethod
        public void Init(BlockDataSO blockData)
        {
            _blockData = blockData;
            _onFloatingBlockClicked = OnFloatingBlockClicked;
            _yPos = GridManager.Instance.CurrentCenterLevel - 2f;
            _hasInit = true;
        
            _rb = GetComponent<Rigidbody>();
        }
        #endregion

        #region PrivateMethod
        private void OnFloatingBlockClicked(FloatingBlock floatingBlock)
        {
            if (GridManager.Instance.State == GridManager.BuildingState.Building)
            {
                GridManager.Instance.SelectFloatingBlock(floatingBlock);
            }
        }


        private void OnMouseOver()
        {
            _smallUI.Get(gameObject).SetActive(false);
            _bigUI.Get(gameObject).SetActive(true);


				if (Input.GetMouseButtonDown(0))
                {
                    GameObject tempUI = Instantiate(indicatorUI, transform.position, Quaternion.Euler(0,0,0));
                    tempUI.GetComponentInChildren<TextMeshProUGUI>().text = red + blue + green.ToString();
                    GridManager.Instance.AddProperty(new PropertyData(red, blue, green, 0));
                    Destroy(gameObject);

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
        /*
        private void moveDirection()
        {
            _rb.AddForce(myDirection * _randomSpeed);
        }
        */
        private void FixedUpdate()
        {
            
            if (transform.position.y < _destroyYpos)
            {
                Destroy(gameObject);
            }
            /*
           if (!_isFlying)
            {
                moveDirection();
            } else {
				_rb.velocity = Vector3.zero;
			}
            */
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