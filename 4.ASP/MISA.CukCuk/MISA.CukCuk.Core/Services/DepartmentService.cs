using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositiories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.Core.Services
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {

        #region Constructors

        public DepartmentService(IDepartmentRepository departmentRepository):base(departmentRepository)
        {
        }

        #endregion

    }
}
