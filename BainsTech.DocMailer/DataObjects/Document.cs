using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Properties;

namespace BainsTech.DocMailer.DataObjects
{
    public class Document : INotifyPropertyChanged
    {
        private string emailAddress;
        private string sendResult;
        private bool sent;

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public Document()
        {
            SendResult = "Pending";
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                if (emailAddress == value) return;
                emailAddress = value;
                OnPropertyChanged();
            }
        }

        public bool Sent
        {
            get { return sent; }
            set
            {
                if (sent == value) return;
                sent = value;
                OnPropertyChanged();
            }
        }

        public string SendResult
        {
            get { return sendResult; }
            set
            {
                if (sendResult == value) return;
                sendResult = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}