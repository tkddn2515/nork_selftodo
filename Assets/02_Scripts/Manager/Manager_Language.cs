using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;

namespace NORK
{
    public class Manager_Language : MonoBehaviour
    {
        /// <summary>
        /// ���
        /// </summary>
        public enum language { kr, en }

        /// <summary>
        /// ���� ������ ���
        /// </summary>
        public static language lang = language.kr;

#if UNITY_EDITOR
        public language unity_Lang;

        private void Update()
        {
            unity_Lang = lang;
        }
#endif

        /// <summary>
        /// �� ���� �ؽ�Ʈ ��������
        /// </summary>
        /// <param name="_kr"></param>
        /// <param name="_lang"></param>
        /// <returns></returns>
        public static string Get_Text(string _kr, language _lang, params string[] _str)
        {
            string _return = "";
            if (_lang.Equals(language.kr))
                _return =  _kr;
            else
            {
                int _index = data_Languages.FindIndex(n => n.kr.Equals(_kr));
                if (!_index.Equals(-1))
                {
                    switch (_lang)
                    {
                        default:
                            _return = data_Languages[_index].kr;
                            break;
                        case language.en:
                            _return = data_Languages[_index].en;
                            break;
                    }
                }
                else
                    _return = _kr;
            }
            for (int i = 0; i < _str.Length; i++)
            {
                _return = _return.Replace($"{{{i}}}", _str[i]);
            }
            return _return;
        }

        /// <summary>
        /// ��� �������ֱ�
        /// </summary>
        /// <param name="_key">Text</param>
        /// <param name="_customlang">���ϴ� ��� ��ȯ ����</param>
        /// <param name="_lang">���ϴ� ���</param>
        /// <returns></returns>
        public static string Get_Text(string _key, params string[] _obj)
        {
            int _index = data_Languages.FindIndex(n => n.kr.Equals(_key));
            string _return = "";
            if (!_index.Equals(-1))
            {
                switch (lang)
                {
                    case language.kr:
                        _return = data_Languages[_index].kr;
                        break;
                    case language.en:
                    default:
                        _return = data_Languages[_index].en;
                        break;
                }
            }
            else
            {
                _return = _key;
            }
            for (int i = 0; i < _obj.Length; i++)
            {
                _return = _return.Replace($"{{{i}}}", _obj[i]);
            }
            return _return;
        }

        /// <summary>
        /// �� �ؽ�Ʈ�� ������ �ִ� ����ü
        /// </summary>
        public struct Data_Language
        {
            public string kr;
            public string en;

            public Data_Language(string _kr, string _en)
            {
                kr = _kr;
                en = _en;
            }
        }

        public List<Prefab_Text> prefab_Texts = new List<Prefab_Text>();
        public void Change_Prefab_Text_Lang()
        {
            for (int i = 0; i < prefab_Texts.Count; i++)
            {
                prefab_Texts[i].ChangeText();
            }
        }
        [ContextMenu("Remove")]
        public void Test()
        {
            prefab_Texts.RemoveAll(n => n == null);
        }
        
