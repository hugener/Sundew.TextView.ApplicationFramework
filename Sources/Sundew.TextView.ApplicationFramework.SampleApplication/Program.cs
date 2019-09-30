using System.Threading;

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
            await textViewNavigator.NavigateToAsync(new MainTextView());
            application.Run();
        }
    }
}
