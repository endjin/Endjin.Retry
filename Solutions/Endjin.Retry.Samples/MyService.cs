namespace Endjin.Retry.Samples
{
    #region Using statements

    using System;
    using System.Threading.Tasks;

    #endregion

    // An example of a simple service offering both synchronous and
    // aysnchronous implementations of its methods
    public class MyService : ISomeService
    {
        private int errorCount;

        public string FirstTask()
        {
            if (this.errorCount < 3)
            {
                this.errorCount += 1;
                throw new Exception("Not this time, matey.");
            }
            return "world";
        }

        public string SecondTask(string parameter)
        {
            if (this.errorCount < 6)
            {
                this.errorCount += 1;
                throw new Exception("Nice try.");
            }
            return "Hello " + parameter;
        }

        public Task<string> FirstTaskAsync()
        {
            return Task<string>.Factory.StartNew(this.FirstTask);
        }

        public Task<string> SecondTaskAsync(string parameter)
        {
            return Task<string>.Factory.StartNew(()=>this.SecondTask(parameter));
        }
    }
}