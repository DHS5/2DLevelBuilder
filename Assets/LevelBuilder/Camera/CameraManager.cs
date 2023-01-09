using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevelBuilder2D
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Level Builder Camera")]
        [SerializeField] private Camera levelBuilderCamera;
        [SerializeField] private Camera refCamera;

        [Header("Player Inputs")]
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


        private void Awake()
        {
            bounds = RoomManager.roomSize / 2;
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, EnableInputs);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, DisableInputs);
            EventManager.StartListening(EventManager.LevelBuilderEvent.QUIT_HELP, EnableInputs);
            EventManager.StartListening(EventManager.LevelBuilderEvent.OPEN_HELP, DisableInputs);
        }
        private void OnDisable()
        {
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_BUILDER, EnableInputs);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_BUILDER, DisableInputs);
            EventManager.StopListening(EventManager.LevelBuilderEvent.QUIT_HELP, EnableInputs);
            EventManager.StopListening(EventManager.LevelBuilderEvent.OPEN_HELP, DisableInputs);
        }


        private void Update()
        {
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
