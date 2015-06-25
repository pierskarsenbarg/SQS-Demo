using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace ReceiverApp
{
    public class QueueReceiverHelper
    {
        const string QueueUrl = "https://sqs.eu-west-1.amazonaws.com/790753336878/PK-Demo-Queue";
        private static AmazonSQSClient _sqsClient;

        public void DoWork()
        {
            var credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["AccessKey"], ConfigurationManager.AppSettings["SecretKey"]);
            _sqsClient = new AmazonSQSClient(credentials, RegionEndpoint.EUWest1);
            Console.WriteLine("Number of messages at once");
            int numberOfMessages;
            int.TryParse(Console.ReadLine(), out numberOfMessages);
            var running = true;
            while (running)
            {
                var messageList = GetMessageFromQueue(numberOfMessages);
                Console.WriteLine("Number of messages: " + messageList.Count);
                if (messageList.Count == 0)
                {
                    Thread.Sleep(5000);
                }
                Console.WriteLine();
                foreach (var message in messageList)
                {
                    Console.WriteLine("Message: " + message.Body);
                    Console.WriteLine("Receipt Handle: " + message.ReceiptHandle);
                    Console.WriteLine("Delete?");
                    var readLine = Console.ReadLine();
                    var delete = readLine != null ? readLine.ToLower() : "n";
                    if (delete == "y")
                    {
                        DeleteMessageFromQueue(message.ReceiptHandle);
                    }
                }
            }
        }

        private List<Message> GetMessageFromQueue(int number)
        {
            var receiveMessageRequest = new ReceiveMessageRequest { QueueUrl = QueueUrl, MaxNumberOfMessages = number };
            var response = _sqsClient.ReceiveMessage(receiveMessageRequest);
            return response.HttpStatusCode == HttpStatusCode.OK ? response.Messages : new List<Message>();
        }

        private bool DeleteMessageFromQueue(string receiptHandle)
        {
            var deleteRequest = new DeleteMessageRequest(QueueUrl, receiptHandle);
            var response = _sqsClient.DeleteMessage(deleteRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
