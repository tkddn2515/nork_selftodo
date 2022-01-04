using UnityEngine;
using LitJson;
using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine.Events;

namespace NORK
{
    public class Manager_Data : MonoBehaviour
    {
        private string default_Folder;

        /// <summary>
        /// 데이터 이름 상수
        /// </summary>
        private const string TODO = "todo", TODO_DONE = "todo_done", TODO_RECORD = "todo_record", TEXTURE = "texture";

        private void Start()
        {
            default_Folder = Application.persistentDataPath;

            CreateDirectory(default_Folder);


        }

        #region 구조체
        /// <summary>
        /// 할 일
        /// </summary>
        [Serializable]
        public struct Data_Todo
        {
            public string id;
            public string mid;
            public string name;
            public string usemanage;
            public string sunday;
            public string monday;
            public string tuesday;
            public string wednesday;
            public string thursday;
            public string friday;
            public string saturday;
            public DateTime managetime;
#if UNITY_EDITOR
            public string strmanageTime;
#endif

            public Data_Todo(string _id, string _mid = "", string _name = "", string _usemanage = "0", string _sunday = "0", string _monday = "0", string _tuesday = "0", string _wednesday = "0", string _thursday = "0", string _friday = "0", string _saturday = "0", string _managetime = "", string _createtime = "")
            {
                id = _id;
                mid = _mid;
                name = _name;
                usemanage = _usemanage;
                sunday = _sunday;
                monday = _monday;
                tuesday = _tuesday;
                wednesday = _wednesday;
                thursday = _thursday;
                friday = _friday;
                saturday = _saturday;
                if (!string.IsNullOrWhiteSpace(_managetime))
                    managetime = Convert.ToDateTime(_managetime);
                else
                    managetime = DateTime.Now;
#if UNITY_EDITOR
                strmanageTime = managetime.ToString();
#endif
            }

            public Data_Todo(JsonData _json)
            {
                id = _json["id"].ToString();
                mid = _json["mid"].ToString();
                name = _json["name"].ToString();
                usemanage = _json["usemanage"].ToString();
                sunday = _json["sunday"].ToString();
                monday = _json["monday"].ToString();
                tuesday = _json["tuesday"].ToString();
                wednesday = _json["wednesday"].ToString();
                thursday = _json["thursday"].ToString();
                friday = _json["friday"].ToString();
                saturday = _json["saturday"].ToString();
                managetime = Convert.ToDateTime(_json["managetime"].ToString());
#if UNITY_EDITOR
                strmanageTime = managetime.ToString();
#endif
            }
        }

        /// <summary>
        ///한 일
        /// </summary>
        [Serializable]
        public struct Data_Todo_Done
        {
            public string id;
            public string mid;
            public string tdid;
            public DateTime date;
#if UNITY_EDITOR
            public string strDate;
#endif
            public Data_Todo_Done(string _id, string _mid, string _tdid, string _date)
            {
                id = _id;
                mid = _mid;
                tdid = _tdid;
                date = DateTime.Parse(_date);
#if UNITY_EDITOR
                strDate = date.ToString("yyyy-MM-dd");
#endif
            }

            public Data_Todo_Done(JsonData _json)
            {
                id = _json["id"].ToString();
                mid = _json["mid"].ToString();
                tdid = _json["tdid"].ToString();
                date = DateTime.Parse(_json["date"].ToString());
#if UNITY_EDITOR
                strDate = date.ToString("yyyy-MM-dd");
#endif
            }
        }
        /// <summary>
        /// 게시글
        /// </summary>
        [Serializable]
        public struct Data_Todo_Record
        {
            public string id;
            public string mid;
            public string tdid;
            public DateTime date;
            public string rids;//리소스 아이디들
            public string names;//리소스 이름들
            public string maintext;
#if UNITY_EDITOR
            public string strDate;
#endif

            public Data_Todo_Record(string _id, string _mid, string _tdid, string _date, string _rids, string _names, string _maintext)
            {
                id = _id;
                mid = _mid;
                tdid = _tdid;
                date = DateTime.Parse(_date);
                rids = _rids;
                names = _names;
                maintext = _maintext;

#if UNITY_EDITOR
                strDate = date.ToString("yyyy-MM-dd");
#endif
            }

