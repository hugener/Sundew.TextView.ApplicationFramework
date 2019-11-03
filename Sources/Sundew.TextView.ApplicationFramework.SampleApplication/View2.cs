using System.Collections.Generic;
using System.Threading.Tasks;
using Sundew.TextView.ApplicationFramework.TextViewRendering;

namespace Sundew.TextView.ApplicationFramework.SampleApplication
{
    public class View2 : ITextView
    {
        public IEnumerable<object> InputTargets
        {
            get { yield return this; }
        }

        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            return Task.CompletedTask;
        }

        public void OnDraw(IRenderContext renderContext)
        {
            renderContext.Home();
            renderContext.WriteLine("View2");
        }

        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }
    }
}