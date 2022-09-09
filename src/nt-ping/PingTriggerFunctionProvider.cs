using Microsoft.Azure.WebJobs.Script.Description;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhnToast.Ping
{
    public class PingTriggerFunctionProvider : IFunctionProvider
    {
        public ImmutableDictionary<string, ImmutableArray<string>> FunctionErrors { get; } = new Dictionary<string, ImmutableArray<string>>().ToImmutableDictionary();

        public Task<ImmutableArray<FunctionMetadata>> GetFunctionMetadataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
