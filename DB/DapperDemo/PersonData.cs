using DapperExtensions;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace DapperDemo
{
    class PersonData : IBaseDb<PersonModel, PersonEnum, int>
    {
        
    }
}
