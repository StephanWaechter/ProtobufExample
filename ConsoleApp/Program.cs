// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var service = new ProtobufCore.Client();
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
