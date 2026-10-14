using UnityEngine;
using UnityEngine.UI;

public class MobileControlRig : MonoBehaviour
{
    [Header("Movement Pad")]
    [SerializeField] private RectTransform movementPad;
    [SerializeField] private Image movementKnob;
    [SerializeField] private float deadZone = 0.2f;

    [Header("Action Buttons")]
    [SerializeField] private Button fireButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button sprintButton;

    public Vector2 moveAxis { get; private set; }
    public bool fireHeld { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool sprintHeld { get; private set; }

    private Vector2 pointerStart;
    private bool draggingMovement;

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
            return;
        }

        moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        fireHeld = Input.GetMouseButton(0);
        sprintHeld = Input.GetKey(KeyCode.LeftShift);
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
    }

    public void CreateRuntimeUi()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
        }

        if (movementPad == null)
        {
            GameObject movePadGO = new GameObject("MovementPad", typeof(RectTransform), typeof(Image));
            movePadGO.transform.SetParent(canvas.transform, false);
            movementPad = movePadGO.GetComponent<RectTransform>();
            movementPad.anchorMin = new Vector2(0f, 0f);
            movementPad.anchorMax = new Vector2(0f, 0f);
            movementPad.sizeDelta = new Vector2(180f, 180f);
            movementPad.anchoredPosition = new Vector2(120f, 120f);
            var moveImg = movePadGO.GetComponent<Image>();
            moveImg.color = new Color(1f, 1f, 1f, 0.18f);
        }

        if (movementKnob == null)
        {
            GameObject knobGO = new GameObject("MovementKnob", typeof(RectTransform), typeof(Image));
            knobGO.transform.SetParent(movementPad.transform, false);
            movementKnob = knobGO.GetComponent<Image>();
            movementKnob.color = new Color(1f, 1f, 1f, 0.7f);
            RectTransform knobRect = knobGO.GetComponent<RectTransform>();
            knobRect.sizeDelta = new Vector2(68f, 68f);
            knobRect.anchoredPosition = Vector2.zero;
        }

        if (fireButton == null)
        {
            GameObject fireGO = new GameObject("FireButton", typeof(RectTransform), typeof(Image), typeof(Button));
            fireGO.transform.SetParent(canvas.transform, false);
            fireButton = fireGO.GetComponent<Button>();
            RectTransform fireRect = fireGO.GetComponent<RectTransform>();
            fireRect.anchorMin = new Vector2(1f, 0f);
            fireRect.anchorMax = new Vector2(1f, 0f);
            fireRect.sizeDelta = new Vector2(150f, 150f);
            fireRect.anchoredPosition = new Vector2(-120f, 120f);
            fireGO.GetComponent<Image>().color = new Color(1f, 0.2f, 0.2f, 0.8f);
        }

        if (jumpButton == null)
        {
            GameObject jumpGO = new GameObject("JumpButton", typeof(RectTransform), typeof(Image), typeof(Button));
            jumpGO.transform.SetParent(canvas.transform, false);
            jumpButton = jumpGO.GetComponent<Button>();
            RectTransform jumpRect = jumpGO.GetComponent<RectTransform>();
            jumpRect.anchorMin = new Vector2(1f, 0f);
            jumpRect.anchorMax = new Vector2(1f, 0f);
            jumpRect.sizeDelta = new Vector2(120f, 120f);
            jumpRect.anchoredPosition = new Vector2(-250f, 310f);
            jumpGO.GetComponent<Image>().color = new Color(0.2f, 0.7f, 1f, 0.8f);
        }

        if (sprintButton == null)
        {
            GameObject sprintGO = new GameObject("SprintButton", typeof(RectTransform), typeof(Image), typeof(Button));
            sprintGO.transform.SetParent(canvas.transform, false);
            sprintButton = sprintGO.GetComponent<Button>();
            RectTransform sprintRect = sprintGO.GetComponent<RectTransform>();
            sprintRect.anchorMin = new Vector2(1f, 0f);
            sprintRect.anchorMax = new Vector2(1f, 0f);
            sprintRect.sizeDelta = new Vector2(120f, 120f);
            sprintRect.anchoredPosition = new Vector2(-410f, 130f);
            sprintGO.GetComponent<Image>().color = new Color(0.3f, 1f, 0.5f, 0.8f);
        }

        fireButton.onClick.AddListener(() => fireHeld = true);
        jumpButton.onClick.AddListener(() => jumpPressed = true);
        sprintButton.onClick.AddListener(() => sprintHeld = true);
    }

    private void HandleTouchInput()
    {
        moveAxis = Vector2.zero;
        fireHeld = false;
        sprintHeld = false;
        jumpPressed = false;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.position.x < Screen.width * 0.45f && touch.position.y < Screen.height * 0.6f)
            {
                Vector2 local = touch.position - new Vector2(120f, 120f);
                float radius = 60f;
                float magnitude = Mathf.Clamp01(local.magnitude / radius);
                moveAxis = new Vector2(Mathf.Clamp(local.x / radius, -1f, 1f), Mathf.Clamp(local.y / radius, -1f, 1f));

                if (movementKnob != null)
                {
                    movementKnob.rectTransform.anchoredPosition = new Vector2(moveAxis.x * 45f, moveAxis.y * 45f);
                }
            }

            if (touch.position.x > Screen.width * 0.55f)
            {
                if (touch.position.y > Screen.height * 0.2f)
                {
                    fireHeld = true;
                }

                if (touch.position.y > Screen.height * 0.52f)
                {
                    jumpPressed = true;
                }

                if (touch.position.y < Screen.height * 0.28f)
                {
                    sprintHeld = true;
                }
            }
        }
    }
}
