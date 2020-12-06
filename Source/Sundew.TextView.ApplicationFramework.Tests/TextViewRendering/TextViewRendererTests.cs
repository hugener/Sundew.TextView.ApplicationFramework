using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sundew.Base.Computation;
using Sundew.Base.Threading;
using Sundew.TextView.ApplicationFramework.DeviceInterface;
using Sundew.TextView.ApplicationFramework.SampleApplication;
using Sundew.TextView.ApplicationFramework.TextViewRendering;
using Xunit;
using Xunit.Abstractions;

namespace Sundew.TextView.ApplicationFramework.Tests.TextViewRendering
{
    public class TextViewRendererTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TextViewRendererTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Dispose_Then_LastReporterCallShouldBeStopped()
        {
            var reporter = new Reporter();
            var testee = new TextViewRenderer(new RenderingContextFactory(new TestOutputDisplayDevice(this.testOutputHelper)), new TimerFactory(), TimeSpan.FromMilliseconds(200), textViewRendererReporter: reporter);
            testee.Start();
            await testee.TrySetViewAsync(new MainTextView(), null).ConfigureAwait(false);
            await Task.Delay(200).ConfigureAwait(false);

            testee.Dispose();

            reporter.Calls.Last().Should().Be("Stopped");
        }

        private class TestOutputDisplayDevice : ITextDisplayDevice
        {
            private readonly ITestOutputHelper testOutputHelper;

            public TestOutputDisplayDevice(ITestOutputHelper testOutputHelper)
            {
                this.testOutputHelper = testOutputHelper;
            }

            public bool CursorEnabled { get; set; }

            public bool CursorBlinking { get; set; }

            public Size Size { get; } = new Size(80, 40);

            public Point CursorPosition { get; } = new Point(0, 0);

            public Result.IfSuccess<ICharacterContext> TryCreateCharacterContext()
            {
                return Result.Error();
            }

            public void WriteLine(object text)
            {
                this.testOutputHelper.WriteLine(text.ToString());
            }

            public void Write(object text)
            {
                this.testOutputHelper.WriteLine(text.ToString());
            }

            public void WriteFormat(string format, params object[] values)
            {
                this.testOutputHelper.WriteLine(format, values);
            }

            public void WriteLineFormat(string format, params object[] values)
            {
                this.testOutputHelper.WriteLine(format, values);
            }

            public void Clear()
            {
            }

            public void SetPosition(int x, int y)
            {
            }
        }

        private class Reporter : ITextViewRendererReporter
        {
            public List<string> Calls { get; } = new List<string>();

            public void SetSource(Type target, object source)
            {
                this.Calls.Add(nameof(SetSource));
            }

            public void Started()
            {
                this.Calls.Add(nameof(Started));
            }

            public void OnViewChanged(ITextView newTextView, ITextView oldTextView)
            {
                this.Calls.Add(nameof(OnViewChanged));
            }

            public void OnDraw(ITextView currentTextView)
            {
                this.Calls.Add(nameof(OnDraw));
            }

            public void OnRendered(ITextView currentTextView, IRenderingContext renderingContext)
            {
                this.Calls.Add(nameof(OnRendered));
            }

            public void OnRendererException(Exception exception)
            {
                this.Calls.Add(nameof(OnRendererException));
            }

            public void WaitingForAccessToChangeViewTo(ITextView newTextView)
            {
                this.Calls.Add(nameof(WaitingForAccessToChangeViewTo));
            }

            public void WaitingForRenderingAborted(ITextView view)
            {
                this.Calls.Add(nameof(WaitingForRenderingAborted));
            }

            public void WaitingForAccessToRenderView()
            {
                this.Calls.Add(nameof(WaitingForAccessToRenderView));
            }

            public void AcquiredViewForRendering(ITextView view)
            {
                this.Calls.Add(nameof(AcquiredViewForRendering));
            }

            public void AbortingRendering(ITextView view)
            {
                this.Calls.Add(nameof(AbortingRendering));
            }

            public void WaitingForViewToInvalidate(ITextView view)
            {
                this.Calls.Add(nameof(WaitingForViewToInvalidate));
            }

            public void ViewAlreadySet(ITextView newTextView)
            {
                this.Calls.Add(nameof(ViewAlreadySet));
            }

            public void Stopping()
            {
                this.Calls.Add(nameof(Stopping));
            }

            public void Stopped()
            {
                this.Calls.Add(nameof(Stopped));
            }
        }
    }
}