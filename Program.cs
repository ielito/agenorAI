using System;
using System.Collections.Generic;
using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using System.Threading.Tasks;

namespace nabucoAi
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            MyLibrary myLibrary = new MyLibrary();
            List<Message> chatHistory = new List<Message>();
            string userPrompt;

            do
            {
                Console.WriteLine("Enter your prompt (or type 'SAIR' to exit):");
                userPrompt = Console.ReadLine();

                if (userPrompt.ToUpper() != "SAIR")
                {
                    chatHistory.Add(new Message
                    {
                        Role = ConversationRole.User,
                        Content = new List<ContentBlock> { new ContentBlock { Text = userPrompt } }
                    });

                    string response = await myLibrary.GetResponseAsync(chatHistory);
                    Console.WriteLine(response);

                    chatHistory.Add(new Message
                    {
                        Role = ConversationRole.Assistant,
                        Content = new List<ContentBlock> { new ContentBlock { Text = response } }
                    });
                }

            } while (userPrompt.ToUpper() != "SAIR");

            Console.WriteLine("Chatbot session ended.");
        }
    }

    public class MyLibrary : IMyLibrary
    {
        public async Task<string> GetResponseAsync(List<Message> chatHistory)
        {
            var client = new AmazonBedrockRuntimeClient(RegionEndpoint.USEast1);
            var modelId = "anthropic.claude-v2:1";

            var request = new ConverseRequest
            {
                ModelId = modelId,
                Messages = chatHistory,
                InferenceConfig = new InferenceConfiguration()
                {
                    MaxTokens = 512,
                    Temperature = 0.5F,
                    TopP = 0.9F
                }
            };

            try
            {
                var response = await client.ConverseAsync(request);
                return response?.Output?.Message?.Content?[0]?.Text ?? "";
            }
            catch (AmazonBedrockRuntimeException e)
            {
                Console.WriteLine($"ERROR: Can't invoke '{modelId}'. Reason: {e.Message}");
                throw;
            }
        }
    }
}
