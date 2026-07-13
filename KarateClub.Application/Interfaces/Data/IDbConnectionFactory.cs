using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KarateClub.Infrastructure.Persistence.Data
{
    public interface IDbConnectionFactory
    {
        // u can use IDbConnection for more abstraction
        SqlConnection CreateConnection();
    }
}
