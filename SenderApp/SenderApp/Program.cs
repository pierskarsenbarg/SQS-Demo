namespace SenderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueHelper = new QueueSenderHelper();
            queueHelper.DoWork();
        }
    }
}
