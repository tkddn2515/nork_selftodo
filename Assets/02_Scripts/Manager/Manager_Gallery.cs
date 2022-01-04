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
                Debug.Log("�������� ������Դϴ�.");
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
                Debug.Log("���߼����� �������� �ʴ� ����Դϴ�.");
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
                Debug.Log("ī�޶� �������� �ʽ��ϴ�.");
                return;
            }
            if (NativeCamera.IsCameraBusy())
            {
                Debug.Log("ī�޶� ������Դϴ�.");
                return;
            }
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                action?.Invoke(path);

            });
        }
    }

}
