// ============================================================================
// ChattyWidgets: Weather Card International & Settings Integration Reference
// Component: com.chatty.weather-card
// ============================================================================

using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChattyWidgets.Core.Services;

/// <summary>
/// World city metadata model for curated presets and geocoding search results.
/// </summary>
public class WorldCityInfo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string NameRu { get; set; } = "";
    public string Country { get; set; } = "";
    public string CountryRu { get; set; } = "";
    public string CountryCode { get; set; } = "";
    public string Flag { get; set; } = "🌍";
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Timezone { get; set; } = "auto";

    public string DisplayName(bool isRussian)
    {
        string city = isRussian && !string.IsNullOrWhiteSpace(NameRu) ? NameRu : Name;
        string country = isRussian && !string.IsNullOrWhiteSpace(CountryRu) ? CountryRu : Country;
        return $"{Flag} {city}, {country}";
    }
}

public static class WeatherConstants
{
    public static readonly List<WorldCityInfo> CuratedCities = new()
    {
        // Russia & CIS
        new() { Id = "mow-ru", Name = "Moscow", NameRu = "Москва", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 55.7558, Lon = 37.6173, Timezone = "Europe/Moscow" },
        new() { Id = "spb-ru", Name = "Saint Petersburg", NameRu = "Санкт-Петербург", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 59.9343, Lon = 30.3351, Timezone = "Europe/Moscow" },
        new() { Id = "nsk-ru", Name = "Novosibirsk", NameRu = "Новосибирск", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 55.0084, Lon = 82.9357, Timezone = "Asia/Novosibirsk" },
        new() { Id = "ekb-ru", Name = "Yekaterinburg", NameRu = "Екатеринбург", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 56.8389, Lon = 60.6057, Timezone = "Asia/Yekaterinburg" },
        new() { Id = "kzn-ru", Name = "Kazan", NameRu = "Казань", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 55.8304, Lon = 49.0661, Timezone = "Europe/Moscow" },
        new() { Id = "aer-ru", Name = "Sochi", NameRu = "Сочи", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 43.6028, Lon = 39.7342, Timezone = "Europe/Moscow" },
        new() { Id = "vvo-ru", Name = "Vladivostok", NameRu = "Владивосток", Country = "Russia", CountryRu = "Россия", CountryCode = "RU", Flag = "🇷🇺", Lat = 43.1198, Lon = 131.8869, Timezone = "Asia/Vladivostok" },
        new() { Id = "ala-kz", Name = "Almaty", NameRu = "Алматы", Country = "Kazakhstan", CountryRu = "Казахстан", CountryCode = "KZ", Flag = "🇰🇿", Lat = 43.2389, Lon = 76.8897, Timezone = "Asia/Almaty" },
        new() { Id = "tse-kz", Name = "Astana", NameRu = "Астана", Country = "Kazakhstan", CountryRu = "Казахстан", CountryCode = "KZ", Flag = "🇰🇿", Lat = 51.1694, Lon = 71.4491, Timezone = "Asia/Almaty" },
        new() { Id = "tas-uz", Name = "Tashkent", NameRu = "Ташкент", Country = "Uzbekistan", CountryRu = "Узбекистан", CountryCode = "UZ", Flag = "🇺🇿", Lat = 41.2995, Lon = 69.2401, Timezone = "Asia/Tashkent" },
        new() { Id = "msq-by", Name = "Minsk", NameRu = "Минск", Country = "Belarus", CountryRu = "Беларусь", CountryCode = "BY", Flag = "🇧🇾", Lat = 53.9045, Lon = 27.5615, Timezone = "Europe/Minsk" },
        new() { Id = "evn-am", Name = "Yerevan", NameRu = "Ереван", Country = "Armenia", CountryRu = "Армения", CountryCode = "AM", Flag = "🇦🇲", Lat = 40.1792, Lon = 44.4991, Timezone = "Asia/Yerevan" },
        new() { Id = "tbs-ge", Name = "Tbilisi", NameRu = "Тбилиси", Country = "Georgia", CountryRu = "Грузия", CountryCode = "GE", Flag = "🇬🇪", Lat = 41.7151, Lon = 44.8271, Timezone = "Asia/Tbilisi" },
        new() { Id = "gyd-az", Name = "Baku", NameRu = "Баку", Country = "Azerbaijan", CountryRu = "Азербайджан", CountryCode = "AZ", Flag = "🇦🇿", Lat = 40.4093, Lon = 49.8671, Timezone = "Asia/Baku" },

        // Europe
        new() { Id = "lon-uk", Name = "London", NameRu = "Лондон", Country = "United Kingdom", CountryRu = "Великобритания", CountryCode = "GB", Flag = "🇬🇧", Lat = 51.5074, Lon = -0.1278, Timezone = "Europe/London" },
        new() { Id = "par-fr", Name = "Paris", NameRu = "Париж", Country = "France", CountryRu = "Франция", CountryCode = "FR", Flag = "🇫🇷", Lat = 48.8566, Lon = 2.3522, Timezone = "Europe/Paris" },
        new() { Id = "ber-de", Name = "Berlin", NameRu = "Берлин", Country = "Germany", CountryRu = "Германия", CountryCode = "DE", Flag = "🇩🇪", Lat = 52.5200, Lon = 13.4050, Timezone = "Europe/Berlin" },
        new() { Id = "rom-it", Name = "Rome", NameRu = "Рим", Country = "Italy", CountryRu = "Италия", CountryCode = "IT", Flag = "🇮🇹", Lat = 41.9028, Lon = 12.4964, Timezone = "Europe/Rome" },
        new() { Id = "mad-es", Name = "Madrid", NameRu = "Мадрид", Country = "Spain", CountryRu = "Испания", CountryCode = "ES", Flag = "🇪🇸", Lat = 40.4168, Lon = -3.7038, Timezone = "Europe/Madrid" },
        new() { Id = "ams-nl", Name = "Amsterdam", NameRu = "Амстердам", Country = "Netherlands", CountryRu = "Нидерланды", CountryCode = "NL", Flag = "🇳🇱", Lat = 52.3676, Lon = 4.9041, Timezone = "Europe/Amsterdam" },
        new() { Id = "vie-at", Name = "Vienna", NameRu = "Вена", Country = "Austria", CountryRu = "Австрия", CountryCode = "AT", Flag = "🇦🇹", Lat = 48.2082, Lon = 16.3738, Timezone = "Europe/Vienna" },
        new() { Id = "waw-pl", Name = "Warsaw", NameRu = "Варшава", Country = "Poland", CountryRu = "Польша", CountryCode = "PL", Flag = "🇵🇱", Lat = 52.2297, Lon = 21.0122, Timezone = "Europe/Warsaw" },
        new() { Id = "prg-cz", Name = "Prague", NameRu = "Прага", Country = "Czech Republic", CountryRu = "Чехия", CountryCode = "CZ", Flag = "🇨🇿", Lat = 50.0755, Lon = 14.4378, Timezone = "Europe/Prague" },
        new() { Id = "ist-tr", Name = "Istanbul", NameRu = "Стамбул", Country = "Turkey", CountryRu = "Турция", CountryCode = "TR", Flag = "🇹🇷", Lat = 41.0082, Lon = 28.9784, Timezone = "Europe/Istanbul" },

        // North America
        new() { Id = "nyc-us", Name = "New York", NameRu = "Нью-Йорк", Country = "United States", CountryRu = "США", CountryCode = "US", Flag = "🇺🇸", Lat = 40.7128, Lon = -74.0060, Timezone = "America/New_York" },
        new() { Id = "lax-us", Name = "Los Angeles", NameRu = "Лос-Анджелес", Country = "United States", CountryRu = "США", CountryCode = "US", Flag = "🇺🇸", Lat = 34.0522, Lon = -118.2437, Timezone = "America/Los_Angeles" },
        new() { Id = "chi-us", Name = "Chicago", NameRu = "Чикаго", Country = "United States", CountryRu = "США", CountryCode = "US", Flag = "🇺🇸", Lat = 41.8781, Lon = -87.6298, Timezone = "America/Chicago" },
        new() { Id = "mia-us", Name = "Miami", NameRu = "Майами", Country = "United States", CountryRu = "США", CountryCode = "US", Flag = "🇺🇸", Lat = 25.7617, Lon = -80.1918, Timezone = "America/New_York" },
        new() { Id = "yyz-ca", Name = "Toronto", NameRu = "Торонто", Country = "Canada", CountryRu = "Канада", CountryCode = "CA", Flag = "🇨🇦", Lat = 43.6532, Lon = -79.3832, Timezone = "America/Toronto" },
        new() { Id = "yvr-ca", Name = "Vancouver", NameRu = "Ванкувер", Country = "Canada", CountryRu = "Канада", CountryCode = "CA", Flag = "🇨🇦", Lat = 49.2827, Lon = -123.1207, Timezone = "America/Vancouver" },

        // Asia & Pacific
        new() { Id = "tok-jp", Name = "Tokyo", NameRu = "Токио", Country = "Japan", CountryRu = "Япония", CountryCode = "JP", Flag = "🇯🇵", Lat = 35.6762, Lon = 139.6503, Timezone = "Asia/Tokyo" },
        new() { Id = "bjs-cn", Name = "Beijing", NameRu = "Пекин", Country = "China", CountryRu = "Китай", CountryCode = "CN", Flag = "🇨🇳", Lat = 39.9042, Lon = 116.4074, Timezone = "Asia/Shanghai" },
        new() { Id = "sha-cn", Name = "Shanghai", NameRu = "Шанхай", Country = "China", CountryRu = "Китай", CountryCode = "CN", Flag = "🇨🇳", Lat = 31.2304, Lon = 121.4737, Timezone = "Asia/Shanghai" },
        new() { Id = "sin-sg", Name = "Singapore", NameRu = "Сингапур", Country = "Singapore", CountryRu = "Сингапур", CountryCode = "SG", Flag = "🇸🇬", Lat = 1.3521, Lon = 103.8198, Timezone = "Asia/Singapore" },
        new() { Id = "sel-kr", Name = "Seoul", NameRu = "Сеул", Country = "South Korea", CountryRu = "Южная Корея", CountryCode = "KR", Flag = "🇰🇷", Lat = 37.5665, Lon = 126.9780, Timezone = "Asia/Seoul" },
        new() { Id = "bkk-th", Name = "Bangkok", NameRu = "Бангкок", Country = "Thailand", CountryRu = "Таиланд", CountryCode = "TH", Flag = "🇹🇭", Lat = 13.7563, Lon = 100.5018, Timezone = "Asia/Bangkok" },
        new() { Id = "del-in", Name = "New Delhi", NameRu = "Нью-Дели", Country = "India", CountryRu = "Индия", CountryCode = "IN", Flag = "🇮🇳", Lat = 28.6139, Lon = 77.2090, Timezone = "Asia/Kolkata" },

        // Middle East & Africa
        new() { Id = "dxb-ae", Name = "Dubai", NameRu = "Дубай", Country = "United Arab Emirates", CountryRu = "ОАЭ", CountryCode = "AE", Flag = "🇦🇪", Lat = 25.2048, Lon = 55.2708, Timezone = "Asia/Dubai" },
        new() { Id = "doh-qa", Name = "Doha", NameRu = "Доха", Country = "Qatar", CountryRu = "Катар", CountryCode = "QA", Flag = "🇶🇦", Lat = 25.2854, Lon = 51.5310, Timezone = "Asia/Qatar" },
        new() { Id = "ruh-sa", Name = "Riyadh", NameRu = "Эр-Рияд", Country = "Saudi Arabia", CountryRu = "Саудовская Аравия", CountryCode = "SA", Flag = "🇸🇦", Lat = 24.7136, Lon = 46.6753, Timezone = "Asia/Riyadh" },
        new() { Id = "tlv-il", Name = "Tel Aviv", NameRu = "Тель-Авив", Country = "Israel", CountryRu = "Израиль", CountryCode = "IL", Flag = "🇮🇱", Lat = 32.0853, Lon = 34.7818, Timezone = "Asia/Jerusalem" },
        new() { Id = "cai-eg", Name = "Cairo", NameRu = "Каир", Country = "Egypt", CountryRu = "Египет", CountryCode = "EG", Flag = "🇪🇬", Lat = 30.0444, Lon = 31.2357, Timezone = "Africa/Cairo" },
        new() { Id = "cpt-za", Name = "Cape Town", NameRu = "Кейптаун", Country = "South Africa", CountryRu = "ЮАР", CountryCode = "ZA", Flag = "🇿🇦", Lat = -33.9249, Lon = 18.4241, Timezone = "Africa/Johannesburg" },

        // South America & Oceania
        new() { Id = "sao-br", Name = "Sao Paulo", NameRu = "Сан-Паулу", Country = "Brazil", CountryRu = "Бразилия", CountryCode = "BR", Flag = "🇧🇷", Lat = -23.5505, Lon = -46.6333, Timezone = "America/Sao_Paulo" },
        new() { Id = "bue-ar", Name = "Buenos Aires", NameRu = "Буэнос-Айрес", Country = "Argentina", CountryRu = "Аргентина", CountryCode = "AR", Flag = "🇦🇷", Lat = -34.6037, Lon = -58.3816, Timezone = "America/Argentina/Buenos_Aires" },
        new() { Id = "syd-au", Name = "Sydney", NameRu = "Сидней", Country = "Australia", CountryRu = "Австралия", CountryCode = "AU", Flag = "🇦🇺", Lat = -33.8688, Lon = 151.2093, Timezone = "Australia/Sydney" },
        new() { Id = "mel-au", Name = "Melbourne", NameRu = "Мельбурн", Country = "Australia", CountryRu = "Австралия", CountryCode = "AU", Flag = "🇦🇺", Lat = -37.8136, Lon = 144.9631, Timezone = "Australia/Melbourne" },
        new() { Id = "akl-nz", Name = "Auckland", NameRu = "Окленд", Country = "New Zealand", CountryRu = "Новая Зеландия", CountryCode = "NZ", Flag = "🇳🇿", Lat = -36.8485, Lon = 174.7633, Timezone = "Pacific/Auckland" }
    };
}

