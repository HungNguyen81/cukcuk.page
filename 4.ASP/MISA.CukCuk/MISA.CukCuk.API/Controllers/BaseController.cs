using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    public class BaseController<MISAEntity> : ControllerBase
    {
        #region Fields

        protected IBaseService<MISAEntity> _service;

        protected ServiceResult _serviceResult;

        #endregion

        #region Constructors

        public BaseController(IBaseService<MISAEntity> service)
        {
            _service = service;
        }

        #endregion

        #region Get Requests

        /// <summary>
        /// Lấy dữ liệu toàn bộ nv
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                _serviceResult = _service.Get();

                if (_serviceResult.IsValid == false)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(200, _serviceResult);
                }

                return StatusCode(200, _serviceResult.Data);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Lấy dữ liệu nv ứng với id
        /// </summary>
        /// <param name="entityId">Id nv</param>
        /// <returns></returns>
        [HttpGet("{entityId}")]
        public IActionResult GetEntityByid(Guid entityId)
        {
            // Lấy dữ liệu và phản hồi cho client
            try
            {
                _serviceResult = _service.GetById(entityId);
                if (_serviceResult.IsValid == false)
                {
                    _serviceResult.Msg = Properties.Resources.MISANoContentMsg;
                    return StatusCode(200, _serviceResult);
                }

                return StatusCode(200, _serviceResult.Data);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region Post Requests

        /// <summary>
        /// Thêm thông tin nv mới vào db
        /// </summary>
        /// <param name="entity">Object thông tin nv muốn thêm vào db</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEntity(MISAEntity entity)
        {
            try
            {
                _serviceResult = _service.Add(entity);
                if (_serviceResult.IsValid == false)
                {
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISAInsertMsg;
                return StatusCode(201, _serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region PUT requests

        /// <summary>
        /// Cập nhật thông tin của 1 nv vào trong db
        /// </summary>
        /// <param name="entityId">Id nv</param>
        /// <param name="entity">Dữ liệu muốn cập nhật</param>
        /// <returns></returns>
        [HttpPut("{entityId}")]
        public IActionResult EditEntity(Guid entityId, MISAEntity entity)
        {
            // Thực thi truy vấn và trả về kết quả cho client
            try
            {
                var serverResult = _service.Update(entity, entityId);

                if (serverResult.IsValid == false)
                {
                    return StatusCode(400, serverResult);
                }

                serverResult.Msg = Properties.Resources.MISAUpdateMsg;
                return StatusCode(200, serverResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion

        #region Delete Requests

        /// <summary>
        /// Xóa nv với id tương ứng
        /// </summary>
        /// <param name="entityId">Id của nv muốn xóa</param>
        /// <returns></returns>

        [HttpDelete("{entityId}")]
        public IActionResult DeleteEntityById(Guid entityId)
        {
            try
            {
                _serviceResult = _service.DeleteOne(entityId);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISADeleteMsg;
                return StatusCode(200, _serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }


        /// <summary>
        /// Xóa nhiều bản ghi qua 1 request
        /// </summary>
        /// <param name="entityIds">List<string> chứa mảng id ứng với các bản ghi cần xóa</string></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteEntities([FromBody] List<Guid> entityIds)
        {
            try
            {
                _serviceResult = _service.DeleteMany(entityIds);

                if (!_serviceResult.IsValid)
                {
                    _serviceResult.Msg = Properties.Resources.MISAErrorMessage;
                    return StatusCode(400, _serviceResult);
                }

                _serviceResult.Msg = Properties.Resources.MISADeleteMsg;

                return StatusCode(200, _serviceResult);
            }
            catch (Exception e)
            {
                var response = new
                {
                    devMsg = e.Message,
                    userMsg = Properties.Resources.MISAErrorMessage,
                    errorCode = "MISA_003",
                    traceId = Guid.NewGuid().ToString()
                };
                return StatusCode(500, response);
            }
        }

        #endregion
    }
}
