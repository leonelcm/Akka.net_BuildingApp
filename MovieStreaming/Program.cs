using System;
using Akka.Actor;
using Akka.Dispatch.MessageQueues;
using Akka.Event;
using MovieStreaming.Actors;
using MovieStreaming.Messages;

namespace MovieStreaming
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor system created");

            Props playbackActorProps = Props.Create<PlaybackActor>();
            Props deadLetterProps = Props.Create<DeadLetterActor>();
            var subscriber = MovieStreamingActorSystem.ActorOf(deadLetterProps);
            MovieStreamingActorSystem.EventStream.Subscribe(subscriber, typeof(DeadLetter));

            IActorRef playbackActorRef = MovieStreamingActorSystem.ActorOf(playbackActorProps, "PlaybackActor");

            playbackActorRef.Tell(new PlayMovieMessage("American Beauty", 3));
            playbackActorRef.Tell(new PlayMovieMessage("American Beauty 1", 3));
            playbackActorRef.Tell(new PlayMovieMessage("American Beauty 2", 3));


            Console.WriteLine(playbackActorRef.Path);
            MovieStreamingActorSystem.ActorSelection(playbackActorRef.Path)
                .Tell(new PlayMovieMessage("Actor Selection 1", 1));
            MovieStreamingActorSystem.ActorSelection(playbackActorRef.Path)
                .Tell(new PlayMovieMessage("Actor Selection 2", 2));
            MovieStreamingActorSystem.ActorSelection(playbackActorRef.Path)
                .Tell(new PlayMovieMessage("Actor Selection 3", 3));

            Console.ReadKey();

            Console.WriteLine("Sending Poison Pill Message");

            //var subscriber = MovieStreamingActorSystem.ActorOf<DeadLetterMailbox>();
            playbackActorRef.Tell(PoisonPill.Instance);

            Console.ReadKey();
            playbackActorRef.Tell(new PlayMovieMessage("Not Processed", 3));

            IMessageQueue queue = MovieStreamingActorSystem.Mailboxes.DeadLetterMailbox.MessageQueue;

            Console.WriteLine(MovieStreamingActorSystem.Mailboxes.DeadLetterMailbox.MessageQueue.Count);

            MovieStreamingActorSystem.Terminate();

            MovieStreamingActorSystem.WhenTerminated.Wait();

            Console.WriteLine("Actor system terminated");

            Console.ReadKey();
        }
    }
}
