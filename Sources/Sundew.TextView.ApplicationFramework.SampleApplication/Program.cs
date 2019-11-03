namespace Sundew.TextView.ApplicationFramework.SampleApplication
{
    using System.Threading.Tasks;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;

    class Program
    {
        static async Task Main(string[] args)
        {
            var application = new Application();
            var textViewNavigator = application.StartRendering(new ConsoleDisplayDevice());
            await textViewNavigator.NavigateToModalAsync(new MainTextView());
            await Task.Delay(500);
            await textViewNavigator.NavigateToModalAsync(new View2());
            await Task.Delay(500);
            await textViewNavigator.NavigateBackAsync();
            application.Run();
        }
    }
}
