using Academics.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academics.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private IMessage _message;
        public ContactController(ILogger<ContactController> logger, IMessage message)
        {
            _logger = logger;
            _message = message;
        }
        public IActionResult Index()
        {
            Message message = new Message
            {
                email = "example@example.com",
                phoneNumber = "+1234567891",
                firstName = "John",
                lastName = "Doe"
            };


            return View(message);
        }

        [HttpPost]
        public IActionResult Index(Message userMessage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Сохранение данных в базу данных
                    _logger.LogInformation("Попытка отправки сообщения для пользователя " + "{firstName} {lastName} на электронный адрес {email}", 
                        userMessage.lastName,userMessage.firstName, userMessage.email);

                    if (_message.SendMessage(userMessage.email, "From Contact from", userMessage.message))
                    {
                        _logger.LogError("При попытке отправить уведомление на {email} с именем {lastName} {firstName} возникла ошибка", userMessage.email, userMessage.lastName, userMessage.firstName);
                    } else
                    {

                        _logger.LogInformation("Попытка отправки сообщения для пользователя " + "{firstName} {lastName} на электронный адрес {email} прошла успешно!", 
                            userMessage.lastName, userMessage.firstName, userMessage.email);                    
                    }
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    _logger.LogError(ex, "Ошибка при отправке сообщения для пользователя {lastName} {firstName} на электронный адрес {email}", 
                        userMessage.lastName, userMessage.firstName, userMessage.email);

                    // Добавление ошибки в ModelState
                    ModelState.AddModelError("", "Произошла ошибка при обработке вашего запроса. Пожалуйста, попробуйте позже.");
                }
            }

            // Если данные невалидны или произошла ошибка, возвращаем форму с ошибками
            return View(userMessage);
        }
        public IActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewsLetterSignUp([FromForm] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Попытка подписки с пустым email");
                return RedirectToAction("Index");
            }

            try
            {
                // Логирование успешной подписки
                _logger.LogInformation("Пользователь с email {Email} пытается подписаться на рассылку", email);

                // Здесь можно добавить логику для сохранения email в базу данных или отправки уведомления
                if (_message.SendMessage(email, " ", "Вы подписались на рассылку"))
                {
                    _logger.LogInformation("Письмо для подписки на рассылку успешно отправлено на {Email}", email);
                } else
                {
                    _logger.LogError("Не удалось отправить письмо для подписки на рассылку на {Email}", email);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _logger.LogError(ex, "Ошибка при подписке на рассылку для email {Email}", email);
                return RedirectToAction("Index");
            }
        }
    }
}
