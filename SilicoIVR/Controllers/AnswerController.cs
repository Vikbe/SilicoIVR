using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SilicoIVR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Lookups.V1;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Task = System.Threading.Tasks.Task;

namespace SilicoIVR.Controllers
{
   

    public class AnswerController : TwilioController
    {
        private readonly IUrlHelper _urlHelper;
        private SilicoDBContext _context;
        private readonly string _accountSid;
        private readonly string _authToken;


        public AnswerController(SilicoDBContext context, IUrlHelper urlHelper)
        {
            _context = context;
            _urlHelper = urlHelper;

            //These should probably be setup in a json config or DB
            _accountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            _authToken = "your_auth_token";
        }

        [HttpPost]
        [Route("answer")]
        public IActionResult Index(CallParams call)
        {
            var response = new VoiceResponse();
         
            
            response.Append(
                new Gather(numDigits: 3, 
                action: new Uri("/answer/gather/extension/en", UriKind.Relative), 
                timeout: 1));

            LogCall(call).Wait();

            response.Append(
                new Gather(numDigits: 1, action: new Uri("/answer/gather/language", UriKind.Relative))
                    .Play(new Uri("/AudioFiles/intromerci.wav", UriKind.Relative))
                    .Play(new Uri("/AudioFiles/engfr.wav", UriKind.Relative)));    
                //.Say("Thanks for calling Silico, if you know your parties extension, enter it now. If you'd like to leave a voice message, press 0 and your call will be returned shortly."));

            //// If the user doesn't enter input, loop

            response.Redirect(new Uri("/answer", UriKind.Relative));
            



            return Content(response.ToString(), "text/xml");
        }



        [Route("answer/gather/language")]
        [HttpPost]
        public IActionResult GatherLanguage(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits))
            {
                switch (digits)
                {
                    case "1":
                        response.Append(
                                new Gather(numDigits: 1, action: new Uri("/answer/gather/english", UriKind.Relative))
                                .Play(new Uri("/AudioFiles/KnowExtensionEN.mp3", UriKind.Relative)));
                        break;
                    case "2":
                        response.Append(
                               new Gather(numDigits: 1, action: new Uri("/answer/gather/french", UriKind.Relative))
                               .Play(new Uri("/AudioFiles/oui.mp3", UriKind.Relative)));
                        break;

                    default:
                        {

                            response.Append(
                                new Gather(numDigits: 1, action: new Uri("/answer/gather/language", UriKind.Relative))
                                .Play(new Uri("/AudioFiles/ForService.mp3", UriKind.Relative)));
                            break;
                        }


                }
            }
            else
            {
                response.Append(
                            new Gather(numDigits: 1, action: new Uri("/answer/gather/language", UriKind.Relative))
                            .Play(new Uri("/AudioFiles/ForService.mp3", UriKind.Relative)));
            }

