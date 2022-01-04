using MEC;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace NORK
{
    public class Manager_Email : MonoBehaviour
    {
        private const string my_Email = "nork.selfttodo@gmail.com";
        private const string my_Pwd = "sft0831&hs";

        public enum Type_Mail { ������ȣ }
        public struct Data_Mail
        {
            public Type_Mail type;
            public Manager_Common.Data_Dictionary main;

            public Data_Mail(Type_Mail _type, Manager_Common.Data_Dictionary _main)
            {
                type = _type;
                main = _main;
            }
        }

        public List<Data_Mail> mails = new List<Data_Mail>()
        {
            new Data_Mail(Type_Mail.������ȣ, new Manager_Common.Data_Dictionary("[SelfToDo]������ȣ", "{0}(��)�� SelftToDo �����ڵ��Դϴ�."))
        };

        public Data_Mail Get_Data_Mail(Type_Mail _type, params object[] _data)
        {
            Data_Mail _dm = mails.Find(n => n.type.Equals(_type));

            for (int i = 0; i < _data.Length; i++)
            {
                _dm.main.value = _dm.main.value.Replace("{" + i + "}", (string)_data[i]);
            }

            return _dm;
        }

        public void Send_Mail(string to, Data_Mail data_mail)
        {
            Timing.RunCoroutine(Cor_Send_Mail(to, data_mail));
        }

        IEnumerator<float> Cor_Send_Mail(string to, Data_Mail data_mail)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(my_Email); // �����»��
            mail.To.Add(to); // �޴� ���
            mail.Subject = data_mail.main.key;
            mail.Body = data_mail.main.value;

            // ÷������ - ��뷮�� �ȵ�.
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment("D:\\Test\\2018-06-11-09-03-17-E7104.mp4"); // ��� �� ���� ����
            //mail.Attachments.Add(attachment);

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(my_Email, my_Pwd) as ICredentialsByHost; // �����»�� �ּ� �� ��й�ȣ Ȯ��
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
            smtpServer.Send(mail);
            Debug.Log("success");
            yield return Timing.WaitForSeconds(0);
        }
    }
}