namespace EkoMon.DomainModel.Models
{
    public class AppSettings
    {
        public string DbConnection { get; set; } = string.Empty;

        public string AzureOpenAIKey { get; set; } = string.Empty;
        public string AzureOpenAIEndpoint { get; set; } = string.Empty;
        public string AzureOpenAIModelDeploymentName { get; set; } = string.Empty;
    }
}