        /// <summary>
        /// ���Ǵ� ��� �� �����ϰ� �ִ� ����Ʈ
        /// </summary>
        public static List<Data_Language> data_Languages = new List<Data_Language>()
        {
            new Data_Language {kr = "���� üũ ��", en = "Checking the version"},
            new Data_Language {kr = "������Ʈ �� �̿��� �ּ���.", en = "Please use it after updating."},

            new Data_Language {kr = "ȯ���մϴ�.\n#Selftodo��\nȸ���̽Ű���?", en = "Welcome.\nAre you a member of\n#Selfodo?"},
            new Data_Language {kr = "ȯ���մϴ�.<br>#Selftodo��<br>ȸ���̽Ű���?", en = "Welcome.<br>Are you a member of<br>#Selfodo?"},
            new Data_Language {kr = "ȸ������ ��, <b>�̿���</b>�� <b>��������ó����ħ</b>,<b>�����þ˸��ޱ�</b>�� ������ ������ �����մϴ�.", en = "When signing up as a member, it is considered that you have agreed to <b> Terms of Use</b> and <b> Privacy Policy</b> and <b> Marketing Notification</b>."},
            new Data_Language {kr = "��, �α��� �ҷ���.", en = "Yes, I want to log in."},
            new Data_Language {kr = "�ƴϿ�, ȸ������ �ҷ���.", en = "No, I want to sign up."},
            new Data_Language {kr = "���̵�", en = "Account"},
            new Data_Language {kr = "��й�ȣ", en = "Password"},
            new Data_Language {kr = "�α���", en = "Login"},
            new Data_Language {kr = "���̵� �Ǵ� ��й�ȣ�� Ȯ���� �ּ���.", en = "Please check your account or password."},
            new Data_Language {kr = "��й�ȣ ã��", en = "Find password"},
            new Data_Language {kr = "ȸ������", en = "Join"},
            new Data_Language {kr = "�̸��� �Է�", en = "Email"},
            new Data_Language {kr = "���̵� �Է�", en = "Account"},
            new Data_Language {kr = "������ȣ", en = "Authentication number"},
            new Data_Language {kr = "�Ϸ�", en = "Complet"},
            new Data_Language {kr = "��й�ȣ �Է�", en = "Enter password"},
            new Data_Language {kr = "��й�ȣ ���Է�", en = "Re-enter password"},
            new Data_Language {kr = "����", en = "Authentication"},
            new Data_Language {kr = "��й�ȣ �缳��", en = "Reset password"},
            new Data_Language {kr = "ĳ���͸� ������ �ּ���.", en = "Please select character."},
            new Data_Language {kr = "���̵� �Է��� �ּ���.", en = "Please enter account"},
            new Data_Language {kr = "�̸����� �Է��� �ּ���.", en = "Please enter email"},
            new Data_Language {kr = "��й�ȣ�� �Է��� �ּ���.", en = "Please enter password"},
            new Data_Language {kr = "�̸��� ���Ŀ� �°� �Է��� �ּ���.", en = "Please enter it according to the email format."},
            new Data_Language {kr = "�ϳ� �̻��� ����, ����, Ư�����ڸ� ������ 8~20�ڸ� �Է��� �ּ���.", en = "Please enter 8-20 characters, including at least one letter, number, and special character."},
            new Data_Language {kr = "������ȣ�� �Է��� �ּ���.", en = "Please enter your authentication number."},
            new Data_Language {kr = "6�ڸ��� ������ȣ�� �Է��� �ּ���.", en = "Please enter a 6-digit authentication number."},
            new Data_Language {kr = "������ȣ�� �ǹٸ��� �ʽ��ϴ�.", en = "The authentication number is not correct."},
            new Data_Language {kr = "������ �Ϸ�Ǿ����ϴ�.", en = "The authentication has been completed."},
            new Data_Language {kr = "�̹� ��ϵ� �̸����Դϴ�.", en = "This email is already registered."},
            new Data_Language {kr = "��ϵ��� ���� �̸����Դϴ�.", en = "This is an unregistered email."},
            new Data_Language {kr = "������ȣ�� ���۵Ǿ����ϴ�.", en = "The authentication number has been sent."},
            new Data_Language {kr = "�̸��� �Է��� �ּ���.", en = "Please enter your name."},

            new Data_Language {kr = "Step 1. �ƹ�Ÿ�� �������ּ���.", en = "Step 1. Choose an avatar."},
            new Data_Language {kr = "Step 2. �̸��� �Է����ּ���.", en = "Step 2. Please enter your name."},
            new Data_Language {kr = "Step 3. ������ �Է����ּ���.", en = "Step 3. Please enter your resolution."},
            new Data_Language {kr = "�̸�", en = "Name"},
            new Data_Language {kr = "��) ������ �ð��� ���ƿ��� �ʴ´�.", en = "ex) Time that has passed does not return."},
            new Data_Language {kr = "{0} ��,\n#Selftodo��\n�����غ����?", en = "{0},\nshould we start\n #Selfodo?"},
            new Data_Language {kr = "��, �����ҰԿ�!", en = "Yes, let's get started!"},
            new Data_Language {kr = "�ƴϿ�, ���ư�����.", en = "No, I want to go back."},
            new Data_Language {kr = "��", en = "S"},
            new Data_Language {kr = "��", en = "M"},
            new Data_Language {kr = "ȭ", en = "T"},
            new Data_Language {kr = "��", en = "W"},
            new Data_Language {kr = "��", en = "T"},
            new Data_Language {kr = "��", en = "F"},
            new Data_Language {kr = "��", en = "S"},
            new Data_Language {kr = "����", en = "Editing"},
            new Data_Language {kr = "���� ����", en = "Editing Todo"},
            new Data_Language {kr = "�̸� �Է�", en = "Enter name"},
            new Data_Language {kr = "���� ����", en = "Delete Todo"},
            new Data_Language {kr = "������ �Է����ּ���.", en = "Please enter the contents."},
            new Data_Language {kr = "���� �Կ��ϱ�", en = "Taking pictures."},
            new Data_Language {kr = "���� ��������", en = "Bring a picture."},
            new Data_Language {kr = "�����", en = "Delete"},
            new Data_Language {kr = "�����ϱ�", en = "Edit"},
            new Data_Language {kr = "����", en = "Save"},
            new Data_Language {kr = "�˸� ��Ʈ", en = "Reminder"},

            //Page_Calender
            new Data_Language {kr = "����", en = "Todo"},

            new Data_Language {kr = "1��", en = "Jan."},
            new Data_Language {kr = "2��", en = "Feb."},
            new Data_Language {kr = "3��", en = "Mar."},
            new Data_Language {kr = "4��", en = "Apr."},
            new Data_Language {kr = "5��", en = "May"},
            new Data_Language {kr = "6��", en = "Jun."},
            new Data_Language {kr = "7��", en = "Jul."},
            new Data_Language {kr = "8��", en = "Aug."},
            new Data_Language {kr = "9��", en = "Sep."},
            new Data_Language {kr = "10��", en = "Oct."},
            new Data_Language {kr = "11��", en = "Nov."},
            new Data_Language {kr = "12��", en = "Dec."},
            new Data_Language {kr = "{0}�� {1}�� {2}��", en = "{0}-{1}-{2}"},
            new Data_Language {kr = "�� �� �߰�", en = "Add Todo"},
            new Data_Language {kr = "�� �� ����", en = "Edit Todo"},
            new Data_Language {kr = "�� ���� �Է��� �ּ���.", en = "Please enter todo name"},
            new Data_Language {kr = "üũ�ϱ�", en = "Check"},
            new Data_Language {kr = "üũ����", en = "Uncheck"},
            new Data_Language {kr = "��Ϲ� �ۼ�", en = "Writing a record"},
            new Data_Language {kr = "��Ϲ� ����", en = "Revise the record"},

            new Data_Language {kr = "����", en = "Everyday."},
            new Data_Language {kr = "����", en = "Weekday"},
            new Data_Language {kr = "�ָ�", en = "Weekend"},

            //Page_Setting
            new Data_Language {kr = "������ ����", en = "Profile setting"},
            new Data_Language {kr = "��� ����", en = "Language setting"},
            new Data_Language {kr = "�÷� ����", en = "Color setting"},
            new Data_Language {kr = "�˸� ��Ʈ ����", en = "Promise setting"},
            new Data_Language {kr = "�˸� ��Ʈ�� �Է��� �ּ���.", en = "Please enter promise"},

            new Data_Language {kr = "������ {0}����� �߰� �����մϴ�.", en = "Up to {0} pictures can be added."},
        };

