using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;

namespace IWantApp.Endpoints.Employees;
    public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        if (!page.HasValue)
        {
            page = 1;
        }

        if (!rows.HasValue)
        {
            rows = 10;
        }

        if (rows > 10)
        {
            return Results.Problem("No more than 10 records are allowed per page");
        }

        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }

}
