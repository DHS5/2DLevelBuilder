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

        // Inputs
        private LevelBuilder_InputActions inputActions;

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
            inputActions = new LevelBuilder_InputActions();

            ItemsMenu.OnStart += EnableInputs;
            ItemsMenu.OnQuit += DisableInputs;

            bounds = new Vector2(32, 16); // to replace
        }

        private void Update()
        {
            if (inputActions.LevelBuilder.MouseWheelClick.ReadValue<float>() > 0)
            {
                OnMouseWheel();
            }
        }

        // ### Inputs ###

        private void EnableInputs()
        {
            inputActions.Enable();

            inputActions.LevelBuilder.MouseWheelScroll.performed += OnMouseWheelScroll;
            inputActions.LevelBuilder.MousePosition.performed += OnMouseMove;
            inputActions.LevelBuilder.MouseWheelClick.performed += OnMouseWheelDown;
        }

        private void DisableInputs()
        {
            inputActions.Disable();

            inputActions.LevelBuilder.MouseWheelScroll.performed -= OnMouseWheelScroll;
            inputActions.LevelBuilder.MousePosition.performed -= OnMouseMove;
            inputActions.LevelBuilder.MouseWheelClick.performed -= OnMouseWheelDown;
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
            else if (scroll.y < 0 && levelBuilderCamera.orthographicSize < 40) levelBuilderCamera.orthographicSize++;
        }
    }
}
