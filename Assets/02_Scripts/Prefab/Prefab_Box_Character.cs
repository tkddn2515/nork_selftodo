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
        /// around 박스
        /// </summary>
        [SerializeField] private Image img_Box;
        /// <summary>
        /// 캐릭터 이미지
        /// </summary>
        [SerializeField] private Image img_Char;
        /// <summary>
        /// 캐릭터 이미지 얻기
        /// </summary>
        public Image get_Img_Char { get { return img_Char; } }
        /// <summary>
        /// 캐릭터 이름
        /// </summary>
        [SerializeField] private Manager_Common.Character_Name char_Name;
        /// <summary>
        /// 캐릭터 이름 얻기
        /// </summary>
        public Manager_Common.Character_Name get_Char_Name { get { return char_Name; } }

        internal override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// 클릭했을 시
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
        /// 선택했을 시
        /// </summary>
        public void Select()
        {
            img_Box.color = Manager.instance.col_Main;
        }
       
        /// <summary>
        /// 선택 해제 됐을 시
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