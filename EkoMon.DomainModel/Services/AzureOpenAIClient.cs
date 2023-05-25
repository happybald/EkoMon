using Azure;
using Azure.AI.OpenAI;
using EkoMon.DomainModel.Models;
using Microsoft.Extensions.Options;
namespace EkoMon.DomainModel.Services
{
    public class AzureOpenAIClient
    {
        private Azure.AI.OpenAI.OpenAIClient? client;
        private string? modelDeploymentName;
        private readonly AppSettings appSettings;
        public AzureOpenAIClient(IOptionsSnapshot<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task<string> GetResponse(string prompt)
        {
            if (await EnsureClientCreated() == false)
                throw new Exception("Not possible to create client");

            var options = new ChatCompletionsOptions();
            options.Temperature = 0.9f;
            options.MaxTokens = 4097;
            options.Messages.Add(new ChatMessage(ChatRole.User, prompt));
            var completionResult = await client!.GetChatCompletionsAsync(modelDeploymentName, options);
            if (completionResult.HasValue == false)
                throw new Exception("Unknown Error");
            return completionResult.Value.Choices.Single().Message.Content;
        }

        private async Task<bool> EnsureClientCreated()
        {
            if (client != null && modelDeploymentName != null)
                return true;

            var apiKey = appSettings.AzureOpenAIKey;
            var endpoint = appSettings.AzureOpenAIEndpoint;
            modelDeploymentName = appSettings.AzureOpenAIModelDeploymentName;
            client = new Azure.AI.OpenAI.OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            return true;
        }
    }
}
