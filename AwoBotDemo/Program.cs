using AwoBot;
using AwoBotDemo.Modules.MessageDeletionModule.Data;
using reInject.PostInjectors.BackgroundWorker;
using System;
using System.Threading.Tasks;

namespace AwoBotDemo
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      await BotRunner.Run(bot => {
        bot.Container.AddBackgroundWorker();
        bot.Container.Register<MessageDeletionContext>(ReInject.DependencyStrategy.NewInstance);
      });
    }
  }
}
