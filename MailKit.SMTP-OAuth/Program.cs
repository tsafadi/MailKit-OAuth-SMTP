using MailKit.Security;
using Schrott;


string tokenEndpoint = $"https://login.microsoftonline.com/<tenant-id>/oauth2/token";
var clientId = "<client-id>";
var clientSecret = "<client-secret>";
var grantType = "client_credentials";
var resource = "https://outlook.office365.com";
var scope = ".default";

var accountEmail = "<sender-email>";
var smtpServer = "smtp.office365.com";
var smtpPort = 587;
var toEmail = "<receiver-email>";

var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
{
    new KeyValuePair<string, string>("client_id", clientId),
    new KeyValuePair<string, string>("client_secret", clientSecret),
    new KeyValuePair<string, string>("grant_type", grantType),
    new KeyValuePair<string, string>("resource", resource),
    new KeyValuePair<string, string>("scope", scope),
});

var accessToken = await TokenService.GetAccessToken(content, tokenEndpoint);
var oauth2 = new SaslMechanismOAuth2(accountEmail, accessToken);
Console.WriteLine(accessToken);

MailService.Send(oauth2, smtpServer, smtpPort, accountEmail, toEmail);