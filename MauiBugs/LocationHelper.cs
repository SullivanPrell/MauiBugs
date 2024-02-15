namespace MauiBugs;

public class LocationHelper
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private bool _locationPermissionStatus;

    private async Task<PermissionStatus> CheckLocationPermissions()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status == PermissionStatus.Unknown)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }
        this._locationPermissionStatus = status == PermissionStatus.Granted;

        return status;
    }

    public async Task<bool> GetUserLocationPermissionStatus()
    {
        try
        {
            await this.CheckLocationPermissions();
            return this._locationPermissionStatus;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Location> GetUserCurrentLocationAsync()
    {
        await this.CheckLocationPermissions();
        if (!this._locationPermissionStatus)
        {
            return null;
        }

        try
        {
            this._isCheckingLocation = true;

            GeolocationRequest request = new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

            this._cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, this._cancelTokenSource.Token);
            return location;
        }

        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
            // TODO: throw or handle or something
            return null;
        }
        finally
        {
            this._isCheckingLocation = false;
        }
    }

    public void CancelRequest()
    {
        if (this._isCheckingLocation && this._cancelTokenSource != null &&
            this._cancelTokenSource.IsCancellationRequested == false)
        {
            this._cancelTokenSource.Cancel();
        }
    }

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        try
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
            {
                return status;
            }

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
