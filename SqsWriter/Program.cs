using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

public class SqsMessageSender
{
    // Specify your AWS Region
    private const string MyRegion = "ap-southeast-2"; // Replace with your actual region
    private const string MyQueueUrl = "https://sqs.ap-southeast-2.amazonaws.com/00000000/mytestsqs"; // Replace with your actual queue URL
    
    private static IAmazonSQS _sqsClient;

    public static async Task Main(string[] args)
    {
        // Initialize the SQS client
        _sqsClient = new AmazonSQSClient(RegionEndpoint.GetBySystemName(MyRegion));

        string messageBody = "Hello, Amazon SQS!";
        await SendMessage(_sqsClient, MyQueueUrl, messageBody);
    }

    /// <summary>
    /// Method to put a message on an SQS queue.
    /// </summary>
    private static async Task SendMessage(IAmazonSQS sqsClient, string qUrl, string messageBody)
    {
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = qUrl,
            MessageBody = messageBody,
            // Optional: Set a delay (in seconds) for standard queues
            // DelaySeconds = 5 
        };

        SendMessageResponse responseSendMsg = await sqsClient.SendMessageAsync(sendMessageRequest);

        Console.WriteLine($"Message added to queue\n {qUrl}");
        Console.WriteLine($"HttpStatusCode: {responseSendMsg.HttpStatusCode}");
        Console.WriteLine($"MessageId: {responseSendMsg.MessageId}");
    }
}
