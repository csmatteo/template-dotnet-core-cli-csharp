using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            String secretName = "MySecret";
            String keyVaultName = "training-no-purge-prot";
            var keyvalueUri = "https://training-no-purge-prot.vault.azure.net";
            SecretClientOptions options = new SecretClientOptions(){
                Retry = {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            var client = new SecretClient(new Uri(keyvalueUri), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(secretName);

            Console.WriteLine("GetSecret:" + secret.Value);
            Console.Write("Enter Secret > ");

            String secretValue = Console.ReadLine();
            client.SetSecret(secretName, secretValue);
            Console.Write("SetSecret:");
            Console.Write("\tKey: " + secretName);
            Console.Write("\tValue: " + secretValue);

            Console.WriteLine("GetSecret: " +secret.Value);

            client.StartDeleteSecret(secretName);
            Console.WriteLine("StartDeleteSecret: " + keyVaultName);

            Console.WriteLine("GetSecret: " +secret.Value);
        }
    }
}
