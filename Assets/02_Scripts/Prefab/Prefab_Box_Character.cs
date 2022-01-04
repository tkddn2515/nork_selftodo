using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NORK
{
    public class Prefab_Box_Character : UI
    {
        [SerializeField] private Manager_Common.Page_Mode mode = Manager_Common.Page_Mode.Set_Profile;

        /// <summary>
        /// around �ڽ�
        /// </summary>
        [SerializeField] private Image img_Box;
        /// <summary>
        /// ĳ���� �̹���
        /// </summary>
        [SerializeField] private Image img_Char;
        /// <summary>
        /// ĳ���� �̹��� ���
        /// </summary>
        public Image get_Img_Char { get { return img_Char; } }
        /// <summary>
        /// ĳ���� �̸�
        /// </summary>
        [SerializeField] private Manager_Common.Character_Name char_Name;
        /// <summary>
        /// ĳ���� �̸� ���
        /// </summary>
        public Manager_Common.Character_Name get_Char_Name { get { return char_Name; } }

        internal override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Ŭ������ ��
        /// </summary>
        public void OnClickButton()
        {
            switch (mode)
            {
                case Manager_Common.Page_Mode.Set_Profile:
                    Manager.instance.page_Set_Profile.OnSelect_Character(this);
                    break;
                case Manager_Common.Page_Mode.Setting:
                    Manager.instance.page_Setting.OnSelect_Character(this);
                    break;
            }
        }

        /// <summary>
        /// �������� ��
        /// </summary>
        public void Select()
        {
            img_Box.color = Manager.instance.col_Main;
        }
       
        /// <summary>
        /// ���� ���� ���� ��
        /// </summary>
        public void DeSelect()
        {
            switch (mode)
            {
                case Manager_Common.Page_Mode.Set_Profile:
                    img_Box.color = Manager.instance.col_C7C7C7;
                    break;
                case Manager_Common.Page_Mode.Setting:
                    img_Box.color = Manager.instance.col_F5F5F5;
                    break;
            }
            
        }
    }
}