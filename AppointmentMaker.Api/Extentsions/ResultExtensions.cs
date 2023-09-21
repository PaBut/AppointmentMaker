using System.Net;
using System.Reflection.Metadata.Ecma335;
using AppointmentMaker.Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Extentsions
{
    public static class ResultErrorExtensions
    {
        public static int GetErrorStatusCode(this Error result)
        {
            return result.Code switch
            {
                "Error.NotFound" => 404,
                "Error.UnAuthorized" => 401,
                _ => 400
            };
        }
    }
}
