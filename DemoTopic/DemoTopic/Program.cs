using Azure.Messaging.ServiceBus;
using System;

ServiceBusClient client;
ServiceBusSender sender;

const int no_Of_Messages = 3;

var clientOptions = new ServiceBusClientOptions
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

//Here we are using Shared Access Key 
client = new ServiceBusClient("Endpoint=sb://nikitacakeshop.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1gNaDlSGoKkgKIKQTp+z7WyPWanqe/S6Moa2ZpsaUvE= ", clientOptions);
sender = client.CreateSender("mytopic");

//creating a Batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
for (int i = 1; i <= no_Of_Messages; i++)

{
    //try adding a message to the batch 
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        throw new Exception($" The Message is too large  to fit in the batch ");
    }
}
try
{ 
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A Batch of {no_Of_Messages} has been published to the Queue");
}
finally
{
    //Need to Cleanup object and Resources in the network 
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press an key to close the Appplication");
Console.ReadKey();