            public Data_Todo_Record(JsonData _json)
            {
                id = _json["id"].ToString();
                mid = _json["mid"].ToString();
                tdid = _json["tdid"].ToString();
                date = DateTime.Parse(_json["date"].ToString());
                rids = _json["rids"].ToString();
                names = _json.Keys.Contains("names") ? _json["names"].ToString() : "";
                maintext = Manager_Common.Regular_Expression_Encryption(_json["maintext"].ToString());

#if UNITY_EDITOR
                strDate = date.ToString("yyyy-MM-dd");
#endif
            }
        }

        /// <summary>
        /// 리소스
        /// </summary>
        [Serializable]
        public struct Data_Resource
        {
            public string name;
            public string rid;
        }
        #endregion

        #region 데이터
        [Header("할 일 리스트")]
        [Header("데이터")]
        [Tooltip("할 일 리스트")]
        public List<Data_Todo> data_ToDos;

        [Header("한 일 리스트")]
        [Tooltip("한 일 리스트")]
        public List<Data_Todo_Done> data_ToDo_Dones;

        [Header("게시글 리스트")]
        [Tooltip("게시글 리스트")]
        public List<Data_Todo_Record> data_ToDo_Records;
        [Tooltip("리소스 id-url 리스트")]
        public List<Manager_Common.Data_Dictionary> resources;
        #endregion

        #region 공통
        private string Get_Todo_Path()
        {
            string _path = Path.Combine(default_Folder, Manager.instance.manager_Member.Get_Data_Member.id);
            CreateDirectory(_path);

            _path = Path.Combine(_path, TODO);
            CreateDirectory(_path);

            return Path.Combine(_path, "todo");
        }

        private string Get_Todo_Done_Path(string tdid, string date)
        {
            string _path = Path.Combine(default_Folder, Manager.instance.manager_Member.Get_Data_Member.id);
            CreateDirectory(_path);

            _path = Path.Combine(_path, TODO_DONE);
            CreateDirectory(_path);

            _path = Path.Combine(_path, tdid);
            CreateDirectory(_path);

            return Path.Combine(_path, date);
        }

        private string Get_Todo_Record_Path(string tdid, string date)
        {
            string _path = Path.Combine(default_Folder, Manager.instance.manager_Member.Get_Data_Member.id);
            CreateDirectory(_path);

            _path = Path.Combine(_path, TODO_RECORD);
            CreateDirectory(_path);

            _path = Path.Combine(_path, tdid);
            CreateDirectory(_path);

            return Path.Combine(_path, date);
        }

        private string Get_Texture_Path(string name)
        {
            string _path = Path.Combine(default_Folder, Manager.instance.manager_Member.Get_Data_Member.id);
            CreateDirectory(_path);

            _path = Path.Combine(_path, TEXTURE);
            CreateDirectory(_path);

            return Path.Combine(_path, name);
        }

        private void Save_Data(string _path, string _json)
        {
            _json = Manager.instance.manager_Common.Compress_Text(_json);
            File.WriteAllText(_path, _json);
        }

        private string Load_Data(string _path)
        {
            if (File.Exists(_path))
                return Manager.instance.manager_Common.Decompress_Text(File.ReadAllText(_path));
            else
                return "";
        }

        void CreateDirectory(string _directory)
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }
        #endregion

        #region 할 일
        public bool todo_List_Loaded;
        /// <summary>
        /// 할 일 리스트 불러오기
        /// </summary>
        public void Todo_List(UnityAction action = null)
        {
            data_ToDos = new List<Data_Todo>();
            todo_List_Loaded = false;
#if USE_LOGIN
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Todo_List(Manager.instance.manager_Member.Get_Data_Member.id, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                for (int i = 0; i < _json.Count; i++)
                {
                    data_ToDos.Add(new Data_Todo(_json[i]));
                }
                todo_List_Loaded = true;

                action?.Invoke();
            }));
#else
            string _data = Load_Data(Get_Todo_Path());
            if (!string.IsNullOrWhiteSpace(_data))
            {
                JsonData _json = JsonMapper.ToObject(_data);

                for (int i = 0; i < _json.Count; i++)
                    data_ToDos.Add(new Data_Todo(_json[i]));
            }
            todo_List_Loaded = true;
            action?.Invoke();
