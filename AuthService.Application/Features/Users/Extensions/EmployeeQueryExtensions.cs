using AuthService.Application.Features.Users.DTOs;

public static class EmployeeQueryExtensions
{
    public static IQueryable<EmployeeReportDto> ApplySearch(
        this IQueryable<EmployeeReportDto> query,
        string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        search = search.Trim();

        return query.Where(x =>
            x.FirstName.Contains(search) ||
            x.LastName.Contains(search) ||
            x.Email.Contains(search) ||
            x.DepartmentName.Contains(search) ||
            x.JobTitleName.Contains(search) ||
            (x.PhoneNumber ?? "").Contains(search) ||
            (x.City ?? "").Contains(search) ||
            (x.Country ?? "").Contains(search));
    }

    public static IQueryable<EmployeeReportDto> ApplySorting(
        this IQueryable<EmployeeReportDto> query,
        string? sortField,
        string? sortDir)
    {
        bool desc = sortDir?.ToLower() == "desc";

        return sortField?.ToLower() switch
        {
            "firstname" => desc
                ? query.OrderByDescending(x => x.FirstName)
                : query.OrderBy(x => x.FirstName),

            "lastname" => desc
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName),

            "email" => desc
                ? query.OrderByDescending(x => x.Email)
                : query.OrderBy(x => x.Email),

            "departmentname" => desc
                ? query.OrderByDescending(x => x.DepartmentName)
                : query.OrderBy(x => x.DepartmentName),

            "jobtitle" => desc
                ? query.OrderByDescending(x => x.JobTitleName)
                : query.OrderBy(x => x.JobTitleName),

            "city" => desc
                ? query.OrderByDescending(x => x.City)
                : query.OrderBy(x => x.City),

            "country" => desc
                ? query.OrderByDescending(x => x.Country)
                : query.OrderBy(x => x.Country),

            _ => query.OrderBy(x => x.FirstName)
        };
    }
}
//==========================================================================================
//using AuthService.Application.Features.Users.DTOs;

//namespace AuthService.Application.Common.Extensions;

//public static class EmployeeQueryExtensions
//{
//    public static IQueryable<EmployeeDto> ApplySearch(
//        this IQueryable<EmployeeDto> query,
//        string? search)
//    {
//        if (string.IsNullOrWhiteSpace(search))
//            return query;

//        search = search.Trim();

//        return query.Where(x =>
//            x.FirstName.Contains(search) ||
//            x.LastName.Contains(search) ||
//            x.Email.Contains(search) ||
//            x.DepartmentName.Contains(search) ||
//            x.JobTitleName.Contains(search) ||
//            (x.PhoneNumber ?? "").Contains(search) ||
//            (x.City ?? "").Contains(search) ||
//            (x.Country ?? "").Contains(search));
//    }

//    public static IQueryable<EmployeeDto> ApplySorting(
//        this IQueryable<EmployeeDto> query,
//        string? sortColumn,
//        string? sortDirection)
//    {
//        bool desc = sortDirection?.ToLower() == "desc";

//        return sortColumn?.ToLower() switch
//        {
//            "firstname" =>
//                desc ? query.OrderByDescending(x => x.FirstName)
//                     : query.OrderBy(x => x.FirstName),

//            "lastname" =>
//                desc ? query.OrderByDescending(x => x.LastName)
//                     : query.OrderBy(x => x.LastName),

//            "email" =>
//                desc ? query.OrderByDescending(x => x.Email)
//                     : query.OrderBy(x => x.Email),

//            "departmentname" =>
//                desc ? query.OrderByDescending(x => x.DepartmentName)
//                     : query.OrderBy(x => x.DepartmentName),

//            "jobtitle" =>
//                desc ? query.OrderByDescending(x => x.JobTitleName)
//                     : query.OrderBy(x => x.JobTitleName),

//            "phonenumber" =>
//                desc ? query.OrderByDescending(x => x.PhoneNumber)
//                     : query.OrderBy(x => x.PhoneNumber),

//            "city" =>
//                desc ? query.OrderByDescending(x => x.City)
//                     : query.OrderBy(x => x.City),

//            "country" =>
//                desc ? query.OrderByDescending(x => x.Country)
//                     : query.OrderBy(x => x.Country),

//            _ =>
//                query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
//        };
//    }
//}