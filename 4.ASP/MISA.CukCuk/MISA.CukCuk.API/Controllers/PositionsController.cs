using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;

namespace MISA.CukCuk.API.Controllers
{
    //[Route("api/v1/positions")]
    //[ApiController]

    public class PositionsController : BaseController<Position>
    {
        #region Constructors

        //@ CreateBy: Hungnn (17-08-2021)

        public PositionsController(IPositionService positionService):base(positionService)
        {            

        }

        #endregion
    }
}
