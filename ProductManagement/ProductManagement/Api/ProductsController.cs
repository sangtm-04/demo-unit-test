using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Interfaces;
using ProductManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductManagement.Api
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public AjaxResult GetAllProducts()
        {
            try
            {
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true,
                    Data = _productRepository.GetAllProducts()
                };
            }
            catch (Exception ex)
            {
                return new AjaxResult
                {
                    Code = 1001,
                    Success = false,
                    Message = "Lỗi hệ thống"
                };
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public AjaxResult GetProductById(int id)
        {
            try
            {
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true,
                    Data = _productRepository.GetProductById(id)
                };
            }
            catch (Exception ex)
            {
                return new AjaxResult
                {
                    Code = 1001,
                    Success = false,
                    Message = "Lỗi hệ thống"
                };
            }
        }

        // POST api/<controller>
        [HttpPost]
        public AjaxResult Post([FromBody]Product product)
        {
            try
            {
                if (product.Price <= 0)
                {
                    return new AjaxResult
                    {
                        Code = 1002,
                        Success = false,
                        Message = "Giá sản phẩm phải lớn hơn 0"
                    };
                }

                if (_productRepository.IsExistedProduct(product.Id))
                {
                    return new AjaxResult
                    {
                        Code = 1003,
                        Success = false,
                        Message = "Id đã tồn tại"
                    };
                }

                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;

                if (!_productRepository.InsertProduct(product))
                {
                    return new AjaxResult
                    {
                        Code = 1004,
                        Success = false,
                        Message = "Thêm mới sản phẩm thất bại"
                    };
                }
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AjaxResult
                {
                    Code = 1001,
                    Success = false,
                    Message = "Lỗi hệ thống"
                };
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public AjaxResult Put(int id, [FromBody]Product product)
        {
            try
            {
                if (id != product.Id)
                {
                    return new AjaxResult
                    {
                        Code = 1002,
                        Success = false,
                        Message = "Id không hợp lệ"
                    };
                }

                product.ModifiedDate = DateTime.Now;

                if (!_productRepository.UpdateProduct(product))
                {
                    return new AjaxResult
                    {
                        Code = 1003,
                        Success = false,
                        Message = "Cập nhật sản phẩm thất bại"
                    };
                }
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AjaxResult
                {
                    Code = 1001,
                    Success = false,
                    Message = "Lỗi hệ thống"
                };
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public AjaxResult Delete(int id)
        {
            try
            {
                if (!_productRepository.DeleteProductById(id))
                {
                    return new AjaxResult
                    {
                        Code = 1002,
                        Success = false,
                        Message = "Xóa sản phẩm thất bại"
                    };
                }
                return new AjaxResult
                {
                    Code = (int)HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AjaxResult
                {
                    Code = 1001,
                    Success = false,
                    Message = "Lỗi hệ thống"
                };
            }
        }
    }
}
