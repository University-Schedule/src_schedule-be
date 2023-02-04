using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Filters;
using Schedule.Handlers;
using Telegram.Bot.Types;
using Volo.Abp.AspNetCore.Mvc;

namespace Schedule.Controllers;

[Route("bot")]
public class BotController : AbpController
{
    [HttpPost]
    [ValidateTelegramBot]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] BotHandler botHandler,
        CancellationToken cancellationToken)
    {
        await botHandler.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}