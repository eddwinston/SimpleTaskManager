using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTaskManager.Service.UseCases.Boards.RevokeAccess;

namespace SimpleTaskManager.Service.Tests.UseCases.Boards.RevokeAccess
{
    [TestClass]
    public class RevokeAccessRequestHandlerTests : RequestHandlerTestsBase
    {
        private RevokeAccessRequestHandler _handler;

        [TestInitialize]
        public void Init()
        {
            _handler = new RevokeAccessRequestHandler(Context);
        }
    }
}
