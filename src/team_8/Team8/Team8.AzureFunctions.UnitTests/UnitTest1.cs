namespace Team8.AzureFunctions.UnitTests
{
    public class Tests
    {

        [Test]
        public void Test1()
        {
            EmailDispatcher dispatcher = new EmailDispatcher();
            dispatcher.Send("Neil");
        }
    }
}