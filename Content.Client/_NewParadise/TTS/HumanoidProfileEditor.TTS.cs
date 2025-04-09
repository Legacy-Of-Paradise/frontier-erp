using System.Linq;
using Content.Client._NewParadise.TTS;
using Content.Shared._NewParadise.TTS;
using Content.Shared.Preferences;
using Robust.Shared.Random;

// ReSharper disable InconsistentNaming

namespace Content.Client.Lobby.UI;

public sealed partial class HumanoidProfileEditor
{
    private TTSManager _ttsMgr = default!;
    private TTSSystem _ttsSys = default!;
    private List<TTSVoicePrototype> _voiceList = default!;

    private readonly List<string> _sampleText = new()
    {
        "Помогите, клоун насилует в технических тоннелях!",
        "ХоС, ваши сотрудники украли у меня собаку и засунули ее в стиральную машину!",
        "Агент синдиката украл пиво из бара и взорвался!",
        "Врача! Позовите врача!"
    };

    private const string AnySexVoiceProto = "SponsorAnySexVoices";

    private void InitializeVoice()
    {
        _ttsMgr = IoCManager.Resolve<TTSManager>();
        _voiceList = _prototypeManager.EnumeratePrototypes<TTSVoicePrototype>().Where(o => o.RoundStart).ToList();

        VoiceButton.OnItemSelected += args =>
        {
            VoiceButton.SelectId(args.Id);
            SetVoice(_voiceList[args.Id].ID);
        };

        VoicePlayButton.OnPressed += _ => { PlayTTS(); };
    }

    private void UpdateTTSVoicesControls()
    {
        if (Profile is null)
            return;

        VoiceButton.Clear();

        var firstVoiceChoiceId = 1;
        for (var i = 0; i < _voiceList.Count; i++)
        {
            var voice = _voiceList[i];
            if (!HumanoidCharacterProfile.CanHaveVoice(voice, Profile.Sex))
            {
                continue;
            }

            var name = Loc.GetString(voice.Name);
            VoiceButton.AddItem(name, i);

            if (firstVoiceChoiceId == 1)
                firstVoiceChoiceId = i;
        }

        var voiceChoiceId = _voiceList.FindIndex(x => x.ID == Profile.VoiceId);

        if (!VoiceButton.TrySelectId(voiceChoiceId) &&
            VoiceButton.TrySelectId(firstVoiceChoiceId))
        {
            SetVoice(_voiceList[firstVoiceChoiceId].ID);
        }
    }

    private void PlayTTS()
    {
        if (Profile == null)
        {
            return;
        }

        _ttsMgr.RequestPreviewTTS(IoCManager.Resolve<IRobustRandom>().Pick(_sampleText), Profile.VoiceId);
    }
}
