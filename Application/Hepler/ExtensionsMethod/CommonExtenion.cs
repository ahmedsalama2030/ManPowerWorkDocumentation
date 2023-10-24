using Application.Common.Pagination;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Application.Helper.ExtentionMethod;
public static class CommonExtenion
{
    public static void AddPagination(this HttpResponse response, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(totalItems, totalPages);
        var camelCaseFormater = new JsonSerializerSettings();
        camelCaseFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
        response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormater));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
    }
