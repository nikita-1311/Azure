using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

ServiceBusClient client;
ServiceBusProcessor processor;

var clientOptions = new ServiceBusClientOptions

{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient("Endpoint=sb://nikitacakeshop.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1gNaDlSGoKkgKIKQTp+z7WyPWanqe/S6Moa2ZpsaUvE= ", clientOptions);
processor = client.CreateProcessor("mytopic","subscription2", new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;
    await processor.StartProcessingAsync();
    Console.WriteLine("Wait for a minuteand then press key to end the processing...!!!");
    Console.ReadKey();
    Console.WriteLine("\n Stopping the reciever ...!!!");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped recieving message .!!!!");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}
async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    System.Console.WriteLine($"Recieved:{body}");
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}