namespace dotnet_exemplar.Domain.Entities;

// Entity to hold API tokens for authorization (valid/revoked)
public class ApiToken
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty; // hashed or raw key value
    public string Email { get; set; } = string.Empty; // email of the user who owns this key
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
}