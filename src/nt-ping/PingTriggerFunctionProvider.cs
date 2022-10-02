using Microsoft.Azure.WebJobs.Script.Description;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using System.Reflection;

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

        private const string RenderPingDocumentKey = nameof(PingTriggerFunctions);

        public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors { get; } = new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

        public Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, HttpBindingMetadata> SetupOpenApiHttpBindings()
        {
            var bindings = new Dictionary<string, HttpBindingMetadata>();

            var renderPingDocument = new HttpBindingMetadata()
            {
                Methods = new List<string>() { HttpMethods.Get },
                Route = "ping/{version}.{extension}",
                AuthLevel = this._settings.AuthLevel?.Document ?? AuthorizationLevel.Anonymous,
            };

            bindings.Add(RenderPingDocumentKey, renderPingDocument);

            return bindings;

        }

        private List<FunctionMetadata> GetFunctionMetadataList()
        {
            var list = new List<FunctionMetadata>();
            list.AddRange(new[]
            {
                this.GetFunctionMetadata(RenderPingDocumentKey)
            });

            return list;
        }

        private FunctionMetadata GetFunctionMetadata(string functionName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var functionMetadata = new FunctionMetadata()
            {
                Name = functionName,
                FunctionDirectory = null,
                ScriptFile = $"assembly:{assembly.FullName}",
                EntryPoint = $"{assembly.GetName().Name}.{typeof(PingTriggerFunctions).Name}.{functionName}",
                Language = "DotNetAssembly"
            };
            return functionMetadata;
        }
    }
}
