namespace Sundew.TextView.ApplicationFramework.SampleApplication
{
    using System.Threading.Tasks;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;

    public static class Program
    {
        public static async Task Main()
        {
            var application = new Application();
            application.RenderException += Application_RenderException;
            var textViewNavigator = application.StartRendering(new ConsoleDisplayDevice());
            await textViewNavigator.NavigateToModalAsync(new MainTextView()).ConfigureAwait(false);
            await Task.Delay(500).ConfigureAwait(false);
            await textViewNavigator.NavigateToModalAsync(new View2()).ConfigureAwait(false);
            await Task.Delay(500).ConfigureAwait(false);
            await textViewNavigator.NavigateBackAsync().ConfigureAwait(false);
            application.Run();
        }

        private static void Application_RenderException(object? sender, TextViewRendering.RenderExceptionEventArgs e)
        {
            e.Continue = true;
        }
    }
}