        #region Google Spread Sheet
        [System.Serializable]
        public struct Data_Google_SpreadSheet
        {
            public Data_Table table;
        }

        public Data_Google_SpreadSheet google_SpreadSheet;

        [System.Serializable]
        public struct Data_Table
        {
            //public List<Data_Cols> cols;
            public List<Data_Rows> rows;
        }

        [System.Serializable]
        public struct Data_Cols
        {
            public string label;
        }


        [System.Serializable]
        public struct Data_Rows
        {
            public List<Data_C> c;
        }

        [System.Serializable]
        public struct Data_C
        {
            public string v;
        }
        IEnumerator<float> Get_Lang()
        {
            data_Languages = new List<Data_Language>();

            UnityWebRequest www = UnityWebRequest.Get("https://docs.google.com/spreadsheets/d/1yCSkTwSWuLUxCi0Cr8ubcfXfrjrNJVp-UxUJk8DUT-U/gviz/tq?tqx=out:json");
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            string _text = www.downloadHandler.text.Substring(47);
            _text = _text.Remove(_text.Length - 2);
            Debug.Log(_text);

            google_SpreadSheet = JsonUtility.FromJson<Data_Google_SpreadSheet>(_text);

            for (int i = 0; i < google_SpreadSheet.table.rows.Count; i++)
            {
                Data_Rows _rows = google_SpreadSheet.table.rows[i];
                data_Languages.Add(new Data_Language(_rows.c[1].v, _rows.c[2].v));
            }
        }
        #endregion
    }
}