/// <summary>
/// Enhanced WeatherWidgetViewModel with CustomSettings persistence, unit conversions (°C/°F, m/s, km/h, mph, mmHg, hPa),
/// online geocoding search, and responsive localization.
/// </summary>
public class WeatherWidgetViewModelEnhanced : NativeViewModelBase
{
    private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(9) };
    private readonly DispatcherTimer _timer;
    private readonly string? _instanceId;

    // Geographic settings
    private string _city = "Москва";
    private double _lat = 55.7558;
    private double _lon = 37.6173;
    private string _country = "RU";
    private string _timezone = "Europe/Moscow";

    // Units: Temp ("C" | "F"), Wind ("ms" | "kmh" | "mph"), Pressure ("mmhg" | "hpa")
    private string _unitTemp = "C";
    private string _unitWind = "ms";
    private string _unitPressure = "mmhg";

    // Raw cached values for instant unit re-rendering without re-fetching
    private double _rawTempC = 14.0;
    private double _rawFeelsLikeC = 12.0;
    private double _rawWindKmh = 12.2;
    private double _rawPressureHpa = 1002.5;
    private int _rawWeatherCode = 0;
    private int _rawHumidity = 64;
    private double _rawRainMm = 0.0;
    private double _rawMinC = 8.0;
    private double _rawMaxC = 16.0;

    // Display properties for XAML bindings
    private string _temperature = "+14°";
    private string _condition = "Ясно";
    private string _weatherIcon = "☀️";
    private string _feelsLike = "Ощущается как +12°";
    private string _humidity = "64%";
    private string _windSpeed = "3.4 м/с";
    private string _pressure = "752 мм";
    private string _minMax = "+8° / +16°";
    private string _rain = "0 мм";
    private string _updateTime = "Обновлено в " + DateTime.Now.ToString("HH:mm");

    // Localized metric labels for XAML
    private string _windLabel = "ВЕТЕР";
    private string _humidityLabel = "ВЛАЖНОСТЬ";
    private string _pressureLabel = "ДАВЛЕНИЕ";
    private string _minMaxLabel = "МИН / МАКС";

    public string City { get => _city; set { _city = value; OnPropertyChanged(); } }
    public double Lat => _lat;
    public double Lon => _lon;
    public string UnitTemp { get => _unitTemp; set { _unitTemp = value; OnPropertyChanged(); ApplyUnits(); } }
    public string UnitWind { get => _unitWind; set { _unitWind = value; OnPropertyChanged(); ApplyUnits(); } }
    public string UnitPressure { get => _unitPressure; set { _unitPressure = value; OnPropertyChanged(); ApplyUnits(); } }

    public string Temperature { get => _temperature; set { _temperature = value; OnPropertyChanged(); } }
    public string Condition { get => _condition; set { _condition = value; OnPropertyChanged(); } }
    public string WeatherIcon { get => _weatherIcon; set { _weatherIcon = value; OnPropertyChanged(); } }
    public string FeelsLike { get => _feelsLike; set { _feelsLike = value; OnPropertyChanged(); } }
    public string Humidity { get => _humidity; set { _humidity = value; OnPropertyChanged(); } }
    public string WindSpeed { get => _windSpeed; set { _windSpeed = value; OnPropertyChanged(); } }
    public string Pressure { get => _pressure; set { _pressure = value; OnPropertyChanged(); } }
    public string MinMax { get => _minMax; set { _minMax = value; OnPropertyChanged(); } }
    public string Rain { get => _rain; set { _rain = value; OnPropertyChanged(); } }
    public string UpdateTime { get => _updateTime; set { _updateTime = value; OnPropertyChanged(); } }

    public string WindLabel { get => _windLabel; set { _windLabel = value; OnPropertyChanged(); } }
    public string HumidityLabel { get => _humidityLabel; set { _humidityLabel = value; OnPropertyChanged(); } }
    public string PressureLabel { get => _pressureLabel; set { _pressureLabel = value; OnPropertyChanged(); } }
    public string MinMaxLabel { get => _minMaxLabel; set { _minMaxLabel = value; OnPropertyChanged(); } }

    public ObservableCollection<ForecastDayItem> Forecast { get; } = new();

    public WeatherWidgetViewModelEnhanced(string? instanceId = null)
    {
        _instanceId = instanceId;

        // 1. Load persisted CustomSettings if present
        if (!string.IsNullOrEmpty(_instanceId) &&
            WidgetManager.Instance.Settings.Widgets.TryGetValue(_instanceId, out var s))
        {
            if (s.CustomSettings.TryGetValue("weatherCity", out var c) && !string.IsNullOrWhiteSpace(c)) _city = c;
            if (s.CustomSettings.TryGetValue("weatherLat", out var latStr) && double.TryParse(latStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lat)) _lat = lat;
            if (s.CustomSettings.TryGetValue("weatherLon", out var lonStr) && double.TryParse(lonStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lon)) _lon = lon;
            if (s.CustomSettings.TryGetValue("weatherCountry", out var co)) _country = co;
            if (s.CustomSettings.TryGetValue("weatherTimezone", out var tz)) _timezone = tz;
            if (s.CustomSettings.TryGetValue("weatherUnitTemp", out var ut)) _unitTemp = ut;
            if (s.CustomSettings.TryGetValue("weatherUnitWind", out var uw)) _unitWind = uw;
            if (s.CustomSettings.TryGetValue("weatherUnitPressure", out var up)) _unitPressure = up;
        }
        else
        {
            // Default according to current UI language
            bool isEn = LocalizationService.Instance.IsEnglish;
            if (isEn)
            {
                _city = "London";
                _lat = 51.5074;
                _lon = -0.1278;
                _country = "GB";
                _timezone = "Europe/London";
                _unitWind = "kmh";
                _unitPressure = "hpa";
            }
        }

        UpdateLocalizedLabels();
        LocalizationService.Instance.LanguageChanged += () =>
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                UpdateLocalizedLabels();
                ApplyUnits();
            });
        };

        _ = RefreshWeatherAsync();

        // Periodic background update (15 mins)
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(15) };
        _timer.Tick += (s, e) => _ = RefreshWeatherAsync();
        _timer.Start();
    }

    public void SetCity(string cityName, double lat, double lon, string country = "RU", string timezone = "auto")
    {
        City = cityName;
        _lat = lat;
        _lon = lon;
        _country = country;
        _timezone = timezone;
        SaveCustomSettings();
        _ = RefreshWeatherAsync();
    }

    public void SetUnits(string tempUnit, string windUnit, string pressureUnit)
    {
        _unitTemp = tempUnit;
        _unitWind = windUnit;
        _unitPressure = pressureUnit;
        SaveCustomSettings();
        ApplyUnits();
    }

    private void SaveCustomSettings()
    {
        if (string.IsNullOrEmpty(_instanceId)) return;
        if (WidgetManager.Instance.Settings.Widgets.TryGetValue(_instanceId, out var s))
        {
            s.CustomSettings["weatherCity"] = _city;
            s.CustomSettings["weatherLat"] = _lat.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            s.CustomSettings["weatherLon"] = _lon.ToString("F4", System.Globalization.CultureInfo.InvariantCulture);
            s.CustomSettings["weatherCountry"] = _country;
            s.CustomSettings["weatherTimezone"] = _timezone;
            s.CustomSettings["weatherUnitTemp"] = _unitTemp;
            s.CustomSettings["weatherUnitWind"] = _unitWind;
            s.CustomSettings["weatherUnitPressure"] = _unitPressure;
            WidgetManager.Instance.SaveSettings();
        }
    }

    private void UpdateLocalizedLabels()
    {
        var loc = LocalizationService.Instance;
        WindLabel = loc.T("Weather.Wind", "ВЕТЕР");
        HumidityLabel = loc.T("Weather.Humidity", "ВЛАЖНОСТЬ");
        PressureLabel = loc.T("Weather.Pressure", "ДАВЛЕНИЕ");
        MinMaxLabel = loc.T("Weather.MinMax", "МИН / МАКС");
    }

    public void ApplyUnits()
    {
        var loc = LocalizationService.Instance;
        bool isEn = loc.IsEnglish;

        // 1. Temperature
        double dispTemp = _unitTemp == "F" ? (_rawTempC * 9.0 / 5.0 + 32.0) : _rawTempC;
        double dispFeels = _unitTemp == "F" ? (_rawFeelsLikeC * 9.0 / 5.0 + 32.0) : _rawFeelsLikeC;
        string tempSign = dispTemp > 0 ? "+" : "";
        string feelsSign = dispFeels > 0 ? "+" : "";
        string tUnit = _unitTemp == "F" ? "°F" : "°";

        Temperature = $"{tempSign}{Math.Round(dispTemp)}{tUnit}";
        string feelsLabel = loc.T("Weather.FeelsLike", isEn ? "Feels like" : "Ощущается как");
        FeelsLike = $"{feelsLabel} {feelsSign}{Math.Round(dispFeels)}{tUnit}";

        // 2. Wind
        if (_unitWind == "mph")
        {
            double mph = _rawWindKmh * 0.621371;
            WindSpeed = $"{mph:F1} mph";
        }
        else if (_unitWind == "kmh")
        {
            WindSpeed = $"{Math.Round(_rawWindKmh)} {(isEn ? "km/h" : "км/ч")}";
        }
        else // "ms"
        {
            double ms = _rawWindKmh / 3.6;
            WindSpeed = $"{ms:F1} {(isEn ? "m/s" : "м/с")}";
        }

        // 3. Pressure
        if (_unitPressure == "hpa")
        {
            Pressure = $"{Math.Round(_rawPressureHpa)} {(isEn ? "hPa" : "гПа")}";
        }
        else // "mmhg"
        {
            int mm = (int)Math.Round(_rawPressureHpa * 0.750062);
            Pressure = $"{mm} {(isEn ? "mmHg" : "мм")}";
        }

        // 4. Min / Max
        double minDisp = _unitTemp == "F" ? (_rawMinC * 9.0 / 5.0 + 32.0) : _rawMinC;
        double maxDisp = _unitTemp == "F" ? (_rawMaxC * 9.0 / 5.0 + 32.0) : _rawMaxC;
        string minSign = minDisp > 0 ? "+" : "";
        string maxSign = maxDisp > 0 ? "+" : "";
        MinMax = $"{minSign}{Math.Round(minDisp)}° / {maxSign}{Math.Round(maxDisp)}°";

        // 5. Condition
        var (desc, icon) = GetWmoInfo(_rawWeatherCode, isEn);
        Condition = desc;
        WeatherIcon = icon;
    }

    public async Task RefreshWeatherAsync()
    {
        try
        {
            string url = FormattableString.Invariant(
                $"https://api.open-meteo.com/v1/forecast?latitude={_lat:F4}&longitude={_lon:F4}&current=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation,weather_code,surface_pressure,wind_speed_10m&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=auto"
            );

            string json = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("current", out var cur))
            {
                _rawTempC = cur.GetProperty("temperature_2m").GetDouble();
                _rawFeelsLikeC = cur.GetProperty("apparent_temperature").GetDouble();
                _rawWeatherCode = cur.GetProperty("weather_code").GetInt32();
                _rawHumidity = cur.GetProperty("relative_humidity_2m").GetInt32();
                _rawWindKmh = cur.GetProperty("wind_speed_10m").GetDouble();
                _rawPressureHpa = cur.GetProperty("surface_pressure").GetDouble();
                _rawRainMm = cur.GetProperty("precipitation").GetDouble();

                var newForecast = new List<ForecastDayItem>();
                if (root.TryGetProperty("daily", out var daily))
                {
                    var times = daily.GetProperty("time").EnumerateArray().ToList();
                    var codes = daily.GetProperty("weather_code").EnumerateArray().ToList();
                    var maxs = daily.GetProperty("temperature_2m_max").EnumerateArray().ToList();
                    var mins = daily.GetProperty("temperature_2m_min").EnumerateArray().ToList();

                    if (maxs.Count > 0 && mins.Count > 0)
                    {
                        _rawMinC = mins[0].GetDouble();
                        _rawMaxC = maxs[0].GetDouble();
                    }

                    bool isEn = LocalizationService.Instance.IsEnglish;
                    var culture = isEn ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("ru-RU");

                    for (int i = 1; i < Math.Min(5, times.Count); i++)
                    {
                        if (DateTime.TryParse(times[i].GetString(), out var dt))
                        {
                            string dayName = dt.ToString("ddd", culture).ToUpper();
                            int c = codes[i].GetInt32();
                            var (_, icon) = GetWmoInfo(c, isEn);
                            double mx = maxs[i].GetDouble();
                            double mxDisp = _unitTemp == "F" ? (mx * 9.0 / 5.0 + 32.0) : mx;
                            string sign = mxDisp > 0 ? "+" : "";
                            newForecast.Add(new ForecastDayItem
                            {
                                DayName = dayName,
                                Icon = icon,
                                Temp = $"{sign}{Math.Round(mxDisp)}°"
                            });
                        }
                    }
                }

                Application.Current?.Dispatcher?.Invoke(() =>
                {
                    Humidity = $"{_rawHumidity}%";
                    Rain = $"{_rawRainMm:F1} мм";
                    string updatedLabel = LocalizationService.Instance.T("Weather.UpdatedAt", "Обновлено в");
                    UpdateTime = $"{updatedLabel} {DateTime.Now:HH:mm}";
                    ApplyUnits();

                    if (newForecast.Count > 0)
                    {
                        Forecast.Clear();
                        foreach (var f in newForecast) Forecast.Add(f);
                    }
                });
            }
        }
        catch { }
    }

    public static (string desc, string icon) GetWmoInfo(int code, bool isEnglish)
    {
        if (isEnglish)
        {
            return code switch
            {
                0 => ("Clear sky", "☀️"),
                1 => ("Mainly clear", "🌤️"),
                2 => ("Partly cloudy", "⛅"),
                3 => ("Overcast", "☁️"),
                45 or 48 => ("Foggy", "🌫️"),
                51 or 53 or 55 => ("Drizzle", "🌦️"),
                56 or 57 => ("Freezing Drizzle", "🌨️"),
                61 or 63 or 65 => ("Rain", "🌧️"),
                66 or 67 => ("Freezing Rain", "🌨️"),
                71 or 73 or 75 => ("Snow fall", "❄️"),
                77 => ("Snow grains", "❄️"),
                80 or 81 or 82 => ("Showers", "🌧️"),
                85 or 86 => ("Snow showers", "🌨️"),
                95 => ("Thunderstorm", "⛈️"),
                96 or 99 => ("Hail Thunderstorm", "⛈️"),
                _ => ("Cloudy", "⛅")
            };
        }

        return code switch
        {
            0 => ("Ясно", "☀️"),
            1 => ("Преимущественно ясно", "🌤️"),
            2 => ("Переменная облачность", "⛅"),
            3 => ("Пасмурно", "☁️"),
            45 or 48 => ("Туман", "🌫️"),
            51 or 53 or 55 => ("Морось", "🌦️"),
            56 or 57 => ("Ледяная морось", "🌨️"),
            61 or 63 or 65 => ("Дождь", "🌧️"),
            66 or 67 => ("Ледяной дождь", "🌨️"),
            71 or 73 or 75 => ("Снегопад", "❄️"),
            77 => ("Снег", "❄️"),
            80 or 81 or 82 => ("Ливневый дождь", "🌧️"),
            85 or 86 => ("Снежный ливень", "🌨️"),
            95 => ("Гроза", "⛈️"),
            96 or 99 => ("Гроза с градом", "⛈️"),
            _ => ("Облачно", "⛅")
        };
    }
}

