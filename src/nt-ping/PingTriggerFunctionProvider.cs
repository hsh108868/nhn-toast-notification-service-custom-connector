using Microsoft.Azure.WebJobs.Script.Description;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;


namespace NhnToast.Ping
{

    public class PingTriggerFunctionProvider : IFunctionProvider
    {
        private readonly OpenApiSettings _settings;
        private readonly Dictionary<string, HttpBindingMetadata> _bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingTriggerFunctionProvider"/> class.
        /// </summary>
        public PingTriggerFunctionProvider(OpenApiSettings settings)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._bindings = this.SetupOpenApiHttpBindings();
        }

        private const string RenderFunctionDocumentKey = nameof(PingTriggerFunctions);

        public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors { get; } = new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

        public Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, HttpBindingMetadata> SetupOpenApiHttpBindings()
        {
            var bindings = new Dictionary<string, HttpBindingMetadata>();

            if (this._settings.HideDocument)
            {
                return bindings;
            }

            var renderFunctionDocument = new HttpBindingMetadata()
            {
                Methods = new List<string>() { HttpMethods.Get },
                Route = "nt-sms/{version}.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            bindings.Add(RenderFunctionDocumentKey, renderFunctionDocument);

            return bindings;

        }

    }
}
