using Microsoft.AspNetCore.Mvc;
using TicTacToe.Orleans.Contracts;

namespace TicTacToe.Orleans.Extensions;

public static class ControllersExtensions
{
    public static Guid GetGuid(this ControllerBase controller)
    {
        if (controller.Request.Cookies[GameContract.CookiePlayerId] is {Length: > 0 } idCookie)
        {
            return Guid.Parse(idCookie);
        }

        var guid = Guid.NewGuid();
        controller.Response.Cookies.Append(GameContract.CookiePlayerId,guid.ToString());
        return guid;
    }
}