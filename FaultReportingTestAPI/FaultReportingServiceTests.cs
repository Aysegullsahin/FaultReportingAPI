using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.BLL.Services.Concrete;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Net;

public class FaultReportingServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IFaultReportingMapper> _mockMapper;
    private readonly FaultReportingService _service;

    public FaultReportingServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockMapper = new Mock<IFaultReportingMapper>();

        _service = new FaultReportingService(
            _mockUow.Object,
            _mockHttpContextAccessor.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldReturnUnprocessableContent_WhenStatusIsSameAsCurrent()
    {
        long faultId = 1;
        var currentStatus = StatusEnum.New;
        var existingEntity = new FaultReporting { Id = faultId, Status = currentStatus };

        _mockUow.Setup(u => u.faultReportingRepository.GetByIdAsync(faultId))
            .ReturnsAsync(new DataResult<FaultReporting> { Success = true, Data = existingEntity });

        var result = await _service.ChangeStatusAsync(faultId, currentStatus);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.UnprocessableContent, result.StatusCode);
        Assert.Equal("Status is already set to the specified value.", result.Message);

        _mockUow.Verify(u => u.faultReportingRepository.CrudAsync(
            It.IsAny<FaultReporting>(),          
            It.IsAny<EntityStateEnum>(),         
            It.IsAny<Dictionary<string, object>>(),
            It.IsAny<long?>()                     
        ), Times.Never);
    }

    [Fact]
    public async Task ChangeStatusAsync_ShouldReturnError_WhenEntityNotFound()
    {
        long id = 1;

        _mockUow.Setup(u => u.faultReportingRepository.GetAsync<FaultReportingDto_S>(It.IsAny<Expression<Func<FaultReporting, bool>>>(), null))
    .ReturnsAsync(new DataResult<CoreBaseEntity>
    {
        Success = false,
        Message = "Not found"
    });

        var result = await _service.ChangeStatusAsync(id, StatusEnum.New);

        Assert.False(result.Success);
        Assert.Equal("Not found", result.Message);
    }
}