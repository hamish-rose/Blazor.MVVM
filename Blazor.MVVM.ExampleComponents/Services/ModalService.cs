using Blazor.MVVM.ExampleComponents.Components.Modal;
using Microsoft.AspNetCore.Components;

namespace Blazor.MVVM.ExampleComponents.Services;

 public static class ModalService
    {
        private static ModalHost? ModalInstance { get; set; }

        /// <summary>
        /// Initialize the modal service passing a reference to the modal host component. Must be completed
        /// before this object may be used.
        /// </summary>
        /// <param name="component"> modal host</param>
        public static void Initialize(ModalHost component)
        {
            ModalInstance = component ?? throw new ArgumentNullException(nameof(component));
        }

        /// <summary>
        /// Show the modal component of type,
        /// <typeparam name="T"> modal component to show. must implement modal component</typeparam>
        /// </summary>
        /// <param name="parameters"> parameters to supply to the modal component.</param>
        /// <returns> modal result</returns>
        public static async Task<ModalResult> ShowAsync<T>(Dictionary<string, object>? parameters = null)
            where T : ModalComponentBase
        {
            if (ModalInstance is null)
            {
                throw new InvalidOperationException("Modal service is not initialized, no modal component");
            }
            
            return await ModalInstance.ShowAsync<T>(parameters is null
                ? ParameterView.Empty
                : ParameterView.FromDictionary(parameters));
        }
    }