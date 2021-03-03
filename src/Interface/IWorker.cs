namespace LogElastic.Interface
{
    interface IWorker
    {
        string ProcessId { get; set; }

        bool RunProcess();
    }
}