﻿using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName query)
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

        return Results.Ok(query.Execute(page.Value, rows.Value));
    }

}
