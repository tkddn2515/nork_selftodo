using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MEC;

namespace Hillis
{
    [SerializeField]
    public struct Data_Inputfield
    {
        public int width;
        public int height;
        public int pos_x;
        public int pos_y;
        public string text;
        public string font;
        public string fontstyle;
        public string underline;
        public float text_size;
        public string text_color;
        public string hint;
        public string hint_color;
        public int length;
        public string gravity_vertical;
        public string gravity_horizontal;
        public string inputtype;
        public string imeoption;
        public int padding_l;
        public int padding_r;
        public int padding_t;
        public int padding_b;
    }
    public enum InputType { normal, password, number, numberDecimal, phone, personname, email, datetime, multiline }
    public enum ImeOption { done, search, previous, next, go, send }
    public enum KeyboardAction { beforetextchanged, textchanged, aftertextchanged, oneditoraction, focuschange, hide, back, rect }
    public enum FontType { ttf, otf }

    [SerializeField]
    public struct Data_KeyboardAction
    {
        public string keyboardAction;
        public string data;
    }

    public class AndroidInputfield : MonoBehaviour
    {
        bool isShowKeyboard = false;
        private Vector3 prev_CustomInputfield_Pos = Vector3.one;
#if UNITY_ANDROID
        private void Update()
        {
            Detect_Touch();
            Rect_Change();
            Bundle_Height();
        }
#endif
        [HideInInspector] public CustomInputfield customInputfield;
        [HideInInspector] public RectTransform rect_CustomInputfield;
        [HideInInspector] public UnityEvent action_BeforeTextChanged;
        [HideInInspector] public UnityEvent<string> action_TextChanged;
        [HideInInspector] public UnityEvent action_AfterTextChanged;
        [HideInInspector] public UnityEvent action_OnEditorAction;

        public void ReceiveMessage(string _text)
        {
            Debug.LogError(_text);
            if (change_CustomInputfield)
            {
                change_CustomInputfield = false;
                return;
            }
            Data_KeyboardAction data_KeyboardAction = JsonUtility.FromJson<Data_KeyboardAction>(_text);
            KeyboardAction keyboardAction = (KeyboardAction)System.Enum.Parse(typeof(KeyboardAction), data_KeyboardAction.keyboardAction);
            switch (keyboardAction)
            {
                case KeyboardAction.beforetextchanged:
                    action_BeforeTextChanged?.Invoke();
                    break;
                case KeyboardAction.textchanged:
                    Debug.LogError(data_KeyboardAction.data);
                    Text_Inpufield(data_KeyboardAction.data);
                    action_TextChanged?.Invoke(data_KeyboardAction.data);
                    break;
                case KeyboardAction.aftertextchanged:
                    action_AfterTextChanged?.Invoke();
                    break;
                case KeyboardAction.oneditoraction:
                    Show_Inpufield();
                    if (action_OnEditorAction != null)
                        action_OnEditorAction.Invoke();
                    else
                        Hide();
                    break;
                case KeyboardAction.hide:
                    Show_Inpufield();
                    customInputfield = null;
                    rect_CustomInputfield = null;
                    break;
                case KeyboardAction.back:
                    break;
            }
        }

        public void Hide()
        {
            if (isShowKeyboard)
            {
                java.Call("Hide");
                isShowKeyboard = false;
            }
        }

        private void Text_Inpufield(string _text)
        {
            if (customInputfield != null)
            {
                if (customInputfield.inputField != null)
                {
                    customInputfield.inputField.text = _text;
                }
            }
        }

        private void Show_Inpufield()
        {
            if (customInputfield != null)
            {
                if (customInputfield.inputField != null)
                {
                    customInputfield.inputField.textComponent.gameObject.SetActive(true);
                }
                customInputfield.GetComponent<EventTrigger>().enabled = true;
            }

        }

        private void Hide_Inpufield()
        {
            if (customInputfield != null)
            {
                if (customInputfield.inputField != null)
                {
                    customInputfield.inputField.textComponent.gameObject.SetActive(false);
                }
                customInputfield.GetComponent<EventTrigger>().enabled = false;
            }
        }

