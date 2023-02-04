using AutoMapper;
using Schedule.Models.Bot;
using Telegram.Bot.Types;

namespace Schedule.Mapping;

public class TelegramBotAutoMapperProfile : Profile
{
    public TelegramBotAutoMapperProfile()
    {
        CreateMap<TelegramUser, User>();
    }
}