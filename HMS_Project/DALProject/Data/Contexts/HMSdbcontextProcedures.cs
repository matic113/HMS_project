﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using DALProject.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DALProject.Data.Contexts
{
    public partial class HMSdbcontext
	{
        private IHMSdbcontextProcedures _procedures;

        public virtual IHMSdbcontextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new HMSdbcontextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public IHMSdbcontextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class HMSdbcontextProcedures : IHMSdbcontextProcedures
    {
        private readonly HMSdbcontext _context;

        public HMSdbcontextProcedures(HMSdbcontext context)
        {
            _context = context;
        }

        public virtual async Task<int> sp_DeleteActiveSubstanceAsync(int? Id, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    Value = Id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[sp_DeleteActiveSubstance] @Id = @Id", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
