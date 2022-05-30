using AwoBot.BaseDataModels;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public class ChannelFilterModel : GuildChannelModel, IDeletionFilter
  {
    public ChannelFilterModel()
    {

    }

    public ChannelFilterModel(IGuildChannel channel) : base(channel)
    {
      Enabled = true;
    }

    public bool Enabled { get; set; }

    public string DisplayText => $"#{GuildChannel.Name}: {(Enabled ? "Enabled" : "Disabled")}";

    public bool IsDeletionPermitted(IMessage message)
    {
      return Enabled == false || message.Channel.Id != GuildChannelId;
    }
  }
}
