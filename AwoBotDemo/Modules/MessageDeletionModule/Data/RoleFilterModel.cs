using AwoBot.BaseDataModels;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwoBotDemo.Modules.MessageDeletionModule.Data
{
  public class RoleFilterModel : GuildRoleModel, IDeletionFilter
  {

    public RoleFilterModel()
    {

    }

    public RoleFilterModel(IRole channel) : base(channel)
    {
    }

    public bool Enabled { get; set; }

    public string DisplayText => $"{GuildRole.Mention}: {(Enabled ? "Enabled" : "Disabled")}";

    public bool IsDeletionPermitted(IMessage message)
    {
      if (Enabled == false)
        return true;

      if (message.Author is IGuildUser user && GuildRoleId.HasValue)
        return user.RoleIds.Contains(GuildRoleId.Value) == false;

      return true;
    }
  }
}