        float starttime = 0;
        Vector2 startpos = Vector2.one;
        private void Detect_Touch()
        {
            if (Input.touchCount.Equals(1) && !IsClickInputfield())
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase.Equals(TouchPhase.Began))
                {
                    starttime = Time.time;
                    startpos = touch.position;
                }
                else if (touch.phase.Equals(TouchPhase.Ended))
                {
                    if ((starttime - Time.time) < 0.5f && Vector2.Distance(startpos, touch.position) < 10f)
                        Hide();
                }
            }
        }

        private void Rect_Change()
        {
            if (customInputfield != null)
            {
                if (prev_CustomInputfield_Pos != rect_CustomInputfield.position)
                {
                    InputfieldRectChange();
                }
                prev_CustomInputfield_Pos = rect_CustomInputfield.position;
            }
        }

        private bool IsClickInputfield()
        {
            return IsClickInputfield(GetEventSystemRaycastResults());
        }

        private bool IsClickInputfield(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int i = 0; i < eventSystemRaysastResults.Count; i++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[i];
                if (curRaysastResult.gameObject.GetComponent<InputField>())
                    return true;
            }
            return false;
        }

        private List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }

        #region
        public static AndroidInputfield instance;
        private AndroidJavaObject java;
        public RectTransform rect_CanvasMain;
        private void Awake()
        {
            screen_Height = Screen.height;
            bundle_X = rect_Bundle.position.x;
            bundle_Y = rect_Bundle.position.y;
            bundle_Z = rect_Bundle.position.z;
            x_ratio = Screen.width / rect_CanvasMain.rect.width;
            y_ratio = Screen.height / rect_CanvasMain.rect.height;
#if !UNITY_EDITOR && UNITY_ANDROID
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            java = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
            BarState();
#else
            Destroy(gameObject);
#endif
        }

        public void OnClickCustomInputfield(CustomInputfield _customInputfield)
        {
            SetCustomInputfield(_customInputfield);
            string data = OnClickInputField();
            if (data != null)
            {
                isShowKeyboard = true;
                java.Call("EditText", data);
            }
        }

        public void InputfieldRectChange()
        {
            string data = RectInputField();
            if (data != null)
                java.Call("RectChange", data);
        }

        bool change_CustomInputfield;

        private void SetCustomInputfield(CustomInputfield _customInputfield)
        {
            Show_Inpufield();
            RectTransform tmp_rect = _customInputfield.GetComponent<RectTransform>();

            prev_CustomInputfield_Pos = tmp_rect.position;
            customInputfield = _customInputfield;
            rect_CustomInputfield = tmp_rect;
            Clear_Action();
            action_BeforeTextChanged = customInputfield.onBeforeTextChanged;
            action_TextChanged = customInputfield.onTextChanged;
            action_AfterTextChanged = customInputfield.onAfterTextChanged;
            action_OnEditorAction = customInputfield.onDone;
            Hide_Inpufield();

            change_CustomInputfield = true;
        }

        private void Clear_Action()
        {
            action_BeforeTextChanged = null;
            action_TextChanged = null;
            action_AfterTextChanged = null;
            action_OnEditorAction = null;
        }
        float x_ratio;
        float y_ratio;
        private string OnClickInputField()
        {
            if (customInputfield.inputField != null)
            {
                InputField _ipf = customInputfield.inputField;

                Data_Inputfield di = new Data_Inputfield();

                float _width = rect_CustomInputfield.rect.width;
                float _height = rect_CustomInputfield.rect.height;

                di.width = Mathf.RoundToInt(_width * x_ratio);
                di.height = Mathf.RoundToInt(_height * y_ratio);

                Vector2 pivot = rect_CustomInputfield.pivot;
                di.pos_x = Mathf.RoundToInt(rect_CustomInputfield.position.x - (pivot.x * rect_CustomInputfield.rect.width));
                di.pos_y = Mathf.RoundToInt(Screen.height - (rect_CustomInputfield.position.y - (pivot.y - 1) * rect_CustomInputfield.rect.height));

                di.text = _ipf.text;
                di.font = _ipf.textComponent.font.name + "." + customInputfield.fontType.ToString().ToLower();
                di.fontstyle = _ipf.textComponent.fontStyle.ToString().ToLower();
                di.text_size = _ipf.textComponent.fontSize / 2.8875f;
                di.text_color = ColorToHex(_ipf.textComponent.color);
                di.hint = _ipf.placeholder.GetComponent<Text>().text;
                di.hint_color = ColorToHex(_ipf.placeholder.GetComponent<Text>().color);
                di.length = _ipf.characterLimit.Equals(0) ? 255 : _ipf.characterLimit;

                switch (_ipf.textComponent.alignment)
                {
                    case TextAnchor.UpperLeft:
                        di.gravity_vertical = "0";
                        di.gravity_horizontal = "0";
                        break;
                    case TextAnchor.UpperCenter:
                        di.gravity_vertical = "0";
                        di.gravity_horizontal = "1";
                        break;
                    case TextAnchor.UpperRight:
                        di.gravity_vertical = "0";
                        di.gravity_horizontal = "2";
                        break;
                    case TextAnchor.MiddleLeft:
                        di.gravity_vertical = "1";
                        di.gravity_horizontal = "0";
                        break;
                    case TextAnchor.MiddleCenter:
                        di.gravity_vertical = "1";
                        di.gravity_horizontal = "1";
                        break;
                    case TextAnchor.MiddleRight:
                        di.gravity_vertical = "1";
                        di.gravity_horizontal = "2";
                        break;
                    case TextAnchor.LowerLeft:
                        di.gravity_vertical = "2";
                        di.gravity_horizontal = "0";
                        break;
                    case TextAnchor.LowerCenter:
                        di.gravity_vertical = "2";
                        di.gravity_horizontal = "1";
                        break;
                    case TextAnchor.LowerRight:
                        di.gravity_vertical = "2";
                        di.gravity_horizontal = "2";
                        break;
                }

                di.inputtype = customInputfield.inputType.ToString();
                di.imeoption = customInputfield.imeOption.ToString();

                RectTransform _text_Rect = _ipf.textComponent.GetComponent<RectTransform>();

                di.padding_r = Mathf.RoundToInt(_text_Rect.offsetMax.x * -1);
                di.padding_t = Mathf.RoundToInt(_text_Rect.offsetMax.y * -1);
                di.padding_l = Mathf.RoundToInt(_text_Rect.offsetMin.x);
                di.padding_b = Mathf.RoundToInt(_text_Rect.offsetMin.y);

                return $"[{JsonUtility.ToJson(di)}]";
            }

            else return null;
        }

        private string RectInputField()
        {
            if (customInputfield.inputField != null)
            {
                Data_Inputfield di = new Data_Inputfield();

                float _width = rect_CustomInputfield.rect.width;
                float _height = rect_CustomInputfield.rect.height;

                di.width = Mathf.RoundToInt(_width * x_ratio);
                di.height = Mathf.RoundToInt(_height * y_ratio);

                Vector2 pivot = rect_CustomInputfield.pivot;
                di.pos_x = Mathf.RoundToInt(rect_CustomInputfield.position.x - (pivot.x * rect_CustomInputfield.rect.width));
                di.pos_y = Mathf.RoundToInt(Screen.height - (rect_CustomInputfield.position.y - (pivot.y - 1) * rect_CustomInputfield.rect.height));

                return $"[{JsonUtility.ToJson(di)}]";
            }

            else return null;
        }


        public string ColorToHex(Color _col)
        {
            string _tmp = ColorUtility.ToHtmlStringRGBA(_col);
            string _rgb = _tmp.Substring(0, 6);
            string _alpha = _tmp.Substring(6, 2);
            return "#" + _alpha + _rgb;
        }

        [Header("키보드")]
        public RectTransform rect_Bundle;
        float bundle_X = 0;
        float bundle_Y = 0;
        float bundle_Z = 0;
        float screen_Height = 0;
        float statusBar_Height = 0;
        float keyboard_ratio = 1;
        float keyboard_Height = 0;

        /// <summary>
        /// 키보드 높이
        /// </summary>
        /// <returns></returns>
        public float KeyBordHeight()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var unityPlayer = java.Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");
            //var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");

            if (view == null/* || dialog == null*/)
            {
                view = unityPlayer.Call<AndroidJavaObject>("getView");
                if (view == null)
                    return 0;
                return 0;
            }
            //var decorHeight = 0;
            //if (false)
            //{
            //    var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

            //    if (decorView != null)
            //        decorHeight = decorView.Call<int>("getHeight");
            //}

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Display.main.systemHeight - rect.Call<int>("height");
            }
