using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KM_Management.Shared;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid Type Of Error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TResponse> Success<TResponse>(TResponse value) => new(true, Error.None, value);
    public static Result<TResponse> Failure<TResponse>(Error error) => new(false, error, default);


    public virtual IActionResult MapResponse()
    {
        if (IsFailure && Error.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BadRequestObjectResult(Error);
        }

        if (IsFailure && Error.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundObjectResult(Error);
        }

        return new OkResult();
    }
}

public class Result<TResponse> : Result
{
    public TResponse? Value { get; }

    protected internal Result(bool isSuccess, Error error, TResponse? value) : base(isSuccess, error)
    {
        Value = value;
    }

    public override IActionResult MapResponse()
    {
        if (IsFailure && Error.StatusCode == HttpStatusCode.BadRequest)
        {
            return new BadRequestObjectResult(Error);
        }

        if (IsFailure && Error.StatusCode == HttpStatusCode.NotFound)
        {
            return new NotFoundObjectResult(Error);
        }

        return CreateOkResponseObject();
    }

    private OkObjectResult CreateOkResponseObject()
    {
        if (Value == null)
        {
            return new OkObjectResult(null);
        }
        var responseData = new CustomOkResponseData();

        var sourcePropertiesType = Value.GetType().FullName;
        var destinationProperties = responseData.GetType().GetProperties().ToList();
        var destinationProperty = destinationProperties.Find(prop => prop.PropertyType.FullName == sourcePropertiesType);
        destinationProperty?.SetValue(responseData, Value, null);

        var responseObj = new CustomOkResponse(responseData);
        return new OkObjectResult(responseObj);
    }

}
