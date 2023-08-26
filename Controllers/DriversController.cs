using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CachingRedis.Models;
using CachingRedis.Models.Data;
using CachingRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachingRedis.Controllers;


[ApiController]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ICacheService _cacheService;
    private readonly AppDbContext _dbContext;

    public DriversController(ILogger<DriversController> logger, ICacheService cacheService, AppDbContext dbContext)
    {
        _logger = logger;
        _cacheService = cacheService;
        _dbContext = dbContext;
    }

    [HttpGet("drivers", Name = "GetDrivers")]
    public async Task<IActionResult> Get(){
        // check if data is in cache
        var cachedData = _cacheService.GetData<IEnumerable<Driver>>("drivers"); // drivers is the key
        if(cachedData != null && cachedData.Count() > 0)
            return Ok(cachedData);

        // if not in cache, get data from db
        cachedData = await _dbContext.Drivers.ToListAsync();
        
        // set expiry time
        var expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
        _cacheService.SetData<IEnumerable<Driver>>("drivers", cachedData, expirationTime);

        return Ok(cachedData);
    }


    [HttpPost("drivers", Name = "AddDriver")]
    public async Task<IActionResult> Post([FromBody] Driver value)
    {
        var addedObject = await _dbContext.Drivers.AddAsync(value);

        // add to cache
        _cacheService.SetData<Driver>($"driver{value.Id}", addedObject.Entity, DateTimeOffset.UtcNow.AddSeconds(30));

        // save changes
        await _dbContext.SaveChangesAsync();
        return Ok(addedObject.Entity);
    }

    [HttpGet("drivers/{id}", Name = "GetDriver")]
    public async Task<IActionResult> Get(int id)
    {
        // check if data is in cache
        var cachedData = _cacheService.GetData<Driver>($"driver{id}"); // driver{id} is the key
        if(cachedData != null)
            return Ok(cachedData);

        // if not in cache, get data from db
        cachedData = await _dbContext.Drivers.FindAsync(id);
        
        // set expiry time
        var expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
        _cacheService.SetData<Driver>($"driver{id}", cachedData, expirationTime);

        return Ok(cachedData);
    }

    [HttpPut("drivers/{id}", Name = "UpdateDriver")]
    public async Task<IActionResult> Put(int id, [FromBody] Driver value)
    {
        var driver = await _dbContext.Drivers.FindAsync(id);
        if(driver == null)
            return NotFound();

        driver.Name = value.Name;
        driver.DriverNb = value.DriverNb;

        // update cache
        _cacheService.SetData<Driver>($"driver{id}", driver, DateTimeOffset.UtcNow.AddSeconds(30));

        // save changes
        await _dbContext.SaveChangesAsync();
        return Ok(driver);
    }

    [HttpDelete("drivers/{id}", Name = "DeleteDriver")]
    public async Task<IActionResult> Delete(int id)
    {
        var driver = await _dbContext.Drivers.FindAsync(id);
        if(driver == null)
            return NotFound();

        // remove from cache
        _cacheService.RemoveData($"driver{id}");

        // remove from db
        _dbContext.Drivers.Remove(driver);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}