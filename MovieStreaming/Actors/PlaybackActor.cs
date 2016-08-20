using System;
using System.Threading;
using Akka.Actor;
using MovieStreaming.Messages;
using Utils.MovieStreaming;

namespace MovieStreaming.Actors
{
    public class PlaybackActor : ReceiveActor
    {

        public PlaybackActor()
        {
            Console.WriteLine("Playback Actor Created");

            //using predicates
            //Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message), message => message.UserId == 1);
            Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
            
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            ColorConsole.WriteLineYellow(
                string.Format
                    ("PlayMovieMessage: {0} - {1}", message.MovieTitle, message.UserId));

            Thread.Sleep(500);

            ColorConsole.WriteLineYellow(
                string.Format
                    ("PlayMovieMessage: {0} - {1} Processed", message.MovieTitle, message.UserId));


        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("Playback Actor pre start");

            base.PreStart();
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("Playback Actor PostStop");

            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen(string.Format("Playback Actor PreRestart - {0}", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen(string.Format("Playback Actor PostRestart - {0}", reason));
            base.PostRestart(reason);
        }
    }
}
