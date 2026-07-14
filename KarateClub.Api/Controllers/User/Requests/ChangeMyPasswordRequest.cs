namespace KarateClub.Api.Controllers.User.Requests
{
    public record ChangeMyPasswordRequest(string CurrentPassword, string NewPassword);
}
