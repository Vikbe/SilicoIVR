using Microsoft.AspNetCore.Mvc;
using SilicoIVR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace SilicoIVR.Controllers
{
   

    public class AnswerController : TwilioController
    {
        private readonly IUrlHelper _urlHelper;
        private SilicoDBContext _context;

        public AnswerController(SilicoDBContext context, IUrlHelper urlHelper)
        {
            _context = context;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        [Route("answer")]
        public IActionResult Index()
        {
            var response = new VoiceResponse();


            response.Append(
                new Gather(numDigits: 2, action: new Uri("/answer/gather", UriKind.Relative))
                    .Say("Thanks for calling Silico, if you know your parties extension, enter it now. If you'd like to leave a voice message, press 0 and your call will be returned shortly."));

            //// If the user doesn't enter input, loop

            response.Redirect(new Uri("/answer", UriKind.Relative));




            return Content(response.ToString(), "text/xml");
        }

   

        [Route("answer/gather")]
        [HttpPost]
        public IActionResult Gather(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits))
            {
                switch (digits)
                {
                    case "0":
                        response.Say("You selected assmunching. Good for you!");
                        break;
                  
                    default:
                        {
                            var agent = _context.Agents
                                .Where(a => a.Extension.ToString() == digits)
                                .FirstOrDefault();

                            if (agent != null)
                            {
                                response.Say($"You will be connected to {agent.Name} shortly.").Pause();



                                var dial = new Dial(action: new Uri($"/answer/connect/{agent.ID}", UriKind.Relative));
                                dial.Number(agent.PhoneNumber, url: new Uri("/answer/screenCall", UriKind.Relative));
                                response.Append(dial);
                            }
                            else
                            {
                                response.Say("Sorry, I don't understand that choice.").Pause();
                            }
                            

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

        [Route("answer/connectMessage")]
        public TwiMLResult ConnectMessage()
        {
            var response = new VoiceResponse()
                .Say("Connecting you to the extraterrestrial in distress");
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

            response.Hangup();

            return TwiML(response);

        }

    }
}
