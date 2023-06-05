using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wallet.DAL.Entities;
using Wallet.Services;
using Wallet.Services.Service_Interfaces;

namespace Wallet.Web.Controllers;

[ApiController]
[Route("api/test")]
public class CustomerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAccountManager _service;

    public CustomerController(IAccountManager service, IMapper mapper)
    {
        _mapper = mapper;
        _service = service;

    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync()
    {
        var customers = await _service.GetAllAsync();
        var customerViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        return Ok(customerViewModels);

        // var result = await _service.GetAllAsync();
        //
        // return Ok(result);
    }

    [HttpPost("AddAccount")]
    public async Task<IActionResult> AddAsync([FromBody] CustomerViewModel customerInfo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        try
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<CustomerViewModel, Customer>(); });
            IMapper mapper = config.CreateMapper();

            Customer user = mapper.Map<Customer>(customerInfo);
            await _service.AddAsync(user);

            return Ok();
        }
        catch (Exception ex)
        {
            // Handle the exception appropriately
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

/*
[HttpPost("AddAccount")]
public async Task<IActionResult> AddAsync([FromBody] CustomerViewModel customerInfo)
{

    Customer user = _mapper.Map<Customer>(customerInfo);
    await _service.AddAsync(user);
    return Ok();
    
    // Customer user = _mapper.Map<Customer>(customerinfo);
    //      await _service.AddAsync(user);
    //     return Ok();
}
}*/

//  _service.AddAsync(customerinfo);
//
// return Ok(customerinfo);

