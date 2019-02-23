using Microsoft.AspNetCore.Mvc;
using SilicoIVR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Task = System.Threading.Tasks.Task;

namespace SilicoIVR.Controllers
{
   

    public class RecordingController : TwilioController
    {
        private readonly IUrlHelper _urlHelper;
        private SilicoDBContext _context;

        private readonly string _accountSid;
        private readonly string _authToken;

        public RecordingController(SilicoDBContext context, IUrlHelper urlHelper)
        {
            _context = context;
            _urlHelper = urlHelper;

            //These should probably be setup in a json config or DB
            _accountSid = "ACfb2c3e52b6217f31405de1c7676c58ec";//"ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            _authToken = "dcaaefa03f710845ee9e3741c9b3b456";//"your_auth_token";
        }

        [HttpPost]
        [Route("record/voiceMail")]
        public TwiMLResult VoiceMail(CallItem call)
        {

            var response = new VoiceResponse();
            response.Play(new Uri("/MessagePrompt.wav", UriKind.Relative));
         
            response.Record(
                maxLength: 20,
                playBeep: true, 
                recordingStatusCallback: new Uri("/record/callback", UriKind.Relative)
            );

         
            response.Hangup();

            return TwiML(response);

        }

        [HttpPost]
        [Route("record/callback")]
        public async Task RecordCallback(RecordingCallback callback, CallItem call)
        {
           

            //Records the message and transcribes it to text
            if (callback.RecordingStatus == "completed")
            {
                using (var client = new HttpClient())
                {
                    var ret = await client.GetAsync(callback.RecordingUrl);



                    ret.EnsureSuccessStatusCode();
                    // Asynchronously read the response
                    using (var dataStream = await ret.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {

                        //Path shouldn't be hardcoded
                        using (var fileStream = new FileStream($@"D:\\Silico\\SilicoIVR\\{callback.RecordingSid}.wav", FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                            fileStream.Close();
                        }

                    }


                }

                var text = await VoiceTranscriber.RecognizeSpeechAsync($@"D:\Silico\SilicoIVR\{callback.RecordingSid}.wav");
                SendTranscription(text);
            }

        } 


        private void SendTranscription(string transcription)
        {
            TwilioClient.Init(_accountSid, _authToken);


            string body = $@"You have a message from: I DONT FUCKING KNOW YET!
                            Transcription: 
                            {transcription}";

            var message = MessageResource.Create(
                body: "Hello there!", 
                from: new Twilio.Types.PhoneNumber("+16477979877"),
                to: new Twilio.Types.PhoneNumber("+16476976677")
            );

        }

    }
}
