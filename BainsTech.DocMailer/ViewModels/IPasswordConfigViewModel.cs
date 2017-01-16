namespace BainsTech.DocMailer.ViewModels
{
    internal interface IPasswordConfigViewModel
    {
        bool IsEmailPasswordNeeded { get; }
        void SetSenderEmailAccountPassword(string encryptedPassword);
    }
}