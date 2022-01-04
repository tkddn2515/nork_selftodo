using System;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.IO;

namespace NORK
{
    public class Manager_Webrequest : MonoBehaviour
    {
        private static string selftodo_url = "https://www.nork.so/nork/selftodo";
        private static string objectstorage_url = "https://www.nork.so/objectstorage";

        #region 현재 국가 가져오기
        [Serializable]
        public class IpApiData
        {
            public string country_name;
            public string country_code;
            public static IpApiData CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<IpApiData>(jsonString);
            }
        }

        public IEnumerator<float> Cor_Get_Country(UnityAction<string> _action)
        {
            string ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            string uri = $"https://ipapi.co/{ip}/json/";

            UnityWebRequest www = UnityWebRequest.Get(uri);

            yield return Timing.WaitUntilDone(www.SendWebRequest());

            string country = "";

            if (www.result == UnityWebRequest.Result.Success)
            {
                country = IpApiData.CreateFromJSON(www.downloadHandler.text).country_code.ToLower();
            }
            if (string.IsNullOrWhiteSpace(country) || !country.Equals("kr"))
                country = "en";
            _action?.Invoke(country);
        }
        #endregion

        #region 인증
        /// <summary>
        /// 인증번호 추가하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Cert_Insert(string to, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "to"
               },
               new string[] {
                   "ct0000",
                   to
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원이 존재하는지 체크 후 인증번호 추가하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Cert_Insert_Check_Member(string to, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "to"
               },
               new string[] {
                   "ct0000_1",
                   to
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 인증번호 확인하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Cert_Check(string to, string cert, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "to",
                   "cert"
               },
               new string[] {
                   "ct0001",
                   to,
                   cert
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);

            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 멤버
        /// <summary>
        /// 회원정보 추가하기(로그인 이용 할 시)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Insert(string email, string pwd, string platform, string token, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "email",
                   "pwd",
                   "platform",
                   "token",
               },
               new string[] {
                   "mb0000",
                   email,
                   pwd,
                   platform,
                   token
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                Debug.Log($"Web Text : {www.downloadHandler.text}");
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 추가하기(로그인 이용 안할 시)
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Insert(string devicemodel, string devicename, string deviceuniqueIdentifier, string nickname, string character, string promise, string platform, string token, UnityAction<string> action)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "devicemodel",
                   "devicename",
                   "deviceuniqueIdentifier",
                   "platform",
                   "nickname",
                   "character",
                   "promise",
                   "token"
               },
               new string[] {
                   "mb0000_1",
                   devicemodel,
                   devicename,
                   deviceuniqueIdentifier,
                   platform,
                   nickname,
                   character,
                   promise,
                   token
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if(!www.result.Equals(UnityWebRequest.Result.Success))
            {
                action?.Invoke(null);
            }
            else
            {
                string webrequest_text = ReturnValue(www.downloadHandler.text);
                action?.Invoke(webrequest_text);
            }

            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 수정하기 - 닉네임, 캐릭터, 다짐
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Update_Nickname_Character_Promise(string id, string nickname, string character, string promise, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id",
                   "nickname",
                   "character",
                   "promise"
               },
               new string[] {
                   "mb0001",
                   id,
                   nickname,
                   character,
                   promise
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                Debug.Log($"Web Text : {webRequest_text}");

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 수정하기 - 비밀번호
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Update_Password(string email, string pwd, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "email",
                   "pwd"
               },
               new string[] {
                   "mb0001_1",
                   email,
                   pwd
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                action?.Invoke();

            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 수정하기 - 닉네임, 캐릭터
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Update_Nickname_Character(string id, string nickname, string character, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id",
                   "nickname",
                   "character"
               },
               new string[] {
                   "mb0001_2",
                   id,
                   nickname,
                   character
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
                action?.Invoke(null);
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                Debug.Log($"Web Text : {webRequest_text}");

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 수정하기 - 다짐
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Update_Promise(string id, string promise, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id",
                   "promise"
               },
               new string[] {
                   "mb0001_3",
                   id,
                   promise
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 회원정보 확인하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Member_Check(string email, string pwd, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "email",
                   "pwd"
               },
               new string[] {
                   "mb0002",
                   email,
                   pwd
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);

            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 할 일
        /// <summary>
        /// 할 일 추가하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Insert(string mid, string name, string usemanage, string sunday, string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, string managetime, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "name",
                   "usemanage",
                   "sunday",
                   "monday",
                   "tuesday",
                   "wednesday",
                   "thursday",
                   "friday",
                   "saturday",
                   "managetime"
               },
               new string[] {
                   "td0000",
                   mid,
                   Manager_Common.Regular_Expression_Encryption(name),
                   usemanage,
                   sunday,
                   monday,
                   tuesday,
                   wednesday,
                   thursday,
                   friday,
                   saturday,
                   managetime,
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result.Equals(UnityWebRequest.Result.Success))
            {
                string result = www.downloadHandler.text;
                Debug.Log("result : " + result);
                action?.Invoke(result);
            }
            else
            {
                action?.Invoke(CurTimeToJson());
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }

            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 할 일 수정하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Update(string id, string name, string usemanage, string sunday, string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, string managetime, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id",
                   "name",
                   "usemanage",
                   "sunday",
                   "monday",
                   "tuesday",
                   "wednesday",
                   "thursday",
                   "friday",
                   "saturday",
                   "managetime"
               },
               new string[] {
                   "td0001",
                   id,
                   Manager_Common.Regular_Expression_Encryption(name),
                   usemanage,
                   sunday,
                   monday,
                   tuesday,
                   wednesday,
                   thursday,
                   friday,
                   saturday,
                   managetime,
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                action?.Invoke();
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 할 일 삭제하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Delete(string id)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id"
               },
               new string[] {
                   "td0003",
                   id
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {

            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 할 일 불러오기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_List(string mid, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid"
               },
               new string[] {
                   "td0002",
                   mid
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 한일
        /// <summary>
        /// 한 일 추가하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Done_Insert(string mid, string tdid, string date, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "tdid",
                   "date"
               },
               new string[] {
                   "tdd0000",
                   mid,
                   tdid,
                   date,
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result.Equals(UnityWebRequest.Result.Success))
            {
                string result = www.downloadHandler.text;
                action?.Invoke(result);
            }
            else
            {
                action?.Invoke(CurTimeToJson());
            }

            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 한 일 삭제하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Done_Delete(string id, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id"
               },
               new string[] {
                   "tdd0001",
                   id
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
                action?.Invoke();
            }
            else
            {
                action?.Invoke();
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 한 일 리스트 불러오기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Done_List(string mid, string tdid, string date, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "tdid",
                   "date",
                   "today"
               },
               new string[] {
                   "tdd0002",
                   mid,
                   tdid,
                   date,
                   DateTime.Now.ToString("yyyy-MM-dd")
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 게시글
        /// <summary>
        /// 게시글 추가하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Record_Insert(string mid, string tdid, string date, string maintext, string rids, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "tdid",
                   "date",
                   "maintext",
                   "rids"
               },
               new string[] {
                   "tdr0000",
                   mid,
                   tdid,
                   date,
                   Manager_Common.Regular_Expression_Encryption(maintext),
                   Manager_Common.Regular_Expression_Encryption(rids)
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result.Equals(UnityWebRequest.Result.Success))
            {
                string result = www.downloadHandler.text;
                action?.Invoke(result);
            }
            else
            {
                action?.Invoke(CurTimeToJson());
            }

            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 게시글 수정하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Record_Update(string id, string maintext, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id",
                   "maintext"
               },
               new string[] {
                   "tdr0001",
                   id,
                   maintext,
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                action?.Invoke();
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 게시글 불러오기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Record_List(string mid, string tdid, string date, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "tdid",
                   "date"
               },
               new string[] {
                   "tdr0002",
                   mid,
                   tdid,
                   date
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }

        /// <summary>
        /// 게시글 삭제하기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Todo_Record_Delete(string id, UnityAction action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "id"
               },
               new string[] {
                   "tdr0003",
                   id
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
                action?.Invoke();
            }
            else
            {
                action?.Invoke();
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 리소스
        /// <summary>
        /// 리소스 불러오기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Resource_List(string rids, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "rids"
               },
               new string[] {
                   "rs0000",
                   rids
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 오브젝트 스토리지
        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Upload(string uploadFilePath, string localFilePath)
        {
            Debug.Log("Cor_Upload : " + uploadFilePath);
            WWWForm form = new WWWForm();

            form.AddField("folder", uploadFilePath);
            form.AddBinaryData("userfile", File.ReadAllBytes(localFilePath), DateTime.Now.ToString("yyyyMMddHHmmss") + "." + Path.GetExtension(localFilePath));

            string url = $"{objectstorage_url}/aws_upload";

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("www Upload " + www.error);
            }
            else
            {
                string webRequest_text = www.downloadHandler.text;
                Debug.Log("webRequest_text : " + webRequest_text);
            }
        }

        /// <summary>
        /// 텍스쳐 업로드
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Upload_Texture(string mid, string folder, string json, UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            form.AddField("mid", mid);
            form.AddField("folder", folder);
            form.AddField("json", json);

            string url = $"{objectstorage_url}/aws_upload_texure";

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("www Upload " + www.error);
            }
            else
            {
                string webRequest_text = www.downloadHandler.text;
                action?.Invoke(webRequest_text);
            }
        }
        #endregion

        #region 로그
        /// <summary>
        /// 로그인 & 접속시간 추가
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Log_Login_Insert(UnityAction<string> action = null)
        {
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "mid",
                   "devicemodel",
                   "devicename",
                   "deviceuniqueIdentifier"
               },
               new string[] {
                   "log0000",
                   Manager.instance.manager_Member.Get_Data_Member.id,
                   SystemInfo.deviceModel,
                   SystemInfo.deviceName,
                   SystemInfo.deviceUniqueIdentifier
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        #region 버전
        /// <summary>
        /// 최신 버전 가져오기
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Check_Version(UnityAction<string> action = null)
        {
            string type = "android";
#if UNITY_ANDROID
            type = "android";
#elif UNITY_IOS
            type = "ios";
#endif
            WWWForm form = new WWWForm();

            string error = Crypto.Encrypt_Web(Manager.instance.manager_Common.Get_Data_Dictionarys(
               new string[] {
                   "php",
                   "type"
               },
               new string[] {
                   "vs0000",
                   type
               }
               ));
            form.AddField("error", error);
            string url = selftodo_url + "/server/sy0000";
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"WebRequset Error : {www.error}, error url is {url}");
                action?.Invoke("{\"version\":001}");
            }
            else
            {
                string webRequest_text = ReturnValue(www.downloadHandler.text);

                if (webRequest_text.Equals("[MULTIVERSE ERROR]"))
                    yield break;
                action?.Invoke(webRequest_text);
            }
            www.Abort();
            www.Dispose();
            www = null;
        }
        #endregion

        /// <summary>
        /// 현재 시간 to Json
        /// </summary>
        /// <returns></returns>
        private string CurTimeToJson()
        {
            return $"{{\"id\":\"local{DateTime.Now.ToString("yyyyMMddHHmmss")}\"}}";
        }

        /// <summary>
        /// [NORK ERROR]인 경우 다음 함수들은 실행되면 안 됨
        /// </summary>
        /// <param name="_text"></param>
        /// <returns></returns>
        public string ReturnValue(string _text, bool crypto = true)
        {
#if UNITY_EDITOR
            if (_text.Contains("Query error"))
            {
                Debug.LogError(_text);
            }
#endif
            string _tmp = _text.Replace("﻿", "").Replace("<br>", "\n");
            if (crypto && !string.IsNullOrEmpty(_tmp.Trim()))
                _tmp = Crypto.Decrypt_Web(_tmp);
            Error error = new Error();
            if (error.IsError(_tmp))
            {
                Debug.LogError("error : " + _tmp);
                return "[NORK ERROR]";
            }
            return _tmp;
        }
    }
    public class Error
    {
        public const string error0001 = "error 0001 : There is no login information. Please log in again.";
        public const string error_1001 = "Warning : 비정상적 접근 감지";

        public bool IsError(string _value)
        {
            bool isError = false;
            if (_value.Contains(error0001))
            {
                isError = true;
            }
            else if (_value.Contains(error_1001))
            {
                isError = true;
            }
            return isError;
        }
    }
}

