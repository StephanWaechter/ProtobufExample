// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

using var messanger = new ProtobufCore.Messenger();
var service = new ProtobufCore.Service(messanger);
service.RequestDataRecived += Service_RequestDataRecived;

void Service_RequestDataRecived(ProtobufCore.DataInfo message)
{
    foreach (var item in message.Items)
    {
        Console.WriteLine(item.Name);
    }
}
service.SendRequestData();
Console.ReadKey();
