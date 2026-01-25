namespace Architecture.Shared.Commons.Result;

public enum ErrorStatus
{
    None = 100,
    Validation = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    UnprocessableEntity = 422,
    Internal = 500,
}
