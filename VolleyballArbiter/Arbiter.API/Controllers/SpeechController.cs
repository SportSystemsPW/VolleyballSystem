using Arbiter.API.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;

namespace Arbiter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpeechController : ControllerBase
    {
        private readonly IOptions<AzureSpeechServiceOptions> _options;

        public SpeechController(IOptions<AzureSpeechServiceOptions> options)
        {
            _options = options;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var config = SpeechConfig.FromSubscription(_options.Value.SubscriptionKey, _options.Value.ServiceRegion);
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "en-us" });

            using var audioInput = AudioConfig.FromWavFileInput("Samples/whatstheweatherlike.wav");
            using (var recognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioInput))
            {
                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    return Ok(result.Text);
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    return UnprocessableEntity("Speech could not be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    return Problem(cancellation.ErrorDetails);
                }

                return NoContent();
            }
        }
    }
}