using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MEC;

namespace NORK
{
    public class Prefab_Texture : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Page_Record.Texture_Type texture_Type;
        [SerializeField] private GameObject go_Loading;
        [SerializeField] private RawImage raw_Img;
        [SerializeField] private GameObject go_Select;
        public RawImage Raw_Img { get { return raw_Img; } }
        [SerializeField] private string url;

        private void OnDisable()
        {
            if (cor_Texture.IsRunning)
                Timing.KillCoroutines(cor_Texture);
        }

        CoroutineHandle cor_Texture;
        public void Set_Data(string _url, Page_Record.Texture_Type _texture_Type)
        {
            Set_Url(_url);
            Active(_texture_Type);
        }

        public void Set_Url(string _url)
        {
            url = _url;
            cor_Texture = Timing.RunCoroutine(Manager.instance.manager_Ui.Cor_Texture(url, (Texture _tex) =>
            {
                raw_Img.texture = _tex;
                go_Loading.SetActive(false);
            }));
        }

        public void Set_Data(Texture2D _tex, Page_Record.Texture_Type _texture_Type)
        {
            Active(_texture_Type);
            raw_Img.texture = _tex;
            go_Loading.SetActive(false);
        }

        public void Active(Page_Record.Texture_Type _texture_Type)
        {
            gameObject.SetActive(true);
            Set_Texture(_texture_Type);
            Select(false);
        }

        public void Set_Texture(Page_Record.Texture_Type _texture_Type)
        {
            texture_Type = _texture_Type;
        }

        public void Clear()
        {
            if(raw_Img.texture != null)
            {
                if (!Manager.instance.manager_Ui.IsErrorTexture(raw_Img))
                    Destroy(raw_Img.texture);
                else
                    raw_Img.texture = null;
            }
            gameObject.SetActive(false);
            go_Loading.SetActive(true);
            Manager.instance.page_Record.Enqueue_Texture(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Manager.instance.manager_Common.PointDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(Manager.instance.manager_Common.PointUp_OneClick())
            {
                switch (texture_Type)
                {
                    case Page_Record.Texture_Type.Cut:
                        Manager.instance.page_Record.OnClickButton_Cut_Texture(this);
                        break;
                    case Page_Record.Texture_Type.Edit:
                        break;
                    case Page_Record.Texture_Type.View:
                        Manager.instance.page_Record.OnClickButton_View_Texture(this);
                        break;
                }
            }
        }

        public void Select(bool _select)
        {
            go_Select.SetActive(_select);
        }
    }
}