namespace Auth.Core.DTO;
public record RegisterRequest(string? Email, string? Password, string? PersonName, GenderOption Gender) 
{
    public RegisterRequest() : this(null, null, null, GenderOption.PreferNotToSay) { }

};
 
