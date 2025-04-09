using Robust.Shared.Configuration;

namespace Content.Shared._NewParadise;

[CVarDefs]
public sealed class NewParadiseCvars
{
    #region TTS
    /// <summary>
    /// Is TTS enabled
    /// </summary>
    public static readonly CVarDef<bool> TtsEnabled =
        CVarDef.Create("tts.enabled", true, CVar.SERVERONLY);

    /// <summary>
    /// URL of the TTS server API.
    /// </summary>
    public static readonly CVarDef<string> TtsApiUrl =
        CVarDef.Create("tts.api_url", string.Empty, CVar.SERVERONLY);

    /// <summary>
    /// Tts api key
    /// </summary>
    public static readonly CVarDef<string> TtsApiKey =
        CVarDef.Create("tts.api_key", string.Empty, CVar.SERVERONLY);

    /// <summary>
    /// TTS Volume
    /// </summary>
    public static readonly CVarDef<float> TtsVolume =
        CVarDef.Create("tts.volume", 0f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// TTS Cache
    /// </summary>
    public static readonly CVarDef<int> TtsMaxCacheSize =
        CVarDef.Create("tts.max_cash_size", 200, CVar.SERVERONLY | CVar.ARCHIVE);


    #endregion
}
