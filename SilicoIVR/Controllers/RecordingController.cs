using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< Updated upstream
using Microsoft.Extensions.Options;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> Stashed changes
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SilicoIVR.Models;
using SilicoIVR.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml.Linq;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Lookups.V1;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Task = System.Threading.Tasks.Task;

namespace SilicoIVR.Controllers
{
   

    public class RecordingController : TwilioController
    {
        private readonly IUrlHelper _urlHelper;
        private SilicoDBContext _context;

        private readonly IvrConfig _config;
        private readonly string _accountSid;
        private readonly string _authToken;

        public RecordingController(SilicoDBContext context, IUrlHelper urlHelper, IOptions<IvrConfig> options)
        {
            _context = context;
            _urlHelper = urlHelper;
            _config = options.Value;
            //These should probably be setup in a json config or DB
            _accountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            _authToken = "your_auth_token";
        }

        [HttpPost]
        [Route("record/voiceMail")]
        public TwiMLResult VoiceMail(CallParams cp)
        {

            if (cp.CallStatus == "completed") {
                
                return new TwiMLResult();
            }


            var call = _context.Calls
                .Include(c => c.AgentCalled)
                .Where(c => c.SID == cp.CallSid).FirstOrDefault();

            //LookUpAddons(call.From);
            var response = new VoiceResponse();
            response.Say(call.AgentCalled.Name);
            response.Play(new Uri("/AudioFiles/NotAvailableEn.mp3", UriKind.Relative));
         
            response.Record(
                maxLength: 120,
                playBeep: true 
                
                
                
                //recordingStatusCallback: new Uri("/record/callback", UriKind.Relative)
            );

         
            response.Hangup();

            return TwiML(response);

        }

        //[HttpPost]
        //[Route("record/callback")]
        //public async Task RecordCallback(RecordingCallback callback, CallParams call)
        //{
           

        //    //Records the message and transcribes it to text
        //    if (callback.RecordingStatus == "completed")
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var ret = await client.GetAsync(callback.RecordingUrl);



        //            ret.EnsureSuccessStatusCode();
                 
        //            using (var dataStream = await ret.Content.ReadAsStreamAsync().ConfigureAwait(false))
        //            {

        //                //Path shouldn't be hardcoded
        //                using (var fileStream = new FileStream($@"D:\\Silico\\SilicoIVR\\{callback.RecordingSid}.wav", FileMode.Create, FileAccess.Write, FileShare.Write))
        //                {
        //                    await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
        //                    fileStream.Close();
        //                }

        //            }


        //        }

        //        var text = await VoiceTranscriber.RecognizeSpeechAsync($@"D:\Silico\SilicoIVR\{callback.RecordingSid}.wav");
        //        SendTranscription(text);
        //    }

        //}


