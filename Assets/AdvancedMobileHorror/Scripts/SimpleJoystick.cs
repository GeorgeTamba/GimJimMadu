using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdvancedHorrorFPS
{
    [Flags]
    public enum ControlMovementDirection
    {
        Horizontal = 0x1,
        Vertical = 0x2,
        Both = Horizontal | Vertical
    }

    public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public Camera CurrentEventCamera { get; set; }

        public float MovementRange = 50f;

        public bool HideOnRelease;
        public bool MoveBase = true;
        public bool SnapsToFinger = true;

        public ControlMovementDirection JoystickMoveAxis = ControlMovementDirection.Both;
        public Image JoystickBase;
        public Image Stick;
        public RectTransform TouchZone;

        private Vector2 _initialStickPosition;
        private Vector2 _intermediateStickPosition;
        private Vector2 _initialBasePosition;
        private RectTransform _baseTransform;
        private RectTransform _stickTransform;

        private float _oneOverMovementRange;

        public float HorizontalValue;
        public float VerticalValue;
        public static SimpleJoystick Instance;

        private void Awake()
        {
            Instance = this;
            _stickTransform = Stick.GetComponent<RectTransform>();
            _baseTransform = JoystickBase.GetComponent<RectTransform>();

            _initialStickPosition = _stickTransform.anchoredPosition;
            _intermediateStickPosition = _initialStickPosition;
            _initialBasePosition = _baseTransform.anchoredPosition;

            _stickTransform.anchoredPosition = _initialStickPosition;
            _baseTransform.anchoredPosition = _initialBasePosition;

            _oneOverMovementRange = 1f / MovementRange;

            if (HideOnRelease)
            {
                Hide(true);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            CurrentEventCamera = eventData.pressEventCamera ?? CurrentEventCamera;

            Vector3 worldJoystickPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_stickTransform, eventData.position,
                CurrentEventCamera, out worldJoystickPosition);

            _stickTransform.position = worldJoystickPosition;
            var stickAnchoredPosition = _stickTransform.anchoredPosition;

            if ((JoystickMoveAxis & ControlMovementDirection.Horizontal) == 0)
            {
                stickAnchoredPosition.x = _intermediateStickPosition.x;
            }
            if ((JoystickMoveAxis & ControlMovementDirection.Vertical) == 0)
            {
                stickAnchoredPosition.y = _intermediateStickPosition.y;
            }

            _stickTransform.anchoredPosition = stickAnchoredPosition;

            Vector2 difference = new Vector2(stickAnchoredPosition.x, stickAnchoredPosition.y) - _intermediateStickPosition;

            var diffMagnitude = difference.magnitude;
            var normalizedDifference = difference / diffMagnitude;

            if (diffMagnitude > MovementRange)
            {
                if (MoveBase && SnapsToFinger)
                {
                    var baseMovementDifference = difference.magnitude - MovementRange;
                    var addition = normalizedDifference * baseMovementDifference;
                    _baseTransform.anchoredPosition += addition;
                    _intermediateStickPosition += addition;
                }
                else
                {
                    _stickTransform.anchoredPosition = _intermediateStickPosition + normalizedDifference * MovementRange;
                }
            }

            var finalStickAnchoredPosition = _stickTransform.anchoredPosition;
            Vector2 finalDifference = new Vector2(finalStickAnchoredPosition.x, finalStickAnchoredPosition.y) - _intermediateStickPosition;
            var horizontalValue = Mathf.Clamp(finalDifference.x * _oneOverMovementRange, -1f, 1f);
            var verticalValue = Mathf.Clamp(finalDifference.y * _oneOverMovementRange, -1f, 1f);

            HorizontalValue = horizontalValue;
            VerticalValue = verticalValue;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _baseTransform.anchoredPosition = _initialBasePosition;
            _stickTransform.anchoredPosition = _initialStickPosition;
            _intermediateStickPosition = _initialStickPosition;

            HorizontalValue = VerticalValue = 0f;

            if (HideOnRelease)
            {
                Hide(true);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (HideOnRelease)
            {
                Hide(false);
            }
            if (SnapsToFinger)
            {
                CurrentEventCamera = eventData.pressEventCamera ?? CurrentEventCamera;

                Vector3 localStickPosition;
                Vector3 localBasePosition;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(_stickTransform, eventData.position,
                    CurrentEventCamera, out localStickPosition);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(_baseTransform, eventData.position,
                    CurrentEventCamera, out localBasePosition);

                _baseTransform.position = localBasePosition;
                _stickTransform.position = localStickPosition;
                _intermediateStickPosition = _stickTransform.anchoredPosition;
            }
            else
            {
                OnDrag(eventData);
            }
        }

        private void Hide(bool isHidden)
        {
            JoystickBase.gameObject.SetActive(!isHidden);
            Stick.gameObject.SetActive(!isHidden);
        }
    }
}