/// <summary>
/// Ready-to-paste implementation of BuildWeatherSettings() for UI/WidgetSettingsWindow.xaml.cs
/// </summary>
public static class WidgetSettingsWeatherExtensions
{
    public static void BuildWeatherSettingsUI(
        StackPanel settingsPanel,
        TextBlock txtWindowTitle,
        TextBlock txtStatus,
        string instanceId,
        object? viewModel,
        Action? onSettingsChanged,
        Func<Border> createCard,
        Action<string, string> addSectionHeader)
    {
        var loc = LocalizationService.Instance;
        bool isRu = loc.IsRussian;

        txtWindowTitle.Text = loc.T("Weather.Title", "Настройки Погоды");
        addSectionHeader(
            loc.T("Weather.Header", "🌤 Настройки прогноза погоды"),
            loc.T("Weather.HeaderSub", "Выберите город, выполните онлайн поиск или задайте координаты и единицы измерения")
        );

        WeatherWidgetViewModelEnhanced? vm = viewModel as WeatherWidgetViewModelEnhanced;

        // ====================================================================
        // CARD 1: Curated World Cities
        // ====================================================================
        var cardCities = createCard();
        var spCities = new StackPanel();

        spCities.Children.Add(new TextBlock
        {
            Text = loc.T("Weather.QuickCity", "Выбор города мира:"),
            FontSize = 12,
            FontWeight = FontWeights.SemiBold,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 0, 0, 6)
        });