#elif UNITY_IOS && !UNITY_EDITOR
            return (float)TouchScreenKeyboard.area.height;
#endif
            return 0;
        }
        bool isFirst = true;
        float _y;
        /// <summary>
        /// 키보드 높이만큼 Bundle(최상위 오브젝트) 올려주기
        /// </summary>
        private void Bundle_Height()
        {
            if (rect_CustomInputfield != null)
            {
                keyboard_ratio = (KeyBordHeight() - statusBar_Height) / screen_Height;
                keyboard_Height = screen_Height * keyboard_ratio;
                if (isFirst)
                {
                    _y = rect_CustomInputfield.position.y - rect_CustomInputfield.rect.height;
                    isFirst = false;
                }
                if (_y < keyboard_Height)
                {
                    float _dis = keyboard_Height - _y;
                    rect_Bundle.position = new Vector3(bundle_X, bundle_Y + _dis, bundle_Z);
                }
            }
            else
            {
                if(!isFirst)
                {
                    rect_Bundle.position = new Vector3(bundle_X, bundle_Y, bundle_Z);
                }
                isFirst = true;
            }
        }
        #endregion

        #region 네비게이션 바/스테이터스 바

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                BarState();
            }
        }
        /// <summary>
        /// 안드로이드 바 숨기기
        /// </summary>
        /// <returns></returns>
        private void BarState()
        {
#if UNITY_ANDROID
            Timing.RunCoroutine(Cor_BarState());
#endif
        }

        /// <summary>
        /// 안드로이드 바 숨기기
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> Cor_BarState()
        {
            float Time = 0.1f;
            ApplicationChrome.navigationBarState = ApplicationChrome.States.TranslucentOverContent;
            ApplicationChrome.statusBarState = ApplicationChrome.States.TranslucentOverContent;
            yield return Timing.WaitForSeconds(Time);
            ApplicationChrome.navigationBarState = ApplicationChrome.States.Hidden;
            ApplicationChrome.statusBarState = ApplicationChrome.States.Hidden;
            Set_StatusBar_Height();
        }

        public void Set_StatusBar_Height()
        {
            statusBar_Height = KeyBordHeight();
        }
        #endregion
    }

}
