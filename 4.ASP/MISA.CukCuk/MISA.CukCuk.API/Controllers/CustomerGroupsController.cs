using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    //[Route("api/v1/customerGroups")]
    //[ApiController]
    public class CustomerGroupsController : BaseController<CustomerGroup>
    {
        #region Constructors

        public CustomerGroupsController(ICustomerGroupService customerGroupService):base(customerGroupService)
        {
                        
        }

        #endregion

    }
}
