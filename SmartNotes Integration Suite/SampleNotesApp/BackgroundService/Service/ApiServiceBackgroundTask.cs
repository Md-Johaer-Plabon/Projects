using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;

namespace BackgroundService.Service
{
    public sealed class ApiServiceBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private AppServiceConnection _appServiceConnection;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            _appServiceConnection = details?.AppServiceConnection;
            if (_appServiceConnection != null)
            {
                _appServiceConnection.RequestReceived += OnRequestReceived;
                _appServiceConnection.ServiceClosed += OnServiceClosed;
            }
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            //
        }

        private void OnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            _deferral.Complete();
        }
    }
}
