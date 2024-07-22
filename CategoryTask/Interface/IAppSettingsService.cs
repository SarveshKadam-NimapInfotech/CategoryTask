namespace CategoryTask.Interface
{
    public interface IAppSettingsService
    {
        Task<bool> GetUseApiFlagAsync();
    }
}
