using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
#region Using New InputSystem
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
#endregion

/// <summary>
/// TODO: test the new input system
/// </summary>
public class BackButtonManager : Singleton_Abs<BackButtonManager>
{
    [Header("Exit Settings")]
    [SerializeField] bool _autoGameQuit = true;
    [SerializeField, ConditionalField(nameof(_autoGameQuit))] MsgBoxController _exitMsgBox;

    [Tooltip("if true => the game is closed after 2 backButtons")]
    [SerializeField] bool _isDoubleClickExit;

    [Tooltip("minimum delay before 2 clicks")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _doubleClickExitDelay;

    [Tooltip("time before the first click is expired (match it with notification)")]
    [SerializeField, ConditionalField(nameof(_isDoubleClickExit))]
    float _clickExpireTime;

    [Header("Canvas Settings")]
    [SerializeField] Canvas _mainCanvas;
    [SerializeField] Color _raycastBgColor = Color.clear;

    List<_PanelsClass> _registeredPanels = new List<_PanelsClass>();
    bool _isFirstClickActive = false;
    Coroutine _exitTimerCoroutine;
    Image _AntiRayCasterImage;
    Button _AntiRaycastButton;

    private void Start()
    {
        DontDestroyOnLoad(transform.root);
        _InitGlobalAntiRayCaster();
    }

    #region Old Input System
#if !ENABLE_INPUT_SYSTEM
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _OnBackButtonPressed();
        }
    }
#endif
    #endregion

    #region New Input System
#if ENABLE_INPUT_SYSTEM
    public void _OnBackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _OnBackButtonPressed();
        }
    }

    public void _OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _TryHandleOutsideClick();
        }
    }
#endif
    #endregion

    private void _InitGlobalAntiRayCaster()
    {
        if (_AntiRayCasterImage == null)
        {
            GameObject obj = new GameObject("GlobalRaycastCatcher", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            obj.transform.SetParent(_mainCanvas.transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);

            _AntiRayCasterImage = obj.GetComponent<Image>();
            _AntiRayCasterImage.color = _raycastBgColor;
            _AntiRayCasterImage.raycastTarget = true;
            _AntiRaycastButton = obj.GetComponent<Button>();
            _AntiRaycastButton.onClick.AddListener(() => _TryHandleOutsideClick());
        }
        _AntiRayCasterImage.gameObject.SetActive(false);
    }
    private void _UpdateAntiRayCasterOrder()
    {
        _PanelsClass topPanel = _GetTopActivePanel();
        if (topPanel == null)
        {
            _AntiRayCasterImage.gameObject.SetActive(false);
            return;
        }

        _mainCanvas.sortingOrder = _GetTopActivePanel()._bbController._GetCanvasPriority();
        _AntiRayCasterImage.gameObject.SetActive(true);
    }
    public void _OnBackButtonPressed()
    {
        _PanelsClass topPanel = _GetTopActivePanel();
        if (topPanel != null)
        {
            topPanel._panel.SetActive(false);
            _UnRegisterPanel(topPanel._panel);
            return;
        }

        if (_isDoubleClickExit && _autoGameQuit)
        {
            if (_isFirstClickActive)
            {
                if (_exitTimerCoroutine != null)
                {
                    StopCoroutine(_exitTimerCoroutine);
                }
                Application.Quit();
            }
            else
            {
                _isFirstClickActive = true;
                _exitTimerCoroutine = StartCoroutine(_ExitTimer());
            }
        }
        else
        {
            _exitMsgBox._StartMsg();
        }
    }
    IEnumerator _ExitTimer()
    {
        yield return new WaitForSeconds(_clickExpireTime);
        _isFirstClickActive = false;
        _exitTimerCoroutine = null;
    }
    public void _RegisterPanel(GameObject iPanel,BackButtonController iController, int iOrder)
    {
        _registeredPanels.Add(new _PanelsClass(iPanel, iController, iOrder));
        _UpdateAntiRayCasterOrder();
    }
    public void _UnRegisterPanel(GameObject iPanel)
    {
        for (int i = _registeredPanels.Count - 1; i >= 0; i--)
        {
            if (_registeredPanels[i]._panel == iPanel)
            {
                _registeredPanels.RemoveAt(i);
                break;
            }
        }
        _UpdateAntiRayCasterOrder();
    }
    _PanelsClass _GetTopActivePanel()
    {
        if (_registeredPanels.Count == 0)
        {
            return null;
        }
        return _registeredPanels.OrderByDescending(p => p._priorityOrder).FirstOrDefault(p => p._panel.activeInHierarchy);
    }
    public void _TryHandleOutsideClick()
    {
        _PanelsClass topPanel = _GetTopActivePanel();
        if (topPanel == null)
        {
            return;
        }

        if (!topPanel._bbController._GetIsBlockOutsideClick())
        {
            return;
        }

        if (topPanel._bbController._GetIsExitOnOutsideClick())
        {
            topPanel._panel.SetActive(false);
            _UnRegisterPanel(topPanel._panel);
        }
    }

    [System.Serializable]
    public class _PanelsClass
    {
        public GameObject _panel;
        [HideInInspector] public BackButtonController _bbController;
        public int _priorityOrder;
        public UnityEvent _optionalEvent;

        public _PanelsClass(GameObject iPanel, BackButtonController iController
            , int iOrder)
        {
            _panel = iPanel;
            _bbController = iController;
            _priorityOrder = iOrder;
        }
    }
}
