using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Academics.Models
{
    public class SmsSender : IMessage
    {
        public bool SendMessage(string to, string subject, string message)
        {
            const string accountSid = "AC5b4d6b9564bb0629cbd01b76033b05cb";
            const string authToken = "f708636f6dd16cc217db95b05bdfed27";

            TwilioClient.Init(accountSid, authToken);

            try
            {
                var result = MessageResource.Create(
                    body: message,
                    from: new PhoneNumber("+14698278213"),
                    to: new PhoneNumber(to));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}