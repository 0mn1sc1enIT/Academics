namespace Academics.Models
{
    public interface IMessage
    {
        public bool SendMessage(string to, string subject, string body);
    }
}
