using Sitecore.WFFM.Abstractions.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Sitecore.Support.Forms.Core.Pipelines
{
  public class ProcessMessage : Sitecore.Forms.Core.Pipelines.ProcessMessage
  {
    public new void SendEmail(ProcessMessageArgs args)
    {
      SmtpClient client = new SmtpClient(args.Host)
      {
        EnableSsl = args.EnableSsl
      };
      if (args.Port != 0)
      {
        client.Port = args.Port;
      }
      client.Credentials = args.Credentials;
      client.Send(this.GetMail(args));
    }
    private MailMessage GetMail(ProcessMessageArgs args)
    {
      MailMessage mail;

      try {
        mail = new MailMessage(args.From.Replace(";", ","), args.To.Replace(";", ",").ToString())
        {
          IsBodyHtml = args.IsBodyHtml
        };
      }
      catch(Exception e)
      {
        throw new Exception("The email message was not sent. Invalid email address(s) or format specified.", e);
      }
      mail.Subject = args.Subject.ToString();
      mail.Body = args.Mail.ToString();

      if (args.CC.Length > 0)
      {
        char[] separator = new char[] { ',' };
        foreach (string str in args.CC.Replace(";", ",").ToString().Split(separator))
        {
          mail.CC.Add(new MailAddress(str));
        }
      }
      if (args.BCC.Length > 0)
      {
        char[] chArray2 = new char[] { ',' };
        foreach (string str2 in args.BCC.Replace(";", ",").ToString().Split(chArray2))
        {
          mail.Bcc.Add(new MailAddress(str2));
        }
      }
      args.Attachments.ForEach(delegate (Attachment attachment) {
        mail.Attachments.Add(attachment);
      });
      return mail;
    }

  }
}