using AwoBot.BaseDataModels;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public class UserFilterModel : GuildUserModel, IDeletionFilter
  {
    public UserFilterModel()
    {

    }

    public UserFilterModel(IGuildUser guildUser) : base(guildUser)
    {
      Enabled = true;
    }

    public string DisplayText => $"{GuildUser.Mention}: {(Enabled ? "Enabled" : "Disabled")}";
    public bool Enabled { get; set; }


    public bool IsDeletionPermitted(IMessage message)
    {
      return Enabled == false || message.Author.Id != message.Author.Id;
    }
  }
}
