using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Filters;
using Schedule.Services;
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
        [FromServices] UpdateHandlers handleUpdateService,
        CancellationToken cancellationToken)
    {
        await handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}