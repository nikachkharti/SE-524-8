namespace Lecture28.DependencyInversion
{
    //ABSTRACTION
    public interface IMessageService
    {
        void SendMessage(string message);
    }


    //LOW MODULE
    public class EmailService : IMessageService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("Email sent: " + message);
        }
    }

    //LOW MODULE
    public class SmsService : IMessageService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("SMS sent: " + message);
        }
    }


    //HIGH MODULE
    public class Notification
    {
        private IMessageService _messageService;

        public Notification(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Send(string message)
        {
            _messageService.SendMessage(message);
        }


    }


}