#endif
        }

        CoroutineHandle cor_Insert_Todo;
        /// <summary>
        /// 할 일 추가
        /// </summary>
        public void Todo_Insert(string _name = "", bool _usemanage = false, string _sunday = "0", string _monday = "0", string _tuesday = "0", string _wednesday = "0", string _thursday = "0", string _friday = "0", string _saturday = "0", string _managetime = "", UnityAction<Data_Todo> action = null)
        {
            Manager.instance.manager_Common.Start_Loading();

            string _Usemanage = _usemanage ? "1" : "0";

            Manager_Common.StartCoroutine(ref cor_Insert_Todo, Manager.instance.manager_Web.Cor_Todo_Insert(Manager.instance.manager_Member.Get_Data_Member.id, _name, _Usemanage, _sunday, _monday, _tuesday, _wednesday, _thursday, _friday, _saturday, _managetime, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                string _id = _json["id"].ToString();
                Data_Todo dtd = new Data_Todo(_id, Manager.instance.manager_Member.Get_Data_Member.id, _name, _Usemanage, _sunday, _monday, _tuesday, _wednesday, _thursday, _friday, _saturday, _managetime, _id);
                data_ToDos.Add(dtd);

                Save_Data(Get_Todo_Path(), JsonMapper.ToJson(data_ToDos));

                action?.Invoke(dtd);
                Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// 할 일 수정
        /// </summary>
        public void Todo_Update(string _id, string _name = "", bool _usemanage = false, string _sunday = "0", string _monday = "0", string _tuesday = "0", string _wednesday = "0", string _thursday = "0", string _friday = "0", string _saturday = "0", string _managetime = "", UnityAction<Data_Todo> action = null)
        {
            string _Usemanage = _usemanage ? "1" : "0";
            
            if(int.TryParse(_id, out int _result))
                Manager_Common.StartCoroutine(ref cor_Insert_Todo, Manager.instance.manager_Web.Cor_Todo_Update(_id, _name, _Usemanage, _sunday, _monday, _tuesday, _wednesday, _thursday, _friday, _saturday, _managetime, null));

            int _index = data_ToDos.FindIndex(n => n.id.Equals(_id));
            Data_Todo dtd = data_ToDos[_index];
            dtd.name = _name;
            dtd.usemanage = _Usemanage;
            dtd.sunday = _sunday;
            dtd.monday = _monday;
            dtd.tuesday = _tuesday;
            dtd.wednesday = _wednesday;
            dtd.thursday = _thursday;
            dtd.friday = _friday;
            dtd.saturday = _saturday;
            dtd.managetime = Convert.ToDateTime(_managetime);
#if UNITY_EDITOR
            dtd.strmanageTime = dtd.managetime.ToString();
#endif
            data_ToDos[_index] = dtd;

            Save_Data(Get_Todo_Path(), JsonMapper.ToJson(data_ToDos));

            action?.Invoke(dtd);
        }

        /// <summary>
        /// 할 일 삭제
        /// </summary>
        public void Todo_Delete(string _id, UnityAction action = null)
        {
            Debug.Log("Delete " + _id);
            if(int.TryParse(_id, out int _result))
                Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Todo_Delete(_id));

            int _index = data_ToDos.FindIndex(n => n.id.Equals(_id));
            if (!_index.Equals(-1))
                data_ToDos.RemoveAt(_index);

            Save_Data(Get_Todo_Path(), JsonMapper.ToJson(data_ToDos));

            action?.Invoke();
        }
        #endregion

        #region 한 일
        public bool todo_Done_List_Loaded = true;

        /// <summary>
        /// 한 일 불러오기
        /// </summary>
        /// <param name="_dateTime"></param>
        public void Todo_Done_List(string _tdid, string _date, UnityAction action = null)
        {
            data_ToDo_Dones = new List<Data_Todo_Done>();
            todo_Done_List_Loaded = false;
#if USE_LOGIN
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Todo_Done_List(Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                for (int i = 0; i < _json.Count; i++)
                {
                    data_ToDo_Done.Add(new Data_Todo_Done(_json[i]));
                }
                todo_Done_List_Loaded = true;
                action?.Invoke();
            }));
#else
            Get_Data($"{int.Parse(_date) - 1}-12");
            for (int i = 1; i < 13; i++)
            {
                Get_Data($"{_date}-{i.ToString("0#")}");
            }
            Get_Data($"{int.Parse(_date) + 1}-01");

            todo_Done_List_Loaded = true;
            action?.Invoke();

            void Get_Data(string _date)
            {
                string _data = Load_Data(Get_Todo_Done_Path(_tdid, _date));
                if (!string.IsNullOrWhiteSpace(_data))
                {
                    JsonData _json = JsonMapper.ToObject(_data);

                    for (int j = 0; j < _json.Count; j++)
                        data_ToDo_Dones.Add(new Data_Todo_Done(_json[j]));
                }
            }
#endif
        }

        CoroutineHandle cor_Insert_Todo_Done;
        /// <summary>
        /// 한 일 추가
        /// </summary>
        public void Todo_Done_Insert(string _tdid = "", string _date = "", UnityAction<string> action = null)
        {
            Manager.instance.manager_Common.Start_Loading();
            Manager_Common.StartCoroutine(ref cor_Insert_Todo_Done, Manager.instance.manager_Web.Cor_Todo_Done_Insert(Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                string _id = _json["id"].ToString();
                Data_Todo_Done dtdd = new Data_Todo_Done(_id, Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date);
                data_ToDo_Dones.Add(dtdd);

                string[] _dates = _date.Split('-');
                List<Data_Todo_Done> dtdds = data_ToDo_Dones.FindAll(n => n.date.Year.ToString().Equals(_dates[0]) && n.date.Month.ToString("0#").Equals(_dates[1]));
                Save_Data(Get_Todo_Done_Path(_tdid, $"{_dates[0]}-{_dates[1]}"), JsonMapper.ToJson(dtdds));
                dtdds = null;

                action?.Invoke(_id);
            Manager.instance.manager_Common.Stop_Loading();
            }));
        }

        /// <summary>
        /// 한 일 취소
        /// </summary>
        public void Todo_Done_Delete(string _id, UnityAction action = null)
        {
            int _index = data_ToDo_Dones.FindIndex(n => n.id.Equals(_id));
            if (!_index.Equals(-1))
            {
                string _tdid = data_ToDo_Dones[_index].tdid;
                string[] _dates = Manager.instance.manager_Common.DateTimeToYM(data_ToDo_Dones[_index].date).Split('-');
	    data_ToDo_Dones.RemoveAt(_index);                
	    List<Data_Todo_Done> dtdds = data_ToDo_Dones.FindAll(n => n.date.Year.ToString().Equals(_dates[0]) && n.date.Month.ToString("0#").Equals(_dates[1]));
                Save_Data(Get_Todo_Done_Path(_tdid, $"{_dates[0]}-{_dates[1]}"), JsonMapper.ToJson(dtdds));
                dtdds = null;
            }

            action?.Invoke();

            if(int.TryParse(_id, out int _result))
                Manager_Common.StartCoroutine(ref cor_Insert_Todo_Done, Manager.instance.manager_Web.Cor_Todo_Done_Delete(_id));
        }
        #endregion

        #region 게시글
        public bool todo_Record_List_Loaded = true;

        /// <summary>
        /// 게시글 불러오기
        /// </summary>
        /// <param name="_dateTime"></param>
        public void Todo_Record_List(string _tdid, string _date)
        {
            data_ToDo_Records = new List<Data_Todo_Record>();
            todo_Record_List_Loaded = false;
#if USE_LOGIN
            Timing.RunCoroutine(Manager.instance.manager_Web.Cor_Todo_Record_List(Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                for (int i = 0; i < _json.Count; i++)
                {
                    data_ToDo_Records.Add(new Data_Todo_Record(_json[i]));
                }
                todo_Record_List_Loaded = true;
            }));
