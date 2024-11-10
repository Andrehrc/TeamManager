namespace ProjectManagementApi.Services.Interfaces
{
    public interface IEncryptDecryptService
    {
        string Encrypt(string simpletext);
        string Decrypt(string entryText);
    }
}
