using System.Text.Json.Serialization;

namespace Todo.Api.Configurations;

internal class FirebaseCredentials
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("project_id")]
    public string Project_Id { get; set; }

    [JsonPropertyName("private_key_id")]
    public string Private_Key_Id { get; set; }

    [JsonPropertyName("private_key")]
    public string Private_Key { get; set; }

    [JsonPropertyName("client_email")]
    public string Client_Email { get; set; }

    [JsonPropertyName("client_id")]
    public string Client_Id { get; set; }

    [JsonPropertyName("auth_uri")]
    public string Auth_Uri { get; set; }

    [JsonPropertyName("token_uri")]
    public string Token_Uri { get; set; }

    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string Auth_Provider_X509_Cert_Url { get; set; }

    [JsonPropertyName("client_x509_cert_url")]
    public string Client_X509_Cert_Url { get; set; }

    [JsonPropertyName("universe_domain")]
    public string Universe_Domain { get; set; }
}