using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using System;
using UnityEngine.Events;

namespace NORK
{
    public class Manager_Gallery : MonoBehaviour
    {
        private const int MAXSIZE = 512;
        public int Get_MaxSize
        {
            get { return MAXSIZE; }
        }

        public void PickPicture(UnityAction<string[]> action)
        {
            if (NativeGallery.IsMediaPickerBusy())
            {
                Debug.Log("갤러리를 사용중입니다.");
                return;
            }
            if (NativeGallery.CanSelectMultipleFilesFromGallery())
            {
                NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((paths) =>
                {
                    if (paths != null)
                    {

                        action?.Invoke(paths);
                    }
                });
                Debug.Log("Permission result : " + permission);
            }
            else
            {
                Debug.Log("다중선택을 지원하지 않는 기기입니다.");
                NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
                {
                    if (path != null && File.Exists(path))
                    {
                        action?.Invoke(new string[1] { path });
                    }
                });
                Debug.Log("Permission result : " + permission);
            }
        }

        public void TakePicture(UnityAction<string> action)
        {
            if (!NativeCamera.DeviceHasCamera())
            {
                Debug.Log("카메라 지원하지 않습니다.");
                return;
            }
            if (NativeCamera.IsCameraBusy())
            {
                Debug.Log("카메라를 사용중입니다.");
                return;
            }
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                action?.Invoke(path);

            });
        }
    }

}
