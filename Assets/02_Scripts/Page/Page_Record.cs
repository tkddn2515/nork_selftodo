using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LitJson;
using UnityEngine.Events;

namespace NORK
{
    public class Page_Record : Page
    {
        #region Commonent
        public enum Record_Type { Insert, Edit, View }
        public enum Texture_Type { Cut, Edit, View }
        [Header("공통")]
        [Tooltip("기록물 타입")]
        [SerializeField] private Record_Type record_Type = Record_Type.Insert;
        [Tooltip("할일")]
        [SerializeField] private Text text_Todo;
        [Tooltip("날짜")]
        [SerializeField] private Text text_Time;
        [Tooltip("캐릭터")]
        [SerializeField] private Image img_Char;

        [Header("메뉴")]
        [Tooltip("헤더에 있는 메뉴 버튼")]
        [SerializeField] private RectTransform rect_Header_Menu_Btn;
        [Tooltip("메뉴")]
        [SerializeField] private RectTransform rect_Menu;
        [Tooltip("메뉴-버튼")]
        [SerializeField] private RectTransform rect_Menu_Button;
        [Tooltip("메뉴-이미지")]
        [SerializeField] private RectTransform rect_Menu_Image;
        [Tooltip("메뉴-수정")]
        [SerializeField] private RectTransform rect_Menu_Edit;

        [Header("게시글")]
        [Tooltip("스크롤뷰")]
        [SerializeField] private RectTransform rect_Scroll;
        [Tooltip("추가 버튼")]
        [SerializeField] private RectTransform rect_Button_Add;
        [Tooltip("프리팹-이미지")]
        [SerializeField] private GameObject go_Prefab_Texture;
        [Tooltip("이미지 스크롤뷰")]
        [SerializeField] private RectTransform rect_Images;
        [Tooltip("게시글")]
        [SerializeField] private RectTransform rect_Content_Main;
        [Tooltip("프리팹부모-게시글")]
        [SerializeField] private RectTransform rect_Texture_Content_Main;
        [Tooltip("텍스쳐 확대 부모")]
        [SerializeField] private RectTransform rect_Big_Texture_Content;
        [Tooltip("텍스쳐 확대")]
        [SerializeField] private RectTransform rect_Big_Texture;
        [Tooltip("텍스쳐 확대")]
        [SerializeField] private RawImage rimg_Big_Texture;
        [Tooltip("본문 텍스트")]
        [SerializeField] private RectTransform rect_MainText;
        [Tooltip("본문 텍스트 라인 이미지")]
        [SerializeField] private RectTransform rect_MainText_ImageLine;
        [Tooltip("본문 텍스트")]
        [SerializeField] private InputField ipf_MainText;

        [Header("Edit")]
        [Tooltip("헤더에 있는 완료 버튼")]
        [SerializeField] private RectTransform rect_Header_Complete_Btn;
        [Tooltip("텍스쳐 경로")]
        [SerializeField] private List<Texture2D> edit_texs;
        [Tooltip("텍스쳐 추가 버튼")]
        [SerializeField] private RectTransform rect_Add_Texture;

        [Header("이미지 컷")]
        [Tooltip("이미지 컷 페이지")]
        [SerializeField] private RectTransform rect_Cut_Page;
        [Tooltip("프리팹부모-이미지 컷")]
        [SerializeField] private RectTransform rect_Texture_Content_Cut;
        [Tooltip("이미지 컷 부위(스크린샷 부위)")]
        [SerializeField] private RectTransform rect_Texture_Cut_Part;
        [Tooltip("이미지 컷 대상 텍스쳐")]
        [SerializeField] private RectTransform rect_Texture_Cut;
        [Tooltip("이미지 컷 한 후 파일들 경로")]
        [SerializeField] private List<Texture2D> cut_texs;

        [Tooltip("텍스쳐 프리펩 Queue")]
        [SerializeField] private Queue<Prefab_Texture> go_Que_Textures = new Queue<Prefab_Texture>();
        [Tooltip("View 텍스쳐 리스트")]
        [SerializeField] private List<Prefab_Texture> prefab_Textures = new List<Prefab_Texture>();
        [Tooltip("Cut 텍스쳐 리스트")]
        [SerializeField] private List<Data_Prefab_Texture_Cut> data_Prefab_Texture_Cuts = new List<Data_Prefab_Texture_Cut>();
        [Tooltip("현재 Record")]
        [SerializeField] private Manager_Data.Data_Todo_Record data_ToDo_Record;

        [SerializeField] private List<Manager_Common.Data_Dictionary> data_Textures;

        /// <summary>
        /// 파이
        /// </summary>
        private float pi;

        private const int PICTURE_COUNT = 3;

        [Serializable]
        public struct Data_Prefab_Texture_Cut
        {
            public Prefab_Texture prefab_Texture;
            public Vector2 position;
            public int rotation;
            public Vector2 size;
        }

        /// <summary>
        /// Enqueque하기
        /// </summary>
        /// <param name="_go"></param>
        public void Enqueue_Texture(Prefab_Texture _go) => go_Que_Textures.Enqueue(_go);
        #endregion

