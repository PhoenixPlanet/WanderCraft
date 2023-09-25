using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TH.Core {

public class BlockMouseSensor : MonoBehaviour
{
	public enum Direction {
		Up,
		Down,
		Left,
		Right
	}

    #region PublicVariables
	public static readonly Dictionary<Direction, Vector3> COLLIDER_CENTER = new Dictionary<Direction, Vector3> {
		{Direction.Up, new Vector3(0, .3f, .75f)},
		{Direction.Down, new Vector3(0, .3f, -.75f)},
		{Direction.Left, new Vector3(-.75f, .3f, 0)},
		{Direction.Right, new Vector3(.75f, .3f, 0)}
	};
	
	public static readonly Dictionary<Direction, Vector3> COLLIDER_SIZE = new Dictionary<Direction, Vector3> {
		{Direction.Up, new Vector3(1, .6f, .5f)},
		{Direction.Down, new Vector3(1, .6f, .5f)},
		{Direction.Left, new Vector3(.5f, .6f, 1)},
		{Direction.Right, new Vector3(.5f, .6f, 1)}
	};
	#endregion

	#region PrivateVariables
	private ComponentGetter<BoxCollider> _boxCollider
		= new ComponentGetter<BoxCollider>(TypeOfGetter.This);
	[ShowInInspector] private Direction _direction;
	private Action<Vector2Int> _onMouseOver;
	private Action<Vector2Int> _onMouseExit;
	private Action<Vector2Int> _onMouseClick;
	#endregion

	#region PublicMethod
	public static Vector2Int GridPosOffset(Direction direction) {
		switch (direction) {
			case Direction.Up:
				return new Vector2Int(0, 1);
			case Direction.Down:
				return new Vector2Int(0, -1);
			case Direction.Left:
				return Vector2Int.left;
			case Direction.Right:
				return Vector2Int.right;
			default:
				return Vector2Int.zero;
		}
	}

	public void Init(Direction direction, Action<Vector2Int> onMouseOver, Action<Vector2Int> onMouseExit, Action<Vector2Int> onMouseClick) {
		_direction = direction;
		_onMouseOver = onMouseOver;
		_onMouseExit = onMouseExit;
		_onMouseClick = onMouseClick;

		_boxCollider.Get(gameObject).center = COLLIDER_CENTER[direction];
		_boxCollider.Get(gameObject).size = COLLIDER_SIZE[direction];
	}
	#endregion
    
	#region PrivateMethod
	private void OnMouseOver() {
		_onMouseOver?.Invoke(GridPosOffset(_direction));
	}

	private void OnMouseExit() {
		_onMouseExit?.Invoke(GridPosOffset(_direction));
	}

	private void OnMouseDown() {
		_onMouseClick?.Invoke(GridPosOffset(_direction));
	}
	#endregion
}

}