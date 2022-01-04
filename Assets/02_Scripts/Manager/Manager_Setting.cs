using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace NORK
{
    public class Manager_Setting : MonoBehaviour
    {
        #region ������
        [System.Serializable]
        public struct Data_Setting
        {
            public string language;//kr, en
            public int color;//0, 1, 2, 3
        }
        public Data_Setting data_Setting;
        #endregion
       
        /// <summary>
        /// ������ ����
        /// </summary>
        public IEnumerator<float> Start_Setting()
        {
            string _load = Manager.instance.manager_Common.Load_Regedit("ssh_s19");
            if (string.IsNullOrWhiteSpace(_load))
            {
                data_Setting.color = 0;
            }
            else
            {
                data_Setting = JsonUtility.FromJson<Data_Setting>(_load);
            }

            if (string.IsNullOrWhiteSpace(data_Setting.language))
            {
                Manager.instance.manager_Common.go_Start_Page_Alert.SetActive(true);
                Manager.instance.manager_Common.text_Start_Page_Alert.gameObject.SetActive(false);
                yield return Timing.WaitUntilDone(Manager.instance.manager_Web.Cor_Get_Country((_country) =>
                {
                    data_Setting.language = _country;
                    Manager.instance.manager_Common.go_Start_Page_Alert.SetActive(false);
                }));
            }

            Setting();
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            Manager.instance.manager_Common.Save_Regedit("ssh_s19", JsonUtility.ToJson(data_Setting));
        }

        /// <summary>
        /// ��� ����
        /// </summary>
        /// <param name="_lang"></param>
        public void Save_Language(string _lang)
        {
            data_Setting.language = _lang;
            Setting_Language();
            Save();
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="_col"></param>
        public void Save_Color(int _col)
        {
            data_Setting.color = _col;
            Setting_Color();
            Save();
        }

        private void Setting()
        {
            //���
            Setting_Language();

            //����
            Setting_Color();
        }

        /// <summary>
        /// ��� ����
        /// </summary>
        private void Setting_Language()
        {
            switch (data_Setting.language)
            {
                case "kr":
                    Manager_Language.lang = Manager_Language.language.kr;
                    break;
                case "en":
                default:
                    Manager_Language.lang = Manager_Language.language.en;
                    break;
            }
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        private void Setting_Color()
        {
            Manager.instance.col_Main = Manager.instance.data_Colors[data_Setting.color].col_Main;
            Manager.instance.col_Main_Light = Manager.instance.data_Colors[data_Setting.color].col_Main_Light;
            Manager.instance.col_Main_Light_Sub = Manager.instance.data_Colors[data_Setting.color].col_Main_Light_Sub;
        }
    }
}