        internal override void OnEnable()
        {
            base.OnEnable();

            Set_Title();
            Init();
        }

        private void OnDisable()
        {
            Stop_Set_Texture();

            for (int i = 0; i < prefab_Textures.Count; i++)
            {
                prefab_Textures[i].Clear();
            }
            prefab_Textures.Clear();
        }

        internal override void Start()
        {
            base.Start();
            pi = (float)(Math.PI / 180);
        }

        private void Update()
        {
            if (Manager_Common.isBack)
                OnClickButton_Back();
        }

        private void Set_Title()
        {
            text_Todo.text = Manager.instance.page_Calender.Cur_Todo.name;
            text_Time.text = $"{Manager_Language.Get_Text("{0}년 {1}월 {2}일", Manager.instance.page_Calender.Date_Year_Month_Day.Year.ToString(), Manager.instance.page_Calender.Date_Year_Month_Day.Month.ToString(), Manager.instance.page_Calender.Date_Year_Month_Day.Day.ToString())} by {Manager.instance.manager_Member.Get_Data_Member.nickname}";

            img_Char.sprite = Manager.instance.manager_Common.data_Characters.Find(n => n.name.ToString().Equals(Manager.instance.manager_Member.Get_Data_Member.character)).img;
        }

        /// <summary>
        /// 기록물 데이터가 있는지 확인
        /// </summary>
        private void Init()
        {
            Page_Calender _Calender = Manager.instance.page_Calender;
            DateTime _now = _Calender.Date_Year_Month_Day;
            int _index = Manager.instance.manager_Data.data_ToDo_Records.FindIndex(n => n.tdid.Equals(_Calender.Cur_Todo.id) && n.date.Year.Equals(_now.Year) && n.date.Month.Equals(_now.Month) && n.date.Day.Equals(_now.Day));
            data_Textures = new List<Manager_Common.Data_Dictionary>();
            Close_View_Texture_Direct();
            if (_index.Equals(-1))//기록물 존재하지 않음
            {
                record_Type = Record_Type.Insert;

                data_ToDo_Record = new Manager_Data.Data_Todo_Record();

                ipf_MainText.text = "";

                rect_Scroll.gameObject.SetActive(false);
                rect_Button_Add.gameObject.SetActive(true);
            }
            else//기록물 존재
            {
                record_Type = Record_Type.View;

                data_ToDo_Record = Manager.instance.manager_Data.data_ToDo_Records[_index];

                ipf_MainText.text = data_ToDo_Record.maintext;

#if USE_LOGIN
                string[] _rids = new string[0];
                if (!string.IsNullOrWhiteSpace(data_ToDo_Record.rids))
                    _rids = data_ToDo_Record.rids.Split(',');
                Instan_Image("view", _rids);
#else
                string[] _names = new string[0];
                if (!string.IsNullOrWhiteSpace(data_ToDo_Record.names))
                    _names = data_ToDo_Record.names.Split(',');
                Instan_Image("view", _names);
#endif
                rect_Scroll.gameObject.SetActive(true);
                rect_Button_Add.gameObject.SetActive(false);
            }

            Reset();
        }

        /// <summary>
        /// 추가 버튼 선택 시
        /// </summary>
        public void OnClickButton_Add_Record()
        {
            rect_Scroll.gameObject.SetActive(true);
            rect_Button_Add.gameObject.SetActive(false);
            rect_Header_Complete_Btn.gameObject.SetActive(true);
        }

        /// <summary>
        /// Record Type에 따라 페이지 초기화
        /// </summary>
        private void Reset()
        {
            switch (record_Type)
            {
                case Record_Type.Insert:
                    rect_Header_Complete_Btn.gameObject.SetActive(false);
                    rect_Header_Menu_Btn.gameObject.SetActive(false);
                    rect_Add_Texture.gameObject.SetActive(true);
                    edit_texs = new List<Texture2D>();
                    ipf_MainText.enabled = true;
                    rect_Images.gameObject.SetActive(true);
                    rect_MainText_ImageLine.gameObject.SetActive(true);
                    ipf_MainText.placeholder.gameObject.SetActive(true);
                    break;
                case Record_Type.Edit:
                    rect_Header_Complete_Btn.gameObject.SetActive(true);
                    rect_Header_Menu_Btn.gameObject.SetActive(false);
                    rect_Add_Texture.gameObject.SetActive(false);
                    if(cur_Prefab_View_Texture)
                    {
                        cur_Prefab_View_Texture.Select(false);
                        cur_Prefab_View_Texture = null;
                    }
                    for (int i = 0; i < prefab_Textures.Count; i++)
                    {
                        prefab_Textures[i].Set_Texture(Texture_Type.Edit);
                    }
                    Manager_Common.StartCoroutine(ref cor_View_Texture, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Big_Texture_Content, new Vector2(1080, 0), 10, null,
                        () => { LayoutRebuilder.ForceRebuildLayoutImmediate(rect_Content_Main); }));
                    ipf_MainText.enabled = true;
                    ipf_MainText.placeholder.gameObject.SetActive(true);
                    Start_Set_Texture();
                    break;
                case Record_Type.View:
                    rect_Header_Menu_Btn.gameObject.SetActive(true);
                    rect_Add_Texture.gameObject.SetActive(false);
                    rect_Header_Complete_Btn.gameObject.SetActive(false);
                    ipf_MainText.enabled = false;
                    ipf_MainText.placeholder.gameObject.SetActive(false);
                    for (int i = 0; i < prefab_Textures.Count; i++)
                    {
                        prefab_Textures[i].Set_Texture(Texture_Type.View);
                    }
                    Start_Set_Texture();
                    break;
            }
        }

