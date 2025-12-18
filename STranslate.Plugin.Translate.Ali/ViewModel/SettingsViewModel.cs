using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace STranslate.Plugin.Translate.Ali.ViewModel;

public partial class SettingsViewModel : ObservableObject, IDisposable
{
    private readonly IPluginContext _context;
    private readonly Settings _settings;

    [ObservableProperty] public partial string Url { get; set; }
    [ObservableProperty] public partial string AccessKeyID { get; set; }
    [ObservableProperty] public partial string AccessKeySecret { get; set; }

    public SettingsViewModel(IPluginContext context, Settings settings)
    {
        _context = context;
        _settings = settings;

        Url = settings.Url;
        AccessKeyID = settings.AccessKeyID;
        AccessKeySecret = settings.AccessKeySecret;

        PropertyChanged += OnSettingsPropertyChanged;
    }

    private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Url))
        {
            _settings.Url = Url;
        }
        else if (e.PropertyName == nameof(AccessKeyID))
        {
            _settings.AccessKeyID = AccessKeyID;
        }
        else if (e.PropertyName == nameof(AccessKeySecret))
        {
            _settings.AccessKeySecret = AccessKeySecret;
        }
        _context.SaveSettingStorage<Settings>();
    }

    public void Dispose() => PropertyChanged -= OnSettingsPropertyChanged;
}
