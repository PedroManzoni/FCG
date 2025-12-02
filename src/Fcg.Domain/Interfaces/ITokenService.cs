using Fcg.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcg.Domain.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}