        var comboCities = new ComboBox
        {
            Height = 32,
            Margin = new Thickness(0, 0, 0, 6),
            Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
            Foreground = Brushes.White
        };

        var cityList = WeatherConstants.CuratedCities;
        int selectedIndex = 0;
        for (int i = 0; i < cityList.Count; i++)
        {
            var c = cityList[i];
            comboCities.Items.Add(c.DisplayName(isRu));
            if (vm != null && (vm.City.Equals(c.Name, StringComparison.OrdinalIgnoreCase) || vm.City.Equals(c.NameRu, StringComparison.OrdinalIgnoreCase)))
            {
                selectedIndex = i;
            }
        }
        comboCities.SelectedIndex = selectedIndex;

        comboCities.SelectionChanged += (s, e) =>
        {
            if (comboCities.SelectedIndex >= 0 && comboCities.SelectedIndex < cityList.Count && vm != null)
            {
                var sel = cityList[comboCities.SelectedIndex];
                string name = isRu ? sel.NameRu : sel.Name;
                vm.SetCity(name, sel.Lat, sel.Lon, sel.CountryCode, sel.Timezone);
                txtStatus.Text = string.Format(loc.T("Weather.CityChanged", "Город изменен на {0}. Загрузка прогноза..."), name);
                onSettingsChanged?.Invoke();
            }
        };

