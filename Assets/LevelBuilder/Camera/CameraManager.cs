using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Dhs5.Utility.EventSystem;

namespace LevelBuilder2D
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Level Builder Camera")]
        [SerializeField] private Camera levelBuilderCamera;
        [SerializeField] private Camera refCamera;

        [Header("Event System")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] PlayerInput playerInput;


        private Vector2 startMousePos;
        private Vector2 mousePos;
        private Vector2 MousePos
        {
            get { return refCamera.ScreenToWorldPoint(mousePos); }
            set { mousePos = value; }
        }
        private Vector2 MouseDelta
        {
            get { return (MousePos - startMousePos) * ZoomCoeff; }
        }
        private Vector2 startCameraPos;

        private Vector2 bounds;


        private float ZoomCoeff { get { return levelBuilderCamera.orthographicSize / 20; } }

        private bool isPointerOnUI;

        private void Awake()
        {
            bounds = RoomManager.roomSize / 2;
        }

        private void OnEnable()
        {
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.OPEN_BUILDER, EnableInputs);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.QUIT_BUILDER, DisableInputs);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.QUIT_HELP, EnableInputs);
            EventManager<LevelBuilderEvent>.StartListening(LevelBuilderEvent.OPEN_HELP, DisableInputs);
        }
        private void OnDisable()
        {
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.OPEN_BUILDER, EnableInputs);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.QUIT_BUILDER, DisableInputs);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.QUIT_HELP, EnableInputs);
            EventManager<LevelBuilderEvent>.StopListening(LevelBuilderEvent.OPEN_HELP, DisableInputs);
        }


        private void Update()
        {
            isPointerOnUI = eventSystem.IsPointerOverGameObject();
            if (playerInput.actions["Move"].ReadValue<float>() > 0)
            {
                OnMouseWheel();
            }
        }

        // ### Inputs ###

        private void EnableInputs()
        {
            playerInput.actions.Enable();

            playerInput.actions["Zoom"].performed += OnMouseWheelScroll;
            playerInput.actions["MousePosition"].performed += OnMouseMove;
            playerInput.actions["Move"].performed += OnMouseWheelDown;
        }

        private void DisableInputs()
        {
            playerInput.actions.Disable();

            playerInput.actions["Zoom"].performed -= OnMouseWheelScroll;
            playerInput.actions["MousePosition"].performed -= OnMouseMove;
            playerInput.actions["Move"].performed -= OnMouseWheelDown;
        }

        private void OnMouseMove(InputAction.CallbackContext ctx)
        {
            MousePos = ctx.ReadValue<Vector2>();
        }

        private void OnMouseWheelScroll(InputAction.CallbackContext ctx)
        {
            if (!isPointerOnUI)
                ZoomInOut(ctx.ReadValue<Vector2>());
        }

        private void OnMouseWheel()
        {
            levelBuilderCamera.transform.position = new Vector3(
                Mathf.Clamp(-MouseDelta.x + startCameraPos.x, -bounds.x, bounds.x),
                Mathf.Clamp(-MouseDelta.y + startCameraPos.y, -bounds.y, bounds.y),
                -10);
        }
        private void OnMouseWheelDown(InputAction.CallbackContext ctx)
        {
            startMousePos = MousePos;
            startCameraPos = levelBuilderCamera.transform.position;
        }


        // ### Functions ###

        private void ZoomInOut(Vector2 scroll)
        {
            if (scroll.y > 0 && levelBuilderCamera.orthographicSize > 1) levelBuilderCamera.orthographicSize--;
            else if (scroll.y < 0 && levelBuilderCamera.orthographicSize < 30) levelBuilderCamera.orthographicSize++;
        }
    }
}
