using Blazor.MVVM.ExampleComponents.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.MVVM.ExampleComponents.Components.Modal
{

    /// <summary>
    /// Modal host - a single instance of this component is rendered in the main layout. A child component / render template
    /// is shown inside the modal as the modal content on demand. 
    /// </summary>
    public sealed partial class ModalHost
    {
        private static Lazy<Task<IJSObjectReference>> _moduleTask;
        
        /// <summary>
        /// Gets or sets the modal host content render fragment.
        /// </summary>
        [Parameter] public RenderFragment? Content { get; set; }

        /// <summary>
        /// Gets or sets the JS runtime object.
        /// </summary>
        [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// The hosted modal component instance.
        /// </summary>
        private ModalComponentBase? _modalComponent = null!;

        /// <summary>
        /// Show the modal - modal host is made visible and an instance of component <typeparam name="T"></typeparam> is
        /// rendered in the modal host. This method when awaited will return once the modal has been closed. <see cref="ModalResult"/> return
        /// value is retrieved from ModalComponent and returned once finished.
        /// </summary>
        /// <param name="parameterView"> parameters to supply to new instance of component T</param>
        /// <typeparam name="T"> type of modal component to be rendered in the host</typeparam>
        /// <returns> modal result.</returns>
        public async Task<ModalResult> ShowAsync<T>(ParameterView? parameterView = null)
            where T : ModalComponentBase
        {
            
            Content = builder =>
            {
                int seq = 0;
                builder.OpenComponent<T>(seq);

                if (parameterView is not null)
                {
                    foreach (ParameterValue parameterValue in parameterView)
                    {
                        builder.AddAttribute(seq++, parameterValue.Name, parameterValue.Value);
                    }
                }

                builder.AddComponentReferenceCapture(seq++, o => _modalComponent = (ModalComponentBase) o);
                builder.CloseComponent();
            };

            StateHasChanged();

            await AwaitModalComponentRender();
            
            await ShowModalAsync();
            ModalResult result = await _modalComponent!.ShowAsync();
            await HideModalAsync();

            _modalComponent = null;
            Content = null;
            
            StateHasChanged();
            
            return result;
        }

        /// <summary>
        /// Delays until the modal content component has been rendered and the component reference is no longer null.
        /// </summary>
        /// <returns> a task. modal content is rendered and ref available once complete</returns>
        private async Task AwaitModalComponentRender()
        {
            int attempts = 100;
            while (_modalComponent is null && attempts >= 0)
            {
                attempts--;
                await Task.Delay(5);
            }

            if (_modalComponent is null)
            {
                throw new InvalidOperationException("Modal content did not render in time.");
            }
            else
            {
                await _modalComponent.ModalRendered;
            }
        }

        protected override async Task OnInitializedAsync()
        {
          _moduleTask = new Lazy<Task<IJSObjectReference>>(JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.MVVM.ExampleComponents/modal.js").AsTask());
          
          ModalService.Initialize(this);
        }

        
        internal static async Task ShowModalAsync()
        {
            if (_moduleTask is null)
                throw new InvalidOperationException("ModalHost is not initialized.");

            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("ShowModal");
        }

        internal static async Task HideModalAsync()
        {
            if (_moduleTask is null)
                throw new InvalidOperationException("ModalHost is not initialized.");
            
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("HideModal");
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}