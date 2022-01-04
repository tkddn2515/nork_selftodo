using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace NORK
{
    public class Manager_UI : MonoBehaviour
    {
        /// <summary>
        /// Recttransform 움직이기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Pos_Anchored(RectTransform rect, Vector2 arrive, float speed = 5, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();

            while (Vector2.Distance(rect.anchoredPosition, arrive) > 5)
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, arrive, Time.smoothDeltaTime * speed);
                action?.Invoke();
                yield return Timing.WaitForSeconds(0);
            }
            rect.anchoredPosition = arrive;
            end_Action?.Invoke();
        }

        /// <summary>
        /// Recttransform 움직이기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Pos_Anchored(RectTransform[] rect, Vector2[] arrive, float speed = 5, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();

            while (Vector2.Distance(rect[0].anchoredPosition, arrive[0]) > 5)
            {
                for (int i = 0; i < rect.Length; i++)
                {
                    rect[i].anchoredPosition = Vector2.Lerp(rect[i].anchoredPosition, arrive[i], Time.smoothDeltaTime * speed);
                }
                
                action?.Invoke();
                yield return Timing.WaitForSeconds(0);
            }
            for (int i = 0; i < rect.Length; i++)
            {
                rect[i].anchoredPosition = arrive[i];
            }
            
            end_Action?.Invoke();
        }

        /// <summary>
        /// Recttransform 회전하기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Rot_Anchored(RectTransform rect, Vector3 arrive, float speed = 5, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();

            Quaternion arriveQuat = Quaternion.Euler(arrive.x, arrive.y, arrive.z);

            while (Vector2.Distance(rect.localEulerAngles, arrive) > 1)
            {
                rect.localRotation = Quaternion.Slerp(rect.localRotation, arriveQuat, Time.smoothDeltaTime * speed);
                action?.Invoke();
                yield return Timing.WaitForSeconds(0);
            }
            rect.anchoredPosition = arrive;
            end_Action?.Invoke();
        }

        /// <summary>
        /// Recttransform 크기조절하기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Scale_Anchored(RectTransform rect, Vector3 arrive, float speed = 5, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();

            while (Vector3.Distance(rect.localScale, arrive) > 0.1f)
            {
                rect.localScale = Vector3.Lerp(rect.localScale, arrive, Time.smoothDeltaTime * speed);
                action?.Invoke();
                yield return Timing.WaitForSeconds(0);
            }
            rect.localScale = arrive;
            end_Action?.Invoke();
        }

        /// <summary>
        /// Recttransform Rect 조절하기
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> Cor_Rect_Anchored(RectTransform rect, Vector2 arrive, float speed = 5, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();

            while (Vector2.Distance(rect.sizeDelta, arrive) > 1)
            {
                rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, arrive, Time.smoothDeltaTime * speed);
                action?.Invoke();
                yield return Timing.WaitForSeconds(0);
            }
            rect.sizeDelta = arrive;
            end_Action?.Invoke();
        }

        /// <summary>
        /// Image 색 변경
        /// </summary>
        public IEnumerator<float> Cor_Color(MaskableGraphic graphic, Color arrive, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            if (graphic == null) yield break;
            start_Action?.Invoke();
            float duration = 0.5f;
            float smoothness = 0.02f;
            float progress = 0;
            float increment = smoothness / duration;
            while (progress < 1)
            {
                graphic.color = Color.Lerp(graphic.color, arrive, progress);
                progress += increment;
                action?.Invoke();
                yield return Timing.WaitForSeconds(smoothness);
            }
            end_Action?.Invoke();
        }

        /// <summary>
        /// Image 색 변경
        /// </summary>
        public IEnumerator<float> Cor_Color(MaskableGraphic[] graphic, Color arrive, UnityAction start_Action = null, UnityAction action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();
            float duration = 0.5f; // This will be your time in seconds.
            float smoothness = 0.02f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
            float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; //The amount of change to apply.
            while (progress < 1)
            {
                for (int i = 0; i < graphic.Length; i++)
                {
                    graphic[i].color = Color.Lerp(graphic[i].color, arrive, progress);
                }
                progress += increment;
                action?.Invoke();
                yield return Timing.WaitForSeconds(smoothness);
            }
            end_Action?.Invoke();
        }

        /// <summary>
        /// CanvasAlpha 알파값 변경
        /// </summary>
        public IEnumerator<float> Cor_CanvasAlpha(CanvasGroup cvg, bool enable, float speed = 0.025f, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();
            if (enable)
            {
                while (cvg.alpha < 1)
                {
                    cvg.alpha += speed;
                    yield return Timing.WaitForSeconds(0);
                }
                cvg.alpha = 1;
            }
            else
            {
                while (cvg.alpha > 0)
                {
                    cvg.alpha -= speed;
                    yield return Timing.WaitForSeconds(0);
                }
                cvg.alpha = 0;
            }
            end_Action?.Invoke();
        }

        /// <summary>
        /// CanvasAlpha 알파값 변경
        /// </summary>
        public IEnumerator<float> Cor_CanvasAlpha(CanvasGroup cvg, float _f, float speed = 0.025f, UnityAction start_Action = null, UnityAction end_Action = null)
        {
            start_Action?.Invoke();
            if(cvg.alpha < _f)
            {
                while (cvg.alpha < _f)
                {
                    cvg.alpha += speed;
                    yield return Timing.WaitForSeconds(0);
                }
                cvg.alpha = _f;
            }
            else
            {
                while (cvg.alpha > _f)
                {
                    cvg.alpha -= speed;
                    yield return Timing.WaitForSeconds(0);
                }
                cvg.alpha = _f;
            }
            end_Action?.Invoke();
        }

        [Header("Error Load Texture")]
        [SerializeField] private Texture tex_Error_Loading;
        /// <summary>
        /// 텍스쳐 로드
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IEnumerator<float> Cor_Texture(string _url, UnityAction<Texture> action = null)
        {
            if (!_url.StartsWith("http"))
                _url = $"file://{_url}";

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.result != UnityWebRequest.Result.Success)
            {
                action?.Invoke(tex_Error_Loading);
            }
            else
            {
                Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                action?.Invoke(tex);
            }
        }

        /// <summary>
        /// 오류텍스쳐인지 확인
        /// </summary>
        /// <param name="_raw"></param>
        /// <returns></returns>
        public bool IsErrorTexture(RawImage _raw)
        {
            return _raw.texture.Equals(tex_Error_Loading);
        }
    }
}