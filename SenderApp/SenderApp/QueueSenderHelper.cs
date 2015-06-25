using System;
using System.Configuration;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SenderApp
{
    public class QueueSenderHelper
    {
        const string QueueUrl = "https://sqs.eu-west-1.amazonaws.com/790753336878/PK-Demo-Queue";
        private static AmazonSQSClient _sqsClient;

        public void DoWork()
        {
            var credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["AccessKey"], ConfigurationManager.AppSettings["SecretKey"]);
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1);
            var running = true;
            while (running)
            {
                Console.WriteLine("Message to send: ");
                var message = Console.ReadLine();
                
                if (message == "end")
                {
                    running = false;
                }
                else
                {
                    AddToQueue(message);
                }
            }
        }

        private void AddToQueue(string data)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = QueueUrl,
                MessageBody = data
            };
            var response = _sqsClient.SendMessage(sendMessageRequest);
            Console.WriteLine("Response status code: " + response.HttpStatusCode);
        }
    }
}
