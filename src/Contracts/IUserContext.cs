namespace IATec.Shared.Api.Authentication.Contracts;

public interface IUserContext
{
    string GetEmail();

    string GetUId();

    string GetProviderType();
}