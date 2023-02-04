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

    public BotHandler(
        ITeacherRepository teacherRepository, 
        ITelegramUserRepository telegramUserRepository, 
        IClassRepository classRepository,
        ITelegramBotClient botClient) 
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
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText) return;

        await SendChatTypingAsync(message, cancellationToken);
        
        var action = messageText switch
        {
            BotConst.CmdStart => SendStartCommand(message, cancellationToken),
            BotConst.BtnStudent => SendGroupEntryCommand(message, cancellationToken),
            BotConst.BtnTeacher => SendChoosingTeacherLetterCommand(message, cancellationToken),
            BotConst.BtnBack => SendBackCommand(message, cancellationToken),
            _ => SelectCommandByStep(message, cancellationToken)
        };
        
        Message sentMessage = await action;
    }

    private async Task<Message> SelectCommandByStep(Message message, CancellationToken cancellationToken)
    {
        if (!await CheckUserExistAsync(message.From))
        {
            return await SendStartCommand(message, cancellationToken);
        }
        
        var step = await _telegramUserRepository.GetUserStepAsync(message.From!.Id);
        
        return step switch 
        {
            EUserStep.ChoosingTeacherStepOne => await SendChoosingTeacherCommand(message, cancellationToken),
            EUserStep.ChoosingTeacherStepTwo => await SendTeacherSaveCommand(message, cancellationToken),
            EUserStep.EnteringGroupName => await SendGroupSaveCommand(message, cancellationToken),
            EUserStep.Start => await SendStartCommand(message, cancellationToken),
            _ => await SendNotFoundCommand(message, cancellationToken),
        };
    }

    private async Task<Message> SendStartCommand(Message message, CancellationToken cancellationToken)
    {
        await CreateTelegramUserAsync(message.From);
            
        ReplyKeyboardMarkup replyKeyboardMarkup = new ( new[] { new KeyboardButton[] { BotConst.BtnStudent, BotConst.BtnTeacher } })
        {
            ResizeKeyboard = true
        };

        return await SendTextMessageAsync(message, BotConst.MsgWhoAreYou, replyKeyboardMarkup, cancellationToken);
    }
    
    private async Task<Message> SendGroupEntryCommand(Message message, CancellationToken cancellationToken)
    {
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.EnteringGroupName);
        
        ReplyKeyboardMarkup replyKeyboardMarkup = new ( new[] { new KeyboardButton[] { BotConst.BtnBack } })
        {
            ResizeKeyboard = true
        };
        
        return await SendTextMessageAsync(message, BotConst.MsgEnterGroup, replyKeyboardMarkup, cancellationToken);
    }
    
    private async Task<Message> SendGroupSaveCommand(Message message, CancellationToken cancellationToken)
    {
        var groupExist = await _classRepository.GroupExistAsync(message.Text);

        if (!groupExist)
        {
            return await SendTextMessageAsync(message, BotConst.MsgWrongGroup, null, cancellationToken);
        }
        
        await _telegramUserRepository.SetGroupAsync(message.From!.Id, message.Text);
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.MainMenu);
        
        await SendTextMessageAsync(message, BotConst.MsgSaveGroup, null, cancellationToken);

        return await SendMainMenuCommand(message, cancellationToken);
    }

    private async Task<Message> SendChoosingTeacherLetterCommand(Message message, CancellationToken cancellationToken)
    {
        var firstLettersTeachers = await _teacherRepository.GetListFirstLettersTeachersAsync();

        var telegramKeyboard = new List<List<KeyboardButton>>();

        var keyboardLine = new List<KeyboardButton>();

        for (var i = 0; i < firstLettersTeachers.Count; i++)
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
        
        return await SendTextMessageAsync(message, BotConst.MsgEnterTeacherLetter, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> SendChoosingTeacherCommand(Message message, CancellationToken cancellationToken)
    {
        var teachersByLetter = await _teacherRepository.GetListTeachersByLetterAsync(message.Text);
        
        var telegramKeyboard = new List<List<KeyboardButton>>();

        var keyboardLine = new List<KeyboardButton>();

        var lines = (teachersByLetter.Count / BotConst.MaxColumns) == 0? 1 : teachersByLetter.Count / BotConst.MaxColumns;
        var oneOrTwoLines = teachersByLetter.Count <= 6 ? 1 : 2;
        lines = lines == 1 ? oneOrTwoLines : lines;

        for (var i = 0; i < teachersByLetter.Count; i++)
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
        
        return await SendTextMessageAsync(message, BotConst.MsgEnterTeacher, replyKeyboardMarkup, cancellationToken);
    }
    
    private async Task<Message> SendTeacherSaveCommand(Message message, CancellationToken cancellationToken)
    {
        var teacherExist = await _teacherRepository.TeacherExistAsync(message.Text);

        if (!teacherExist)
        {
            return await SendTextMessageAsync(message, BotConst.MsgWrongTeacher, null, cancellationToken);
        }
        
        await _telegramUserRepository.SetTeacherAsync(message.From!.Id, message.Text);
        
        await _telegramUserRepository.UpdateUserStepAsync(message.From!.Id, EUserStep.MainMenu);
        
        await SendTextMessageAsync(message, BotConst.MsgSaveTeacher, null, cancellationToken);
        
        return await SendMainMenuCommand(message, cancellationToken);
    }
    
    private async Task<Message> SendMainMenuCommand(Message message, CancellationToken cancellationToken)
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

        return await SendTextMessageAsync(message, BotConst.MsgMainMenu, replyKeyboardMarkup, cancellationToken);
    }

    private async Task<Message> SendBackCommand(Message message, CancellationToken cancellationToken)
    {
        var step = await _telegramUserRepository.GetUserStepAsync(message.From!.Id);

        return step switch 
        {
            EUserStep.ChoosingTeacherStepOne => await SendStartCommand(message, cancellationToken),
            EUserStep.ChoosingTeacherStepTwo => await SendChoosingTeacherLetterCommand(message, cancellationToken),
            EUserStep.EnteringGroupName => await SendStartCommand(message, cancellationToken),
            _ => await SendNotFoundCommand(message, cancellationToken),
        };
    }
    
    private async Task<Message> SendNotFoundCommand(Message message, CancellationToken cancellationToken)
    {
        return await SendTextMessageAsync(message, "🤔", null, cancellationToken);
    }
    
    private async Task<Message> SendTextMessageAsync(Message message, string text, [CanBeNull] IReplyMarkup replyMarkup, CancellationToken cancellationToken)
    {
        return await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: text,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken);
    }

    private async Task SendChatTypingAsync(Message message, CancellationToken cancellationToken)
    {
        await _botClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);
    }

    private async Task CreateTelegramUserAsync(User user)
    {
        if (!await CheckUserExistAsync(user))
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

        var telegramUser = await _telegramUserRepository.GetByTelegramIdAsync(user.Id);
        await _telegramUserRepository.UpdateUserStepAsync(telegramUser.TelegramId, EUserStep.Start);
    }
    
    private async Task<bool> CheckUserExistAsync(User user)
    {
        return await _telegramUserRepository.GetByTelegramIdAsync(user.Id) is not null;
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        //TODO:
        return Task.CompletedTask;
    }
}
