using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Schedule.Constants;
using Schedule.Enums;
using Schedule.Interfaces;
using Schedule.Models.Bot;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.Handlers;

public class BotHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IClassRepository _classRepository;

    public BotHandler(ITelegramBotClient botClient, 
        ITeacherRepository teacherRepository, 
        ITelegramUserRepository telegramUserRepository, 
        IClassRepository classRepository)
    {
        _botClient = botClient;
        _teacherRepository = teacherRepository;
        _telegramUserRepository = telegramUserRepository;
        _classRepository = classRepository;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message }                       => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message }                 => BotOnMessageReceived(message, cancellationToken),
            _                                              => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText) return;

        var action = messageText switch
        {
            BotConst.CmdStart => SendStartKeyboardCommand(_botClient, message, cancellationToken),
            BotConst.BtnStudent => ChooseGroupCommand(_botClient, message, cancellationToken),
            BotConst.BtnTeacher => ChooseTeacherLetterCommand(_botClient, message, cancellationToken),
            BotConst.BtnBack => BackCommand(_botClient, message, cancellationToken),
            _ => StepCommand(_botClient, message, cancellationToken)
        };
        
        Message sentMessage = await action;
    }

    private async Task<Message> ChooseTeacherLetterCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        var firstLettersTeachers = await _teacherRepository.GetListFirstLettersTeachersAsync();

        var telegramKeyboard = new List<List<KeyboardButton>>();

        var keyboardLine = new List<KeyboardButton>();

        for (var i = 0; i < firstLettersTeachers.Count(); i++)
        {
            keyboardLine.Add(firstLettersTeachers[i]);
               
            if (keyboardLine.Count == BotConst.MaxColumns || i == firstLettersTeachers.Count - 1)
            {
                telegramKeyboard.Add(new List<KeyboardButton>(keyboardLine));
                keyboardLine.Clear();
            }
        }
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.ChoosingTeacherStepOne);
        
        telegramKeyboard.Add(new List<KeyboardButton>(new List<KeyboardButton>() { BotConst.BtnBack }));
            
        ReplyKeyboardMarkup replyKeyboardMarkup = new(telegramKeyboard)
        {
            ResizeKeyboard = true
        };
        
        return await SendTextMessageAsync(botClient, message, BotConst.MsgEnterTeacherLetter, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> BackCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        var step = await _telegramUserRepository.GetUserStepAsync(message.From!.Id);

        return step switch 
        {
            EUserStep.ChoosingTeacherStepOne => await SendStartKeyboardCommand(botClient, message, cancellationToken),
            EUserStep.ChoosingTeacherStepTwo => await ChooseTeacherLetterCommand(botClient, message, cancellationToken),
            EUserStep.EnteringGroupName => await SendStartKeyboardCommand(botClient, message, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private async Task<Message> ChooseTeacherCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        var teachersByLetter = await _teacherRepository.GetListTeachersByLetterAsync(message.Text);
        
        var telegramKeyboard = new List<List<KeyboardButton>>();

        var keyboardLine = new List<KeyboardButton>();

        var countLines = (teachersByLetter.Count / BotConst.MaxColumns) == 0? 1 : teachersByLetter.Count / BotConst.MaxColumns;
        var lines = countLines == 1 ? teachersByLetter.Count <= 6 ? 1 : 2 : countLines;

        for (var i = 0; i < teachersByLetter.Count(); i++)
        {
            keyboardLine.Add(teachersByLetter[i]);
               
            if (keyboardLine.Count == lines || i == teachersByLetter.Count - 1)
            {
                telegramKeyboard.Add(new List<KeyboardButton>(keyboardLine));
                keyboardLine.Clear();
            }
        }
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.ChoosingTeacherStepTwo);
        
        telegramKeyboard.Add(new List<KeyboardButton>(new List<KeyboardButton>() { BotConst.BtnBack }));
            
        ReplyKeyboardMarkup replyKeyboardMarkup = new(telegramKeyboard)
        {
            ResizeKeyboard = true
        };
        
        return await SendTextMessageAsync(botClient, message, BotConst.MsgEnterTeacher, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> StepCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        var step = await _telegramUserRepository.GetUserStepAsync(message.From!.Id);
        
        return step switch 
        {
            EUserStep.ChoosingTeacherStepOne => await ChooseTeacherCommand(botClient, message, cancellationToken),
            EUserStep.ChoosingTeacherStepTwo => await SaveTeacherCommand(botClient, message, cancellationToken),
            EUserStep.EnteringGroupName => await SaveGroupCommand(botClient, message, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private async Task<Message> SaveGroupCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        var group = await _classRepository.GroupExistAsync(message.Text);

        if (!group)
        {
           return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: BotConst.MsgWrongGroup,
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }
        
        await _telegramUserRepository.SetGroupAsync(message.From!.Id, message.Text);
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.MainMenu);
        
        await SendTextMessageAsync(botClient, message, BotConst.MsgSaveGroup, null, cancellationToken);

        return await GetMainMenu(botClient, message, cancellationToken);
    }

    private async Task<Message> GetMainMenu(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        ReplyKeyboardMarkup replyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[] { BotConst.MsgScheduleForToday },
                new KeyboardButton[] { BotConst.MsgScheduleForTomorrow },
                new KeyboardButton[] { BotConst.MsgScheduleForWeek, BotConst.MsgScheduleForNextWeek },
                new KeyboardButton[] { BotConst.MsgScheduleForDay, BotConst.MsgScheduleByCabinet },
                new KeyboardButton[] { BotConst.MsgSettings },
            })
        {
            ResizeKeyboard = true
        };

        return await SendTextMessageAsync(botClient, message, BotConst.MsgMainMenu, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> SaveTeacherCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        await _telegramUserRepository.SetTeacherAsync(message.From!.Id, message.Text);
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.MainMenu);
        
        await SendTextMessageAsync(botClient, message, BotConst.MsgSaveTeacher, null, cancellationToken);
        
        return await GetMainMenu(botClient, message, cancellationToken);
    }

    private async Task<Message> ChooseGroupCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.EnteringGroupName);
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new ( new[] { new KeyboardButton[] { BotConst.BtnBack } })
        {
            ResizeKeyboard = true
        };
        
        return await SendTextMessageAsync(botClient, message, BotConst.MsgEnterGroup, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> SendStartKeyboardCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await SendChatTypingAsync(botClient, message, cancellationToken);

        await CreateTelegramUser(message.From);
            
        ReplyKeyboardMarkup replyKeyboardMarkup = new ( new[] { new KeyboardButton[] { BotConst.BtnStudent, BotConst.BtnTeacher } })
        {
            ResizeKeyboard = true
        };

        return await SendTextMessageAsync(botClient, message, BotConst.MsgWhoAreYou, replyKeyboardMarkup, cancellationToken);
    }

    private async Task SendChatTypingAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        await botClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);
    }

    private async Task<Message> SendTextMessageAsync(ITelegramBotClient botClient, Message message, string msg, [CanBeNull] IReplyMarkup replyMarkup, CancellationToken cancellationToken)
    {
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: BotConst.MsgWhoAreYou,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken);
    }
    
    private async Task CreateTelegramUser(User user)
    {
        var telegramUser = await _telegramUserRepository.GetByTelegramIdAsync(user.Id);

        if (telegramUser is null)
        {
            await _telegramUserRepository.InsertAsync(new TelegramUser()
            {
                TelegramId = user.Id,
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsPremium = user.IsPremium,
                LanguageCode = user.LanguageCode,
                CurrentStep = EUserStep.Start.ToString()
            });
            
            return;
        }

        await _telegramUserRepository.UpdateUserStepAsync(telegramUser.TelegramId, EUserStep.Start);
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        //TODO:
        return Task.CompletedTask;
    }
}
