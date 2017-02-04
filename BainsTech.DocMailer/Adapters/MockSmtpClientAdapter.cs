using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BainsTech.DocMailer.Adapters
{
    internal class MockSmtpClientAdapter : ISmtpClientAdapter
    {
        private static readonly object MyLock = new object();
        private static readonly object MyLock2 = new object();
        //private MailerTestData testData;
        private static volatile XmlSerializer xmlSerializer;

        private static XmlSerializer XmlSerializer
        {
            get
            {
                // only create a new instance if one doesn't already exist.
                if (xmlSerializer == null)
                {
                    // use this lock to ensure that only one thread is access
                    // this block of code at once.
                    lock (MyLock)
                    {
                        if (xmlSerializer == null)
                        {
                            var factory = new XmlSerializerFactory();
                            xmlSerializer = factory.CreateSerializer(typeof(MailerTestData));
                        }
                    }
                }
                // return instance where it was just created or already existed.
                return xmlSerializer;
            }
        }
        
        public MockSmtpClientAdapter()
        {
            Read();
        }
        public void Dispose()
        {
            
        }

        public bool EnableSssl { get; set; }
        public void SetCredentials(string senderEmailAddress, string senderEmailAccountPasswordEncrypted)
        {
            
        }

        public void Send(IMailMessageAdapter mailMessage)
        {
            lock (MyLock2)
            {
                var testData = Read();

                if (mailMessage.Subject.Contains("PAYE"))
                {
                    foreach (var attachment in mailMessage.MailMessage.Attachments)
                    {
                        var attachedFile = attachment.Name.ToLower();
                        var doc = testData.PayeDocuments
                            .FirstOrDefault(d => d.FileName.Contains(attachedFile));

                        if (doc == null)
                            throw new Exception($"Attached file not found:{attachedFile}");


                        var sentTo = mailMessage.MailMessage.To.First().Address.ToLower();
                        if (doc.SendTo != sentTo)
                        {
                            throw new Exception($"SendTo mismatch {doc.SendTo} != {sentTo}");
                        }

                        if (doc.ExpectedSubject != mailMessage.Subject)
                        {
                            throw new Exception($"Subject mismatch {doc.ExpectedSubject} != {mailMessage.Subject}");
                        }

                        Task.Delay(500).Wait();
                    }
                }
                else
                {
                    foreach (var attachment in mailMessage.MailMessage.Attachments)
                    {
                        var attachedFile = attachment.Name.ToLower();
                        var doc = testData.PayrollDocuments
                            .FirstOrDefault(d => d.FileName.Contains(attachedFile));

                        if(doc == null)
                            throw new Exception($"Attached file not found:{attachedFile}");

                        var sentTo = mailMessage.MailMessage.To.First().Address.ToLower();
                        if (doc.SendTo != sentTo)
                        {
                            throw new Exception($"SendTo mismatch {doc.SendTo} != {sentTo}");
                        }

                        if (doc.ExpectedSubject != mailMessage.Subject)
                        {
                            throw new Exception($"Subject mismatch {doc.ExpectedSubject} != {mailMessage.Subject}");
                        }

                        Task.Delay(500).Wait();
                    }
                }
            }
        }

        private MailerTestData Read()
        {
            //lock (MyLock2)
            {
                using (var myReader = new  StreamReader(@"DocMailerTestData.xml", false))
                {
                    var testData = (MailerTestData)XmlSerializer.Deserialize(myReader);
                    return testData;
                    //myReader.Close();
                    //return testData;
                }
            }
        }

        private void Write(MailerTestData testData)
        {
            //lock (MyLock2)
            {
               
                using (var myWriter = TextWriter.Synchronized(new StreamWriter(@"DocMailerTestData.xml", false)))
                {
                    XmlSerializer.Serialize(myWriter, testData);
                    //myWriter.Close();
                }
                //Task.Delay(2000).Wait();
            }

        }
        
    }
}