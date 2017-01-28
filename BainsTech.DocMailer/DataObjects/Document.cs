using System.ComponentModel;
using System.Runtime.CompilerServices;
using BainsTech.DocMailer.Infrastructure;
using BainsTech.DocMailer.Properties;

namespace BainsTech.DocMailer.DataObjects
{
    public enum DocumentStatus
    {
        ReadyToSend,
        IncompatibleFileName,
        NoMappedEmail,
        Sending,
        Sent,
        SendFailed,
        SentDocMoveFailed
    }

    public class Document : INotifyPropertyChanged
    {
        private string emailAddress;
        private string statusDesc;
        
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

        public bool IsReadyToSend => Status == DocumentStatus.ReadyToSend || Status == DocumentStatus.SendFailed;

        private DocumentStatus status;
        public DocumentStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
                StatusDesc = status.ToDisplayString();
            }
        }
        
        public string StatusDesc
        {
            get { return statusDesc; }
            set
            {
                if (statusDesc == value) return;
                statusDesc = value;
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