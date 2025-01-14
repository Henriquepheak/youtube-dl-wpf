﻿using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDl.Wpf.Utils;

namespace YoutubeDl.Wpf.Models;

public class Settings
{
    /// <summary>
    /// Defines the default configuration version
    /// used by this version of the app.
    /// </summary>
    public const int DefaultVersion = 1;

    /// <summary>
    /// Defines the default custom output filename template.
    /// We use yt-dlp's default as the default custom value.
    /// </summary>
    public const string DefaultCustomFilenameTemplate = "%(title)s [%(id)s].%(ext)s";

    /// <summary>
    /// Gets or sets the settings version number.
    /// Defaults to <see cref="DefaultVersion"/>.
    /// </summary>
    public int Version { get; set; } = DefaultVersion;

    public BaseTheme AppColorMode { get; set; } = BaseTheme.Inherit;

    public BackendTypes Backend { get; set; } = BackendTypes.Ytdlp;

    public string BackendPath { get; set; } = "";

    public BackendArgument[] BackendGlobalArguments { get; set; } = Array.Empty<BackendArgument>();

    public BackendArgument[] BackendDownloadArguments { get; set; } = Array.Empty<BackendArgument>();

    public bool BackendAutoUpdate { get; set; } = true;

    public DateTimeOffset BackendLastUpdateCheck { get; set; }

    public string FfmpegPath { get; set; } = "";

    public string Proxy { get; set; } = "";

    public int LoggingMaxEntries { get; set; } = 1024;

    public Preset? SelectedPreset { get; set; } = Preset.Auto;

    public string SelectedPresetText { get; set; } = "Auto";

    public Preset[] CustomPresets { get; set; } = Array.Empty<Preset>();

    public bool AddMetadata { get; set; } = true;

    public bool DownloadThumbnail { get; set; } = true;

    public bool DownloadSubtitles { get; set; } = true;

    public bool DownloadSubtitlesAllLanguages { get; set; }

    public bool DownloadAutoGeneratedSubtitles { get; set; }

    public bool DownloadPlaylist { get; set; }

    public bool UseCustomOutputTemplate { get; set; }

    public string CustomOutputTemplate { get; set; } = DefaultCustomFilenameTemplate;

    public bool UseCustomPath { get; set; }

    public string DownloadPath { get; set; } = "";

    public string[] DownloadPathHistory { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Loads settings from Settings.json.
    /// </summary>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
    /// <returns>
    /// A ValueTuple containing a <see cref="Settings"/> object and an optional error message.
    /// </returns>
    public static async Task<(Settings settings, string? errMsg)> LoadSettingsAsync(CancellationToken cancellationToken = default)
    {
        var (settings, errMsg) = await FileHelper.LoadJsonAsync("Settings.json", SettingsJsonSerializerContext.Default.Settings, cancellationToken).ConfigureAwait(false);
        errMsg ??= settings.UpdateSettings();
        return (settings, errMsg);
    }

    /// <summary>
    /// Saves settings to Settings.json.
    /// </summary>
    /// <param name="settings">The <see cref="Settings"/> object to save.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
    /// <returns>
    /// An optional error message.
    /// Null if no errors occurred.
    /// </returns>
    public static Task<string?> SaveSettingsAsync(Settings settings, CancellationToken cancellationToken = default)
        => FileHelper.SaveJsonAsync("Settings.json", settings, SettingsJsonSerializerContext.Default.Settings, false, false, cancellationToken);

    /// <summary>
    /// Updates the loaded settings to the latest version.
    /// If the loaded settings have a higher version number,
    /// an error message is returned.
    /// </summary>
    /// <returns>
    /// An optional error message.
    /// Null if no errors occurred.
    /// </returns>
    public string? UpdateSettings()
    {
        switch (Version)
        {
            case 0: // nothing to do
                Version++;
                goto case 1; // go to the next update path
            case DefaultVersion: // current version
                return null;
            default:
                return $"Settings version {Version} is newer than supported.";
        }
    }
}
