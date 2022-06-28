using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5577857493:AAGZ286-rxkLoyDgIwFcfsjIdE9k3ZFs8Sc");
string[] pics = new[] { "Люки теплосети: Северный люк - затоплен; Южный люк - работает исправно; Восточный люк - работает исправно; Западный люк - работает исправно;", "Люки теплосети: Северный люк - работает исправно; Южный люк - работает исправно; Восточный люк - затоплен; Западный люк - работает исправно;", "Люки теплосети: Северный люк - работет исправно; Южный люк - работает исправно; Восточный люк - работает исправно; Западный люк - работает исправно;", "Люки теплосети: Северный люк - затоплен; Южный люк - затоплен; Восточный люк - затоплен; Западный люк - затоплен;" };
Random rnd = new Random();
using var cts = new CancellationTokenSource();
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};

botClient.StartReceiving(
    HandleUpdatesAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начал прослушку @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }

    if (update.Type == UpdateType.CallbackQuery)
    {
        await HandleCallbackQuery(botClient, update.CallbackQuery);
        return;
    }
}

async Task HandleMessage(ITelegramBotClient botClient, Message message)
{
    if (message.Text == "/start")
    {
        await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите команду: /viewing | /help");
        return;
    }

    if (message.Text == "/help")
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Зачем ты нужен?", "buy"),
                InlineKeyboardButton.WithCallbackData("Кто тебя создал?", "bi"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Любишь мемы?", "ka"),
            },
        });
        await botClient.SendTextMessageAsync(message.Chat.Id, "На что мне нужно ответить?", replyMarkup: keyboard);
        return;
    }

    if (message.Text == "/viewing")
    {
        InlineKeyboardMarkup keyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Карта мест-ти", "lay"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Полив. установки", "er"),
                InlineKeyboardButton.WithCallbackData("Люки теплосети", "nnn"),
            },
        });
        await botClient.SendTextMessageAsync(message.Chat.Id, "Я к вашим услугам:", replyMarkup: keyboard);
        return;
    }

    await botClient.SendTextMessageAsync(message.Chat.Id, $"Вы сказали:\n{message.Text}");
}
async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
{
    if (callbackQuery.Data.StartsWith("buy"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Меня создали, т.к. многие хотят знать о состоянии эксплуатируемого оборудовани: владельцам бизнеса необходимо знать общее состояние оборудования, эксплуатирующим подразделениям и Заказчикам, необходимо получать оперативные сообщения об авариях и неисправностях, а также иметь возможность удаленно ими управлять. Для просмотра напишите /viewing"
        );
        return;
    }

    if (callbackQuery.Data.StartsWith("bi"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Меня создала команда 《Старт》. Связаться с тех. поддержкой можно тут: @Slayer666228"
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("ka"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Просто обожаю! Мой любимый мем - Доге) https://static.wikia.nocookie.net/memes9731/images/d/da/IA2G3ORVG.jpg/revision/latest?cb=20200528160845&path-prefix=ru"
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("lay"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"https://glavagronom.ru/media/uploads/xv7kAI_14-s1200.jpg"
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("er"))
    {
        await botClient.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Поливальные установки: Южная установка - работает исправно; Юго-восточная установка - поломка шланга, 13% воды выливается; Восточная установка - работает исправно; Юго-западная установка - работает исправно; Северная установка - поломка насадки, полив осуществляется неравномерно; Северо-западная установка - работает неисправно, шланг сильно поломан; Северо-восточная установка - работает исправно; Западная установка - работает исправно;"
        );
        return;
    }
    if (callbackQuery.Data.StartsWith("nnn"))
    {
        int i = rnd.Next(0, 4);
        await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, pics[i]);
        return;
    }
    await botClient.SendTextMessageAsync(
        callbackQuery.Message.Chat.Id,
        $"You choose with data: {callbackQuery.Data}"
        );
    return;
}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
