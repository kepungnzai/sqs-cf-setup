using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

public class SqsMessageSender
{
    // Specify your AWS Region
    private const string MyRegion = "ap-southeast-2"; // Replace with your actual region
    private const string MyQueueUrl = "https://sqs.ap-southeast-2.amazonaws.com/000000/mytestsqs"; // Replace with your actual queue URL
    
    private static IAmazonSQS _sqsClient;

    public static async Task Main(string[] args)
    {
        // Initialize the SQS client
        _sqsClient = new AmazonSQSClient(RegionEndpoint.GetBySystemName(MyRegion));

        string messageBody = "Hello, Amazon SQS!";
        //await SendMessage(_sqsClient, MyQueueUrl, messageBody);

        var cancellationTokenSource = new CancellationTokenSource();
        await ReceiveMessage(_sqsClient, cancellationTokenSource.Token, qUrl: MyQueueUrl);
    }

    /// <summary>
    /// Method to put a message on an SQS queue.
    /// </summary>
    private static async Task SendMessage(IAmazonSQS sqsClient, string qUrl, string messageBody)
    {
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = qUrl,
            MessageBody = messageBody
        };

        SendMessageResponse responseSendMsg = await sqsClient.SendMessageAsync(sendMessageRequest);

        Console.WriteLine($"Message added to queue\n {qUrl}");
        Console.WriteLine($"HttpStatusCode: {responseSendMsg.HttpStatusCode}");
        Console.WriteLine($"MessageId: {responseSendMsg.MessageId}");
    }

    private static async Task ReceiveMessage(IAmazonSQS sqsClient, CancellationToken cancellationToken, string qUrl)
    {
        var messageCount = 0;
        
        var request = new ReceiveMessageRequest
        {
            QueueUrl = qUrl,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 20,
            AttributeNames = new List<string> { "All" },
            MessageAttributeNames = new List<string> { "All" }
        };

        foreach (var _ in Enumerable.Range(0, 5)) // Loop to receive messages multiple times
        {
            var response = await sqsClient.ReceiveMessageAsync(request, cancellationToken);
            if (response.Messages.Count == 0)
            {
                Console.WriteLine("∅ No messages received. Waiting again...\n");
            }

            Console.WriteLine($"\n✓ Received {response.Messages.Count} message(s):\n");

            foreach (var message in response.Messages)
            {
                messageCount++;
                Console.WriteLine($"MessageId: {message.MessageId}");
                Console.WriteLine($"Body: {message.Body}");

                Console.WriteLine("Deleting message from queue...");
                await sqsClient.DeleteMessageAsync(new DeleteMessageRequest
                {
                    QueueUrl = qUrl,
                    ReceiptHandle = message.ReceiptHandle
                });

                //await ProcessMessageAsync(sqsClient, message);
            }
        }        

        Console.WriteLine($"[Total processed: {messageCount}]\n");
    }
}