        public void OnClickButton_Back()
        {
            Active(false);
        }

        CoroutineHandle cor_Menu_Move;
        CoroutineHandle cor_Menu_Alpha;
        /// <summary>
        /// 메뉴버튼 누를시
        /// </summary>
        public void OnClickButton_Menu(bool _enable)
        {
            CanvasGroup _cvg = rect_Menu.GetComponent<CanvasGroup>();

            if(_enable)
            {
                switch (record_Type)
                {
                    case Record_Type.Edit:
                    case Record_Type.Insert:
                        rect_Menu_Image.gameObject.SetActive(true);
                        rect_Menu_Edit.gameObject.SetActive(false);
                        rect_Header_Complete_Btn.gameObject.SetActive(true);
                        rect_Header_Menu_Btn.gameObject.SetActive(false);
                        break;
                    case Record_Type.View:
                        rect_Menu_Image.gameObject.SetActive(false);
                        rect_Menu_Edit.gameObject.SetActive(true);
                        rect_Header_Complete_Btn.gameObject.SetActive(false);
                        rect_Header_Menu_Btn.gameObject.SetActive(true);
                        break;
                }
            }
            if(_enable)
            {
                Manager_Common.StartCoroutine(ref cor_Menu_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Menu_Button, _enable ? new Vector2(0, rect_Menu_Button.rect.height) : Vector2.zero, 10, () => { rect_Menu.gameObject.SetActive(_enable); }));
            }
            else
            {
                Manager_Common.StartCoroutine(ref cor_Menu_Move, Manager.instance.manager_Ui.Cor_Pos_Anchored(rect_Menu_Button, _enable ? new Vector2(0, rect_Menu_Button.rect.height) : Vector2.zero, 10, null, null,() => { rect_Menu.gameObject.SetActive(_enable); }));
            }
           
