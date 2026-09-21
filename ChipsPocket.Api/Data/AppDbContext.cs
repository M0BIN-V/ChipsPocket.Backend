using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChipsPocket.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
}