            return TwiML(response);
        }

        [Route("answer/gather/english")]
        [HttpPost]
        public IActionResult EnglishService(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits))
            {
                switch (digits)
                {
                    case "1":
                        response.Append(
                            new Gather(numDigits: 3, finishOnKey: "#" , action: new Uri("/answer/gather/extension/en", UriKind.Relative))
                            .Play(new Uri("/AudioFiles/EnterExtension.mp3", UriKind.Relative)));
                        break;
                    case "2":
                        response.Redirect(new Uri("/answer/general", UriKind.Relative));
                      
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    default:
                        {
                           

                            break;
                        }
                }
            }
            else
            {
                // If no input was sent, redirect to the /voice route
                response.Redirect(new Uri("/answer", UriKind.Relative));
            }

            return TwiML(response);
        }

        [Route("answer/gather/french")]
        [HttpPost]
        public IActionResult FrenchService(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits)) {
                switch (digits) {
                    case "1":
                        response.Append(
                            new Gather(numDigits: 3, finishOnKey: "#", action: new Uri("/answer/gather/extension/en", UriKind.Relative))
                            .Play(new Uri("/AudioFiles/EnterExtension.mp3", UriKind.Relative)));
                        break;
                    case "2":
                        response.Redirect(new Uri("/answer/general", UriKind.Relative)); 
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    default: {


                            break;
                        }
                }
            }
            else {
                // If no input was sent, redirect to the /voice route
                response.Redirect(new Uri("/answer", UriKind.Relative));
            }

            return TwiML(response);
        }


        [Route("answer/gather/extension/en")]
        [HttpPost]
        public IActionResult GatherExtensionEN(CallParams cp)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(cp.Digits))
            {

                var agent = _context.Agents
                    .Where(a => a.Extension.ToString() == cp.Digits)
                    .FirstOrDefault();

                if (agent != null)
                {



                    var call = _context.Calls.Where(c => c.SID == cp.CallSid).FirstOrDefault();

                    if (call != null) {
                        call.AgentCalled = agent;
                        _context.Calls.Update(call);
                        _context.SaveChanges();
                    }
                        


                    //response.Say($"You will be connected to {agent.Name} shortly.").Pause();
                    var dial = new Dial(action: new Uri($"/answer/connect/{agent.ID}", UriKind.Relative));
                    dial.Number(agent.PhoneNumber, url: new Uri("/answer/screenCall", UriKind.Relative));
                  
                    response.Append(dial);

                 

                    
                   
                }
                else
                {
                    response.Say("Sorry, I don't understand that choice.").Pause();
                }
            }
            else
            {
                // If no input was sent, redirect to the /voice route
                response.Redirect(new Uri("/answer", UriKind.Relative));
            }

            return TwiML(response);
        }

        [Route("answer/gather/extension/fr")]
        [HttpPost]
        public IActionResult GatherExtensionFR(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits)) {

                var agent = _context.Agents
                    .Where(a => a.Extension.ToString() == digits)
                    .FirstOrDefault();

                if (agent != null) {
                    response.Say($"You will be connected to {agent.Name} shortly.").Pause();
                    var dial = new Dial(action: new Uri($"/answer/connect/{agent.ID}", UriKind.Relative));
                    dial.Number(agent.PhoneNumber, url: new Uri("/answer/screenCall", UriKind.Relative));

                    response.Append(dial);

                    //response.Play(new Uri("/AudioFiles/Goodbye.mp3", UriKind.Relative));
                    //response.Hangup();



                }
                else {
                    response.Say("Sorry, I don't understand that choice.").Pause();
                }
            }
            else {
                // If no input was sent, redirect to the /voice route
                response.Redirect(new Uri("/answer", UriKind.Relative));
            }

            return TwiML(response);
        }


        [Route("answer/general")]
        [HttpPost]
        public IActionResult GeneralInquiries()
        {
            var response = new VoiceResponse();

            //var inquiries = _context.IvrOptions
            //    .Where(o => o.Name == "generalinquiry").First();


            response.Redirect(new Uri("/record/voiceMail", UriKind.Relative));




               
               
            return TwiML(response);
        }




        [HttpPost]
        [Route("answer/screenCall")]
        public TwiMLResult ScreenCall(string from)
        {
            var response = new VoiceResponse();

            var incomingCallMessage = "You have an incoming call from: " +
                                      from;
            var gather = new Gather(
                numDigits: 1,
                action: new Uri("/answer/connectMessage", UriKind.Relative)
            );
            gather.Say(incomingCallMessage)
                  .Say("Press any key to accept");
            response.Append(gather);

            response.Say("Sorry. Did not get your response");
            response.Hangup();

            return TwiML(response);
        }


        [HttpPost] 
        [Route("answer/connect/{id}")]
        public TwiMLResult ConnectToAgent(int id)
        {
            var agent = _context.Agents
                               .Where(a => a.ID == id)
                               .FirstOrDefault();

            var response = new VoiceResponse();

            response.Redirect(new Uri("/record/voiceMail", UriKind.Relative));

            //response.Hangup();

            return TwiML(response);

        }


        private async Task LogCall(CallParams call)
        {
            string callerName = null;
            string callerType = null;


            var addOns = JObject.Parse(call.AddOns);


            if (addOns["status"]?.ToString() == "successful") {
                if (addOns["results"]?["twilio_caller_name"]?["status"].ToString() == "successful") {

                    callerName = addOns["results"]?["twilio_caller_name"]?["result"]?["caller_name"]?["caller_name"].ToString();
                    callerType = addOns["results"]?["twilio_caller_name"]?["result"]?["caller_name"]?["caller_type"].ToString();
                }

            }       
            
            //NOT DOOONNNE
                _context.Calls.Add(new Call {
                SID = call.CallSid,
                Name = (!string.IsNullOrEmpty(callerName)) ? callerName : (!string.IsNullOrEmpty(call.CallerName)) ? call.CallerName : null,
                CallerType = callerType,
                From = call.From,
                To = call.To,
                Zipcode = call.CallerZip,
                City = call.CallerCity,
                State = call.CallerState,
                Country = call.CallerCountry
            });

           await _context.SaveChangesAsync();
        }
    }
}
