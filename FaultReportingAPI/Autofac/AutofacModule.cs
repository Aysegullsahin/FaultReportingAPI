using Autofac;
using FaultReportingAPI.BLL.Mapper;
using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.BLL.Services.Concrete;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;
using FaultReportingAPI.DAL.UnitOfWork.Concrete;
using FaultReportingAPI.Functions;
using Microsoft.AspNetCore.Authorization;

namespace FaultReportingAPI.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            builder.RegisterType<FaultReportingService>().As<IFaultReportingService>().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<FaultReportingMapper>().As<IFaultReportingMapper>().SingleInstance();
            builder.RegisterType<UserMapper>().As<IUserMapper>().SingleInstance();

            builder.RegisterType<RoleAuthorizationHandler>().As<IAuthorizationHandler>().SingleInstance();

        }
    }
}
