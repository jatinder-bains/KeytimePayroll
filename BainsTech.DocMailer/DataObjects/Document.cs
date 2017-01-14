using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Annotations;

namespace BainsTech.DocMailer.DataObjects
{
    public class Document : INotifyPropertyChanged
    {
        private string emailAddress;
        private string fileName;
        private string sendResult;
        private bool sent;

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName == value) return;
                fileName = value;
                OnPropertyChanged();
            }
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