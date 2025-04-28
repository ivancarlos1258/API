using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace API.Utility
{
    public class EmailAnexo
    {
        public string Nome { get; set; }

        public Stream Arquivo { get; set; }
    }

    public class ConfiguracaoSmtp
    {
        public string Servidor { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool Ssl { get; set; }
        public int Porta { get; set; }
        public string Remetente { get; set; }
    }

    public static class Email
    {
        public static void EnviarEmail(string destinatario, string assunto, string corpo, bool html = false, List<EmailAnexo> anexos = null)
        {
            var recipientes = new List<string>
            {
                destinatario
            };
            EnviarEmail(recipientes, assunto, corpo, html, anexos);
        }

        public static void EnviarEmail(
            List<string> destinatarios, 
            string assunto, 
            string corpo, 
            bool html = false, 
            List<EmailAnexo> anexos = null)
        {
            var config = ObterConfiguracao();

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Remetente", config.Remetente));
            foreach (var item in destinatarios) 
            {
                message.To.Add(new MailboxAddress("Destinatário", item));
            }

            message.Subject = assunto;

            if (!html)
            {
                message.Body = new TextPart("plain")
                {
                    Text = corpo
                };
            }
            else 
            {
                var multipart = new Multipart("mixed");

                var textPart = new TextPart("html")
                {
                    Text = corpo
                };

                multipart.Add(textPart);

                if (anexos != null)
                {
                    var att = new AttachmentCollection();
                    foreach (var anexo in anexos)
                    {
                        var attachment = new MimePart
                        {
                            Content = new MimeContent(anexo.Arquivo, ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = anexo.Nome
                        };

                        multipart.Add(attachment);
                    }


                }
                message.Body = multipart;
            }

            using var client = new SmtpClient();
            client.Connect(config.Servidor, config.Porta, config.Ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            client.Authenticate(config.Usuario, config.Senha);

            client.Send(message);
            client.Disconnect(true);
        }

        private static ConfiguracaoSmtp ObterConfiguracao()
        {
            try
            {
                var settings = new Configuracao();

                return new ConfiguracaoSmtp
                {
                    Servidor = settings.AppSettings["ConfiguracaoEmail:Servidor"],
                    Usuario = settings.AppSettings["ConfiguracaoEmail:Usuario"],
                    Senha = settings.AppSettings["ConfiguracaoEmail:Senha"],
                    Ssl = bool.Parse(settings.AppSettings["ConfiguracaoEmail:SSL"]),
                    Porta = int.Parse(settings.AppSettings["ConfiguracaoEmail:Porta"]),
                    Remetente = settings.AppSettings["ConfiguracaoEmail:Remetente"]
                };
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("Não foi possível carregar as configurações para conexão no e-mail: " + ex.Message);
            }
        }
    }
}
