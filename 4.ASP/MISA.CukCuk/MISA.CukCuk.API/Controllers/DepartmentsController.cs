using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    //[Route("api/v1/departments")]
    //[ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        #region Constructors

        public DepartmentsController(IDepartmentService departmentService):base(departmentService)
        {
            
        }

        #endregion
    }
}
