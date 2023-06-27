using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token);

var startmenu = new MenuButtonCommands();
var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates
    if (update.Message is not { } message)
        return;
    var chatId = message.Chat.Id;

    //only text
    if (message.Text is { } messageText)
    {

        Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");

        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
        KeyboardButton.WithRequestContact("Share Contact")
        })
        {
            ResizeKeyboard = false
        };

        if (messageText.Equals("/start"))
        {
            await botClient.SendVenueAsync(
                chatId: chatId,
                latitude: 46.47165114377023f,
                longitude: 30.7353956533852f,
                title: "Notebook-Service",
                address: "вулиця Мала Арнаутська, 70, Одеса, Одеська область, 65000",
                cancellationToken: cancellationToken);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "send a contact to register",
                replyMarkup: replyKeyboardMarkup);
        }

    }

    //only contact
    if (message.Contact is { } messageContact)
    {

        Console.WriteLine($"Received a '{message.Contact.PhoneNumber}' message in chat {chatId}.");

        ReplyKeyboardRemove replyKeyboardRemove = new ReplyKeyboardRemove();
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "done",
            replyMarkup: replyKeyboardRemove);

        //using (var context = new ApplicationDbContext())
        //{
        //    // Параметр процедуры

        //    var phoneNumberParam = new SqlParameter("@I_PHONE", SqlDbType.VarChar) { Value = new string(message.Text) };

        //    // Вызов процедуры и получение результатов
        //    var employees = await context.Employees
        //        .FromSqlRaw("EXEC NBS_GET_WITEM @I_PHONE", phoneNumberParam)
        //        .ToListAsync();

        //    // Формирование текста сообщения с результатами процедуры
        //    var resultMessage = "Results:\n";
        //    foreach (var employee in employees)
        //    {
        //        resultMessage += $"ID: {employee.O_WITEM_ID}, Date: {employee.O_WITEM_DATE}, RegNum: {employee.O_WITEM_REGNUM}, Status ID: {employee.O_WITEMSTATUS_ID}, Status Name: {employee.O_WITEMSTATUS_NAME}\n";
        //    }

        //    // Отправка сообщения с результатами
        //    await botClient.SendTextMessageAsync(
        //        chatId: chatId,
        //        text: resultMessage,
        //        replyMarkup: new InlineKeyboardMarkup(
        //            InlineKeyboardButton.WithCallbackData(messageText = "press")),
        //        cancellationToken: cancellationToken);
        //}

    }

}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}