using System.Text;
using Microsoft.Azure.ServiceBus;

const string QueueConnectionString =
    "connectionString";
const string QueuePath = "ProductChanged";
IQueueClient _queueClient;

await SendMessagesAsync();
Console.WriteLine("messages were sent");
Console.ReadLine();

async Task SendMessagesAsync()
{
    _queueClient = new QueueClient(QueueConnectionString, QueuePath);
    var messages = "teste,Mensagem1,Olá,Vou tirar 10!,Be Welcome"
        .Split(',')
        .Select(msg => {
            Console.WriteLine($"Will send message: {msg}");
            return new Message(Encoding.UTF8.GetBytes(msg));
        })
        .ToList();
    await _queueClient.SendAsync(messages);
    await _queueClient.CloseAsync();
}

 async Task ReceiveMessagesAsync()
{
    _queueClient = new QueueClient(QueueConnectionString, QueuePath);
    _queueClient.RegisterMessageHandler(MessageHandler,
        new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
    Console.ReadLine();
    await _queueClient.CloseAsync();
}

 Task ExceptionHandler(ExceptionReceivedEventArgs exceptionArgs)
{
    Console.WriteLine($"Message handler encountered an exception {exceptionArgs.Exception}.");
    var context = exceptionArgs.ExceptionReceivedContext;
    Console.WriteLine($"Endpoint:{context.Endpoint}, Path:{context.EntityPath}, Action:{context.Action}");
    return Task.CompletedTask;
}

 async Task MessageHandler(Message msg , CancellationToken cancellationToken)
{
    Console.WriteLine($"Received message:{Encoding.UTF8.GetString(msg.Body)}");
    await _queueClient.CompleteAsync(
        msg.SystemProperties.LockToken);
}
