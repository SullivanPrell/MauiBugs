namespace MauiBugs;

public partial class MainPage : ContentPage
{
    int count = 0;
    private Location loc;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnLocationButtonClicked(object sender, EventArgs e)
    {
        count++;
        try
        {
            await GetCurrentLocation();
            await GetCurrentLocationInst2();
            if (loc is null)
            {
                LocationBtn.Text = $"Error! Null ref";
            }
            else
            {
                LocationBtn.Text = $"User lat: {loc.Latitude} | User long: {loc.Longitude}";
                LocationCounterLabel.Text = count.ToString();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    public async Task GetCurrentLocation()
    {
        try
        {
            var helper = new LocationHelper();

            var location = await helper.GetUserCurrentLocationAsync();
            if (location != null)
            {
                Console.WriteLine(
                    $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                loc = location;
            }
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException3
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    public async Task GetCurrentLocationInst2()
    {
        try
        {
            var helper = new LocationHelper();

            var location = await helper.GetUserCurrentLocationAsync();
            if (location != null)
            {
                Console.WriteLine(
                    $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                loc = location;
            }
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException3
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }
}
