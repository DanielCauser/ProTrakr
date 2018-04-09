using Prism.Logging;
using Prism.Services;

namespace ProTrakr.Services
{
    public class UtilityService : IUtilityService
    {
        public UtilityService(IPageDialogService pageDialogService,
            IErrorManagementService errorManagementService,
            IDeviceService deviceService,
            ILoggerFacade loggerFacade)
        {
            PageDialogService = pageDialogService;
            ErrorManagementService = errorManagementService;
            DeviceService = deviceService;
            LoggerFacade = loggerFacade;
        }
        
        public IPageDialogService PageDialogService { get; }
        public IErrorManagementService ErrorManagementService { get; }
        public IDeviceService DeviceService { get; }
        public ILoggerFacade LoggerFacade { get; }
    }
}