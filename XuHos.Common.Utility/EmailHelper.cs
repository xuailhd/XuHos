using System;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace XuHos.Common.Utility
{
    public class EmailHelper : IDisposable
    {

        #region 变量
        /// <summary>
        /// 发送人地址
        /// </summary>
        private string SendUser;
        /// <summary>
        /// 显示名称
        /// </summary>
        private string DisplayName;

        private SmtpClient smtpMail;

        private string  smtp;
        private int smtpport;
        #endregion

        #region 构造函数
        public EmailHelper(string sMTPAddress, int sMTPPort, string sendUser, string displayName, string sendPwd, bool isUseSSL)
        {
            this.SendUser = sendUser;
            this.DisplayName = displayName;


            this.smtp = sMTPAddress;
            this.smtpport = sMTPPort;

            smtpMail = new SmtpClient(sMTPAddress, sMTPPort);
            smtpMail.Credentials = new NetworkCredential(sendUser, sendPwd);
            smtpMail.EnableSsl = isUseSSL;
        }
        #endregion


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailFrom">发送人</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="mailTos">接收人列表</param>
        /// <param name="mailCcs">抄送人列表</param>
        /// <param name="mailBccs">密送人列表</param>
        /// <param name="mailSubject">主题</param>
        /// <param name="mailBody">内容</param>
        /// <param name="attachments">附件</param>
        /// <param name="priority">优先级</param>
        /// <param name="isBodyHtml">是否是HTML</param>
        /// <param name="bodyEncoding">编码</param>
        /// <returns></returns>
        public bool SendMail(string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            bool mailSent = false;

            #region 设置属性值

            if (string.IsNullOrEmpty(mailFrom))
                mailFrom = this.SendUser;

            if (string.IsNullOrEmpty(displayName))
                displayName = this.DisplayName;

            MailMessage Email = GetMailMessage(mailFrom, displayName, mailTos, mailCcs, mailBccs, mailSubject, mailBody, attachments, priority, isBodyHtml, bodyEncoding);


            #endregion

            try
            {
                //发送邮件
                smtpMail.Send(Email);
                mailSent = true;
            }
            catch(System.Exception ex)
            {
                mailSent = false;
            }

            return mailSent;
        }

        /// <summary>
        /// 获得Email信息
        /// </summary>
        /// <param name="mailFrom">发送人</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="mailTos">收件人列表</param>
        /// <param name="mailCcs">抄送人列表</param>
        /// <param name="mailBccs">密送人列表</param>
        /// <param name="mailSubject">主题</param>
        /// <param name="mailBody">内容</param>
        /// <param name="attachments">附件列表</param>
        /// <param name="priority">优先级</param>
        /// <param name="isBodyHtml">是否是HTML</param>
        /// <param name="bodyEncoding">编码</param>
        /// <returns></returns>
        private MailMessage GetMailMessage(string mailFrom, string displayName, string[] mailTos, string[] mailCcs, string[] mailBccs, string mailSubject, string mailBody, string[] attachments, MailPriority priority, bool isBodyHtml, System.Text.Encoding bodyEncoding)
        {
            // 1.创建邮件对像
            MailMessage emailMessage = new MailMessage();

            // 2.创建发件人邮件地址
            if (string.IsNullOrEmpty(mailFrom))
                mailFrom = this.SendUser;

            if (string.IsNullOrEmpty(displayName))
                displayName = this.DisplayName;

            MailAddress mailFromObject = new MailAddress(mailFrom, displayName);
            emailMessage.From = mailFromObject;

            // 3.循环加入收件人地址
            if (mailTos != null)
            {
                foreach (string mailto in mailTos)
                {
                    if (!string.IsNullOrEmpty(mailto))
                    {
                        emailMessage.To.Add(mailto);
                    }
                }
            }

            // 4.循环加入抄送地址
            if (mailCcs != null)
            {
                foreach (string cc in mailCcs)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        emailMessage.CC.Add(cc);
                    }
                }
            }

            // 5.循环加入密送地址
            if (mailBccs != null)
            {
                foreach (string bcc in mailBccs)
                {
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        emailMessage.Bcc.Add(bcc);
                    }
                }
            }

            // 6.循环加入附件信息
            if (attachments != null)
            {
                foreach (string file in attachments)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        Attachment att = new Attachment(file);
                        emailMessage.Attachments.Add(att);
                    }
                }
            }

            // 7.主题
            emailMessage.Subject = mailSubject;
            // 8.内容
            emailMessage.Body = mailBody;
            // 9.优先级
            emailMessage.Priority = priority;
            // 10.是否为HTML格式
            emailMessage.IsBodyHtml = isBodyHtml;
            // 11.编码
            emailMessage.BodyEncoding = bodyEncoding;

            return emailMessage;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                smtpMail.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
