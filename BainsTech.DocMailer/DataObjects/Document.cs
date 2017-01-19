using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Properties;

namespace BainsTech.DocMailer.DataObjects
{
    public class Document : INotifyPropertyChanged
    {
        private string emailAddress;
        private string status;
        private bool isReadyToSend;

        public string FilePath { get; set; }

        public string FileName { get; set; }

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

        public bool IsReadyToSend
        {
            get { return isReadyToSend; }
            set
            {
                if (isReadyToSend == value) return;
                isReadyToSend = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                status = value;
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