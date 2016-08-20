using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;
using Utils.MovieStreaming;

namespace MovieStreaming.Actors
{
    public class DeadLetterActor : ReceiveActor
    {
        public DeadLetterActor()
        {
            Receive<DeadLetter>(message => HandleDeadLeatterMessage(message));
        }

        private void HandleDeadLeatterMessage(DeadLetter message)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(message.ToString());

            Console.WriteLine("Recipient: {0}\nSender: {1}", message.Recipient, message.Sender);

            if (message.Message is PlayMovieMessage)
            {
                var msg = (PlayMovieMessage) message.Message;
                Console.WriteLine("Dead Letter PlayMovieMessage: {0} - {1}", msg.MovieTitle, msg.UserId);
            }

            Console.ResetColor();
        }
    }
}
