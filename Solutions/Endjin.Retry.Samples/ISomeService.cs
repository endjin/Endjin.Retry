namespace Endjin.Retry.Samples
{
    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface ISomeService
    {
        Task<string> FirstTaskAsync();

        Task<string> SecondTaskAsync(string parameter);

        string FirstTask();

        string SecondTask(string parameter);
    }
}