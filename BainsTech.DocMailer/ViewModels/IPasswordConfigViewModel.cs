namespace BainsTech.DocMailer.ViewModels
{
    internal interface IPasswordConfigViewModel
    {
        bool IsEmailPasswordNeeded { get; set; }
        void SetSenderEmailAccountPassword(string encryptedPassword);
    }
}