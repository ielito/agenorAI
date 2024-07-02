using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.BedrockRuntime.Model;
using OutSystems.ExternalLibraries.SDK;

namespace nabucoAi
{
    [OSInterface]
    public interface IMyLibrary
    {
        Task<string> GetResponseAsync(List<Message> chatHistory);
    }
}