        [HttpPost]
        [Route("record/watson")]
        public async Task Watsoncallback()
        {
            
            var addOns = JObject.Parse(Request.Form["AddOns"]);

            if (addOns["status"]?.ToString() == "successful") {
                if (addOns["results"]?["ibm_watson_speechtotext"]?["status"].ToString() == "successful") {

                    TwilioClient.Init(_accountSid, _authToken);

                    var url = addOns["results"]?["ibm_watson_speechtotext"]?["payload"]?[0]?["url"].ToString();

                    

                    var client = TwilioClient.GetRestClient();

                    //Get the transcript from watson
                    var res = await client.RequestAsync(new Twilio.Http.Request(Twilio.Http.HttpMethod.Get, url));

                    var watsonroot = JsonConvert.DeserializeObject<WatsonRoot>(res.Content);

                    var transcript = watsonroot.results.FirstOrDefault().results.Where(r => r.final == true).FirstOrDefault().alternatives.Where(a => a.confidence != 0).FirstOrDefault();

                    //Get the associated CallSid from the recording json
                    url = addOns["results"]?["ibm_watson_speechtotext"]?["links"]?["recording"].ToString();
                    res = await client.RequestAsync(new Twilio.Http.Request(Twilio.Http.HttpMethod.Get, url + ".json"));

                    var recResource = JObject.Parse(res.Content);
                    var callSid = recResource["call_sid"]?.ToString();

                    var call = _context.Calls.Where(c => c.SID == callSid).FirstOrDefault();

                    var recording = _context.Recordings.Add(new Recording
                    {
                        SID = recResource["sid"]?.ToString(),
                        duration = Double.Parse(recResource["duration"]?.ToString()), 
                        Transcription = transcript.transcript,
                        Call = call
                    });
                  
                    await _context.SaveChangesAsync();

                    SendEmail(recording.Entity);
                }

            }
        }
        [HttpPost]
        [Route("record/transcribe")]
        public async Task TranscribeCallback(Transcription callback)
        {
            if (callback.TranscriptionStatus == "completed") {

                TwilioClient.Init(_accountSid, _authToken);


                string body = $@"You have a message from: {callback.From}!
                            Transcription: {callback.TranscriptionText}";

               using(var client = new SmtpClient()) {
                    client.Credentials = new NetworkCredential("", "");
                    client.Host = "";
                    client.Port = 0;
                }


                var message = MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber("+999999999999"),
                    to: new Twilio.Types.PhoneNumber("+999999999999")
                );

            }

           
            //SendTranscription(text);
            

        }


        //private void SendTranscription(string transcription)
        //{
        //    TwilioClient.Init(_accountSid, _authToken);


        //    string body = $@"You have a message from: Somebody
        //                    Transcription: 
        //                    {transcription}";

        //    var message = MessageResource.Create(
        //        body: body, 
        //        from: new Twilio.Types.PhoneNumber("+16477979877"),
        //        to: new Twilio.Types.PhoneNumber("+16476976677")
        //    );

        //} 

        private void LookUpAddons(string number)
        {
            TwilioClient.Init(_accountSid, _authToken);

            //var addOns = new List<string> {
            //    "twilio_caller_name"
            //};



            //var caller = PhoneNumberResource.Fetch(
            //    addOns: addOns,
            //    pathPhoneNumber: new Twilio.Types.PhoneNumber(number), 

            //);

            //var addOnData = JObject.Parse(caller.AddOns.ToString());

            //if (addOnData["status"]?.ToString() == "successful") {
            //    if (addOnData["results"]?["whitepages_pro_caller_id"]?["status"]?.ToString() == "successful") {

            //        var info = new CallerInfo({
            //            Name = addOnData["results"]?["whitepages_pro_caller_id"]?["status"]?.ToString(),
            //            Address = addOnData["results"]?["whitepages_pro_caller_id"]?["current_addresses"]?[].ToString()

            //        })



            //    }

            //    }

        }

        //This is terrible and shouldnt be like this, but im tired
        private void SendEmail(Recording recording)
        {
            var call = recording.Call;
            using (var client = new SmtpClient()) {
                client.Credentials = new NetworkCredential("apikey", "jibberishkey");
                client.Host = "smtp.sendgrid.net";
                client.Port = 587;

                string subject = ""; 
                if(!string.IsNullOrEmpty(call.Name)) 
                    subject = $"Voice message from {call.Name + " " + call.From }"; 
                else
                    subject = $"Voice message from {call.From }";

                var message = new MailMessage();
                message.From = new MailAddress("ivr@silico.ca");
                message.To.Add("redacted@email.ca");
                message.To.Add("someotheremail@email.ca");
                message.Subject = $"Voice message from {call.Name + " " + call.From }"; 
                message.Body =  $@"You have a message from: {call.From}!
Name: {call.Name} 
Caller Type: {call.CallerType} 
Location: {call.Zipcode}, {call.City}, {call.State}, {call.Country}
Transcription: {recording.Transcription}";
                client.Send(message);
            }
        }

    }
}
