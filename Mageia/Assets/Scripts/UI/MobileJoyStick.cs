using UnityEngine;
using UnityEngine.EventSystems;

namespace MageiaGame
{
    public class MobileJoyStick : MonoBehaviour
    {
        public GameObject stick;
        public GameObject board;

        public Vector3 direction;

        private Vector3 initStickPos;
        private Vector3 initBoardPos;

        private float stickRadius;

        private static MobileJoyStick instance;
        public static MobileJoyStick Instance // Assign Singlton
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MobileJoyStick>();
                    if (Instance == null)
                    {
                        var instanceContainer = new GameObject("MobileJoyStick");
                        instance = instanceContainer.AddComponent<MobileJoyStick>();
                    }
                }
                return instance;
            }
        }

        void Start()
        {
            stickRadius = board.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
            initBoardPos = board.transform.position;
        }

        private bool IsLocked()
        {
            bool locked = PauseGame.GameIsPaused || GameplayController.TransistionInProgess;

            if (locked) OnEndDrag();

            return locked;
        }

        public void OnBeginDrag()
        {
            if (IsLocked()) return;

            // Move joystick UI to were the touch happened
            board.transform.position = Input.mousePosition;
            stick.transform.position = Input.mousePosition;
            initStickPos = Input.mousePosition;
        }

        public void OnDrag(BaseEventData baseEventData)
        {
            if (IsLocked()) return;

            // Calculate move direction using mouse posistion
            PointerEventData pointerEventData = baseEventData as PointerEventData;
            Vector3 DragPosition = pointerEventData.position;
            direction = (DragPosition - initStickPos).normalized;

            float stickDistance = Vector3.Distance(DragPosition, initStickPos);

            // Clamp stick inside of board
            if (stickDistance < stickRadius)
            {
                stick.transform.position = initStickPos + direction * stickDistance;
            }
            else
            {
                stick.transform.position = initStickPos + direction * stickRadius;
            }
        }

        public void OnEndDrag()
        {
            // Reset posistion when finish dragging
            direction = Vector3.zero;
            board.transform.position = initBoardPos;
            stick.transform.position = initBoardPos;
        }
    }

}