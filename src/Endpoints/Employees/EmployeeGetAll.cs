using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
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

        var db = new SqlConnection(configuration["ConnectionStrings:IWantDb"]);

        var query = 
            @"SELECT Email, ClaimValue AS 'Name'
            FROM AspNetUsers U 
            INNER JOIN AspNetUserClaims C ON U.Id = C.UserId
            AND ClaimType = 'Name'
            ORDER BY Name
            OFFSET (@page -1 ) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var employees = db.Query<EmployeeResponse>(
            query, 
            new { page, rows}
        );

        return Results.Ok(employees);
    }

}
