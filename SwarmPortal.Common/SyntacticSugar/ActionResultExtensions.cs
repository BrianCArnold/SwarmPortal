using Microsoft.AspNetCore.Mvc;

namespace SwarmPortal.Common;
public static class ActionResultExtensions
{
    public static ActionResult<T> Ok<T>(this T model) => new OkObjectResult(model);
    public static async Task<ActionResult<T>> OkAsync<T>(this Task<T> model) => new OkObjectResult(await model);
}