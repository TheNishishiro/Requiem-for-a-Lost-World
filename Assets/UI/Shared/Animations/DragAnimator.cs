using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shared.Animations
{
	public class DragAnimator : MonoBehaviour
	{
		public float distance = 10f; // distance for hand to travel
		public float moveSpeed = 120f; 
		private Vector3 _startPosition;
		private Vector3 _endPosition;
		private Image _image;

		private void Start()
		{
			_startPosition = transform.position;
			_image = GetComponent<Image>();
			_endPosition = _startPosition + distance * Vector3.right; // adjust this if you want different direction
		}

		private void Update()
		{
			if (transform.position != _endPosition)
			{
				// move our position a step closer to the target
				transform.position = Vector3.MoveTowards(transform.position, _endPosition, moveSpeed * Time.deltaTime);

				// calculate and apply the fade out effect
				var currentDistance = Vector3.Distance(transform.position, _endPosition);
				var totalDistance = Vector3.Distance(_startPosition, _endPosition);

				// the color of the image
				var color = _image.color;
				// Alpha is calculated same as before,
				color.a = currentDistance / totalDistance;

				// assign it back to the image's color
				_image.color = color;
			}
			else // If position reached to end position, reset it back to start position
			{
				transform.position = _startPosition;
				// reset alpha back to 1 (no transparency);
				var color = _image.color;
				color.a = 1f;
				_image.color = color;
			}
		}
	}
}