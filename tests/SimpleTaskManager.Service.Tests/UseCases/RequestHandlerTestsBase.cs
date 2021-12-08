using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Data.Sql;
using SimpleTaskManager.Service.Configurations.AutoMapper;
using System;

namespace SimpleTaskManager.Service.Tests.UseCases
{
    [TestClass]
    public abstract class RequestHandlerTestsBase
    {
        protected SimpleTaskManagerContext Context;
        protected IMapper Mapper;

        [TestInitialize]
        public void InitBase()
        {
            Context = GetNewContext();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            Mapper = mockMapper.CreateMapper();
        }

        [TestCleanup]
        public void CleanupBase()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        private SimpleTaskManagerContext GetNewContext()
        {
            var options = new DbContextOptionsBuilder<SimpleTaskManagerContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new SimpleTaskManagerContext(options);
        }
    }
}