        spCities.Children.Add(comboCities);
        cardCities.Child = spCities;
        settingsPanel.Children.Add(cardCities);

        // ====================================================================
        // CARD 2: Online City Search (Open-Meteo Geocoding)
        // ====================================================================
        var cardSearch = createCard();
        var spSearch = new StackPanel();

        spSearch.Children.Add(new TextBlock
        {
            Text = loc.T("Weather.SearchOnline", "Онлайн поиск любого города (Open-Meteo):"),
            FontSize = 12,
            FontWeight = FontWeights.SemiBold,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 0, 0, 6)
        });

        var dockSearch = new DockPanel { Margin = new Thickness(0, 0, 0, 6) };
        var btnSearch = new Button
        {
            Content = loc.T("Weather.SearchBtn", "🔍 Найти"),
            Width = 84,
            Height = 30,
            DockPanel.Dock = DockPanel.Right,
            Margin = new Thickness(6, 0, 0, 0),
            Background = new SolidColorBrush(Color.FromArgb(30, 56, 189, 248)),
            Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248)),
            FontWeight = FontWeights.SemiBold,
            BorderThickness = new Thickness(0),
            Cursor = Cursors.Hand
        };

        var txtSearch = new TextBox
        {
            Height = 30,
            Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
            Foreground = Brushes.White,
            BorderBrush = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255)),
            Padding = new Thickness(6, 4, 6, 4),
            VerticalContentAlignment = VerticalAlignment.Center
        };

        dockSearch.Children.Add(btnSearch);
        dockSearch.Children.Add(txtSearch);
        spSearch.Children.Add(dockSearch);

        var comboResults = new ComboBox
        {
            Height = 30,
            Visibility = Visibility.Collapsed,
            Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
            Foreground = Brushes.White,
            Margin = new Thickness(0, 4, 0, 0)
        };

        var searchResults = new List<WorldCityInfo>();

        async void DoSearch()
        {
            string q = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(q)) return;

            btnSearch.IsEnabled = false;
            txtStatus.Text = loc.T("Weather.SearchOnline", "Поиск городов...");

            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(6) };
                string lang = isRu ? "ru" : "en";
                string url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(q)}&count=6&language={lang}&format=json";
                string json = await client.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                searchResults.Clear();
                comboResults.Items.Clear();

                if (doc.RootElement.TryGetProperty("results", out var resArr))
                {
                    foreach (var item in resArr.EnumerateArray())
                    {
                        string name = item.GetProperty("name").GetString() ?? "";
                        string country = item.TryGetProperty("country", out var cElem) ? cElem.GetString() ?? "" : "";
                        string countryCode = item.TryGetProperty("country_code", out var ccElem) ? ccElem.GetString() ?? "" : "";
                        string tz = item.TryGetProperty("timezone", out var tzElem) ? tzElem.GetString() ?? "auto" : "auto";
                        double lat = item.GetProperty("latitude").GetDouble();
                        double lon = item.GetProperty("longitude").GetDouble();

                        var info = new WorldCityInfo
                        {
                            Name = name,
                            NameRu = isRu ? name : "",
                            Country = country,
                            CountryRu = isRu ? country : "",
                            CountryCode = countryCode,
                            Lat = lat,
                            Lon = lon,
                            Timezone = tz
                        };
                        searchResults.Add(info);
                        comboResults.Items.Add($"📍 {name}, {country} ({lat:F2}, {lon:F2})");
                    }
                }

                if (searchResults.Count > 0)
                {
                    comboResults.Visibility = Visibility.Visible;
                    comboResults.SelectedIndex = 0;
                    txtStatus.Text = $"Найдено результатов: {searchResults.Count}";
                }
                else
                {
                    txtStatus.Text = "Город не найден.";
                    comboResults.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                txtStatus.Text = "Ошибка сети при поиске города.";
            }
            finally
            {
                btnSearch.IsEnabled = true;
            }
        }

        btnSearch.Click += (s, e) => DoSearch();
        txtSearch.KeyDown += (s, e) => { if (e.Key == Key.Enter) DoSearch(); };

        comboResults.SelectionChanged += (s, e) =>
        {
            if (comboResults.SelectedIndex >= 0 && comboResults.SelectedIndex < searchResults.Count && vm != null)
            {
                var sel = searchResults[comboResults.SelectedIndex];
                vm.SetCity(sel.Name, sel.Lat, sel.Lon, sel.CountryCode, sel.Timezone);
                txtStatus.Text = string.Format(loc.T("Weather.CityChanged", "Город изменен на {0}. Загрузка прогноза..."), sel.Name);
                onSettingsChanged?.Invoke();
            }
        };

        spSearch.Children.Add(comboResults);
        cardSearch.Child = spSearch;
        settingsPanel.Children.Add(cardSearch);

        // ====================================================================
        // CARD 3: Manual Lat / Lon Input
        // ====================================================================
        var cardCoords = createCard();
        var spCoords = new StackPanel();

        spCoords.Children.Add(new TextBlock
        {
            Text = loc.T("Weather.ManualCoords", "Точные координаты (Широта / Долгота):"),
            FontSize = 12,
            FontWeight = FontWeights.SemiBold,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 0, 0, 6)
        });

        var gridCoords = new Grid();
        gridCoords.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridCoords.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        gridCoords.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var txtLat = new TextBox
        {
            Text = vm?.Lat.ToString("F4", System.Globalization.CultureInfo.InvariantCulture) ?? "55.7558",
            Height = 30,
            Margin = new Thickness(0, 0, 4, 0),
            Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
            Foreground = Brushes.White,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(txtLat, 0);

        var txtLon = new TextBox
        {
            Text = vm?.Lon.ToString("F4", System.Globalization.CultureInfo.InvariantCulture) ?? "37.6173",
            Height = 30,
            Margin = new Thickness(4, 0, 6, 0),
            Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)),
            Foreground = Brushes.White,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(txtLon, 1);

        var btnApplyCoords = new Button
        {
            Content = loc.T("Weather.ApplyCoordsBtn", "Применить"),
            Width = 84,
            Height = 30,
            Background = new SolidColorBrush(Color.FromArgb(30, 56, 189, 248)),
            Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248)),
            FontWeight = FontWeights.SemiBold,
            BorderThickness = new Thickness(0),
            Cursor = Cursors.Hand
        };
        Grid.SetColumn(btnApplyCoords, 2);

        btnApplyCoords.Click += (s, e) =>
        {
            if (double.TryParse(txtLat.Text.Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var nLat) &&
                double.TryParse(txtLon.Text.Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var nLon))
            {
                if (nLat >= -90.0 && nLat <= 90.0 && nLon >= -180.0 && nLon <= 180.0 && vm != null)
                {
                    vm.SetCity($"{nLat:F2}°, {nLon:F2}°", nLat, nLon, "Custom", "auto");
                    txtStatus.Text = "Координаты применены успешно.";
                    onSettingsChanged?.Invoke();
                    return;
                }
            }
            txtStatus.Text = loc.T("Weather.InvalidCoords", "Неверные координаты.");
        };

        gridCoords.Children.Add(txtLat);
        gridCoords.Children.Add(txtLon);
        gridCoords.Children.Add(btnApplyCoords);
        spCoords.Children.Add(gridCoords);

        cardCoords.Child = spCoords;
        settingsPanel.Children.Add(cardCoords);

        // ====================================================================
        // CARD 4: Measurement Units (°C/°F, m/s/kmh/mph, mmHg/hPa)
        // ====================================================================
        var cardUnits = createCard();
        var spUnits = new StackPanel();

        spUnits.Children.Add(new TextBlock
        {
            Text = loc.T("Weather.UnitsTitle", "Единицы измерения и форматы:"),
            FontSize = 12,
            FontWeight = FontWeights.SemiBold,
            Foreground = Brushes.White,
            Margin = new Thickness(0, 0, 0, 8)
        });

        // 4.1 Temperature
        spUnits.Children.Add(new TextBlock { Text = loc.T("Weather.UnitTemp", "Температура:"), FontSize = 11, Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)), Margin = new Thickness(0, 0, 0, 3) });
        var comboTemp = new ComboBox { Height = 28, Margin = new Thickness(0, 0, 0, 8), Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)), Foreground = Brushes.White };
        comboTemp.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitCelsius", "Градусы Цельсия (°C)"), Tag = "C" });
        comboTemp.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitFahrenheit", "Градусы Фаренгейта (°F)"), Tag = "F" });
        comboTemp.SelectedIndex = vm?.UnitTemp == "F" ? 1 : 0;

        // 4.2 Wind
        spUnits.Children.Add(new TextBlock { Text = loc.T("Weather.UnitWind", "Скорость ветра:"), FontSize = 11, Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)), Margin = new Thickness(0, 0, 0, 3) });
        var comboWind = new ComboBox { Height = 28, Margin = new Thickness(0, 0, 0, 8), Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)), Foreground = Brushes.White };
        comboWind.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitMs", "Метры в секунду (м/с)"), Tag = "ms" });
        comboWind.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitKmh", "Километры в час (км/ч)"), Tag = "kmh" });
        comboWind.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitMph", "Мили в час (mph)"), Tag = "mph" });
        comboWind.SelectedIndex = vm?.UnitWind switch { "kmh" => 1, "mph" => 2, _ => 0 };

        // 4.3 Pressure
        spUnits.Children.Add(new TextBlock { Text = loc.T("Weather.UnitPressure", "Атмосферное давление:"), FontSize = 11, Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)), Margin = new Thickness(0, 0, 0, 3) });
        var comboPressure = new ComboBox { Height = 28, Margin = new Thickness(0, 0, 0, 8), Background = new SolidColorBrush(Color.FromRgb(30, 41, 59)), Foreground = Brushes.White };
        comboPressure.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitMmhg", "Миллиметры рт. ст. (мм)"), Tag = "mmhg" });
        comboPressure.Items.Add(new ComboBoxItem { Content = loc.T("Weather.UnitHpa", "Гектопаскали / мбар (гПа / hPa)"), Tag = "hpa" });
        comboPressure.SelectedIndex = vm?.UnitPressure == "hpa" ? 1 : 0;

        void OnUnitChanged()
        {
            if (vm == null) return;
            string t = (comboTemp.SelectedItem as ComboBoxItem)?.Tag as string ?? "C";
            string w = (comboWind.SelectedItem as ComboBoxItem)?.Tag as string ?? "ms";
            string p = (comboPressure.SelectedItem as ComboBoxItem)?.Tag as string ?? "mmhg";
            vm.SetUnits(t, w, p);
            onSettingsChanged?.Invoke();
        }

        comboTemp.SelectionChanged += (s, e) => OnUnitChanged();
        comboWind.SelectionChanged += (s, e) => OnUnitChanged();
        comboPressure.SelectionChanged += (s, e) => OnUnitChanged();

        spUnits.Children.Add(comboTemp);
        spUnits.Children.Add(comboWind);
        spUnits.Children.Add(comboPressure);

        cardUnits.Child = spUnits;
        settingsPanel.Children.Add(cardUnits);

        // ====================================================================
        // CARD 5: Refresh Button
        // ====================================================================
        var btnRefresh = new Button
        {
            Content = loc.T("Weather.RefreshBtn", "⟳ Обновить прогноз погоды сейчас"),
            Height = 34,
            Margin = new Thickness(0, 0, 0, 10),
            Background = new SolidColorBrush(Color.FromArgb(28, 56, 189, 248)),
            Foreground = new SolidColorBrush(Color.FromRgb(56, 189, 248)),
            FontWeight = FontWeights.Bold,
            BorderThickness = new Thickness(0),
            Cursor = Cursors.Hand
        };
        btnRefresh.Click += async (s, e) =>
        {
            if (vm != null)
            {
                txtStatus.Text = loc.T("Weather.SearchOnline", "Обновление погоды...");
                await vm.RefreshWeatherAsync();
                txtStatus.Text = loc.T("Weather.Refreshed", "Погода успешно обновлена!");
                onSettingsChanged?.Invoke();
            }
        };
        settingsPanel.Children.Add(btnRefresh);
    }
}
