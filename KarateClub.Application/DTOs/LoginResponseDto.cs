using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; init; } = "";

        public UserDto User { get; init; } = null!;
    }
}