            Manager_Common.StartCoroutine(ref cor_Menu_Alpha, Manager.instance.manager_Ui.Cor_CanvasAlpha(_cvg, _enable ? true : false));

        }

        /// <summary>
        /// 사진찍기
        /// </summary>
        public void OnClickButton_TakeCapture()
        {
            if(edit_texs.Count >= PICTURE_COUNT)
            {
                Manager.instance.manager_Common.Show_Alert0(Manager_Language.Get_Text("사진은 {0}장까지 추가 가능합니다.", PICTURE_COUNT.ToString()));
                return;
            }
            Manager.instance.manager_Gallery.TakePicture((string _path) =>
            {
                if (_path != null && File.Exists(_path))
                {
                    Instan_Image("cut",new string[1] { _path });
                }
            });
        }

        /// <summary>
        /// 사진 가져오기
        /// </summary>
        public void OnClickButton_GetPickture()
        {
            if (edit_texs.Count >= PICTURE_COUNT)
            {
                Manager.instance.manager_Common.Show_Alert0(Manager_Language.Get_Text("사진은 {0}장까지 추가 가능합니다.", PICTURE_COUNT.ToString()));
                return;
            }
            Manager.instance.manager_Gallery.PickPicture((string[] _path) =>
            {
                if (edit_texs.Count + _path .Length> PICTURE_COUNT)
                {
                    Manager.instance.manager_Common.Show_Alert0(Manager_Language.Get_Text("사진은 {0}장까지 추가 가능합니다.", PICTURE_COUNT.ToString()));
                    string[] _new_Path = new string[PICTURE_COUNT - edit_texs.Count];
                    for (int i = 0; i < _new_Path.Length; i++)
                    {
                        _new_Path[i] = _path[i];
                    }
                    _path = null;
                    Instan_Image("cut", _new_Path);
                }
                else
                {
                    Instan_Image("cut", _path);
                }
            });
        }

        CoroutineHandle cor_Resource_List;
        /// <summary>
        /// 이미지 프리팹 생성
        /// </summary>
        /// <param name="_path"></param>
        private void Instan_Image(string where, string[] _path)
        {
            Timing.RunCoroutine(Cor_Instan_Image());
            IEnumerator<float> Cor_Instan_Image()
            {
                switch (where)
                {
                    case "cut":
                        Manager.instance.manager_Common.Start_Loading();
                        OnClickButton_Menu(false);
                        data_Prefab_Texture_Cuts.Clear();
                        for (int i = 0; i < _path.Length; i++)
                        {
                            Prefab_Texture pref = null;
                            if (go_Que_Textures.Count > 0)
                            {
                                pref = go_Que_Textures.Dequeue();
                                pref.gameObject.SetActive(true);
                                pref.transform.SetParent(rect_Texture_Content_Cut);
                                pref.transform.SetAsLastSibling();
                            }
                            else
                            {
                                pref = Instantiate(go_Prefab_Texture, rect_Texture_Content_Cut).GetComponent<Prefab_Texture>();
                            }
                            Data_Prefab_Texture_Cut _pt = new Data_Prefab_Texture_Cut();
                            _pt.prefab_Texture = pref;
                            _pt.position = Vector2.zero;
                            _pt.size = Vector2.zero;
                            _pt.rotation = 0;
                            data_Prefab_Texture_Cuts.Add(_pt);
                            _pt.prefab_Texture.Set_Data(_path[i], Texture_Type.Cut);
                            yield return Timing.WaitForOneFrame;
                        }
                        if (data_Prefab_Texture_Cuts.Count > 0)
                        {
                            yield return Timing.WaitUntilTrue(() => { return data_Prefab_Texture_Cuts[0].prefab_Texture.Raw_Img.texture != null; });
                            OnClickButton_Cut_Texture(data_Prefab_Texture_Cuts[0].prefab_Texture);
                        }
                        OnClickButton_Menu(false);
                        rect_Cut_Page.gameObject.SetActive(true);
                        Manager.instance.manager_Common.Stop_Loading();
                        break;
                    case "edit":
                        
                        for (int i = 0; i < _path.Length; i++)
                        {
                            Prefab_Texture pref = null;
                            if (go_Que_Textures.Count > 0)
                            {
                                pref = go_Que_Textures.Dequeue();
                                pref.gameObject.SetActive(true);
                                pref.transform.SetParent(rect_Texture_Content_Main);
                                pref.transform.SetAsLastSibling();
                            }
                            else
                            {
                                pref = Instantiate(go_Prefab_Texture, rect_Texture_Content_Main).GetComponent<Prefab_Texture>();
                            }
                            prefab_Textures.Add(pref);
                            pref.Set_Data(_path[i], Texture_Type.Edit);
                        }
                        break;
                    case "view"://_path = rids
#if USE_LOGIN
                        List<Manager_Common.Data_Dictionary> _paths = new List<Manager_Common.Data_Dictionary>();
                        for (int i = 0; i < _path.Length; i++)
                        {
                            Manager_Common.Data_Dictionary _dd = new Manager_Common.Data_Dictionary();
                            _dd.key = _path[i];
                            int _index = Manager.instance.manager_Data.resources.FindIndex(n => n.key.Equals(_path[i]));
                            if (_index.Equals(-1))
                                _dd.value = "";
                            else
                                _dd.value = Manager.instance.manager_Data.resources[_index].value;

                            _paths.Add(_dd);
                        }

                        //path가 없는 rid들
                        List<Prefab_Texture> _pts = new List<Prefab_Texture>();
                        List<string> _strs = new List<string>();

                        for (int i = 0; i < _paths.Count; i++)
                        {
                            Prefab_Texture pref = null;
                            if (go_Que_Textures.Count > 0)
                            {
                                pref = go_Que_Textures.Dequeue();
                                pref.gameObject.SetActive(true);
                                pref.transform.SetParent(rect_Texture_Content_Main);
                                pref.transform.SetAsLastSibling();
                            }
                            else
                            {
                                pref = Instantiate(go_Prefab_Texture, rect_Texture_Content_Main).GetComponent<Prefab_Texture>();
                            }
                            prefab_Textures.Add(pref);
                            if (_paths[i].value != "")
                                pref.Set_Data(_paths[i].value, Texture_Type.View);
                            else
                            {
                                pref.Active(Texture_Type.View);
                                _pts.Add(pref);
                                _strs.Add(_paths[i].key);
                            }
                        }

                        List<Manager_Common.Data_Dictionary> _needPaths = _paths.FindAll(n => n.value.Equals(""));
                        if (_needPaths.Count > 0)
                        {
                            List<string> _needrids = new List<string>();
                            for (int i = 0; i < _needPaths.Count; i++)
                            {
                                _needrids.Add($"\'{_needPaths[i].key}\'");
                            }

                            string _rids = string.Join(",", _needrids);
                            _rids = $"({_rids})";
                            cor_Resource_List = Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Resource_List(_rids, (string _return) =>
                            {
                                JsonData _json = JsonMapper.ToObject(_return);

                                for (int i = 0; i < _json.Count; i++)
                                {
                                    string _id = _json[i]["id"].ToString();
                                    string _path = _json[i]["url"].ToString();

                                    Manager.instance.manager_Data.resources.Add(new Manager_Common.Data_Dictionary(_id, _path));

                                    int _index = _strs.FindIndex(n => n.Equals(_id));
                                    if (!_index.Equals(-1))
                                        _pts[_index].Set_Url(_path);
                                }
                            }));

                            _needrids = null;
                        }
                        _paths = _needPaths = null;
#else
                        for (int i = 0; i < _path.Length; i++)
                        {
                            Prefab_Texture pref = null;
                            if (go_Que_Textures.Count > 0)
                            {
                                pref = go_Que_Textures.Dequeue();
                                pref.gameObject.SetActive(true);
                                pref.transform.SetParent(rect_Texture_Content_Main);
                                pref.transform.SetAsLastSibling();
                            }
                            else
                            {
                                pref = Instantiate(go_Prefab_Texture, rect_Texture_Content_Main).GetComponent<Prefab_Texture>();
                            }
                            prefab_Textures.Add(pref);
                            pref.Set_Data(Manager.instance.manager_Data.Texture_FullPath(_path[i]), Texture_Type.View);
                        }
#endif
                        break;
                }
                _path = null;
            }
        }

        /// <summary>
        /// 텍스쳐 갯수에 따라 페이지 세팅 시작
        /// </summary>
        private void Start_Set_Texture()
        {
            Stop_Set_Texture();
            cor_Set_Texture = Timing.RunCoroutine(Cor_Set_Texture());
        }

        /// <summary>
        /// 텍스쳐 갯수에 따라 페이지 세팅 멈추기
        /// </summary>
        private void Stop_Set_Texture()
        {
            if (cor_Set_Texture.IsRunning)
                Timing.KillCoroutines(cor_Set_Texture);
        }

        CoroutineHandle cor_Set_Texture;
        /// <summary>
        /// 텍스쳐 갯수에 따라 페이지 세팅
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> Cor_Set_Texture()
        {
            if (prefab_Textures.Count.Equals(1))
            {
                rect_Images.gameObject.SetActive(false);
                rect_MainText_ImageLine.gameObject.SetActive(false);
                yield return Timing.WaitUntilTrue(() => { return prefab_Textures[0].Raw_Img.texture != null; });
                OnClickButton_View_Texture(prefab_Textures[0]);
            }
            else if (prefab_Textures.Count.Equals(0))
            {
                rect_Images.gameObject.SetActive(false);
                rect_MainText_ImageLine.gameObject.SetActive(false);
            }
            else
            {
                rect_Images.gameObject.SetActive(true);
                rect_MainText_ImageLine.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 이미지 프리팹 생성
        /// </summary>
        /// <param name="_path"></param>
        private void Instan_Image(string where, Texture2D[] _tex)
        {
            for (int i = 0; i < _tex.Length; i++)
            {
                Prefab_Texture pref = null;
                if (go_Que_Textures.Count > 0)
                {
                    pref = go_Que_Textures.Dequeue();
                    pref.gameObject.SetActive(true);
                    pref.transform.SetParent(rect_Texture_Content_Main);
                    pref.transform.SetAsLastSibling();
                }
                else
                {
                    pref = Instantiate(go_Prefab_Texture, rect_Texture_Content_Main).GetComponent<Prefab_Texture>();
                }
                prefab_Textures.Add(pref);
                pref.Set_Data(_tex[i], Texture_Type.Edit);
            }
        }

        /// <summary>
        /// 게시글 수정
        /// </summary>
        public void OnClickButton_Edit()
        {
            OnClickButton_Menu(false);
            record_Type = Record_Type.Edit;
            Reset();
        }

        /// <summary>
        /// 게시글 삭제
        /// </summary>
        public void OnClickButton_Delete()
        {
            Manager.instance.manager_Data.Todo_Record_Delete(data_ToDo_Record.id, () =>
            {
                
            });

            rect_Menu_Button.anchoredPosition = new Vector2(0, rect_Menu_Button.rect.height);
            rect_Menu.gameObject.SetActive(false);

            Page_Calender calender = Manager.instance.page_Calender;

            calender.Find_Prefab_Day_Month(calender.Date_Year_Month_Day).Set_Record("-1");
            calender.Find_Prefab_Day_Year(calender.Date_Year_Month_Day).Set_Record("-1");

            string[] _names = new string[0];
            if (!string.IsNullOrWhiteSpace(data_ToDo_Record.names))
                _names = data_ToDo_Record.names.Split(',');

            for (int i = 0; i < _names.Length; i++)
            {
                Manager.instance.manager_Data.Texture_Delete(_names[i]);
            }

            data_ToDo_Record = new Manager_Data.Data_Todo_Record();
            Active(false);
        }

        #region 게시글 View
        [SerializeField] private Prefab_Texture cur_Prefab_View_Texture;
        CoroutineHandle cor_View_Texture;
        bool isShow_ViewTexture;
        /// <summary>
        /// 텍스쳐 선택시
        /// </summary>
        /// <param name="_prefab_Texture"></param>
        public void OnClickButton_View_Texture(Prefab_Texture _prefab_Texture)
        {
            if (cur_Prefab_View_Texture != null)
            {
                if (cur_Prefab_View_Texture.Equals(_prefab_Texture))
                {
                    if (isShow_ViewTexture)
                        Manager_Common.StartCoroutine(ref cor_View_Texture, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Big_Texture_Content, new Vector2(1080, 0), 10,
                            () => { isShow_ViewTexture = false; },
                            () => { LayoutRebuilder.ForceRebuildLayoutImmediate(rect_Content_Main); },
                            () =>
                            {
                                if (cur_Prefab_View_Texture != null)
                                {
                                    cur_Prefab_View_Texture.Select(false);
                                    cur_Prefab_View_Texture = null;
                                }
                                rimg_Big_Texture.texture = null;
                            }));
                    else
                        Show(_prefab_Texture);
                }
                else
                {
                    cur_Prefab_View_Texture.Select(false);
                    if (cor_View_Texture.IsRunning)
                    {
                        Show(_prefab_Texture);
                    }
                    else
                    {
                        cur_Prefab_View_Texture = _prefab_Texture;
                        cur_Prefab_View_Texture.Select(true);
                        rimg_Big_Texture.texture = cur_Prefab_View_Texture.Raw_Img.texture;
                    }
                }
            }
            else
            {
                Show(_prefab_Texture);
            }

            void Show(Prefab_Texture _prefab_Texture)
            {
                cur_Prefab_View_Texture = _prefab_Texture;
                cur_Prefab_View_Texture.Select(true);
                rimg_Big_Texture.texture = cur_Prefab_View_Texture.Raw_Img.texture;
                Manager_Common.StartCoroutine(ref cor_View_Texture, Manager.instance.manager_Ui.Cor_Rect_Anchored(rect_Big_Texture_Content, new Vector2(1080, 816), 10,
                    () => { isShow_ViewTexture = true; },
                    () => { LayoutRebuilder.ForceRebuildLayoutImmediate(rect_Content_Main); }));
            }
        }

        private void Close_View_Texture_Direct()
        {
            if (cur_Prefab_View_Texture != null)
            {
                cur_Prefab_View_Texture.Select(false);
                cur_Prefab_View_Texture = null;
            }

            rimg_Big_Texture.texture = null;
            rect_Big_Texture_Content.sizeDelta = new Vector2(1080, 0);
        }
        #endregion

        #region 게시글 Insert
        [Header("게시글 Insert")]
        [SerializeField] private Prefab_Texture cur_Prefab_Cut_Texture;
        [SerializeField] private int cur_Prefab_Texture_Index;
        /// <summary>
        /// 컷 텍스쳐 선택시
        /// </summary>
        /// <param name="_prefab_Texture"></param>
        public void OnClickButton_Cut_Texture(Prefab_Texture _prefab_Texture)
        {
            if (cur_Prefab_Cut_Texture != null)
            {
                cur_Prefab_Cut_Texture.Select(false);
            }
            cur_Prefab_Cut_Texture = _prefab_Texture;
            cur_Prefab_Cut_Texture.Select(true);
            int _index = data_Prefab_Texture_Cuts.FindIndex(n => n.prefab_Texture.Equals(_prefab_Texture));
            cur_Prefab_Texture_Index = _index;
            if (!_index.Equals(-1))
            {
                Vector2 _pos = data_Prefab_Texture_Cuts[_index].position;
                int _rot = data_Prefab_Texture_Cuts[_index].rotation;
                Vector2 _size = data_Prefab_Texture_Cuts[_index].size;
                Texture _tex = data_Prefab_Texture_Cuts[_index].prefab_Texture.Raw_Img.texture;
                rect_Texture_Cut.GetComponent<RawImage>().texture = _tex;
                if (_size.Equals(Vector2.zero))
                {
                    float _width = _tex.width;
                    float _height = _tex.height;

                    if (_width < rect_Texture_Cut_Part.rect.width || _height < rect_Texture_Cut_Part.rect.height)
                    {
                        float w_ratio = rect_Texture_Cut_Part.rect.width / _width;
                        float h_ratio = rect_Texture_Cut_Part.rect.height / _height;
                        float ratio = w_ratio > h_ratio ? w_ratio : h_ratio;
                        _width *= ratio;
                        _height *= ratio;
                    }


                    _size = new Vector2(_width, _height);
                    Data_Prefab_Texture_Cut _dptc = data_Prefab_Texture_Cuts[_index];
                    _dptc.size = _size;
                    data_Prefab_Texture_Cuts[_index] = _dptc;
                }
                rect_Texture_Cut.sizeDelta = _size;
                rect_Texture_Cut.anchoredPosition = _pos;
                rect_Texture_Cut.localEulerAngles = new Vector3(0, 0, _rot);
            }
        }

        /// <summary>
        /// 컷 대상 텍스쳐 움직일 시
        /// </summary>
        public void OnDrag_Texture_Cut(BaseEventData _data)
        {
            if (!Input.touchCount.Equals(1)) return;
            Vector2 scrollData = ((PointerEventData)_data).delta; ;
            OnMove_Texture_Cut(scrollData);
        }

        /// <summary>
        /// 텍스쳐 움직이기
        /// </summary>
        /// <param name="scrollData"></param>
        private void OnMove_Texture_Cut(Vector2 scrollData)
        {
            rect_Texture_Cut.anchoredPosition += scrollData;
            float z = Mathf.Round(rect_Texture_Cut.localEulerAngles.z);
            float sin = Mathf.Abs(Mathf.Sin(pi * z));
            float cos = Mathf.Abs(Mathf.Cos(pi * z));
            Vector2 initalRectSize = rect_Texture_Cut.sizeDelta;
            rect_Texture_Cut.anchoredPosition = new Vector2(
                Get_Size_Clamp(rect_Texture_Cut.anchoredPosition.x, initalRectSize.x * cos + initalRectSize.y * sin),
                Get_Size_Clamp(rect_Texture_Cut.anchoredPosition.y, initalRectSize.y * cos + initalRectSize.x * sin)
                );

            float Get_Size_Clamp(float _default, float _size)
            {
                float _new_Size = (_size - 813) * 0.5f;
                if (_new_Size < 0)
                    _new_Size *= -1;
                float _f = Mathf.Clamp(_default, -_new_Size, _new_Size);
                return _f;
            }

            Data_Prefab_Texture_Cut _dptc = data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index];
            _dptc.position = rect_Texture_Cut.anchoredPosition;
            data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index] = _dptc;
        }

        public void OnPointDown_Texture_Cut()
        {
            if (Input.touchCount.Equals(2))
            {
                if(cor_Texture_Scale_Cut.IsRunning)
                    Timing.KillCoroutines(cor_Texture_Scale_Cut);

                cor_Texture_Scale_Cut = Timing.RunCoroutine(Cor_Texture_Scale_Cut());
            }
        }

        public void OnPointUp_Texture_Cut()
        {
            if(cor_Texture_Scale_Cut.IsRunning)
            {
                Timing.KillCoroutines(cor_Texture_Scale_Cut);
                Data_Prefab_Texture_Cut _dptc = data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index];
                _dptc.size = rect_Texture_Cut.sizeDelta;
                data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index] = _dptc;

            }
        }

        public void OnClickButton_Texture_Cut_Rotation(int _rot)
        {
            rect_Texture_Cut.localEulerAngles += new Vector3(0, 0, _rot);
            Data_Prefab_Texture_Cut _dptc = data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index];
            _dptc.rotation = (int)rect_Texture_Cut.localEulerAngles.z;
            data_Prefab_Texture_Cuts[cur_Prefab_Texture_Index] = _dptc;

            OnMove_Texture_Cut(Vector2.zero);
        }

        CoroutineHandle cor_Texture_Scale_Cut;
        IEnumerator<float> Cor_Texture_Scale_Cut()
        {
            float initialFingersDistance = distance();
            Vector2 initalRectSize = rect_Texture_Cut.sizeDelta;
            while (true)
            {
                if(Input.touches.Length.Equals(2))
                {
                    float new_Distance = distance();

                    if (Touches(Input.touches[0]) || Touches(Input.touches[1]))
                    {
                        initialFingersDistance = new_Distance;
                        yield return Timing.WaitForSeconds(0);
                        continue;
                    }
                   
                    float fingersDistance = new_Distance - initialFingersDistance;
                    fingersDistance *= 2;
                    float new_Width = initalRectSize.x + fingersDistance;
                    float new_Height = initalRectSize.y * new_Width / initalRectSize.x;

                    if (new_Width >= 50 && new_Height >= 50)
                    {
                        Vector2 _new = new Vector2(new_Width, new_Height);
                        initalRectSize = rect_Texture_Cut.sizeDelta = _new;
                        OnMove_Texture_Cut(Vector2.zero);
                    }
                    else
                    {
                    }
                    initialFingersDistance = new_Distance;
                }
                yield return Timing.WaitForSeconds(0);
            }
            bool Touches(Touch _touch)
            {
                return _touch.phase.Equals(TouchPhase.Began) || _touch.phase.Equals(TouchPhase.Canceled) || _touch.phase.Equals(TouchPhase.Ended);
            }
            float distance()
            {
                return Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            }
        }

        public void OnClickButton_Back_Cut()
        {
            rect_Cut_Page.gameObject.SetActive(false);

            for (int i = 0; i < data_Prefab_Texture_Cuts.Count; i++)
            {
                if (data_Prefab_Texture_Cuts[i].prefab_Texture.Raw_Img.texture != null)
                    Destroy(data_Prefab_Texture_Cuts[i].prefab_Texture.Raw_Img.texture);
                Destroy(data_Prefab_Texture_Cuts[i].prefab_Texture.gameObject);
            }
            data_Prefab_Texture_Cuts.Clear();
        }

        /// <summary>
        /// 컷 완료시
        /// </summary>
        public void OnClickButton_Complete_Cut()
        {
            StartCoroutine(Cor_Complete_Cut());
        }

        /// <summary>
        /// 컷 완료시
        /// </summary>
        /// <returns></returns>
        IEnumerator Cor_Complete_Cut()
        {
            cut_texs = new List<Texture2D>();
            Cut_Part_Set_Anchor(1);
            for (int i = 0; i < data_Prefab_Texture_Cuts.Count; i++)
            {
                OnClickButton_Cut_Texture(data_Prefab_Texture_Cuts[i].prefab_Texture);
                yield return new WaitForEndOfFrame();
                cut_texs.Add(Capture());
            }
            Cut_Part_Set_Anchor(0);
            rect_Cut_Page.gameObject.SetActive(false);
            for (int i = 0; i < data_Prefab_Texture_Cuts.Count; i++)
            {
                data_Prefab_Texture_Cuts[i].prefab_Texture.Clear();
            }
            data_Prefab_Texture_Cuts.Clear();
            edit_texs.AddRange(cut_texs);

            Instan_Image("edit", cut_texs.ToArray());
            cut_texs.Clear();
        }

        /// <summary>
        /// 캡쳐
        /// </summary>
        /// <returns></returns>
        public Texture2D Capture()
        {
            int _widht = Display.main.systemWidth;
            int _height = Display.main.systemHeight;

            Texture2D screenShot = new Texture2D(_widht, _height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, _widht, _height), 0, 0, false);
            screenShot.Apply();

            int part_Width = (int)(rect_Texture_Cut_Part.rect.width * Manager.instance.manager_Common.ratio);
            int part_Heighth = (int)(rect_Texture_Cut_Part.rect.height * Manager.instance.manager_Common.ratio);

            Texture2D croppedTexture = new Texture2D(part_Width, part_Heighth);

            int pixel_x = (int)(rect_Texture_Cut_Part.position.x);
            int pixel_y = (int)(rect_Texture_Cut_Part.position.y);
            croppedTexture.SetPixels(screenShot.GetPixels(pixel_x, pixel_y, part_Width, part_Heighth));
            croppedTexture.Apply();
            Destroy(screenShot);

            return croppedTexture;
        }

        /// <summary>
        /// 캡쳐 영역 앵커 조정
        /// </summary>
        /// <param name="_index">0 : 상단, 1 : 좌측 하단</param>
        private void Cut_Part_Set_Anchor(int _index)
        {
            Vector3 tempPos = rect_Texture_Cut_Part.position;
            Vector2 change = Vector2.zero;
            switch (_index)
            {
                case 0:
                    change = new Vector2(0.5f, 1);
                    break;
                case 1:
                    change = Vector2.zero;
                    break;
            }
            rect_Texture_Cut_Part.anchorMax = rect_Texture_Cut_Part.anchorMin = change;
            rect_Texture_Cut_Part.position = tempPos;
            SetPivot(rect_Texture_Cut_Part, change);
        }

        public void SetPivot(RectTransform target, Vector2 pivot)
        {
            if (!target) return;
            var offset = pivot - target.pivot;
            offset.Scale(target.rect.size);
            var wordlPos = target.position + target.TransformVector(offset);
            target.pivot = pivot;
            target.position = wordlPos;
        }

        /// <summary>
        /// 게시글 추가/수정 하기
        /// </summary>
        public void OnClickButton_Edit_Complete()
        {
            Timing.RunCoroutine(Cor_Edit_Complete());
        }

        IEnumerator<float> Cor_Edit_Complete()
        {
            Manager.instance.manager_Common.Start_Loading();
            Page_Calender _pc = Manager.instance.page_Calender;
            switch (record_Type)
            {
                case Record_Type.Edit:
                    Manager.instance.manager_Data.Todo_Record_Update(data_ToDo_Record, ipf_MainText.text, () =>
                    {
                        record_Type = Record_Type.View;
                        data_ToDo_Record.maintext = ipf_MainText.text;
                        Reset();
                        Manager.instance.manager_Common.Stop_Loading();
                    });
                    break;
                case Record_Type.Insert:
                    data_Textures = new List<Manager_Common.Data_Dictionary>();
                    if (edit_texs.Count.Equals(0))
                    {
                        Insert("", "");
                    }
                    else
                    {
                        string name = $"{Manager.instance.manager_Member.Get_Data_Member.id}{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                        string[] texture_names = new string[edit_texs.Count];
                        for (int i = 0; i < edit_texs.Count; i++)
                        {
                            Manager_Common.Data_Dictionary dd = new Manager_Common.Data_Dictionary();
                            dd.key = $"{name}{i}";
                            byte[] _b = edit_texs[i].EncodeToJPG();
                            dd.value = Convert.ToBase64String(_b);
                            Manager.instance.manager_Data.Texture_Insert(dd.key, _b);
                            texture_names[i] = dd.key;
                            data_Textures.Add(dd);
                        }
                        string json = JsonMapper.ToJson(data_Textures);
                        string folder = $"selftodo/upload/member/{Manager.instance.manager_Member.Get_Data_Member.id}/texture";
                        yield return Timing.WaitUntilDone(Manager.instance.manager_Web.Cor_Upload_Texture(Manager.instance.manager_Member.Get_Data_Member.id, folder, json, (string _return) =>
                        {
                            string t = string.Join(",", texture_names);
                            Insert(_return, t);
                        }));
                    }

                    void Insert(string _rids, string _names)
                    {
                        Manager.instance.manager_Data.Todo_Record_Insert(_pc.Cur_Todo.id, _pc.Date_Year_Month_Day.ToString("yyyy-MM-dd"), ipf_MainText.text, _rids, _names, (Manager_Data.Data_Todo_Record _dtdr) =>
                        {
                            record_Type = Record_Type.View;
                            data_ToDo_Record = _dtdr;
                            Reset();

                            Page_Calender calender = Manager.instance.page_Calender;
                            calender.Find_Prefab_Day_Month(calender.Date_Year_Month_Day).Set_Record(_dtdr.id);
                            calender.Find_Prefab_Day_Year(calender.Date_Year_Month_Day).Set_Record(_dtdr.id);

                            Manager.instance.manager_Common.Stop_Loading();
                        });
                    }
                    break;
            }
        }
        #endregion
    }
}