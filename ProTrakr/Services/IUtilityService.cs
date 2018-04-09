using Prism.Logging;
using Prism.Navigation;
using Prism.Services;

namespace ProTrakr.Services
{
    public interface IUtilityService
    {
        IPageDialogService PageDialogService { get; }
        IErrorManagementService ErrorManagementService { get; }
        IDeviceService DeviceService { get; }
        ILoggerFacade LoggerFacade { get; }
    }
}