#else
            Get_Data($"{int.Parse(_date) - 1}-12");
            for (int i = 1; i < 13; i++)
            {
                Get_Data($"{_date}-{i.ToString("0#")}");
            }
            Get_Data($"{int.Parse(_date) + 1}-01");

            todo_Record_List_Loaded = true;

            void Get_Data(string _date)
            {
                string _data = Load_Data(Get_Todo_Record_Path(_tdid, _date));
                if (!string.IsNullOrWhiteSpace(_data))
                {
                    JsonData _json = JsonMapper.ToObject(_data);

                    for (int j = 0; j < _json.Count; j++)
                        data_ToDo_Records.Add(new Data_Todo_Record(_json[j]));
                }
            }
#endif
        }

        CoroutineHandle cor_Insert_Todo_Record;
        /// <summary>
        /// 게시글 추가
        /// </summary>
        public void Todo_Record_Insert(string _tdid = "", string _date = "", string _maintext = "", string _rids = "", string _names = "", UnityAction<Data_Todo_Record> action = null)
        {
            Manager_Common.StartCoroutine(ref cor_Insert_Todo_Record, Manager.instance.manager_Web.Cor_Todo_Record_Insert(Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date, _maintext, _rids, (string _return) =>
            {
                JsonData _json = JsonMapper.ToObject(_return);
                string _id = _json["id"].ToString();
                Data_Todo_Record dtdr = new Data_Todo_Record(_id, Manager.instance.manager_Member.Get_Data_Member.id, _tdid, _date, _rids, _names, _maintext);
                data_ToDo_Records.Add(dtdr);

                string[] _dates = _date.Split('-');
                List<Data_Todo_Record> dtdrs = data_ToDo_Records.FindAll(n => n.date.Year.ToString().Equals(_dates[0]) && n.date.Month.ToString("0#").Equals(_dates[1]));
                Save_Data(Get_Todo_Record_Path(_tdid, $"{_dates[0]}-{_dates[1]}"), JsonMapper.ToJson(dtdrs));
                dtdrs = null;

                action?.Invoke(dtdr);
            }));
        }

        /// <summary>
        /// 게시글 수정
        /// </summary>
        public void Todo_Record_Update(Data_Todo_Record _dtdr, string _maintext = "", UnityAction action = null)
        {
            int _index = data_ToDo_Records.FindIndex(n => n.id.Equals(_dtdr.id));
            if (!_index.Equals(-1))
            {
                Data_Todo_Record dtdr = data_ToDo_Records[_index];
                dtdr.maintext = _maintext;
                data_ToDo_Records[_index] = dtdr;

                string _tdid = dtdr.tdid;
                string[] _dates = Manager.instance.manager_Common.DateTimeToYM(dtdr.date).Split('-');
                List<Data_Todo_Record> dtdrs = data_ToDo_Records.FindAll(n => n.date.Year.ToString().Equals(_dates[0]) && n.date.Month.ToString("0#").Equals(_dates[1]));
                Save_Data(Get_Todo_Record_Path(_tdid, $"{_dates[0]}-{_dates[1]}"), JsonMapper.ToJson(dtdrs));
                dtdrs = null;
            }
            action?.Invoke();
            if(int.TryParse(_dtdr.id, out int _result))
                Manager_Common.StartCoroutine(ref cor_Insert_Todo_Record, Manager.instance.manager_Web.Cor_Todo_Record_Update(_dtdr.id, _maintext));
        }

        /// <summary>
        /// 게시글 삭제
        /// </summary>
        public void Todo_Record_Delete(string _id, UnityAction action = null)
        {
            int _index = data_ToDo_Records.FindIndex(n => n.id.Equals(_id));
            if (!_index.Equals(-1))
            {
                string _tdid = data_ToDo_Records[_index].tdid;
                string[] _dates = Manager.instance.manager_Common.DateTimeToYM(data_ToDo_Records[_index].date).Split('-');
                data_ToDo_Records.RemoveAt(_index);
                List<Data_Todo_Record> dtdrs = data_ToDo_Records.FindAll(n => n.date.Year.ToString().Equals(_dates[0]) && n.date.Month.ToString("0#").Equals(_dates[1]));
                Save_Data(Get_Todo_Record_Path(_tdid, $"{_dates[0]}-{_dates[1]}"), JsonMapper.ToJson(dtdrs));
                dtdrs = null;
            }

            action?.Invoke();
            if(int.TryParse(_id, out int _result))
                Manager_Common.StartCoroutine(ref cor_Insert_Todo_Record, Manager.instance.manager_Web.Cor_Todo_Record_Delete(_id));
        }
        #endregion

        #region 텍스쳐
        /// <summary>
        /// 텍스쳐 로컬 저장
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_bytes"></param>
        public void Texture_Insert(string _name, byte[] _bytes)
        {
            File.WriteAllBytes(Get_Texture_Path(_name), _bytes);
        }

        public void Texture_Delete(string _name)
        {
            string _fullPath = Get_Texture_Path(_name);
            if (File.Exists(_fullPath))
            {
                File.Delete(_fullPath);
            }
        }

        public string Texture_FullPath(string _name)
        {
            return Get_Texture_Path(_name);
        }
        #endregion
    }
}