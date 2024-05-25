namespace ProjectOnsMagasin;

public interface IJwtProvider
{
    string GenrateAccessToken(User user);
}
